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

/// <summary>
/// 投影网格与unity单位的比例是2:1，
/// 因此9x9的建筑就需要占据4.5x4.5 untiy单位的面积；
/// Sprite的资源原始图宽度固定为：size / 根号2 * 比例，
/// 高度根据图片内容可变，
/// 因此缩放时需要实时根据Sprite大小更新Sprite的高度，
/// 以防止被地板裁切。
/// </summary>

[ExecuteInEditMode]
public class Element : MonoBehaviour
{
    public const float MeshSizeInUnity = 0.5f;
    private const float Sqrt2 = 1.414f;

    public ElementType m_type = ElementType.WorldElement;
    public int m_lodLevel = 0;
    public int m_size = 3;
    public Vector2 m_logicPos = Vector2.zero;
    public float m_scaleMin = 1.0f;
    public float m_scaleMax = 2.0f;
    public int m_vanishLodLevel = -1;
    public bool m_isBuildingPlant = false;
    public bool m_isMainBuilding = false;
    public ScaleAnchor m_scaleAnchor = ScaleAnchor.BottomLeft;

    private float m_currentSize; // unity单位
    private float m_currentSpriteCenterHeight;
    private float m_currentScale = 1.0f;
    private bool m_visible = true;
    private ZoomController m_zoomController;
    private Nation m_myNation = null;

    public float Size
    {
        get { return m_size; }
    }

    public float CurrentSize
    {
        get { return m_currentSize; }
    }

    public float CurrentScale
    {
        get { return m_currentScale; }
    }

    public float SpriteHeight
    {
        get { return m_currentSpriteCenterHeight; }
    }

    public bool Visible
    {
        get { return m_visible; }
    }

    void Start()
    {
        m_currentSize = m_size * MeshSizeInUnity * m_currentScale;
        m_currentSpriteCenterHeight = GetComponentInChildren<SpriteRenderer>().size.y / 2 * m_currentScale;
        m_zoomController = GameObject.FindObjectOfType<ZoomController>();
        if (transform.parent != null)
        {
            m_myNation = transform.parent.GetComponent<Nation>();
        }
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
        if (m_isBuildingPlant && m_myNation != null)
        {
            isVisible = m_myNation.IsAllElementsSettleDown;
            if (m_visible != isVisible)
                gameObject.GetComponentInChildren<SpriteRenderer>().enabled = isVisible;
            m_visible = isVisible;
            return;
        }
        
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
            if (curLodLevel == 0 || m_isMainBuilding)
            {
                updateScaleSize();
            }
        }

        if (m_visible != isVisible)
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = isVisible;
        m_visible = isVisible;
    }

    void updateScaleSize()
    {
        float percent = 1.0f;
        int curLodLevel = m_zoomController.m_curLodLevel;
        float curFov = Camera.main.fieldOfView;

        float minFov;
        float maxFov;
        if (m_vanishLodLevel > 0)
        {
            minFov = GlobalController.CameraFovLodRangeLow + (m_vanishLodLevel < 0 ? 0 : m_vanishLodLevel) * GlobalController.CameraFovLodDelta;
        }
        else
        {
            minFov = GlobalController.CameraFovMin;
        }
        maxFov = GlobalController.CameraFovLodRangeLow + m_lodLevel * GlobalController.CameraFovLodDelta;

        percent = (curFov - minFov) / (maxFov - minFov);

        m_currentScale = Mathf.Lerp(m_scaleMin, m_scaleMax, percent);
        transform.localScale = new Vector3(m_currentScale, m_currentScale, m_currentScale);

        m_currentSize = m_size * m_currentScale * MeshSizeInUnity;
        m_currentSpriteCenterHeight = GetComponentInChildren<SpriteRenderer>().size.y / 2 * m_currentScale;

        Vector3 oldPosition = transform.position;
        transform.position = new Vector3(oldPosition.x, m_currentSpriteCenterHeight, oldPosition.z);

        // 更新Sprite Z坐标
        if (m_type == ElementType.BuildingElement)
        {
            //Transform orthoCamera = GameObject.FindObjectOfType<ZoomController>().transform.GetChild(1);
            //float rotation = orthoCamera.rotation.eulerAngles.x;
            //float deltaZ = m_currentSpriteCenterHeight / Mathf.Tan(rotation) / 2;
            //GetComponentInChildren<SpriteRenderer>().transform.localPosition = new Vector3(0, 0, -deltaZ);
            float deltaZ = m_size * MeshSizeInUnity * Mathf.Sin(45 * Mathf.Deg2Rad) / 1;
            GetComponentInChildren<SpriteRenderer>().transform.localPosition = new Vector3(0, 0, -deltaZ);
        }
    }

    public void HideByAreaOverlap()
    {
        if (m_visible)
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            m_visible = false;
        }
    }

    public float TestUpdateCurrentSize()
    {
        m_currentSize = m_size * MeshSizeInUnity * m_currentScale;
        m_currentSpriteCenterHeight = GetComponentInChildren<SpriteRenderer>().size.y / 2 * m_currentScale;

        return m_currentSpriteCenterHeight;
    }
}
