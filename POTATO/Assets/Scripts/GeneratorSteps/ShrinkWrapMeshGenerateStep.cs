using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShrinkWrapMeshGenerateStep : GenerateStep
{
    private int subdivideRecursions = 5;
    private IndexFormat indexFormat = IndexFormat.UInt32;
    private string meshName = "Asteroid Mesh";
    private string shrinkObjectName = "ShrinkObject";
    
    public override GameObject Process(GameObject gameObject)
    {
        gameObject.GetComponent<MeshFilter>()!.mesh = null;
        gameObject.GetComponent<MeshCollider>()!.sharedMesh = null;

        Mesh mesh = GetIcoSphereMesh();
        mesh.name = meshName;
        mesh.indexFormat = indexFormat;

        GameObject shrinkObject = AddShrinkGameObject(mesh, gameObject.transform);
        mesh = Shrink(shrinkObject, gameObject);
        
        gameObject.GetComponent<MeshFilter>()!.mesh = mesh;
        gameObject.GetComponent<MeshCollider>()!.sharedMesh = mesh;
        
        DisablePrimitiveShapes(gameObject);
        return gameObject;
    }

    public override void AddGUI()
    {
        
    }
    
    private Mesh GetIcoSphereMesh()
    {
        IcoSphereMesh ico = new IcoSphereMesh();
        ico.InitAsIcosohedron();
        ico.Subdivide(subdivideRecursions);
        return ico.GenerateMesh(indexFormat);
    }

    private GameObject AddShrinkGameObject(Mesh mesh, Transform parent)
    {
        GameObject shrinkObject = new GameObject(shrinkObjectName);
        shrinkObject.transform.parent = parent;
        shrinkObject.transform.position = parent.position;
        shrinkObject.transform.localScale = new Vector3(10, 10, 10);
        shrinkObject.AddComponent<MeshFilter>()!.mesh = mesh;
        return shrinkObject;
    }

    private Mesh Shrink(GameObject shrinkObject, GameObject parent)
    {
        MeshFilter meshFilter = shrinkObject.GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.sharedMesh;
        Transform transform = shrinkObject.transform;
        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        
        System.Array.Copy(mesh.vertices, vertices, vertices.Length);

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = ShrinkVertex(vertices[i], mesh.normals[i], transform, parent.transform);
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        
        Debug.Log("Done. Vertices count " + vertices.Length);
        Object.DestroyImmediate(shrinkObject);
        
        return mesh;
    }

    private Vector3 ShrinkVertex(Vector3 vertex, Vector3 normal, Transform transform, Transform parent)
    {
        Vector3 rayDirection = -normal;
        RaycastHit hit;

        if ( Physics.Raycast( transform.TransformPoint(vertex), rayDirection, out hit, Vector3.Distance(transform.TransformPoint(vertex), transform.position) ) ) {
            Debug.DrawRay(transform.TransformPoint(vertex), hit.point - transform.TransformPoint(vertex), Color.black, 2f);
            return parent.InverseTransformPoint(hit.point);
        }
        Debug.Log("oops");
        return Vector3.zero;
    }

    private void DisablePrimitiveShapes(GameObject gameObject)
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
