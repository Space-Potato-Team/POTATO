using UnityEngine;
using UnityEngine.Rendering;

public class AsteroidData : MonoBehaviour
{
    [Range(1, 6)]
    public int subDivideRecursions;    

    [Range(1, 200)]
    public int smoothRecursions;
    public IndexFormat indexFormat;
    public float ShrinkDiameter;
    
    [Range(1, 100)]
    public float asteroidDensity;
    public bool addGravity;
    
    //Crater fields
    public float CraterGrouping;
    public float CraterMultiplier;
    [Range(1, 10)]
    public float maxCraterSize;
    [Range(0.1f, 1)]
    public float minCraterSize;
    public int CraterAmount;
    [Range(0.1f, 10)]
    public float CraterDepth;
    [Range(0.1f, 10)]
    public float minForceRequired;
    public bool addColisions;
    [Range(0.1f, 10)]
    public float impactForceMultiplier;
}
