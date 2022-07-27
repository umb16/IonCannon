using System;
using UnityEngine;
using UnityEngine.EventSystems;

/*
    Any element that needs to display a tooltip should attach this script. 
*/

public class TooltipElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public string TooltipText;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (!String.IsNullOrEmpty(TooltipText))
            TooltipController.Instance.AssignTooltip(TooltipText);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        TooltipController.Instance.UnassignTooltip();
    }

    public void SetText(string text)
    {
        TooltipText = text;
    }

}
