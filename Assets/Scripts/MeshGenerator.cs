using System.Collections;
using UnityEngine;

public class MeshGenerator
{

    public Vector3[] vertices;
    public int[] triangles;

    public static MeshData GenerateTerrainMesh(int width, int height)
    {

         MeshData meshData = new MeshData(width, height);
         return meshData;
    }
}


public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;

    public MeshData(int width, int height) {
         vertices = new Vector3[(width-1) * (height-1)];
         triangles = new int[width * height * 6];

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(1, 0, 0);
        vertices[1] = new Vector3(0, 0, 1);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

    }

    public Mesh CreateMesh()
    {
         Mesh mesh = new Mesh();
         mesh.vertices = vertices;
         mesh.triangles = triangles;
         mesh.RecalculateNormals();
         return mesh;
    }
}