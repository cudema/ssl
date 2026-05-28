using UnityEngine;
using UnityEngine.EventSystems;

public class MoveMap : MonoBehaviour, IDragHandler
{
    [SerializeField]
    Transform cameraTransform;
    [SerializeField]
    Canvas parentCanvas;

    [SerializeField]
    float temp;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 correctedDelta = eventData.delta / parentCanvas.scaleFactor;
        cameraTransform.position -= new Vector3(correctedDelta.x, 0, correctedDelta.y) / temp;
    }

    void OnEnable()
    {
        ResetPosition();
    }

    public void ResetPosition()
    {
        cameraTransform.position = StageManager.instance.node.transform.position;
    }
}
