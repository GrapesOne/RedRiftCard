using System;
using UnityEngine;

[Serializable]
public class TagsComparer
{
    [SerializeField] private TagComponent.Tag tag;

    Collider2D[] buf = new Collider2D[8];
    public TagsComparer(TagComponent.Tag tag) => this.tag = tag;

    public Transform CompareContinuous( Vector3 v2, float error)
    {
        var res = Physics2D.OverlapCircleNonAlloc(v2, error, buf);
        for (var i = 0; i < res; i++)
        {
            if (!buf[i].TryGetComponent<TagComponent>(out var component)) continue;
            if (component.TagsEqual(tag)) return component.transform;
        }

        return null;
    }

   
}