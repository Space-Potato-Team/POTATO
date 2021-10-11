using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GravityScriptableObject", menuName = "ScriptableObjects/GravityScriptableObject", order = 1)]
public class GravityScript : ScriptableObject
{
    public float density;

    public List<AsteroidAttractor> attractors;
}
