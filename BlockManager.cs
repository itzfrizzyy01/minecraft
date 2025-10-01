// BlockManager.cs
using UnityEngine;

public static class BlockManager {
    public static readonly int chunkSize = 16;
    public static readonly int worldHeight = 128;

    // simple color per block type (replace with textures/material atlas later)
    public static Color GetColor(BlockType t) {
        switch(t) {
            case BlockType.Dirt: return new Color(0.545f,0.271f,0.075f);
            case BlockType.Grass: return new Color(0.129f,0.545f,0.129f);
            case BlockType.Stone: return Color.gray;
            default: return Color.clear;
        }
    }
}
