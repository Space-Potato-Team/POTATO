using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SocialPlatforms;

public class ShrinkWrapSphere : MonoBehaviour
{
    [SerializeField] public float range = 1;
    [SerializeField, Range(0,4)] private int subdivideRecursions;
    
    public void Setup()
    {
        IcoSphereMesh ico = new IcoSphereMesh();
        ico.InitAsIcosohedron(range);
        ico.Subdivide(subdivideRecursions);
        Mesh mesh = ico.GenerateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void Shrink()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.sharedMesh;

        GetComponent<MeshCollider>().enabled = false;
        
        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        System.Array.Copy(mesh.vertices, vertices, vertices.Length);

        for (int i = 0; i < vertices.Length; i++) {
            Vector3 rayDirection = -mesh.normals[i];
            RaycastHit hit;
            if ( Physics.Raycast( transform.TransformPoint(vertices[i]), rayDirection, out hit, 100f ) ) {
                Debug.DrawRay(transform.TransformPoint(vertices[i]), hit.point - (transform.TransformPoint(vertices[i])), Color.black, 1f);
                if (vertices[i] + transform.position == hit.point)
                {
                    Debug.Log(hit.collider.name);
                    Debug.DrawRay(vertices[i] + transform.position, -rayDirection, Color.red, 100f);
                }
                vertices[i] = transform.InverseTransformPoint(hit.point);
            }
            else {
                Debug.Log(vertices[i] + " is fucked");
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

[CustomEditor(typeof(ShrinkWrapSphere))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        ShrinkWrapSphere shrinkingScript = (ShrinkWrapSphere)target;
        if(GUILayout.Button("Setup"))
        {
            shrinkingScript.Setup();
        }
        if(GUILayout.Button("Shrink"))
        {
            shrinkingScript.Shrink();
        }

        if (GUILayout.Button("stop"))
        {
            shrinkingScript.StopAllCoroutines();
        }
    }
}
