using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDragEvent : MonoBehaviour, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void PointerEvent(PointerEventData eventData);

    public delegate void PointerClickEvent(PointerEventData eventData);

    public PointerEvent BeginDrag;
    public void SetBeginDrag(PointerEvent pointerEvent)
    {
        this.BeginDrag = pointerEvent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (BeginDrag != null)
            BeginDrag(eventData);
    }

    public PointerEvent Drag;
    public void SetDrag(PointerEvent pointerEvent)
    {
        this.Drag = pointerEvent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Drag != null)
            Drag(eventData);
    }

    public PointerEvent EndDrag;
    public void SetEndDrag(PointerEvent pointerEvent)
    {
        this.EndDrag = pointerEvent;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (EndDrag != null)
            EndDrag(eventData);
    }

    void OnDestroy()
    {
        BeginDrag = null;
        Drag = null;
        EndDrag = null;
    }

    public static UIDragEvent Get(GameObject ui)
    {
        var listener = ui.GetComponent<UIDragEvent>();
        if (listener == null)
            listener = ui.AddComponent<UIDragEvent>();

        return listener;
    }

}