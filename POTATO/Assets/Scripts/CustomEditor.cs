using UnityEditor;
using UnityEngine;

public class AsteroidTool : EditorWindow
{
    //Test Data
    GameObject asteroid;
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool;
    float density = 1.23f;

    //Bool for foldingmenu
    private List<bool> showDropDown;

    // Add menu item named "Asteroid Tool" to the Window menu
    [MenuItem("Window/Asteroid Tool")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(AsteroidTool));
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        //      asteroid = EditorGUILayout.ObjectField("Asteroid", asteroid, typeof(GameObject), false) as GameObject;

        //      myBool = EditorGUILayout.Toggle("Toggle", myBool);

        //      groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        //      density = EditorGUILayout.Slider("Density", density, 1 , 23);
        //      EditorGUILayout.EndToggleGroup();

        //      if(GUILayout.Button("Generate Asteroid"))
        //      {
        //          //TODO Generate Asteroid
        //      }

        //This starts fold out window

        foreach (isFolded in showDropDown)
        {
            isFolded = EditorGUILayout.Foldout(isFolded, "hoooi");
            if (showDropDown)
                EditorGUILayout.TextField(myString);
        }
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
