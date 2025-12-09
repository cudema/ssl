using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatPanel : MonoBehaviour, IPointerClickHandler
{
    AdderStatData data;

    [SerializeField]
    Image image;
    [SerializeField]
    Text effectName;
    [SerializeField]
    Text tooltip;
    [SerializeField]
    StatAdder statAdder;

    public void OnPointerClick(PointerEventData eventData)
    {
        statAdder.AddStat(data.stat);
    }

    public void Setup(AdderStatData setData)
    {
        data = setData;
        image.sprite = data.icon;
        effectName.text = data.stat.ToString();
        tooltip.text = data.tooltip;
    }
}
