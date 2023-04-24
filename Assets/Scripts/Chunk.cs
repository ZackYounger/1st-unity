using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{

    public bool[,,] map;
    public Vector3 position;
    public Vector3Int[] dirs;
    public int[][] triangleForEachFace;
    public Vector3[] voxelVertices;

    public GameObject obj;

    public Chunk(Vector3 relativePosition, int size)
    {
        relativePosition = position;
        map = new bool[size, size, size];
        dirs = new Vector3Int[]
        {
            new Vector3Int(0, 1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, 0, 1),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, -1),
            new Vector3Int(0, -1, 0)
        }; // UNESWD  
        triangleForEachFace = new int[][]
        {
            new int[]{2, 1, 0, 3, 1, 2},
            new int[]{0, 1, 4, 6, 4, 1},
            new int[]{4, 2, 0, 2, 4, 5},
            new int[]{7, 1, 3, 1, 7, 6},
            new int[]{3, 2, 7, 5, 7, 2},
            new int[]{6, 5, 4, 6, 7, 5}
        }; //D(NWES dunno)U
        voxelVertices = new Vector3[]{
            new Vector3(0,0,0),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1)
        };
        //Populate map with data
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    //float noise = PerlinNoise3D(x, y, z);
                    //float xf = new float x;
                    float noise = Mathf.PerlinNoise((float)x/ size, (float)z/size);
                    if (noise > 0.3)
                    {
                        map[x, y, z] = true;
                    }
                    else map[x, y, z] = false;
                }
            }
        }

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        int totalVoxels = 0;
        //construct mesh
        for (int x = 1; x < size - 1; x++)
        {
            for (int y = 1; y < size - 1; y++)
            {
                for (int z = 1; z < size - 1; z++)
                {
                    Vector3Int voxelPosition = new Vector3Int(x, y, z);
                    if (!map[voxelPosition.x, voxelPosition.y, voxelPosition.z])
                    {
                        bool addedVertices = false;
                        for (int faceI = 0; faceI < 6; faceI++)
                        {
                            Vector3Int examineCoords = voxelPosition + dirs[faceI];
                            // TODO check inside bounds if (0 < examineCoords.x)
                            //only does the mesh on air land borders
                            if (!map[examineCoords.x, examineCoords.y, examineCoords.z])
                                //there must be a nicer way to do this^^^
                            {
                                //add vertices if they havent been already
                                if (!addedVertices)
                                {
                                    foreach (Vector3 vertex in voxelVertices)
                                    {
                                        //offsets to match voxel position
                                        Vector3 j = voxelPosition + vertex;
                                        vertices.Add(j);
                                    }
                                    //ensures they are not added again
                                    totalVoxels++;
                                    addedVertices = true;
                                }
                                foreach (int triangleIndex in triangleForEachFace[faceI])
                                {
                                    if (totalVoxels * 8 + triangleIndex == 98784)
                                    {
                                        Debug.Log(totalVoxels);
                                        Debug.Log(triangleIndex);
                                        Debug.Log(totalVoxels * 8 + triangleIndex);
                                        Debug.Log(" ");
                                    }
                                    // sub 1 because not including this voxel for the indexing
                                    triangles.Add((totalVoxels - 1) * 8 + triangleIndex);
                                }
                            }
                        }
                    }
                }
            }
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        obj = GameObject.Find("Map");
        obj.GetComponent<MeshFilter>().mesh = mesh;
    }
    /*
    public void OnDrawGizmos(List<Vector3> vertices)
    {
        for (int i = 0; i < vertices.Capacity; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }*/

    public static float PerlinNoise3D(float x, float y, float z)
    {
        y += 1;
        z += 2;
        float xy = _perlin3DFixed(x, y);
        float xz = _perlin3DFixed(x, z);
        float yz = _perlin3DFixed(y, z);
        float yx = _perlin3DFixed(y, x);
        float zx = _perlin3DFixed(z, x);
        float zy = _perlin3DFixed(z, y);
        return xy * xz * yz * yx * zx * zy;
    }
    static float _perlin3DFixed(float a, float b)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
    }  
}