using UnityEngine;


public interface IGenerateStep
{

       public Mesh Process(Mesh mesh); 

       public void AddGUI();
}
