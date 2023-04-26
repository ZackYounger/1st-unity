using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{

    private bool[,,] map;
    private Vector3Int chunkPosition;
    private Vector3Int chunkMapPosition;
    private Vector3Int[] dirs;
    private int[][] triangleForEachFace;
    private int[][] quadForEachFace;
    private Vector3Int[] voxelVertices;
    private int chunkSize;

    private Noise noiseGenerator;

    public Chunk(Vector3Int chunkPosition, int size, int seed, int numOctaves, float frequency, float amplitude, float persistance, float lucunarity)
    {
        noiseGenerator = new Noise();
        //float f = noiseGenerator.GetNoise(1, 1, 1, 1, 1f, 1f, 1f, 1f);
        chunkMapPosition = chunkPosition;
        chunkPosition = new Vector3Int(chunkMapPosition.x * size, chunkMapPosition.y * size, chunkMapPosition.z * size);
        chunkSize = size;
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
            new int[]{6, 5, 4, 6, 7, 5}, //+y
            new int[]{3, 2, 7, 5, 7, 2}, //+x
            new int[]{7, 1, 3, 1, 7, 6}, //+z
            new int[]{0, 1, 4, 6, 4, 1}, //-x
            new int[]{4, 2, 0, 2, 4, 5}, //-z
            new int[]{2, 1, 0, 3, 1, 2} //-y
        }; //UNESWD
        quadForEachFace = new int[][]
{
            new int[]{7, 5, 6, 4}, //+y
            new int[]{3, 2, 7, 5}, //+x
            new int[]{7, 6, 3, 1}, //+z
            new int[]{0, 1, 4, 6}, //-x
            new int[]{4, 5, 0, 2}, //-z
            new int[]{1, 0, 3, 2} //-y
        }; //UNESWD

        voxelVertices = new Vector3Int[]{
            new Vector3Int(0,0,0),
            new Vector3Int(0, 0, 1),
            new Vector3Int(1, 0, 0),
            new Vector3Int(1, 0, 1),
            new Vector3Int(0, 1, 0),
            new Vector3Int(1, 1, 0),
            new Vector3Int(0, 1, 1),
            new Vector3Int(1, 1, 1)
        };

        //Populate map with data
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                /*
                float noise = Mathf.PerlinNoise(
                    (float)(x+ chunkPosition.x) * frequency / 256, (float)(z+ chunkPosition.z) * frequency / 256
                    );*/
                float height = noiseGenerator.GetNoise(x + chunkPosition.x, z + chunkPosition.z, seed, numOctaves, frequency, amplitude, persistance, lucunarity);
                for (int y = 0; y < size; y++)
                {
                    if (y < height)
                    {
                        map[x, y, z] = true;
                    }
                    else map[x, y, z] = false;
                }
            }
        }

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        //int totalVoxels = 0;
        int totalQuads = 0;
        int[] quadOrder = new int[]{0, 1, 2, 3, 2, 1};
        //construct mesh
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    Vector3Int voxelPosition = new Vector3Int(x, y, z);
                    if (map[voxelPosition.x, voxelPosition.y, voxelPosition.z])
                    {
                        //bool addedVertices = false;
                        for (int faceI = 0; faceI < 6; faceI++)
                        {
                            Vector3Int examineCoords = voxelPosition + dirs[faceI];
                            // TODO check inside bounds if (0 < examineCoords.x)
                            //only does the mesh on air land borders
                            bool placeQuad = false;
                            if (0 <= examineCoords.x && examineCoords.x < chunkSize && 0 <= examineCoords.y && examineCoords.y < chunkSize && 0 <= examineCoords.z && examineCoords.z < chunkSize)
                            {
                                //checking blocks inside chunk
                                if (!map[examineCoords.x, examineCoords.y, examineCoords.z])
                                //there must be a nicer way to do this^^^
                                {
                                    placeQuad = true;
                                }
                            } else
                            {
                                //handles quads examining outside of the chunk
                                float height = noiseGenerator.GetNoise(x + chunkPosition.x, z + chunkPosition.z, seed, numOctaves, frequency, amplitude, persistance, lucunarity);
                                //DOUBT THIS LINE
                                if ((int)Mathf.Floor(height) == y)
                                {
                                    placeQuad = true;
                                }
                            }
                            if (placeQuad)
                            {
                                //add 4 vertices for every quad
                                foreach (int vertexI in quadForEachFace[faceI])
                                {
                                    //offsets to match relative chunk position and total chunk position
                                    Vector3 j = voxelPosition + voxelVertices[vertexI] + chunkPosition;
                                    vertices.Add(j);
                                }
                                //add triangles quads
                                foreach (int quadI in quadOrder)
                                {
                                    triangles.Add(totalQuads * 4 + quadI);
                                }
                                totalQuads++;
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
        mesh.RecalculateTangents();
        DisplayMesh(mesh);
        //obj = GameObject.Find("Map");
        //obj.GetComponent<MeshFilter>().mesh = mesh;
    }

    private GameObject gameObject;

    public void DisplayMesh(Mesh mesh)
    {
        string gameObjectName = "[" + chunkMapPosition.x.ToString() + ", " + chunkMapPosition.y.ToString() + ", " + chunkMapPosition.z.ToString() + "]";
        gameObject = GameObject.Find(gameObjectName);
        if (gameObject == null)
        {
            gameObject = new GameObject(gameObjectName);
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();
            Material newMat = Resources.Load("Materials/Mesh Mat", typeof(Material)) as Material;
            gameObject.GetComponent<Renderer>().material = newMat;
        }
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }
}
