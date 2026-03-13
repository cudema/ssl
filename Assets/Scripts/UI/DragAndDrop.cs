using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private Vector2 _originalPosition;

    public Transform startParent { get; private set; }

    public int index;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup()
    {
        _canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        _originalPosition = _rectTransform.anchoredPosition;
        
        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
        
        transform.SetAsLastSibling();
        transform.SetParent(InventoryManager.instance.transform);
        //eventData.pointerDrag.GetComponent<EffectItem>().OnRemoveEffect();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter == null || (!eventData.pointerEnter.CompareTag("Slot") && !eventData.pointerEnter.CompareTag("Item")))
        {
            //_rectTransform.anchoredPosition = _originalPosition;
            transform.SetParent(startParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void SetStartParent(Transform parent)
    {
        startParent = parent;
    }
}
