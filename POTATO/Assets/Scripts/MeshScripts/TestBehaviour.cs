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

    private void Start()
    {
        GetComponent<Rigidbody>().AddTorque(new Vector3(0.7f, 0.5f, 0.4f));
    }

    public void Test()
    {
        ShrinkWrapMeshGenerateStep step = new ShrinkWrapMeshGenerateStep();
        step.Process(gameObject);
    }

    public void Test2()
    {
        SmoothMeshGenerateStep step = new SmoothMeshGenerateStep();
        step.Process(gameObject);
    }

    public void Test3()
    {
        DetailGenerateStep step = new DetailGenerateStep();
        step.Process(gameObject);
    }

    public void Test4()
    {
        foreach (Transform child in transform)
        {
            Debug.DrawRay(child.GetComponent<Collider>().bounds.max, Vector3.up * 10f, Color.cyan, 10f);
        }
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
        
        if(GUILayout.Button("step 3"))
        {
            script.Test3();
        }
        
        if(GUILayout.Button("test"))
        {
            script.Test4();
        }
    }
}
