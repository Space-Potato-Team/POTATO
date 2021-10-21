using UnityEngine;
using UnityEngine.Rendering;

public class AsteroidData : MonoBehaviour
{
    public int subDivideRecursions, smoothRecursions;
    public IndexFormat indexFormat;
    public float ShrinkDiameter;
    
    public float asteroidDensity;
    public bool addGravity;
    
    //Crater fields
    public float CraterGrouping;
    public float CraterMultiplier;
    public float maxCraterSize;
    public float minCraterSize;
    public int CraterAmount;
    public float CraterDepth;
    public float minForceRequired;
    public bool addColisions;
    public float impactForceMultiplier;
}
