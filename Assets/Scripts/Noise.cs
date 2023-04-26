using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise 
{
    /*
    public float GetNoise(int x, int z, int seed)
    {
        float xVal = (float)(x + seed + 9999) / 64;
        float zVal = (float)(z + seed + 9099) / 64;
        float noise = Mathf.PerlinNoise((float)xVal, (float)zVal);
        return noise;*/
    public float GetNoise(int x, int z, int seed, int numOctaves, float frequency, float amplitude, float persistance, float lucunarity)
    {
        int newX = x + seed + 9999;
        int newZ = z + seed + 9999;
        frequency /= 1000;
        float totalNoise = 0;
        for (int octaveI = 0; octaveI < numOctaves; octaveI++)
        {
            float octaveNoise = Mathf.PerlinNoise(newX * frequency, newZ * frequency);
            totalNoise += octaveNoise * amplitude;

            frequency *= lucunarity;
            amplitude *= persistance;

            newX += 9999;
            newZ += 9999;
        }
        return totalNoise;
        /*
         * TODO
         * Find the sum of the geometric series of numOctaves, persistance and amplitude
         * Use that as a divisor when returning totalNoise so the final height is not affected by numOctaves
         */
    }
}
