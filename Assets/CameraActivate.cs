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
    void Start()
    {
        if (shader != null)
        {
            material = new Material(shader);
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
        // GameObject sub = GameObject.Find("Sub");
        // if (cameraID == sub.GetComponent<Sub>().activeCam)
        // {
        //     this.GetComponent<Camera>().enabled = true;
        // }
        // else
        // {
        //     this.GetComponent<Camera>().enabled = false;
        // }
    }
}
