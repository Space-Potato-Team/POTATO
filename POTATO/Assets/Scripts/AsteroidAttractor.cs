using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AsteroidAttractor : MonoBehaviour
{
    const float G = 667.4f;

    public GravityScript gravityScript;

    [ReadOnly]
    public float volume;
    [ReadOnly]
    public float density;

    public Rigidbody rb;

    void Attract(AsteroidAttractor objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb;



        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        if (distance == 0f)
        {
            return;
        }

        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }

    void OnEnable()
    {

        //Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        //volume = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;

        rb = gameObject.GetComponent<Rigidbody>();
        density = gravityScript.density;


        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        volume = VolumeOfMesh(mesh);


        rb.mass = volume * density;

        //Debug.Log("mass: " + rb.mass);
        //Debug.Log("Volume: " + rb.mass / density);
        rb.useGravity = false;

        gravityScript.attractors.Add(this);
    }

    private void OnDisable()
    {
        gravityScript.attractors.Remove(this);
    }

    void FixedUpdate()
    {
        //AsteroidAttractor[] attractors = FindObjectsOfType<AsteroidAttractor>();
        Debug.Log(gravityScript.attractors);
        foreach (AsteroidAttractor attractor in gravityScript.attractors)
        {
            if (attractor != this)
            {
                Attract(attractor);
            }
        }
    }




    public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;

        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    public float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        volume *= this.transform.localScale.x * this.transform.localScale.y * this.transform.localScale.z;
        return Mathf.Abs(volume);
    }
}




//using System.Collections;
//using System.Collections.Generic;
//using Unity.Collections;
//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class AsteroidAttractor : MonoBehaviour
//{
//    const float G = 667.4f;

//    public float density;

//    [ReadOnly]
//    public float volume;

//    public Rigidbody rb;

//    public static List<AsteroidAttractor> Attractors;
//    void Attract (AsteroidAttractor objToAttract)
//    {
//        Rigidbody rbToAttract = objToAttract.rb;

//        Vector3 direction = rb.position - rbToAttract.position;
//        float distance = direction.magnitude;

//        if (distance == 0f)
//        {
//            return;
//        }

//        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
//        Vector3 force = direction.normalized * forceMagnitude;

//        rbToAttract.AddForce(force);
//    }

//    void OnEnable()
//    {

//        //Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
//        //volume = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;

//        rb = gameObject.GetComponent<Rigidbody>();


//        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
//        volume = VolumeOfMesh(mesh);


//        rb.mass = volume * density;
//        //rb.SetDensity(density);

//        //Debug.Log("mass: " + rb.mass);
//        //Debug.Log("Volume: " + rb.mass / density);
//        rb.useGravity = false;

//        if (Attractors == null)
//        {
//            Attractors = new List<AsteroidAttractor>();
//        }

//        Attractors.Add(this);
//    }

//    private void OnDisable()
//    {
//        Attractors.Remove(this);
//    }

//    void FixedUpdate()
//    {
//        AsteroidAttractor[] attractors = FindObjectsOfType<AsteroidAttractor>();
//        foreach (AsteroidAttractor attractor in attractors)
//        {
//            if (attractor != this)
//            {
//                Attract(attractor);
//            }
//        }
//    }




//    public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
//    {
//        float v321 = p3.x * p2.y * p1.z;
//        float v231 = p2.x * p3.y * p1.z;
//        float v312 = p3.x * p1.y * p2.z;
//        float v132 = p1.x * p3.y * p2.z;
//        float v213 = p2.x * p1.y * p3.z;
//        float v123 = p1.x * p2.y * p3.z;

//        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
//    }

//    public float VolumeOfMesh(Mesh mesh)
//    {
//        float volume = 0;

//        Vector3[] vertices = mesh.vertices;
//        int[] triangles = mesh.triangles;

//        for (int i = 0; i < triangles.Length; i += 3)
//        {
//            Vector3 p1 = vertices[triangles[i + 0]];
//            Vector3 p2 = vertices[triangles[i + 1]];
//            Vector3 p3 = vertices[triangles[i + 2]];
//            volume += SignedVolumeOfTriangle(p1, p2, p3);
//        }
//        volume *= this.transform.localScale.x * this.transform.localScale.y * this.transform.localScale.z;
//        return Mathf.Abs(volume);
//    }
//}
