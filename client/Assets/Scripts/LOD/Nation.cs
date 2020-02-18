using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nation : MonoBehaviour
{
    private Element[] m_elements;
    private int m_elementsCount;
    private int m_noScaleElementsCount;
    private List<Element> m_hideElementsBuffer;

    public bool IsAllElementsSettleDown
    {
        get { return m_noScaleElementsCount < 1; }
    }

    void Start()
    {
        m_elements = transform.GetComponentsInChildren<Element>();
        m_elementsCount = m_elements.Length;
        m_hideElementsBuffer = new List<Element>();
    }

    void Update()
    {
        updateNoScaleElements();
    }

    void LateUpdate()
    {
        updateProjectionAreaFighting();
    }

    void updateNoScaleElements()
    {
        int count = 0;
        foreach (var element in m_elements)
        {
            if (element.CurrentScale < 1.01f)
            {
                count++;
            }
        }
        m_noScaleElementsCount = m_elementsCount - count;
    }
    
    void updateProjectionAreaFighting()
    {
        m_hideElementsBuffer.Clear();
        foreach (var outterElement in m_elements)
        {
            if (m_hideElementsBuffer.Contains(outterElement))
                continue;
            if (!outterElement.Visible)
                continue;
            foreach (var innerElement in m_elements)
            {
                if (outterElement == innerElement)
                    continue;
                if (!innerElement.Visible)
                    continue;
                if (isContain(outterElement, innerElement))
                {
                    Element element2Hide = outterElement.CurrentSize < innerElement.CurrentSize ? outterElement : innerElement;
                    element2Hide.HideByAreaOverlap();
                }
            }
        }
    }

    /// <summary>
    /// 简化的分离轴算法
    /// 比较x,z坐标，若均有重叠则矩形相交
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    bool isContain(Element lhs, Element rhs)
    {
        Vector3 lhsPosition = lhs.transform.position;
        Vector3 rhsPosition = rhs.transform.position;
        float lhsHalfSize = lhs.CurrentSize / 2;
        float rhsHalfSize = rhs.CurrentSize / 2;
        float lhsXMin, lhsXMax, lhsZMin, lhsZMax;
        float rhsXMin, rhsXMax, rhsZMin, rhsZMax;
        lhsXMin = lhsPosition.x - lhsHalfSize;
        lhsXMax = lhsPosition.x + lhsHalfSize;
        lhsZMin = lhsPosition.z - lhsHalfSize;
        lhsZMax = lhsPosition.z + lhsHalfSize;
        rhsXMin = rhsPosition.x - rhsHalfSize;
        rhsXMax = rhsPosition.x + rhsHalfSize;
        rhsZMin = rhsPosition.z - rhsHalfSize;
        rhsZMax = rhsPosition.z + rhsHalfSize;

        if (!isOverlap(lhsXMin, lhsXMax, rhsXMin, rhsXMax))
            return false;
        if (!isOverlap(lhsZMin, lhsZMax, rhsZMin, rhsZMax))
            return false;

        return true;
    }

    bool isOverlap(float lhsRangeLow, float lhsRangeHigh, float rhsRangeLow, float rhsRangeHigh)
    {
        if ((rhsRangeLow < lhsRangeHigh && rhsRangeLow > lhsRangeLow) ||
            (rhsRangeHigh < lhsRangeHigh && rhsRangeHigh > lhsRangeLow))
            return true;
        return false;
    }
}
