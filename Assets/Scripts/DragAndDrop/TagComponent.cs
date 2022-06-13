using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagComponent : MonoBehaviour
{
    public enum Tag
    {
        player, opposite
    }

    [SerializeField] private Tag tag = Tag.player;

    public bool TagsEqual(Tag _tag) => tag == _tag;
    public bool IsPlayer() => tag == Tag.player;
    public bool IsOpposite() => tag == Tag.opposite;
}
