// Block.cs
using UnityEngine;

public enum BlockType { Air = 0, Dirt = 1, Grass = 2, Stone = 3 }

[System.Serializable]
public struct Block {
    public BlockType type;
    public bool IsSolid() => type != BlockType.Air;
}
