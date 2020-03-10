using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomController : MonoBehaviour
{

    #region constant variable
    private const float SCALE_FACTOR = 12.0f;
    private Vector2 m_old_position1;
    private Vector2 m_old_position2;
    private bool m_is_double_finger = false;
    #endregion

    public Camera m_orthoCamera;
    public int m_curLodLevel = 0;
    public float m_height = 12.0f;
    public float m_orthoSize = 7.0f;
    public Vector3 m_targetPos;

    // Test
    public int TestCurrentLodLevel;

    //void Update()
    //{
    //    float delta = processZoom();
    //    m_height += delta;
    //    m_height = Mathf.Clamp(m_height, GlobalController.CameraMinHeight, GlobalController.CameraMaxHeight);

    //    UpdateCameraPosition();

    //    m_curLodLevel = Mathf.FloorToInt((m_height - GlobalController.CameraHeightLodRangeLow) / GlobalController.LODCameraHeightDelta);
    //    m_curLodLevel = Mathf.Clamp(m_curLodLevel, 0, GlobalController.LODLevelCount);

    //    //float curPercent = (m_height - m_curLodLevel * GlobalController.LODCameraHeightDelta) / GlobalController.LODCameraHeightDelta;
    //    //Debug.Log(string.Format("cur lod level = {0}, cur scale percent = {1}", m_curLodLevel, curPercent));
    //}

    void Update()
    {
        float delta = processZoom();
        float fov = Camera.main.fieldOfView;
        fov += delta;
        fov = Mathf.Clamp(fov, GlobalController.CameraFovMin, GlobalController.CameraFovLodRangeHigh);
        Camera.main.fieldOfView = fov;

        m_curLodLevel = Mathf.FloorToInt((fov - GlobalController.CameraFovLodRangeLow) / GlobalController.CameraFovLodDelta);
        m_curLodLevel = Mathf.Clamp(m_curLodLevel, 0, GlobalController.LODLevelCount);

        //float curPercent = (m_height - m_curLodLevel * GlobalController.LODCameraHeightDelta) / GlobalController.LODCameraHeightDelta;
        //Debug.Log(string.Format("cur lod level = {0}, cur scale percent = {1}", m_curLodLevel, curPercent));
    }

    float processZoom()
    {
        float delta = 0;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
        delta = Input.GetAxis("Mouse ScrollWheel") * SCALE_FACTOR;
#elif UNITY_ANDROID || UNITY_IPHONE || UNITY_IPAD
        if (Input.touchCount == 2)
        {
            // 当从单指或两指以上触摸进入两指触摸的时候,记录一下触摸的位置
            // 保证计算缩放都是从两指手指触碰开始的
            if (!m_is_double_finger)
            {
                m_old_position1 = Input.GetTouch(0).position;
                m_old_position2 = Input.GetTouch(1).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                // 计算出当前两点触摸点的位置
                var tempPosition1 = Input.GetTouch(0).position;
                var tempPosition2 = Input.GetTouch(1).position;

                float currentTouchDistance = Vector2.Distance(tempPosition1, tempPosition2);
                float lastTouchDistance = Vector2.Distance(m_old_position1, m_old_position2);

                //备份上一次触摸点的位置，用于对比
                m_old_position1 = tempPosition1;
                m_old_position2 = tempPosition2;

                delta = (lastTouchDistance - currentTouchDistance) * 0.25f * Time.deltaTime;
            }

            m_is_double_finger = true;
        }
        else
            m_is_double_finger = false;
#endif
        return delta;
    }

    Vector3 updateDeltaVector()
    {
        Camera mc = Camera.main;
        Quaternion qrot = mc.transform.rotation;
        Vector3 erot = qrot.eulerAngles;
        Vector3 vec = Vector3.up * m_height;
        vec = Quaternion.AngleAxis(erot.x - 90, Vector3.right) * vec;
        vec = Quaternion.AngleAxis(erot.y, Vector3.up) * vec;

        return vec;
    }

    [ContextMenu("更新相机高度/正交相机Size")]
    public void UpdateCameraPosition()
    {
        transform.position = m_targetPos + updateDeltaVector();
        float orthoRangeHigh = GlobalController.CameraMinHeight + GlobalController.LODCameraHeightDelta * 3;
        float orthoRangeLow = GlobalController.CameraMinHeight;
        m_orthoSize = Mathf.Lerp(GlobalController.OrthoCameraSizeRangeLow, GlobalController.OrthoCameraSizeRangeHigh, (m_height - orthoRangeLow) / (orthoRangeHigh  - orthoRangeLow));
        m_orthoCamera.orthographicSize = m_orthoSize;
    }

    [ContextMenu("更新测试LodLevel")]
    public void UpdateTestLodLevel()
    {
        m_curLodLevel = TestCurrentLodLevel;
    }
}
