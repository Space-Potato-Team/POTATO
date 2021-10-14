using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestBehaviour : MonoBehaviour
{

    [SerializeField] private Mesh _mesh;
    public void Test()
    {
        ShrinkWrapMeshGenerateStep step = new ShrinkWrapMeshGenerateStep();
        step.mesh = _mesh;
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
