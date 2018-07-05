using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringManager : MonoBehaviour {

    public Text ScoreText;

    private float score = 0;

    private void Update()
    {
        score += Time.deltaTime * 100;

        ScoreText.text = "Score : " + (int)score;
    }
}
