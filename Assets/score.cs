using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    public Text score_text;
    public static int curr_score;
    // Start is called before the first frame update
    void Start()
    {
        curr_score = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        score_text.text = "Score : "  + curr_score;
    }
}
