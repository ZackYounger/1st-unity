using System.Collections;
using UnityEngine;


public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;

    public MeshData(int xd, int yd, int zd, int scale)
    {
        vertices = new Vector3[]{
            new Vector3(0,0,0),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1)
        };
        for (int vertex = 0; vertex < vertices.Length; vertex++)
        {
            vertices[vertex] *= scale;
            vertices[vertex] += new Vector3(xd, yd, zd);
        }

        triangles = new int[]{ 2, 1, 0,
            3, 1, 2,
            0, 1, 4,
            6, 4, 1,
            4, 2, 0,
            2, 4, 5,
            7, 1, 3,
            1, 7, 6,
            3, 2, 7,
            5, 7, 2,
            6, 5, 4,
            6, 7, 5
        };
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