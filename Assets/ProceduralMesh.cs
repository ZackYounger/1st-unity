using UnityEngine;
using System.Collections;

[RequireComponent(typeof (MeshFilter))]
public class ProceduralMesh: MonoBehaviour {

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    void Awake() {
        mesh = GetComponent<MeshFilter> (). mesh;
    }

    // Use this for initialization
    void Start () {
        MakeMeshData ();
        CreateMesh ();
    }

    void MakeMeshData (){
        //create an array of vertices
        vertices = new Vector3[]{ new Vector3(0,0,0), new Vector3(0,0,1), new Vector3 (1,0,0) };
        //create an array of integers
        triangles = new int[] { 0, 1, 2 };
    }
    
    void CreateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}