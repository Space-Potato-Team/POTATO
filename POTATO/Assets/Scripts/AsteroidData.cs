using UnityEngine;
using UnityEngine.Rendering;

public class AsteroidData : MonoBehaviour
{
    public int subDivideRecursions = 5;
    public int smoothRecursions = 100;
    public IndexFormat indexFormat = IndexFormat.UInt32;
    
    public float asteroidDensity = 1.3f;
    public bool addGravity = true;
    
    //Crater fields
    public float CraterGrouping;
    public float maxCraterSize = 10;
    public float minCraterSize = 1;
    public int CraterAmount = 150;
    public float CraterDepth = 0.25f;
    public float minForceRequired = 100;
    public bool addColisions = true;
    public float impactForceMultiplier = 0.5f;
}
