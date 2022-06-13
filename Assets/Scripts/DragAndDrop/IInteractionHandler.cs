using UnityEngine;

public interface IInteractionHandler
{
    bool Interaction(Transform caller, Transform callee);
    bool BadInteraction(Transform caller);
}
