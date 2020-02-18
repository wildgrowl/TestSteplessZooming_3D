using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestProjectionQuad : MonoBehaviour
{
    Element element;
    MeshRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        element = transform.parent.GetComponent<Element>();
        render = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (element)
        {
            if (render)
                render.enabled = element.Visible;
            float size = element.Size * Element.MeshSizeInUnity;
            float height = element.transform.localPosition.y - 0.1f;
            transform.localScale = new Vector3(size, size, size);
            Vector3 parentPosition = element.transform.localPosition;
            //transform.localPosition = new Vector3(parentPosition.x, -height, parentPosition.z);
            transform.position = new Vector3(parentPosition.x, 0.1f, parentPosition.z);
        }
    }
}
