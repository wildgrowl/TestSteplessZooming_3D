using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneClickBoardUI : MonoBehaviour
{
    public const float SCALE_FACTOR = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        UIEvent.Get(gameObject).SetPointerClick(OnBGBoardClick);
        UIDragEvent.Get(gameObject).SetDrag(OnBGBoardDrag);
    }

    public void OnBGBoardClick(PointerEventData eventData)
    {

    }

    public void OnBGBoardDrag(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            Vector3 delta = eventData.delta * SCALE_FACTOR;
            Transform cameraCarrier = Camera.main.transform.parent;
            //ZoomController zoom = cameraCarrier.GetComponent<ZoomController>();
            //Vector3 oldTargetPos = zoom.m_targetPos;
            //zoom.m_targetPos = oldTargetPos - new Vector3(delta.x, 0, delta.y);
            Vector3 oldPosition = cameraCarrier.transform.position;
            cameraCarrier.transform.position = oldPosition + new Vector3(delta.x, 0, delta.y);
        }
    }
}
