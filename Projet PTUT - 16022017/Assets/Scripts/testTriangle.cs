using UnityEngine;
using System.Collections;

public class testTriangle : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        // make changes to the Mesh by creating arrays which contain the new values
        mesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0) };
        mesh.colors32 = new Color32[] { new Color32(255, 0, 0, 255), new Color32(255, 0, 0, 255), new Color32(255, 0, 0, 255) };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
        mesh.triangles = new int[] { 0, 1, 2 };

    }
}
