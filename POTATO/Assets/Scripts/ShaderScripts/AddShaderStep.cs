using UnityEditor;
using UnityEngine;

public class AddShaderStep : MonoBehaviour
{
    public void Apply()
    {
        var material = new Material(Shader.Find("Shader Graphs/AsteroidDetailShader"));
        var renderer = GetComponent<Renderer>();

        renderer.material = material;
    }
}

[CustomEditor(typeof(AddShaderStep))]
public class AsteroidShaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var script = (AddShaderStep)target;

        if (GUILayout.Button("Apply"))
        {
            script.Apply();
        }
    }
}