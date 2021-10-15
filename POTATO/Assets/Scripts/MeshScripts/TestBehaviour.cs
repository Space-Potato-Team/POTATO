using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestBehaviour : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Transform craterPoint;
    public void Test()
    {
        ShrinkWrapMeshGenerateStep step = new ShrinkWrapMeshGenerateStep();
        step.Process(gameObject);
        Debug.Log(gameObject.GetComponent<MeshFilter>().sharedMesh.triangles.Length);
    }

    public void Test2()
    {
        SmoothMeshGenerateStep step = new SmoothMeshGenerateStep();
        step.Process(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(0.5f,0.5f,0.5f));
    }

    private void OnMouseDown()
    {
        _mesh = GetComponent<MeshFilter>()!.mesh;
        GetComponent<MeshFilter>()!.mesh = 
        CraterCreator.addCraterToMeshOnPosition(_mesh, 
            transform.InverseTransformPoint(craterPoint.position), craterPoint.forward * 1, 0.1f);
    }
}


[CustomEditor(typeof(TestBehaviour))]
public class TestBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        TestBehaviour script = (TestBehaviour)target;
        if(GUILayout.Button("step 1"))
        {
            script.Test();
        }
        
        if(GUILayout.Button("step 2"))
        {
            script.Test2();
        }
    }
}
