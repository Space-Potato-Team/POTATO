using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AsteroidTool : EditorWindow
{
    GameObject asteroid;

    AsteroidData asteroidData;

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
        steps.Add(new ShrinkWrapMeshGenerateStep());
        steps.Add(new SmoothMeshGenerateStep());

        SetEditorSelectedObject();

        if (asteroid != null)
        {
            asteroidAttractor = asteroid.GetComponent<AsteroidAttractor>();
        }else
        {
                //TODO Find a better way to make it not null
            asteroidAttractor = new AsteroidAttractor();
            asteroidData = new AsteroidData();
        }
    }

    //Update with method to refresh the selection of the user in the editor
    private void Update()
    {
        SetEditorSelectedObject();
    }

    private void OnGUI()
    {

        if (GUILayout.Button("Generate Asteroid"))
        {
            asteroid = new GameObject("Asteroid");
            asteroid.transform.position = new Vector3(0, 0, 0);
            asteroid.AddComponent<MeshFilter>();
            asteroid.AddComponent<MeshRenderer>();
            asteroid.AddComponent<MeshCollider>();
            asteroidData = asteroid.AddComponent<AsteroidData>();

            asteroidAttractor = asteroid.AddComponent<AsteroidAttractor>();
            
            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Cube);
            child.transform.parent = asteroid.transform;
        }

        EditorGUI.BeginDisabledGroup(asteroid == null && asteroidData != null);

        asteroidAttractor.density = EditorGUILayout.Slider(asteroidAttractor!.density, 0, 100);
        asteroidData!.subDivideRecursions = EditorGUILayout.IntField(asteroidData!.subDivideRecursions);
        asteroidData!.smoothRecursions = EditorGUILayout.IntField(asteroidData!.smoothRecursions);

        EditorGUI.EndDisabledGroup();

        // Fold menu for child menu
        isFolded = EditorGUILayout.Foldout(isFolded, "Child object");

        if (isFolded)
        {
            if (GUILayout.Button("Generate Object"))
            {
                CreateChildObject();
            }
        }

        if (GUILayout.Button("Export"))
        {
            // Ask if the user is sure the want to generate the asteroid if the click the yes button then the asteroid can be generated
            if (!EditorUtility.DisplayDialog("Warning", "Generating the asteroid may take some time. Are you sure you want to proceed?", "Cancel", "Ok"))
            {
                    foreach(GenerateStep step in steps)
                    {
                            step.Process(asteroid);
                    }
            }
        }

        //This starts fold out window for all the generation steps

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
