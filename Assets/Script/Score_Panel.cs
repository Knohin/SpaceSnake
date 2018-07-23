using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score_Panel : MonoBehaviour {

    public Text save_Score;
    public Text meteo_Score;
    public Text time_Score;

    public Text total_Score;
    
	// Update is called once per frame
	void Update () {

        save_Score.text = ((int)ScoringManager.save_Score).ToString();
        meteo_Score.text = ((int)ScoringManager.meteo_Score).ToString();
        time_Score.text = ((int)ScoringManager.time_Score).ToString();

        total_Score.text = ((int)ScoringManager.score).ToString();
    }
}
