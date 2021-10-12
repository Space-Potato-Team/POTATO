using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestBehaviour : MonoBehaviour
{
    public void Test()
    {
        ShrinkWrapMeshGenerateStep step = new ShrinkWrapMeshGenerateStep();
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
        if(GUILayout.Button("Setup"))
        {
            script.Test();
        }
    }
}
