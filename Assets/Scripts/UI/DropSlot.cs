using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    int slotIndex;

    public void OnDrop(PointerEventData eventData)
    {
        // 드래그 중인 오브젝트가 있다면
        if (eventData.pointerDrag != null)
        {
            // 1. 드래그 중인 물체가 있는지 확인
            GameObject draggedObject = eventData.pointerDrag;
            if (draggedObject == null) return;

            DragAndDrop dragItem = draggedObject.GetComponent<DragAndDrop>();
            if (dragItem == null) return;

            // 2. 이 슬롯에 이미 아이템이 있는지 확인 (자식 오브젝트 검사)
            if (transform.childCount > 0)
            {
                // 기존에 있던 아이템을 가져옴
                Transform existingItem = transform.GetChild(0);

                // 기존 아이템을 드래그 시작했던 부모(startParent)에게 보냄
                existingItem.SetParent(dragItem.startParent);
                existingItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                existingItem.GetComponent<DragAndDrop>().index = eventData.pointerDrag.GetComponent<DragAndDrop>().index;
            }

            // 3. 드래그해 온 아이템을 이 슬롯의 자식으로 설정
            draggedObject.transform.SetParent(transform);
            draggedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            InventoryManager.instance.ChangeSlot(eventData.pointerDrag.GetComponent<DragAndDrop>().index, slotIndex);
            eventData.pointerDrag.GetComponent<DragAndDrop>().index = slotIndex;
        }
    }
}