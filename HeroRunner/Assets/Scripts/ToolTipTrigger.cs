using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum ToolTipType
{
    FuelCrate,
    LiveCrate,
    SecretKey
}

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolTipType _type;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        FindObjectOfType<HelpScript>().ShowTypeOfToolTipText(_type);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        FindObjectOfType<HelpScript>().HideToolTip();
    }
}
