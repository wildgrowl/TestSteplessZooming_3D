using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEvent : MonoBehaviour, IEventSystemHandler, IPointerClickHandler, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, ICancelHandler, ISubmitHandler, IMoveHandler
{

    public delegate void PointerClickEvent(PointerEventData eventData);

    public PointerClickEvent PointerClick;
    public void SetPointerClick(PointerClickEvent pointerEvent)
    {
        this.PointerClick = pointerEvent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.PointerClick != null)
        {
            PointerClick(eventData);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnScroll(PointerEventData eventData)
    {
        
    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        
    }

    public void OnDeselect(BaseEventData eventData)
    {
        
    }

    public void OnCancel(BaseEventData eventData)
    {
        
    }

    public void OnSubmit(BaseEventData eventData)
    {
        
    }

    public void OnMove(AxisEventData eventData)
    {
        
    }

    public static UIEvent Get(GameObject ui)
    {
        var listener = ui.GetComponent<UIEvent>();
        if (listener == null)
        {
            listener = ui.AddComponent<UIEvent>();
        }

        return listener;
    }
}
