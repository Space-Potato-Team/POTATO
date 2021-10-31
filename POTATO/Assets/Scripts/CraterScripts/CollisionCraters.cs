using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(AsteroidData), typeof(Rigidbody))]
public class CollisionCraters : MonoBehaviour
{
    private Mesh mesh;
    private AsteroidData asteroidData;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // Get the mesh of the Component
        mesh = GetComponent<MeshFilter>().mesh;
        asteroidData = GetComponent<AsteroidData>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 impactVector = (collision.relativeVelocity * collision.relativeVelocity.magnitude) * collision.rigidbody.mass;

        //Check if the relativeVelocity * Mass produces enough force to crater the asteroid
        if (impactVector.magnitude >= asteroidData.minForceRequired)
        {
            //Take the largest value between the minimum cratersize and the maximum crater size or the magnitude
            //If the magnitude is very low this will prevent that the cratersize will be too big
            //Multiply this value by the impactForceMultiplier set.
            var craterSize = Mathf.Max(Mathf.Min(asteroidData.maxCraterSize, impactVector.magnitude), asteroidData.minCraterSize) * asteroidData.impactForceMultiplier;

            Debug.Log(craterSize);

            //Call static crater creator method
            CraterCreator.addCraterToMeshOnPosition(mesh, transform.InverseTransformPoint(collision.rigidbody.position), impactVector.normalized, craterSize, asteroidData.CraterDepth);

            //Recalculate the position of the changed vertices
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            //Apply the mesh back to the Asteroid
            GetComponent<MeshCollider>().sharedMesh = mesh;

            //Recalculate the Mass of the Asteroid
            MassScript.CalculateMass(gameObject, rb, asteroidData.asteroidDensity);
        }
    }
}
