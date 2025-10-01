// Chunk.cs
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Chunk : MonoBehaviour {
    public Vector2Int chunkCoord;
    private Block[,,] blocks;
    private MeshFilter mf;
    private MeshRenderer mr;

    public void Init(Vector2Int coord) {
        chunkCoord = coord;
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();
        mr.material = new Material(Shader.Find("Standard"));
        blocks = new Block[BlockManager.chunkSize, BlockManager.worldHeight, BlockManager.chunkSize];
    }

    public void SetBlock(int x, int y, int z, BlockType type) {
        if (InRange(x,y,z)) blocks[x,y,z].type = type;
    }

    public BlockType GetBlock(int x,int y,int z) {
        if (!InRange(x,y,z)) return BlockType.Air;
        return blocks[x,y,z].type;
    }

    private bool InRange(int x,int y,int z) {
        return x>=0 && x<BlockManager.chunkSize && y>=0 && y<BlockManager.worldHeight && z>=0 && z<BlockManager.chunkSize;
    }

    public void BuildMesh() {
        var verts = new System.Collections.Generic.List<Vector3>();
        var tris = new System.Collections.Generic.List<int>();
        var cols = new System.Collections.Generic.List<Color>();

        for (int x=0;x<BlockManager.chunkSize;x++) {
            for (int y=0;y<BlockManager.worldHeight;y++) {
                for (int z=0;z<BlockManager.chunkSize;z++) {
                    var t = blocks[x,y,z].type;
                    if (t == BlockType.Air) continue;
                    // for each face check neighbor; if neighbor is air add face
                    AddCubeFaces(x,y,z,t, verts, tris, cols);
                }
            }
        }

        Mesh m = new Mesh();
        m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        m.SetVertices(verts);
        m.SetTriangles(tris, 0);
        m.SetColors(cols);
        m.RecalculateNormals();
        mf.mesh = m;
    }

    private void AddCubeFaces(int x,int y,int z, BlockType type,
        System.Collections.Generic.List<Vector3> verts,
        System.Collections.Generic.List<int> tris,
        System.Collections.Generic.List<Color> cols) {

        // local helper to add one face
        void AddFace(Vector3 a, Vector3 b, Vector3 c, Vector3 d) {
            int start = verts.Count;
            verts.Add(a); verts.Add(b); verts.Add(c); verts.Add(d);
            tris.Add(start); tris.Add(start+1); tris.Add(start+2);
            tris.Add(start); tris.Add(start+2); tris.Add(start+3);
            Color col = BlockManager.GetColor(type);
            cols.Add(col); cols.Add(col); cols.Add(col); cols.Add(col);
        }

        // neighbor check
        bool IsAir(int nx,int ny,int nz) {
            if (nx<0 || nx>=BlockManager.chunkSize || nz<0 || nz>=BlockManager.chunkSize) return true; // naive: treat outside as empty (neighboring chunk required for correct result)
            if (ny<0 || ny>=BlockManager.worldHeight) return true;
            return blocks[nx,ny,nz].type == BlockType.Air;
        }

        float fx = chunkCoord.x * BlockManager.chunkSize + x;
        float fz = chunkCoord.y * BlockManager.chunkSize + z;
        Vector3 p = new Vector3(fx, y, fz);

        // +X face
        if (IsAir(x+1,y,z)) AddFace(p + new Vector3(1,0,0), p + new Vector3(1,0,1), p + new Vector3(1,1,1), p + new Vector3(1,1,0));
        // -X face
        if (IsAir(x-1,y,z)) AddFace(p + new Vector3(0,0,1), p + new Vector3(0,0,0), p + new Vector3(0,1,0), p + new Vector3(0,1,1));
        // +Y face
        if (IsAir(x,y+1,z)) AddFace(p + new Vector3(0,1,0), p + new Vector3(1,1,0), p + new Vector3(1,1,1), p + new Vector3(0,1,1));
        // -Y face
        if (IsAir(x,y-1,z)) AddFace(p + new Vector3(0,0,1), p + new Vector3(1,0,1), p + new Vector3(1,0,0), p + new Vector3(0,0,0));
        // +Z face
        if (IsAir(x,y,z+1)) AddFace(p + new Vector3(1,0,1), p + new Vector3(0,0,1), p + new Vector3(0,1,1), p + new Vector3(1,1,1));
        // -Z face
        if (IsAir(x,y,z-1)) AddFace(p + new Vector3(0,0,0), p + new Vector3(1,0,0), p + new Vector3(1,1,0), p + new Vector3(0,1,0));
    }
}
