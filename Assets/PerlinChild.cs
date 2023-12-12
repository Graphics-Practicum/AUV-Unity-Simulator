using UnityEngine;

public class PerlinChild : MonoBehaviour
{
  public float worldXBound = 1f;
  public float worldYBound = 1f;
  public float worldZBound = 10f;
  public float worldXLowerBound = 0f;
  public float worldYLowerBound = 0f;
  public float worldZLowerBound = 0f;
  public bool isColor;
  // public Material mat;
  void Awake()
  {
    Material mat = GetComponent<Renderer>().material;
    // Material mat = Object.Instantiate(mat);
    mat.SetFloat("boundX", worldXBound);
    mat.SetFloat("boundY", worldYBound);
    mat.SetFloat("boundZ", worldZBound);
    mat.SetFloat("lowerBoundX", worldXLowerBound);
    mat.SetFloat("lowerBoundY", worldYLowerBound);
    mat.SetFloat("lowerBoundZ", worldZLowerBound);
    if (isColor)
    {
      mat.SetInt("isColor", 1);
    }
    else
    {
      mat.SetInt("isColor", 0);
    }
  }
}