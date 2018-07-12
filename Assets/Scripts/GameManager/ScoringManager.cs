using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringManager : MonoBehaviour {

    public Text ScoreText;

    private bool isScoring = true;

    private float score = 0;

    private void Update()
    {
        if (!isScoring)
            return;

        score += Time.deltaTime * 100;

        ScoreText.text = "Score : " + (int)score;
    }

    public void StartScoring()
    {
        isScoring = true;
    }
    public void StopScoring()
    {
        isScoring = false;
    }
}
