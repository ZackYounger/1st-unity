using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public int chunkSize;
    public int seed;
    public Chunk[,,] chunks;
    public int chunkViewDist;

    public float frequency;
    public float amplitude;
    public float persistance;
    public float lucunarity;
    public int numOctaves;

    Dictionary<Vector3, Chunk> chunkDictionairy = new Dictionary<Vector3, Chunk>();

    public void GenerateMap()
    {
        //Delete all game objects
        object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object obj in objects)
        {
            GameObject gameObject = (GameObject)obj;
            if (gameObject.name[0] == '[')
            {
                //Destroy(gameObject);
                gameObject.GetComponent<MeshFilter>().mesh = null;
            }
            
        }


        chunkDictionairy = new Dictionary<Vector3, Chunk>();
        for (int x=-chunkViewDist+1; x<chunkViewDist; x++)
        {
            for (int z=-chunkViewDist+1; z<chunkViewDist; z++)
            {
                Vector3 chunkPosKey = new Vector2(x, z);
                Vector3Int chunkPos = new Vector3Int(x, 1, z);
                chunkDictionairy.Add(chunkPosKey, new Chunk(chunkPos, chunkSize, seed, numOctaves, frequency, amplitude, persistance, lucunarity));
            }
        }
        /*
        Vector3Int pos = new Vector3Int(0, 0, 0);
        Chunk chunk1 = new Chunk(pos, chunkSize, seed, amplitude, frequency);
        Vector3Int pos2 = new Vector3Int(chunkSize, 0, 0);
        Chunk chunk2 = new Chunk(pos2, chunkSize, seed, amplitude, frequency);*/
    }
}