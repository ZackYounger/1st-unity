using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public void GenerateCube()
    {
        Vector3 pos = new Vector3(0, 0, 0);
        Chunk chunk1 = new Chunk(pos, 16);
    }

    public GameObject obj;
     
    public void DrawMesh(MeshData meshData)//, Texture2D texture)
    {
        Debug.Log("Displaying");
        obj = GameObject.Find("Map"); 
        obj.GetComponent<MeshFilter>().mesh = meshData.CreateMesh();
    }
}