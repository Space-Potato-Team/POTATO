using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(AsteroidAttractor))]
public class GetMesh : MonoBehaviour
{
    public Mesh mesh;

    [Range(0, 10)]
    public float craterDepth = 0;

    [Range(0, 10)]
    public float craterWidth = 0;

    [Range(0, 10)]
    public float minForceRequired = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Get the mesh of the Component
        mesh = GetComponent<MeshFilter>().mesh;

    }

    private void OnCollisionEnter(Collision collision)
    {
        float impact = collision.relativeVelocity.magnitude * collision.rigidbody.mass;

        //Check if the relativeVelocity * Mass produces enough force to crater the asteroid
        if (impact >= minForceRequired)
        {
            //Get all the vertices of the Component in an array
            Vector3[] vertices = new Vector3[mesh.vertices.Length];

            //Create a copy of all the vertices in the Component
            System.Array.Copy(mesh.vertices, vertices, vertices.Length);

            //Check all the contactpoints of the collision
            foreach (ContactPoint c in collision.contacts)
            {
                Debug.Log(c.normal);
                //For loop that checks all the copied vertices of the component
                for (int i = 0; i < vertices.Length; i++)
                {
                    float craterSize = impact * craterWidth;
                    float distance = Vector3.Distance(transform.TransformPoint(vertices[i]), c.point);

                    //Gets all the copied vertices within a certain distance of the contact point
                    //still have to make a variable for the distance
                    if (distance <= craterSize)
                    {
                        float temp = impact * ((craterSize - distance) / craterSize);

                        //Changes the position of the copied vertices in the direction of the collider's normal
                        vertices[i] = (vertices[i] + transform.InverseTransformVector(c.normal * temp * craterDepth));


                    }
                }
            }

            //Changes the vertices of the component to the changed vertices
            mesh.vertices = vertices;

            //Recalculate the position of the changed vertices
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            GetComponent<MeshCollider>().sharedMesh = mesh;

            //Destroy the Meteoroid
            if (collision.collider.CompareTag("Meteoroid"))
            {
                Destroy(collision.gameObject);
            }

            gameObject.GetComponent<AsteroidAttractor>()!.CalculateMass();
        }
    }
}
