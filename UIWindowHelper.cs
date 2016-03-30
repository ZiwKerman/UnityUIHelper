using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
/// <summary>
/// Helper class for creating and managing a simple window with Title and optional resize handle.
/// Canvas and EventSystem must be present, as are helper scripts: PanelResizer.cs, PanelDragger.cs and PanelFocuser.cs
/// Title zone height, title font and font size are customizable, default font is system's Arial and default size is 14
/// Resize zone is assumed square and is defined by resizeZoneHeight
/// 
/// All window's content should go under windowContentGO GameObject, this window is not scrollable.
/// </summary>
public class UIWindowHelper
{
    public GameObject windowGO, windowTitleGO, windowResizeZoneGO, windowContentGO;
    public GameObject parent;
    private Vector2 pointerOffset;

    public int width = 100, height = 60, minWidth = 30, minHeight = 30;

    private string windowTitle = "New Window";

    
    public int titleHeight = 30;
    public int resizeZoneHeight = 20;

    private Sprite windowBackgroundSprite = null, windowDragButtonSprite = null;

    private Color _windowColor = new Color(1f, 1f, 1f, 1f);

    public bool closable = true;
    public bool resizable = true;
    public bool clampToScreen = true;

    public Font TitleFont
    {
        get
        {
            if (windowTitleGO == null)
                return null;

            var t = windowTitleGO.GetComponent<Text>();
            if (t == null)
                return null;
            else
                return t.font;
        }

        set
        {
            if (windowTitleGO == null)
                return;

            var t = windowTitleGO.GetComponent<Text>();
            if (t == null)
                return;
            else
                t.font = value;
        }
    }

    public int TitleFontSize
    {
        get
        {
            if (windowTitleGO == null)
                return -1;

            var t = windowTitleGO.GetComponent<Text>();
            if (t == null)
                return -1;
            else
                return t.fontSize;
        }

        set
        {
            if (windowTitleGO == null)
                return;

            var t = windowTitleGO.GetComponent<Text>();
            if (t == null)
                return;
            else
                t.fontSize = value;
        }
    }

    public Sprite BackgroundSprite
    {
        get { return windowBackgroundSprite; }
        set { SetBackgroundSprite(value); }
    }

    public Sprite ResizeCornerSprite
    {
        get { return windowDragButtonSprite; }
        set { SetResizeCornerSprite(value); }
    }

    public Color WindowColor
    {
        get { return _windowColor; }
        set { SetWindowColor(value); }
    }

    public Vector3 Position
    {
        get { return windowGO.transform.position; }
        set { windowGO.transform.position = value; }
    }

    public string WindowTitle
    {
        get { return windowTitle; }
        set { SetWindowTitle(value); }
    }

    public Vector2 WindowSize
    {
        get
        {
            return new Vector2(width, height);
        }
        set
        {
            SetWindowSize(value);
        }
    }

    public UIWindowHelper(GameObject canvasGO)
    {
        parent = canvasGO;
        windowGO = CreatePanel(parent);
        windowTitleGO = CreateTitle();

        windowContentGO = CreateContent();

        if (resizable)
        {
            windowResizeZoneGO = CreateResizeZone();
        }
        
        //ResizeContent();
    }

    public void SetWindowSize(Vector2 newSize)
    {
        var rt = windowGO.transform as RectTransform;

        rt.sizeDelta = newSize;
        ResizeContent();
    }

    public void SetBackgroundSprite(Sprite s)
    {
        if (windowGO == null)
            return;

        var img = windowGO.GetComponent<Image>();

        if (img == null)
            return;

        img.sprite = s;
    }

    public void SetResizeCornerSprite(Sprite s)
    {
        if (windowResizeZoneGO == null)
            return;

        var img = windowResizeZoneGO.GetComponent<Image>();

        if (img == null)
            return;

        img.sprite = s;
    }

    public void SetWindowColor(Color c)
    {
        if (windowGO == null)
            return;

        var img = windowGO.GetComponent<Image>();

        if (img == null)
            return;

        img.color = c;
    }

    public void SetWindowTitle(string t)
    {
        if (windowGO == null)
            return;

        var title = windowTitleGO.GetComponent<Text>();

        if (title == null)
            return;

        title.text = t;
        windowGO.name = t + " Panel GameObject";

    }

    private GameObject CreatePanel(GameObject parent)
    {
        GameObject panelRoot = new GameObject(windowTitle + " Panel GameObject");
        RectTransform rectTransform = panelRoot.AddComponent<RectTransform>();

        Vector2 size = new Vector2(width, height);
        rectTransform.sizeDelta = size;
        rectTransform.SetParent(parent.transform, false);

        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.pivot = new Vector2(0f, 1f);
        
        Image image = panelRoot.AddComponent<Image>();
        image.sprite = windowBackgroundSprite;
        image.type = Image.Type.Sliced;
        image.color = _windowColor;

        panelRoot.AddComponent<PanelFocuser>();

        /*var lg = panelRoot.AddComponent<VerticalLayoutGroup>();
        lg.childForceExpandWidth = true;
        lg.childForceExpandHeight = true;
        lg.childAlignment = TextAnchor.UpperRight;
        */
        return panelRoot;
    }

    private GameObject CreateTitle()
    {
        var rectTransform = windowGO.transform as RectTransform;

        GameObject windowTitleGameObject = new GameObject("TitleDragZone GameObject");
        RectTransform titleRectTransform = windowTitleGameObject.AddComponent<RectTransform>();

        titleRectTransform.SetParent(rectTransform, false);
        /*
        var le = windowTitleGameObject.AddComponent<LayoutElement>();
        le.minHeight = le.preferredHeight = titleHeight;
        le.flexibleHeight = 0;
        */
        //stretch the title
        titleRectTransform.anchorMin = new Vector2(0f, 1f);
        titleRectTransform.anchorMax = Vector2.one;
        titleRectTransform.anchoredPosition = Vector2.zero;
        titleRectTransform.sizeDelta = new Vector2(0, titleHeight);
        titleRectTransform.pivot = new Vector2(0f, 1f);
            
        Text lbl = windowTitleGameObject.AddComponent<Text>();
        lbl.text = windowTitle;
        lbl.color = new Color(0f, 0f, 0f, 1f);
        lbl.fontSize = 14;
        //lbl.font = Resources.Load<Font>("Assets/UI/Fonts/kenvector_future_thin.ttf");
        lbl.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        lbl.alignment = TextAnchor.MiddleCenter;

        windowTitleGameObject.AddComponent<PanelDragger>();

        return windowTitleGameObject;
    }

    private GameObject CreateResizeZone()
    {
        var rectTransform = windowGO.transform as RectTransform;

        GameObject resizeZoneGO = new GameObject("ResizeZone GameObject");
        RectTransform resizeRectTransform = resizeZoneGO.AddComponent<RectTransform>();

        resizeRectTransform.SetParent(rectTransform, false);
        /*
        var le = resizeZoneGO.AddComponent<LayoutElement>();
        le.minHeight = le.preferredHeight = resizeZoneHeight;
        le.flexibleHeight = 0;
        */
        //var lg = resizeZoneGO.AddComponent<HorizontalLayoutGroup>();

        
        resizeRectTransform.anchorMin = new Vector2(1f, 0f);
        resizeRectTransform.anchorMax = new Vector2(1f, 0f);
        resizeRectTransform.anchoredPosition = Vector2.zero;
        resizeRectTransform.sizeDelta = new Vector2(resizeZoneHeight, resizeZoneHeight);
        resizeRectTransform.pivot = new Vector2(0f, 1f);
        resizeRectTransform.localPosition += new Vector3(-resizeZoneHeight, resizeZoneHeight);
        
        Image image = resizeZoneGO.AddComponent<Image>();
        image.sprite = windowDragButtonSprite;
        image.type = Image.Type.Sliced;
        image.color = new Color(1f,1f,1f,0.5f);
        
        var resizerScript = resizeZoneGO.AddComponent<PanelResizer>();

        resizerScript.window = this;
        resizerScript.minSize = new Vector2(minWidth, minHeight);
        resizerScript.maxSize = new Vector2(1000, 1000);

        return resizeZoneGO;
    }

    private GameObject CreateContent()
    {
        var rectTransform = windowGO.transform as RectTransform;

        GameObject windowContentGameObject = new GameObject("ContentZone GameObject");
        RectTransform ContentRectTransform = windowContentGameObject.AddComponent<RectTransform>();

        ContentRectTransform.SetParent(rectTransform, false);

        //var le = windowContentGameObject.AddComponent<LayoutElement>();
        
        //stretch the Content
        ContentRectTransform.anchorMin = Vector2.zero;
        ContentRectTransform.anchorMax = Vector2.one;
        ContentRectTransform.anchoredPosition = Vector2.zero;
        ContentRectTransform.sizeDelta = Vector2.zero;
        
        return windowContentGameObject;
    }

    public void ResizeContent()
    {
        //need to recalc the anchors to keep the content from overlapping over the title and resize corner
        var rectTransform = windowGO.transform as RectTransform;
        var ContentRectTransform = windowContentGO.GetComponent<RectTransform>();

        //var windowWidth = rectTransform.sizeDelta.x;
        var windowHeight = rectTransform.sizeDelta.y;

        float newMinAnchorY = resizeZoneHeight / windowHeight;
        float newMaxAnchorY = 1 - titleHeight / windowHeight;
        
        //stretch the Content
        ContentRectTransform.anchorMin = new Vector2(0f, newMinAnchorY);
        ContentRectTransform.anchorMax = new Vector2(1f, newMaxAnchorY);
        ContentRectTransform.anchoredPosition = Vector2.zero;
        ContentRectTransform.sizeDelta = Vector2.zero;
    }

    public void DisableResize()
    {
        if(windowResizeZoneGO != null)
        {
            windowResizeZoneGO.SetActive(false);
        }
    }

    public void EnableResize()
    {
        if (windowResizeZoneGO != null)
        {
            windowResizeZoneGO.SetActive(true);
        }
        else
        {
            windowResizeZoneGO = CreateResizeZone();
        }
    }
}
