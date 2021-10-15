using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraterCreator
{

    public static Mesh addCraterToMeshOnPosition(Mesh mesh, Vector3 position, Vector3 direction, float craterSize)
    {
        
        //Get all the vertices of the Component in an array
        List<Vector3> vertices = mesh.vertices.ToList();

        for (int i = 0; i < vertices.Count; i++)
        {
            float impact = direction.magnitude;
            float distance = Vector3.Distance(vertices[i], position);
                
                //Gets all the copied vertices within a certain distance of the contact point
                //still have to make a variable for the distance
            if (distance <= craterSize)
            {
                    float temp = impact * ((craterSize - distance) / craterSize);
                
                    //Changes the position of the copied vertices in the direction of the collider's normal
                    vertices[i] = (vertices[i] + direction * temp * craterSize);
                
                }
        }
        mesh.vertices = vertices.ToArray();
        return mesh;
    }
}
