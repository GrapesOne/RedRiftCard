using System;
using UnityEngine;

public abstract class BaseHandler : MonoBehaviour , IInteractionHandler
{
    public static Action<Transform> ActionGetOnStartDrag;
    public static Action<Transform> ActionGetOnEndDrag;
    public static Action<Transform> ActionGetOnLateEndDrag;

    public abstract void OnStartDrag(Transform caller);
    public abstract void OnEndDrag(Transform caller);
    public abstract void OnLateEndDrag(Transform caller);
    public abstract bool Interaction(Transform caller, Transform callee);
    public abstract bool BadInteraction(Transform caller);
   
}

