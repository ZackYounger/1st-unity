using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class MapGenerator : MonoBehaviour
{

    Mesh mesh;

    public int xSize = 5;
    public int zSize = 5;

    public void GenerateMap()
    {
         MeshData meshData = MeshGenerator.GenerateTerrainMesh(xSize, zSize);

        //MeshDisplayer.DrawMesh(meshData);
    }

    private void OnDrawGizmos()
    {

        MeshData meshData = MeshGenerator.GenerateTerrainMesh(xSize, zSize);

        for (int i = 0; i < meshData.vertices.Length; i++)
        {
            Gizmos.DrawSphere(meshData.vertices[i], .1f);
        }
    }

}




/*
using UnityEngine;
using System.Collections;

[RequireComponent(typeof (MeshFilter))]
public class MapGenerator: MonoBehaviour
{

    Mesh mesh;

    public int xSize = 5;
    public int zSize = 5;

    Vector3[] vertices;
    int[] triangles;

    // Use this for initialization
    public void GenerateMap()
    {

        mesh = GetComponent<MeshFilter>().sharedMesh;
        MakeMeshData ();
        CreateMesh ();

        

    }

    void MakeMeshData()
    {

        float xOffset = Random.Range(0f, 1f) * 10000;
        float zOffset = Random.Range(0f, 1f) * 10000;

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        int i = 0;
        for (int x = 0; x <= xSize; x++)
        {
            for (int z = 0; z <= zSize; z++)
            {
                float y = Mathf.PerlinNoise(x * .3f + xOffset, z * .3f + zOffset) * 2f;
                vertices[i] = new Vector3(x, y, z);
                
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + 1;
                triangles[tris + 2] = vert + xSize + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 2;
                triangles[tris + 5] = vert + xSize + 1;
                vert++;
                tris += 6;
            }
            vert++;
        }
    }
    
    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
     }
}
 */