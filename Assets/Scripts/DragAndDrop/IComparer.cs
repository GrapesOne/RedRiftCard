using UnityEngine;

public interface IComparer
{ 
    bool CompareContinuous( Vector3 v2, float error);
    bool Compare( Vector3 v2, float error);
}
