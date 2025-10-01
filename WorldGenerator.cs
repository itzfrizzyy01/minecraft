// WorldGenerator.cs
using UnityEngine;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {
    public int radiusChunks = 2;
    public GameObject chunkPrefab;
    private Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();

    void Start() {
        if (chunkPrefab == null) {
            chunkPrefab = new GameObject("Chunk", typeof(MeshFilter), typeof(MeshRenderer), typeof(Chunk));
        }
        GenerateChunks();
    }

    void GenerateChunks() {
        for (int cx = -radiusChunks; cx <= radiusChunks; cx++) {
            for (int cz = -radiusChunks; cz <= radiusChunks; cz++) {
                CreateChunk(new Vector2Int(cx, cz));
            }
        }
    }

    Chunk CreateChunk(Vector2Int coord) {
        if (chunks.ContainsKey(coord)) return chunks[coord];
        GameObject go = Instantiate(chunkPrefab, transform);
        go.name = $"Chunk_{coord.x}_{coord.y}";
        Chunk chunk = go.GetComponent<Chunk>();
        chunk.Init(coord);
        FillChunk(chunk);
        chunk.BuildMesh();
        chunks.Add(coord, chunk);
        return chunk;
    }

    void FillChunk(Chunk chunk) {
        int sx = chunk.chunkCoord.x * BlockManager.chunkSize;
        int sz = chunk.chunkCoord.y * BlockManager.chunkSize;
        for (int x=0;x<BlockManager.chunkSize;x++) {
            for (int z=0;z<BlockManager.chunkSize;z++) {
                float worldX = sx + x;
                float worldZ = sz + z;
                float height = Mathf.FloorToInt((Mathf.PerlinNoise(worldX * 0.02f, worldZ * 0.02f) * 20f) + 20f);
                for (int y=0; y<BlockManager.worldHeight; y++) {
                    if (y > height) {
                        chunk.SetBlock(x,y,z, BlockType.Air);
                    } else if (y == height) {
                        chunk.SetBlock(x,y,z, BlockType.Grass);
                    } else if (y > height - 4) {
                        chunk.SetBlock(x,y,z, BlockType.Dirt);
                    } else {
                        chunk.SetBlock(x,y,z, BlockType.Stone);
                    }
                }
            }
        }
    }
}
