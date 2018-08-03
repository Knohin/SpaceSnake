using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringManager : MonoBehaviour {

    public Text ScoreText;

    public static float score = 0;

    public static float save_Score;
    public static float meteo_Score;
    public static float time_Score;

    private void Start()
    {
        score = 0;
        save_Score = 0;
        meteo_Score = 0;
        time_Score = 0;
    }

    private void Update()
    {
        time_Score += Time.deltaTime * 50;

        score = save_Score + meteo_Score + time_Score;

        ScoreText.text = "Score : " + (int)score;
    }
}
