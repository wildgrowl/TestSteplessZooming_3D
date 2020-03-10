using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class EditSceneExporter
{
    const int MAP_SIZE = 12000;
    const int BLOCK_SIZE = 480;
    const int COORD_SIZE = 10;
    const float CITY_MESH_SIZE = 0.5f;


    [MenuItem("Custom/ExportScene", false, 1)]
    public static void Execute()
    {
        ScriptableObject so = GenerateAreaData();
        TraverseMap(so);
        SaveAreaData(so);
    }

    private static ScriptableObject GenerateAreaData()
    {
        ScriptableObject so = ScriptableObjectUtility.CreateScriptableObject(typeof(AreaData)) as AreaData;
        return so;
    }

    private static void TraverseMap(ScriptableObject so)
    {
        AreaData data = so as AreaData;
        data.id = 1712;
        data.name = "1712";
        data.position = new Vector2Int(0, 0);
        data.blockDatas = new List<BlockData>();

        int blockCount = MAP_SIZE / BLOCK_SIZE;

        for (int i = 0; i < blockCount; i++)
        {
            int y = BLOCK_SIZE / 2 + i * BLOCK_SIZE - MAP_SIZE / 2;
            for (int j = 0; j < blockCount; j++)
            {
                BlockData blockData = new BlockData();
                blockData.id = i * blockCount + j;
                int x = BLOCK_SIZE / 2 + j * BLOCK_SIZE - MAP_SIZE / 2;
                blockData.position = new Vector2Int(x, y);
                blockData.cities = new List<CityData>();
                blockData.elements = new List<ElementData>();

                data.blockDatas.Add(blockData);
            }
        }

        int elemCount = 0;
        int cityCount = 1;

        City[] cities = GameObject.FindObjectsOfType<City>();
        foreach (var city in cities)
        {
            int cityElementCount = 0;
            Vector3 position = city.transform.localPosition * 2 + new Vector3(MAP_SIZE / 2, 0, MAP_SIZE / 2);
            int blockX = Mathf.FloorToInt(position.x / BLOCK_SIZE);
            int blockY = Mathf.FloorToInt(position.z / BLOCK_SIZE);
            int blockIndex = blockX * blockCount + blockY;

            CityData cityData = new CityData();
            cityData.id = cityCount++;
            int cityPositionX = Mathf.FloorToInt(position.x / COORD_SIZE);
            int cityPositionY = Mathf.FloorToInt(position.z / COORD_SIZE);
            cityData.position = new Vector2Int(cityPositionX, cityPositionY);
            cityData.profile = string.Empty;
            cityData.mainBuilding = -1;
            cityData.elements = new List<ElementData>();

            Element[] childElements = city.GetComponentsInChildren<Element>();
            foreach (var elem in childElements)
            {
                ElementData elementData = new ElementData();
                elementData.id = cityData.id * 1000 + cityElementCount++;
                elementData.position = cityData.position;
                Vector3 elemPosition = elem.transform.localPosition;
                int offsetX = Mathf.FloorToInt(elemPosition.x / CITY_MESH_SIZE);
                int offsetY = Mathf.FloorToInt(elemPosition.z / CITY_MESH_SIZE);
                elementData.offset = new Vector2Int(offsetX, offsetY);
                string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(elem);
                prefabPath = prefabPath.Replace(".prefab", "");
                prefabPath = prefabPath.Replace("Assets/Resources/", "");
                elementData.prefab = prefabPath;

                ElementType type = elem.m_type;
                if (type == ElementType.WorldNormal)
                {
                    Debug.LogError("World Element In City Error!");
                    continue;
                }
                else if (type == ElementType.CityMain)
                {
                    cityData.mainBuilding = elementData.id;
                }
                cityData.elements.Add(elementData);
            }
            data.blockDatas[blockIndex].cities.Add(cityData);
        }

        Element[] elements = GameObject.FindObjectsOfType<Element>();
        foreach (var elem in elements)
        {
            Vector3 position = elem.transform.localPosition * 2 + new Vector3(MAP_SIZE / 2, 0, MAP_SIZE / 2);
            int blockX = Mathf.FloorToInt(position.x / BLOCK_SIZE);
            int blockY = Mathf.FloorToInt(position.z / BLOCK_SIZE);
            int blockIndex = blockX * blockCount + blockY;

            bool isWorldElement = elem.m_type == ElementType.WorldNormal;

            if (isWorldElement)
            {
                ElementData elementData = new ElementData();
                elementData.id = elemCount++;
                elementData.position = data.blockDatas[blockIndex].position;
                int offsetX = Mathf.FloorToInt(elem.transform.localPosition.x / CITY_MESH_SIZE);
                int offsetY = Mathf.FloorToInt(elem.transform.localPosition.z / CITY_MESH_SIZE);
                elementData.offset = new Vector2Int(offsetX, offsetY);
                string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(elem);
                prefabPath = prefabPath.Replace(".prefab", "");
                prefabPath = prefabPath.Replace("Assets/Resources/", "");
                elementData.prefab = prefabPath;
                data.blockDatas[blockIndex].elements.Add(elementData);
            }
            else
            {
                continue;
            }
        }
    }

    private static void SaveAreaData(ScriptableObject so)
    {
        string path = "Assets/Resources/SceneData/" + "scene_data_1712.asset";
        //string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path);
        //AssetDatabase.CreateAsset(so, assetPathAndName);
        AssetDatabase.CreateAsset(so, path);
    }
}
