using UnityEngine;

public class SpriteOrder : MonoBehaviour
{
    [SerializeField] private int startOrder;

    void OnValidate()
    {
        SetOrderToChildren();
    }
    [ContextMenu("SetOrderToChildren")]
    
    void SetOrderToChildren()
    {
        SetOrderToChildren(transform, startOrder);
    }
    void SetOrderToChildren(int order)
    {
        SetOrderToChildren(transform, order);
    }
    public int SetOrderToChildren(Transform parent, int i)
    {
        Renderer renderer;
        foreach (Transform child in parent)
        {
            if (child.TryGetComponent(out renderer))
            {
                renderer.sortingOrder = i++;
            }

            i = SetOrderToChildren(child, i);
        }

        return i;
    }
}