using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoadScene : MonoBehaviour
{
    const float CITY_MESH_SIZE = 0.5f;

    public AreaData m_sceneData;

    void Start()
    {
        GameObject elementRoot = new GameObject();
        elementRoot.name = "element_root";
        foreach (var block in m_sceneData.blockDatas)
        {
            GameObject newBlock = new GameObject();
            newBlock.name = "block_" + block.id;

            foreach (var city in block.cities)
            {
                GameObject newCity = new GameObject();
                newCity.name = "city_" + city.id;

                foreach (var element in city.elements)
                {
                    GameObject prefab = Resources.Load<GameObject>(element.prefab);
                    GameObject newElement = GameObject.Instantiate<GameObject>(prefab);
                    newElement.name = "element_" + element.id;
                    newElement.transform.SetParent(newCity.transform);
                    newElement.transform.localPosition = new Vector3(element.offset.x * CITY_MESH_SIZE, 0, element.offset.y * CITY_MESH_SIZE);
                    
                }
                newCity.transform.SetParent(newBlock.transform);
            }

            foreach (var element in block.elements)
            {
                GameObject prefab = Resources.Load<GameObject>(element.prefab);
                GameObject newElement = GameObject.Instantiate<GameObject>(prefab);
                newElement.name = "element_" + element.id;
                newElement.transform.SetParent(newBlock.transform);
                newElement.transform.localPosition = new Vector3(element.offset.x * CITY_MESH_SIZE, 0, element.offset.y * CITY_MESH_SIZE);
            }

            newBlock.transform.SetParent(elementRoot.transform);
            newBlock.transform.position = new Vector3(block.position.x, 0, block.position.y);
        }
    }
}
