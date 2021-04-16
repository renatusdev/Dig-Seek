using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScoreUI : MonoBehaviour
{
    public TextMeshProUGUI uiHScore;
    public TextMeshProUGUI uiScore;
    public TMP_StyleSheet highscoreStyle;

    public Timer speedRunTimer;

    private void Start()
    {
        // If not a new highscore, set to normal text
        // If a new highscore, set to exotic text.

        int score = PlayerPrefs.GetInt("HiderScore");
        int highscore = PlayerPrefs.GetInt("HiderHScore");

        if(score > highscore)
        {
            PlayerPrefs.SetInt("HiderHScore", score);
            uiHScore.textStyle = highscoreStyle.GetStyle("H1");
            uiHScore.SetText("Highscore: " + score);
        }
        else
        {
            uiHScore.textStyle = highscoreStyle.GetStyle("Normal");
            uiHScore.SetText("Highscore: " + highscore);
        }

        uiScore.SetText("Score: " + score);
    }
}
