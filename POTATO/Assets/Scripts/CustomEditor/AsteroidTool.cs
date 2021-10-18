using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class AsteroidTool : EditorWindow
{
    private int CHILD_OBJECT = 0, MESH_SETTINGS = 1, SHADER_SETTINGS = 2, CRATER_SETTINGS = 3;

    GameObject asteroid;

    AsteroidData asteroidData;

    AsteroidAttractor asteroidAttractor;

    // Bool that keeps track if the child window is folded out or in
    bool[] foldList;

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
        steps.Add(new DetailShaderGenerateStep());

        SetEditorSelectedObject();

        if (asteroid != null)
        {
            asteroidAttractor = asteroid.GetComponent<AsteroidAttractor>();
            asteroidData = asteroid.GetComponent<AsteroidData>();
        }

        foldList = new bool[] { false, false, false, false };
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

            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            child.transform.parent = asteroid.transform;
        }

        //Check if asteroid is null if it is then set placeholders
        if (asteroid != null)
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(asteroidData == null);

            asteroidData!.asteroidDensity = EditorGUILayout.Slider("Asteroid Density:", asteroidData!.asteroidDensity, 0, 100);

            EditorGUI.EndDisabledGroup();

            // Fold menu for child menu
            foldList[CHILD_OBJECT] = EditorGUILayout.Foldout(foldList[CHILD_OBJECT], "Add Object");

            if (foldList[CHILD_OBJECT])
            {
                if (GUILayout.Button("Add Cube"))
                {
                    CreateChildObject(PrimitiveType.Cube);
                }

                if (GUILayout.Button("Add Sphere"))
                {
                    CreateChildObject(PrimitiveType.Sphere);
                }

                if (GUILayout.Button("Add Cylinder"))
                {
                    CreateChildObject(PrimitiveType.Cylinder);
                }
                if (GUILayout.Button("Add Capsule"))
                {
                    CreateChildObject(PrimitiveType.Capsule);
                }
            }

            // Fold menu for mesh menu
            foldList[MESH_SETTINGS] = EditorGUILayout.Foldout(foldList[MESH_SETTINGS], "Mesh settings");

            if (foldList[MESH_SETTINGS])
            {

                asteroidData!.subDivideRecursions = EditorGUILayout.IntField("Subdivide Recursions:", asteroidData!.subDivideRecursions);
                asteroidData!.smoothRecursions = EditorGUILayout.IntField("Smoothing Recursions:", asteroidData!.smoothRecursions);
                asteroidData!.indexFormat = (IndexFormat)EditorGUILayout.EnumPopup(asteroidData!.indexFormat);

                if (GUILayout.Button("Export"))
                {
                    // Ask if the user is sure the want to generate the asteroid if the click the yes button then the asteroid can be generated
                    if (!EditorUtility.DisplayDialog("Warning", "Generating the asteroid may take some time. Are you sure you want to proceed?", "Cancel", "Ok"))
                    {
                        foreach (GenerateStep step in steps)
                        {
                            step.Process(asteroid);
                        }
                    }
                }
            }

            // Fold menu for shader settings 
            foldList[SHADER_SETTINGS] = EditorGUILayout.Foldout(foldList[SHADER_SETTINGS], "Shader settings");

            if (foldList[SHADER_SETTINGS])
            {

            }

            // Fold menu for crater settings 
            foldList[CRATER_SETTINGS] = EditorGUILayout.Foldout(foldList[CRATER_SETTINGS], "Crater settings");

            if (foldList[CRATER_SETTINGS])
            {
                //TODO shader input
            }
        }
        else
        {
            SetPlaceholders();
        }
    }

    // Method for creating a child object for the asteroid
    private void CreateChildObject(PrimitiveType type)
    {
        if (asteroid != null)
        {
            GameObject child = GameObject.CreatePrimitive(type);

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
                asteroidData = asteroid.GetComponent<AsteroidData>();
                asteroidAttractor = asteroid.GetComponent<AsteroidAttractor>();
            }
            else if (selectedObject.transform.parent.name == "Asteroid")
            {
                asteroid = selectedObject.transform.parent.gameObject;
                asteroidData = asteroid.GetComponent<AsteroidData>();
                asteroidAttractor = asteroid.GetComponent<AsteroidAttractor>();
            }
        }
    }

    public void SetPlaceholders()
    {

        EditorGUI.BeginDisabledGroup(asteroidData == null);

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        EditorGUILayout.Slider("Asteroid Density:", 0, 0, 100);

        EditorGUILayout.Foldout(foldList[CRATER_SETTINGS], "Add object");
        EditorGUILayout.Foldout(foldList[CRATER_SETTINGS], "Mesh settings");
        EditorGUILayout.Foldout(foldList[CRATER_SETTINGS], "Shader settings");
        EditorGUILayout.Foldout(foldList[CRATER_SETTINGS], "Crater settings");

        EditorGUI.EndDisabledGroup();
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
