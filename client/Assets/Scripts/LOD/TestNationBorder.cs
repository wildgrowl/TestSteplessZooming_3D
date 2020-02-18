using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNationBorder : MonoBehaviour
{
    public int m_lodLevel = 4;
    MeshRenderer m_render;
    ZoomController m_controller;

    // Start is called before the first frame update
    void Start()
    {
        m_render = GetComponent<MeshRenderer>();
        m_controller = GameObject.FindObjectOfType<ZoomController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_render && m_controller)
        {
            m_render.enabled = m_lodLevel >= m_controller.m_curLodLevel;
        }
    }
}
