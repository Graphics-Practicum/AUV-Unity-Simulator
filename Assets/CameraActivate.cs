using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class CameraActivate : MonoBehaviour
{
    public int cameraID;
    public Shader shader;
    Material material = null;
    public int worldResolution = 4;
    public float worldXLowerBound = -10f;
    public float worldYLowerBound = -10f;
    public float worldZLowerBound = -5f;
    public float worldXBound = 10f;
    public float worldYBound = 10f;
    public float worldZBound = 5f;// Arbitrary, set them later
    private Vector3[] squashedPerlin;
    private ComputeBuffer buffer;
    Vector3 randomUnitVec()
    {
        Vector3 vec = new Vector3(UnityEngine.Random.Range(0.0f, 1.0f) - 0.5f, UnityEngine.Random.Range(0.0f, 1.0f) - 0.5f, UnityEngine.Random.Range(0.0f, 1.0f) - 0.5f);
        return vec.normalized;
    }
    void Start()
    {
        if (shader != null)
        {
            squashedPerlin = new Vector3[worldResolution * worldResolution * worldResolution];
            for (var x = 0; x < worldResolution; x++)
            {
                for (var y = 0; y < worldResolution; y++)
                {
                    for (var z = 0; z < worldResolution; z++)
                    {
                        squashedPerlin[z + y * worldResolution + x * worldResolution * worldResolution] = randomUnitVec();
                        // perlin[x, y, z] = randomUnitVec();
                    }
                }
            }
            buffer = new ComputeBuffer(worldResolution * worldResolution * worldResolution, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector3)));
            buffer.SetData(squashedPerlin);
            material = new Material(shader);
            material.SetBuffer("perlin", buffer);
            material.SetFloat("boundX", worldXBound);
            material.SetFloat("boundY", worldYBound);
            material.SetFloat("boundZ", worldZBound);
            material.SetFloat("lowerBoundX", worldXLowerBound);
            material.SetFloat("lowerBoundY", worldYLowerBound);
            material.SetFloat("lowerBoundZ", worldZLowerBound);
            material.SetInt("xGridMax", worldResolution);
            material.SetInt("yGridMax", worldResolution);
            material.SetInt("zGridMax", worldResolution);
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (shader == null)
        {
            Graphics.Blit(source, destination);
            return;
        }
        Graphics.Blit(source, destination, material);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject sub = GameObject.Find("Sub");
        if (cameraID == sub.GetComponent<Sub>().activeCam)
        {
            this.GetComponent<Camera>().enabled = true;
        }
        else
        {
            this.GetComponent<Camera>().enabled = false;
        }
    }
}
