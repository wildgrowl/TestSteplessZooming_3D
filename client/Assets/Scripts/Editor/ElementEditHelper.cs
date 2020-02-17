using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ElementEditHelper
{
    [MenuItem("Custom/调整Y坐标", false, 1)]
    public static void InitElementYPos()
    {
        Element[] elements = GameObject.FindObjectsOfType<Element>();
        foreach (var element in elements)
        {
            Vector3 oldPostion = element.transform.position;
            element.TestUpdateCurrentSize();
            float currentSize = element.CurSize;
            element.transform.position = new Vector3(oldPostion.x, currentSize / 2, oldPostion.z);
        }
    }

    [MenuItem("Custom/显示全部地图元素", false, 1)]
    public static void ShowAllMapElement()
    {
        Element[] elements = GameObject.FindObjectsOfType<Element>();
        foreach (var element in elements)
        {
            element.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
    }
}
