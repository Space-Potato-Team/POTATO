using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkWrapSphere : MonoBehaviour {

    void Start() {
        Shrink();
    }

    void Shrink()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;

        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        System.Array.Copy(mesh.vertices, vertices, vertices.Length);

        for (int i = 0; i < vertices.Length; i++) {
            Vector3 rayDirection = -mesh.normals[i];
            Debug.DrawRay(vertices[i], rayDirection, Color.black, 5f);
            RaycastHit hit;
            if ( Physics.Raycast( vertices[i], rayDirection, out hit, 100f ) ) {
                Debug.Log("hit");
                vertices[i] = hit.point * 2f;
            }
            else {
                Debug.Log("bad");
                vertices[i] = Vector3.zero;
            }
        }

        mesh.vertices = vertices;

        Debug.Log("Done. Vertices count " + vertices.Length);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        
        transform.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
