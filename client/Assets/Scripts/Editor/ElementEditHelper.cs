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
            float halfHeight = element.TestUpdateCurrentSize();
            element.transform.position = new Vector3(oldPostion.x, halfHeight, oldPostion.z);
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

    [MenuItem("Custom/添加投影面", false, 1)]
    public static void AddProjectionPlane()
    {
        Element[] elements = GameObject.FindObjectsOfType<Element>();
        foreach (var element in elements)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/ProjectionQuad");
            GameObject plane = GameObject.Instantiate(prefab, element.transform);
        }
    }

    [MenuItem("Custom/删除投影面", false, 1)]
    public static void RemoveProjectionPlane()
    {
        TestProjectionQuad[] planes = GameObject.FindObjectsOfType<TestProjectionQuad>();
        foreach (var plane in planes)
        {
            GameObject.DestroyImmediate(plane.gameObject);
        }
    }
}
