using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AsteroidAttractor : MonoBehaviour
{
    //semi-realistic gravity strengh (unchangable)
    const float G = 667.4f;

    //script with values for designers to play with
    public GravityScript gravityScript;

    //volume of an object which can only be viewed
    [ReadOnly]
    public float volume;

    //set density
    public float density;

    //current object rigidbody
    public Rigidbody rb;

    //AsteroidAttractor objToAttract: script of obj which has to be attracted to current obj
    //function that attracts the given object to the current object using addforce
    void Attract(AsteroidAttractor objToAttract)
    {
        //get other objects rigidboyd
        Rigidbody rbToAttract = objToAttract.rb;

        //get distance lenght between current object and other object
        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        //save resources if objects are next to each other
        if (distance == 0f)
        {
            return;
        }

        //calculate strenght of pull using G * (mass / disance^2)
        //reversegravityscriptfalloff is the power of how long it takes for the objects to lose most gravitational pull, the higher the number the faster the fall off
        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, gravityScript.reverseGravityStrengthFallOff);

        //get correct direction for pull with calculated force
        Vector3 force = direction.normalized * forceMagnitude;

        //push the object towards current object (pull force)
        rbToAttract.AddForce(force);
    }

    //calculates the mass of the current object
    public void CalculateMass()
    {
        //get mesh of current object and get volume of that mesh and set it as current object volume
        Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        //set volume based on localscale
        volume = VolumeOfMesh(mesh);

        //calculate and set mass by: volume * density
        rb.mass = volume * density;
    }

    //set base values when script is enabled
    void OnEnable()
    {
        //set current object rigidbody
        rb = gameObject.GetComponent<Rigidbody>();

        //caluclate and set mass of current object
        CalculateMass();
        
        //set gravity to false for script to work
        rb.useGravity = false;

        //add current script to array with all AsteroidAttractor scripts
        gravityScript.attractors.Add(this);
    }

    //runs when current script is disabled
    private void OnDisable()
    {
        //remove current script from array with all AsteroidAttractor scripts
        gravityScript.attractors.Remove(this);
    }

    //runs every physics update
    void FixedUpdate()
    {
        //calculate every gravity pull from all other AsteroidAttractor objects towards current object 
        foreach (AsteroidAttractor attractor in gravityScript.attractors)
        {
            //doesn't attract current object to itselfs
            if (attractor != this)
            {
                Attract(attractor);
            }
        }
    }

    //calculate volume based on local scale
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

    //calculate volume based on local scale
    public float VolumeOfMesh(Mesh mesh)
    {
        float localVolume = 0;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            localVolume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        localVolume *= this.transform.localScale.x * this.transform.localScale.y * this.transform.localScale.z;
        return Mathf.Abs(localVolume);
    }
}