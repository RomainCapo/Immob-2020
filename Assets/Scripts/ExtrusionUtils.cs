using UnityEngine;

/// <summary>
/// This class contains utility methods for the extrusion of the 3D plane from the JSON plane.
/// </summary>
public class ExtrusionUtils
{
    private static readonly GameObject apartment;

    static ExtrusionUtils()
    {
        apartment = GameObject.FindGameObjectWithTag("Apartment");
    }

    /// <summary>
    /// Instanciate a GameObject from a prefab, rotate the object if it is on the z-axis, scale the object, scale the texture, add tag, name and layer
    /// </summary>
    /// <param name="prefab">GameObject prefab to instanciate</param>
    /// <param name="startPos">Start position of the object to instanciate</param>
    /// <param name="stopPos">Stop position of the object to instanciate</param>
    /// <param name="height">Height of the object to instanciate</param>
    /// <param name="tag">Tag to object to instanciate</param>
    /// <param name="name">Name of the object to instanciate</param>
    /// <param name="layer">Layer of the object to instanciate</param>
    /// <returns></returns>
    public static GameObject InstanciateComponent(GameObject prefab, Vector3 startPos, Vector3 stopPos, float height, string tag, string name, int layer)
    {
        GameObject extrudedObject = GameObject.Instantiate(prefab) as GameObject;
        Vector3 centerPos = new Vector3(startPos.x + stopPos.x, 0, startPos.y + stopPos.y) / 2;

        float scaleX = Mathf.Abs(startPos.x - stopPos.x);
        float scaleY = Mathf.Abs(startPos.y - stopPos.y);

        float scale = 0;

        if (scaleX == 0)
        {
            scale = scaleY;
            extrudedObject.transform.Rotate(new Vector3(0, 90, 0));
        }
        if (scaleY == 0)
        {
            scale = scaleX;
        }

        extrudedObject.transform.position = centerPos;
        extrudedObject.transform.localScale = new Vector3(scale, height, 0);
        extrudedObject.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(scale, height);
        extrudedObject.transform.parent = apartment.transform;

        extrudedObject.tag = tag;
        extrudedObject.name = name;
        extrudedObject.layer = layer;

        return extrudedObject;
    }

    /// <summary>
    /// Create an edge of a wall
    /// </summary>
    /// <param name="prefab">GameObject prefab to instanciate</param>
    /// <param name="pos">Position of the edge to instanciate</param>
    /// <param name="height">Height of the edge</param>
    /// <param name="width">Width of the edge</param>
    /// <param name="tag">Tag to edge to instanciate</param>
    /// <param name="name">Name of the edge to instanciate</param>
    /// <param name="layer">Layer of the edge to instanciate</param>
    public static void InstanciateEdge(GameObject prefab, Vector3 pos, float height, float width,  string name, string tag = "Wall", int layer=10)
    {
        GameObject edge = ExtrusionUtils.InstanciateComponent(prefab, pos, pos, height, "Wall", name, 10);
        edge.transform.position += new Vector3(0, height / 2, 0);
        edge.transform.localScale += new Vector3(width, 0, width);
        edge.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(width, height);
    }
}
