using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Perlin3D : MonoBehaviour
{
    public int worldXBound = 1000;
    public int worldYBound = 1000;
    public int worldZBound = 1000; // These are arbitrary and should be set with actual values given 
    public int worldResolution = 16; // These determine the grid sizes for the perlin noise generation, might want to split them up 
    public Vector3[,,] perlin;
    public Vector3[] squashedPerlin;

    // Start is called before the first frame update
    Vector3 randomUnitVec()
    {
        Vector3 vec = new Vector3(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
        return vec.normalized;
    }
    void Start()
    {
        perlin = new Vector3[worldResolution, worldResolution, worldResolution];
        for (var x = 0; x < worldResolution; x++)
        {
            for (var y = 0; y < worldResolution; y++)
            {
                for (var z = 0; z < worldResolution; z++)
                {
                    squashedPerlin.Append(randomUnitVec());
                    // perlin[x, y, z] = randomUnitVec();
                }
            }
        }
    }
    double interp(double start, double end, double along)
    {
        if (along <= 0)
        {
            along = 0;
        }
        else if (along >= 1)
        {
            along = 1;
        }
        return start + (3 * along * along - 2 * along * along * along) * (end - start); // Using smoothstep here https://en.wikipedia.org/wiki/Smoothstep
    }
    double getGrad(double x, double y, double z, int x_grid, int y_grid, int z_grid)
    {
        Vector3 grid = perlin[x_grid, y_grid, z_grid];
        return (x - x_grid) * grid.x + (y - y_grid) * grid.y + (z - z_grid) * grid.z;
    }
    double getValueAtCoords(double x, double y, double z)
    {
        double scaledX = x / (worldXBound + 1) * worldResolution;
        double scaledY = y / (worldYBound + 1) * worldResolution;
        double scaledZ = z / (worldZBound + 1) * worldResolution;
        int x_less = (int)Math.Floor(scaledX);
        int x_greater = x_less + 1;
        int y_less = (int)Math.Floor(scaledY);
        int y_greater = y_less + 1;
        int z_less = (int)Math.Floor(scaledZ);
        int z_greater = z_less + 1;
        double dx = scaledX - x_less;
        double dy = scaledY - y_less;
        double dz = scaledZ - z_less;
        double lll = getGrad(x, y, z, x_less, y_less, z_less);
        double llg = getGrad(x, y, z, x_less, y_less, z_greater);
        double lgl = getGrad(x, y, z, x_less, y_greater, z_less);
        double lgg = getGrad(x, y, z, x_less, y_greater, z_greater);
        double gll = getGrad(x, y, z, x_greater, y_less, z_less);
        double glg = getGrad(x, y, z, x_greater, y_less, z_greater);
        double ggl = getGrad(x, y, z, x_greater, y_greater, z_less);
        double ggg = getGrad(x, y, z, x_greater, y_greater, z_greater);
        return interp(interp(interp(lll, gll, dx), interp(lgl, ggl, dx), dy), interp(interp(llg, glg, dx), interp(lgg, ggg, dx), dy), dz); // man i hope this is right
    }

    // Update is called once per frame
    void Update()
    {

    }
}
