using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelResizer : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Vector2 minSize;
    public Vector2 maxSize;
    public UIWindowHelper window;

    private RectTransform rectTransform;
    private Vector2 currentPointerPosition;
    private Vector2 previousPointerPosition;

    void Awake()
    {
        rectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        rectTransform.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out previousPointerPosition);
    }

    public void OnDrag(PointerEventData data)
    {
        if (rectTransform == null)
            return;

        Vector2 sizeDelta = rectTransform.sizeDelta;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out currentPointerPosition);
        Vector2 resizeValue = currentPointerPosition - previousPointerPosition;

        if(window != null)
        {
            float minHeight, minWidth;
            minHeight = LayoutUtility.GetMinHeight(window.windowContentGO.transform as RectTransform) + window.titleHeight + window.resizeZoneHeight;
            minWidth = LayoutUtility.GetMinWidth(window.windowContentGO.transform as RectTransform);
            minSize = new Vector2(minWidth, minHeight);
        }

        sizeDelta += new Vector2(resizeValue.x, -resizeValue.y);
        sizeDelta = new Vector2(
            Mathf.Clamp(sizeDelta.x, minSize.x, maxSize.x),
            Mathf.Clamp(sizeDelta.y, minSize.y, maxSize.y)
            );

        if (window != null)
            window.SetWindowSize(sizeDelta);
        else
            rectTransform.sizeDelta = sizeDelta;

        previousPointerPosition = currentPointerPosition;
    }
}