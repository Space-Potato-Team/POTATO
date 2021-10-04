using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkWrapSphere : MonoBehaviour {
    
    

    void Start()
    {
        Planet p = new Planet();
        p.InitAsIcosohedron();
        p.Subdivide(4);

        Mesh mesh = p.GenerateMesh();
        
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        Shrink();
    }

    void Shrink()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;

        GetComponent<MeshCollider>().enabled = false;
        
        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        System.Array.Copy(mesh.vertices, vertices, vertices.Length);
        
        for (int i = 0; i < vertices.Length; i++) {
            Vector3 rayDirection = -mesh.normals[i];
            RaycastHit hit;
            if ( Physics.Raycast( vertices[i] + transform.position, rayDirection, out hit, 100f ) ) {
                Debug.Log("hit");
                Debug.DrawRay(vertices[i] + transform.position, hit.point - (vertices[i] + transform.position), Color.black, 100f);
                if (vertices[i] + transform.position == hit.point)
                {
                    Debug.Log(hit.collider.name);
                    Debug.DrawRay(vertices[i] + transform.position, -rayDirection, Color.red, 100f);
                }
                
                vertices[i] = hit.point - transform.position;
                
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
        transform.GetComponent<MeshRenderer>().enabled = true;
        transform.GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshCollider>().enabled = true;
    }
}
