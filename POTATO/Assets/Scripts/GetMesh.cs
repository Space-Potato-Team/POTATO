using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetMesh : MonoBehaviour
{
    public Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {

        // Get the mesh of the Component
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check if collider is a Meteoroid
        if (collision.collider.CompareTag("Meteoroid"))
        {
            //Get all the vertices of the Component in an array
            Vector3[] vertices = new Vector3[mesh.vertices.Length];

            //Create a copy of all the vertices in the Component
            System.Array.Copy(mesh.vertices, vertices, vertices.Length);

            //Check all the contactpoints of the collision
            foreach (ContactPoint c in collision.contacts)
            {
                Rigidbody meteoroid = collision.collider.GetComponent<Rigidbody>();

                var speed = meteoroid.mass * meteoroid.velocity;
                Debug.Log(meteoroid.mass);
                Debug.Log(meteoroid.velocity);
                Debug.Log(speed);

                //For loop that checks all the copied vertices of the component
                for (int i=0; i < vertices.Length; i++) {

                    //Gets all the copied vertices within a certain distance of the contact point
                    //still have to make a variable for the distance
                    if (Vector3.Distance(transform.TransformPoint(vertices[i]), c.point) <= 2)
                    {
                        Debug.Log(vertices[i]);
                        //Changes the position of the copied vertices in the direction of the collider's normal
                        //Do this change times the scale / still have to make a variable for the scale
                        vertices[i] = (vertices[i] + transform.InverseTransformVector(c.normal * 1));
                        Debug.Log("Changed vertices" + vertices[i]);
                    }
                }
            }

            //Changes the vertices of the component to the changed vertices
            mesh.vertices = vertices;

            //Recalculate the position of the changed vertices
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            //Destroy the Meteoroid
            Destroy(collision.gameObject);
        }
            
    }



}
