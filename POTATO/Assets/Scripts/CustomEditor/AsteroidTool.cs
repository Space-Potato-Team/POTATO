using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AsteroidTool : EditorWindow
{
    GameObject asteroid;

    AsteroidAttractor asteroidAttractor;

    // Bool that keeps track if the child window is folded out or in
    bool isFolded = false;

    private List<GenerateStep> steps;

    // Add menu item named "Asteroid Tool" to the Window menu
    [MenuItem("Window/Asteroid Tool")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(AsteroidTool));
    }

    // When the Asteroid Tool window is opened initialize all the generation steps and get the selected object 
    public void Awake()
    {
        steps = new List<GenerateStep>();
        steps.Add(new Step());
        steps.Add(new Step());

        SetEditorSelectedObject();

        if (asteroid != null)
        {
            asteroidAttractor = asteroid.GetComponent<AsteroidAttractor>();
        }else
        {
                //TODO Find a better way to make it not null
            asteroidAttractor = new AsteroidAttractor();
        }
    }

    //Update with method to refresh the selection of the user in the editor
    private void Update()
    {
        SetEditorSelectedObject();
    }

    private void OnGUI()
    {
        EditorGUI.BeginDisabledGroup(asteroid == null);

        asteroidAttractor.density = EditorGUILayout.Slider(asteroidAttractor.density, 0, 100);

        EditorGUI.EndDisabledGroup();

        if (GUILayout.Button("Generate Object"))
        {
            asteroid = new GameObject("Asteroid");
            asteroid.transform.position = new Vector3(0, 0, 0);
            asteroidAttractor = asteroid.AddComponent<AsteroidAttractor>();
        }

        // Fold menu for child menu
        isFolded = EditorGUILayout.Foldout(isFolded, "Child object");

        if (isFolded)
        {
            if (GUILayout.Button("Generate Object"))
            {
                CreateChildObject();
            }
        }

        if (GUILayout.Button("Generate Asteroid"))
        {
            // Ask if the user is sure the want to generate the asteroid if the click the yes button then the asteroid can be generated
            if (EditorUtility.DisplayDialog("Warning", "Generating the asteroid may take some time. Are you sure you want to proceed?", "Cancel", "Ok"))
            {
                //TODO Generate Asteroid 
            }
        }

        //This starts fold out window for all the generation steps
        foreach (GenerateStep step in steps)
        {
            step.AddGUI();
        }
    }

    // Method for creating a child object for the asteroid
    private void CreateChildObject()
    {

        if (asteroid != null)
        {
            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Cube);

            //Check if asteroid already has a child so the next child is placed properly 
            if (asteroid.transform.childCount > 0)
            {
                Transform lastChild = asteroid.transform.GetChild(asteroid.transform.childCount - 1);
                child.transform.position = lastChild.position + Vector3.back;
            }
            else
            {
                child.transform.position = asteroid.transform.position + Vector3.back;
            }

            child.transform.parent = asteroid.transform;
        }
        else
        {
            // If the asteroid object is empty show a dialog option that there must be one selected.
            EditorUtility.DisplayDialog("No asteroid selected in the editor", "Please select an asteroid first", "Cancel", "Ok");
        }
    }

    private void SetEditorSelectedObject()
    {
        GameObject selectedObject = Selection.activeGameObject;

        // Check if an object is selected in the editor
        if (selectedObject != null)
        {
            if (selectedObject.name == "Asteroid")
            {
                asteroid = selectedObject;
            }
        }
    }


    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}

public class Step : GenerateStep
{
    //This value has to static otherwise it does not save
    static private float density;

    override public GameObject Process(GameObject gameObject)
    {
        return null;
    }

    bool isFolded = true;
    override public void AddGUI()
    {

        isFolded = EditorGUILayout.Foldout(isFolded, "Input step name here");

        if (isFolded)
        {
            density = EditorGUILayout.Slider(density, 0, 100);
        }

    }
}
