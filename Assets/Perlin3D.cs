using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Vector5 : object
{
    public double[] values;
    public Vector5(double v1, double v2, double v3, double v4, double v5)
    {
        this.values = new double[] { v1, v2, v3, v4, v5 };
    }
    public double dot(Vector5 other)
    {
        return this.values[0] * other.values[0] + this.values[1] * other.values[1] + this.values[2] * other.values[2] + this.values[3] * other.values[3] + this.values[4] * other.values[4];
    }
    public double getNorm()
    {
        return Math.Sqrt(this.values.Sum());
    }
    public void normalize()
    {
        this.values[0] = this.values[0] / this.getNorm();
        this.values[1] = this.values[1] / this.getNorm();
        this.values[2] = this.values[2] / this.getNorm();
        this.values[3] = this.values[3] / this.getNorm();
        this.values[4] = this.values[4] / this.getNorm();
    }
}

public class Perlin3D : MonoBehaviour
{
    public int worldXBound = 1000;
    public int worldYBound = 1000;
    public int worldZBound = 1000;
    public int cameraXBound = 1920;
    public int cameraYBound = 1080; // These are arbitrary and should be set with actual values given 
    public int worldResolution = 16;
    public int resolution = 256; // These determine the grid sizes for the perlin noise generation, might want to split them up into four or five separate pieces though
    public Vector5[,,,,] perlin;

    // Start is called before the first frame update
    Vector5 randomUnitVec()
    {
        Vector5 vec = new Vector5(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
        vec.normalize();
        return vec;
    }
    void Start()
    {
        perlin = new Vector5[worldResolution, worldResolution, worldResolution, resolution, 256];
        for (var x = 0; x < worldResolution; x++)
        {
            for (var y = 0; y < worldResolution; y++)
            {
                for (var z = 0; z < worldResolution; z++)
                {
                    for (var x1 = 0; x1 < resolution; x1++)
                    {
                        for (var y1 = 0; y1 < resolution; y1++)
                        {
                            perlin[x, y, z, x1, y1] = randomUnitVec(); //forgive me
                        }
                    }
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
    double getValueAtCoords(double x, double y, double z, double x1, double y1)
    {
        double scaledX = x / (worldXBound + 1) * worldResolution;
        double scaledY = y / (worldYBound + 1) * worldResolution;
        double scaledZ = z / (worldZBound + 1) * worldResolution;
        double scaledX1 = x1 / (cameraXBound + 1) * worldResolution;
        double scaledY1 = y1 / (cameraYBound + 1) * worldResolution;
        int x_less = (int)Math.Floor(scaledX);
        int x_greater = x_less + 1;
        int y_less = (int)Math.Floor(scaledY);
        int y_greater = y_less + 1;
        int z_less = (int)Math.Floor(scaledZ);
        int z_greater = z_less + 1;
        int x1_less = (int)Math.Floor(scaledX1);
        int x1_greater = x1_less + 1;
        int y1_less = (int)Math.Floor(scaledY1);
        int y1_greater = y1_less + 1;
        double dx = scaledX - x_less;
        double dy = scaledY - y_less;
        double dz = scaledZ - z_less;
        double dx1 = scaledX1 - x1_less;
        double dx2 = scaledY1 - y1_less;


    }

    // Update is called once per frame
    void Update()
    {

    }
}
