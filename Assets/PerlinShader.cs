using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PerlinShader : MonoBehaviour
{
    public Shader shader;
    // Start is called before the first frame update
    public int worldResolution = 10;
    // public float worldXLowerBound = 0f;
    // public float worldYLowerBound = 0f;
    // public float worldZLowerBound = 0f;
    // public float worldXBound = 1f;
    // public float worldYBound = 1f;
    // public float worldZBound = 10f;// Arbitrary, set them later
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
            // Renderer[] renderers = GetComponent<Transform>().GetComponentsInChildren<Renderer>(true);
            // foreach (Renderer r in renderers)
            for (var i = 0; i < GetComponent<Transform>().childCount; i++)
            {
                Renderer r = GetComponent<Transform>().GetChild(i).gameObject.GetComponent<Renderer>();
                Material material = r.material;
                material.shader = shader;
                buffer = new ComputeBuffer(worldResolution * worldResolution * worldResolution, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector3)));
                buffer.SetData(squashedPerlin);
                material.SetBuffer("perlin", buffer);
                material.SetInt("xGridMax", worldResolution);
                material.SetInt("yGridMax", worldResolution);
                material.SetInt("zGridMax", worldResolution);
                GetComponent<Transform>().GetChild(i).gameObject.SetActive(true);

                // material.SetFloat("boundX", 0);
                // material.SetFloat("boundY", 0);
                // material.SetFloat("boundZ", 0);
                // material.SetFloat("lowerBoundX", 1);
                // material.SetFloat("lowerBoundY", 1);
                // material.SetFloat("lowerBoundZ", 1);
            }
            // material.SetFloat("boundX", worldXBound);
            // material.SetFloat("boundY", worldYBound);
            // material.SetFloat("boundZ", worldZBound);
            // material.SetFloat("lowerBoundX", worldXLowerBound);
            // material.SetFloat("lowerBoundY", worldYLowerBound);
            // material.SetFloat("lowerBoundZ", worldZLowerBound);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
