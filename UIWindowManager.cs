using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIWindowManager : MonoBehaviour
{
    DefaultControls.Resources defaultResources = new DefaultControls.Resources();
    public Sprite bgSprite, checkmarkSprite, dropdownSprite, inputFieldSprite, knobSprite, maskSprite, standardSprite;
    public Font defaultFont;
    public GameObject canvasGO;

    private bool firstUpdate = true;

    // Helper function that returns a Canvas GameObject; preferably a parent of the selection, or other existing Canvas.
    static public GameObject GetOrCreateCanvasGameObject()
    {
        // No canvas in selection or its parents? Then use just any canvas..
        var canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
        if (canvas != null && canvas.gameObject.activeInHierarchy)
            return canvas.gameObject;

        // No canvas in the scene at all? Then create a new one.
        return CreateNewUI();
    }

    // Helper methods

    static public GameObject CreateNewUI()
    {
        // Root for the UI
        var root = new GameObject("Canvas");
        root.layer = 1;
        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        root.AddComponent<CanvasScaler>();
        root.AddComponent<GraphicRaycaster>();
        
        // if there is no event system add one...
        CreateEventSystem(null);
        return root;
    }

    private static void CreateEventSystem(GameObject parent)
    {
        var esys = Object.FindObjectOfType<EventSystem>();
        if (esys == null)
        {
            var eventSystem = new GameObject("EventSystem");
            if (parent != null)
                eventSystem.transform.SetParent(parent.transform, false);

            esys = eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }

    void Start ()
    {
        canvasGO = GetOrCreateCanvasGameObject();
        CreateEventSystem(canvasGO);

        /*var newPanel2 = new UIWindowHelper(canvasGO);
        newPanel2.BackgroundSprite = bgSprite;
        newPanel2.Position = new Vector3(100f, 100f);
        newPanel2.ResizeCornerSprite = standardSprite;
        */

        var newPanel3 = new UIWindowHelper(canvasGO);
        newPanel3.WindowTitle = "Testing Title";
        newPanel3.TitleFont = defaultFont;
        newPanel3.TitleFontSize = 16;
        newPanel3.BackgroundSprite = bgSprite;
        newPanel3.WindowSize = new Vector2(800, 600);
        newPanel3.Position = new Vector3(100f, 100f);

        var lg = newPanel3.windowContentGO.AddComponent<VerticalLayoutGroup>();
        lg.padding = new RectOffset(5, 5, 3, 3);
        lg.spacing = 1;
        
        for (int i = 0; i < 10; i++)
        {
            var newText = DefaultControls.CreateText(defaultResources);
            newText.transform.SetParent(newPanel3.windowContentGO.transform, false);
            var t = newText.GetComponent<Text>();
            t.font = defaultFont;
            t.fontSize = 12;
            t.text = "New Text Element #" + i;
            print("Creating element " + i);

            var le = newText.AddComponent<LayoutElement>();
            le.minHeight = 20;
            le.preferredHeight = 22;
            le.minWidth = t.preferredWidth;
        }
        
    }
	
	void Update ()
    {
	    if(firstUpdate)
        {
            var newPanel3 = GameObject.Find("ContentZone GameObject");

            print("First Update");
            print("VLG min height:" + LayoutUtility.GetMinHeight((RectTransform)newPanel3.transform));
            print("VLG pref height:" + LayoutUtility.GetPreferredHeight((RectTransform)newPanel3.transform));

            print("VLG min width:" + LayoutUtility.GetMinWidth((RectTransform)newPanel3.transform));
            print("VLG pref width:" + LayoutUtility.GetPreferredWidth((RectTransform)newPanel3.transform));


            //firstUpdate = false;
        }
	}
}
