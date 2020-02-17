using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ScaleAnchor
{
    BottomLeft = 0,
    BottomRight = 1,
    TopRight = 2,
    TopLeft = 3
}

public enum ElementType
{
    WorldElement = 0,
    BuildingElement = 1
}

[ExecuteInEditMode]
public class Element : MonoBehaviour
{
    public const float MeshSizeInUnity = 0.2f;

    public ElementType m_type = ElementType.WorldElement;
    public int m_lodLevel = 0;
    public int m_size = 3;
    public Vector2 m_logicPos = Vector2.zero;
    public float m_scaleMin = 1.0f;
    public float m_scaleMax = 2.0f;
    public int m_vanishLodLevel = -1;
    public ScaleAnchor m_scaleAnchor = ScaleAnchor.BottomLeft;

    private float m_currentSize; // unity单位
    private float m_currentScale = 1.0f;
    private bool m_visible = true;
    private ZoomController m_zoomController;

    public float CurSize
    {
        get { return m_currentSize; }
    }

    void Start()
    {
        m_currentSize = m_size * MeshSizeInUnity * m_currentScale;
        m_zoomController = GameObject.FindObjectOfType<ZoomController>();
    }

    void Update()
    {
        if (m_zoomController == null)
            m_zoomController = GameObject.FindObjectOfType<ZoomController>();
        CheckVisibleAndScale();
    }

    void CheckVisibleAndScale()
    {
        bool isVisible = false;
        int curLodLevel = m_zoomController.m_curLodLevel;
        if (m_lodLevel < curLodLevel)
        {
            if (m_visible)
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
            m_visible = false;
            return;
        }

        if (m_vanishLodLevel > 0 && m_vanishLodLevel >= curLodLevel)
        {
            if (m_visible)
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
            m_visible = false;
            return;
        }

        isVisible = true;
        
        if (m_type == ElementType.WorldElement)
        {
            if (curLodLevel > 0)
            {
                updateScaleSize();
            }
        }
        else
        {
            if (curLodLevel == 0)
            {
                updateScaleSize();

                // TODO. Update visible by checking area fighting.
            }
        }

        if (m_visible != isVisible)
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = isVisible;
        m_visible = isVisible;
        //gameObject.SetActive(isVisible);
    }

    void updateScaleSize()
    {
        float percent = 1.0f;
        int curLodLevel = m_zoomController.m_curLodLevel;
        float curHeight = m_zoomController.m_height;

        float minHeight;
        float maxHeight;
        if (m_vanishLodLevel > 0)
        {
            minHeight = GlobalController.CameraHeightLodRangeLow + (m_vanishLodLevel < 0 ? 0 : m_vanishLodLevel) * GlobalController.LODCameraHeightDelta;
        }
        else
        {
            minHeight = GlobalController.CameraMinHeight;
        }
        maxHeight = GlobalController.CameraHeightLodRangeLow + m_lodLevel * GlobalController.LODCameraHeightDelta;

        percent = (curHeight - minHeight) / (maxHeight - minHeight);

        m_currentScale = Mathf.Lerp(m_scaleMin, m_scaleMax, percent);
        transform.localScale = new Vector3(m_currentScale, m_currentScale, 1.0f);

        m_currentSize = m_size * m_currentScale * MeshSizeInUnity;

        Vector3 oldPosition = transform.position;
        transform.position = new Vector3(oldPosition.x, m_currentSize / 2, oldPosition.z);
    }

    public void TestUpdateCurrentSize()
    {
        m_currentSize = m_size * MeshSizeInUnity * m_currentScale;
    }
}
