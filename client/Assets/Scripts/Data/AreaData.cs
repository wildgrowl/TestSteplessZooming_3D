using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockData
{
    public int id;
    public Vector2Int position;

    public List<ElementData> elements;
    public List<CityData> cities;
}

[Serializable]
public class CityData
{
    public int id;
    public Vector2Int position;

    public string profile;
    public int mainBuilding;
    public List<ElementData> elements;
}

[Serializable]
public class ElementData
{
    public int id;
    public Vector2Int position;
    public Vector2Int offset;

    public string prefab;
}

public class AreaData : ScriptableObject
{
    public int id;
    public Vector2Int position;
    public List<BlockData> blockDatas;
}
