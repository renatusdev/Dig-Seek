using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Scoreboard : MonoBehaviour
{
    public static Scoreboard i; 

    public bool playerIsHider;
    public GameObject endScreenUI;

    public Sprite blueRoundWonUI;
    public Sprite redRoundWonUI;
    public Sprite finalRedRoundWonUI;
    public Sprite finalBlueRoundWonUI;
    
    public GameObject finalRoundWon;
    public GameObject[] redSide;
    public GameObject[] blueSide;

    [Header("Tools")]
    public Timer timer;
    public ScoreNotification scoreNotification;
    public ScoreHUD scoreHUD;

    public bool pointGiven;
    float blockPointAdditionTimer;

    private void Awake()
    {
        if(i == null)
        {
            Reset();
            i = this;
        }
        else
            Destroy(this);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Scoreboard");

        if (objs.Length > 1)
            Destroy(this.gameObject);

        timer.actionOnStop = () => Scoreboard.i.AddBluePoint(ScoreType.HID);

        DontDestroyOnLoad(gameObject);
    }

    public void AddBluePoint(ScoreType scoreType)
    {
        if(pointGiven)
        {
            Debug.Log("[SCOREBOARD] score type [" + scoreType + "] tried to give point but was stopped." );
            return;
        }


        int roundsWon = PlayerPrefs.GetInt("HiderRounds") + 1;
        PlayerPrefs.SetInt("HiderRounds", roundsWon);

        if(playerIsHider)
        {
            if(scoreType.Equals(ScoreType.HID))
            {
                int currentTime = Mathf.RoundToInt(timer.GetTime());
                int pointsOnHid = (((timer.startTime - currentTime)/timer.startTime) * ScoreAmount.pointsOnHid);
                
                AddScore("HiderScore", pointsOnHid);
                scoreNotification.Play(ScoreType.HID, pointsOnHid);
            }
            else
            {
                AddScore("HiderScore", ScoreAmount.pointOnKill);
                scoreNotification.Play(ScoreType.ELIMINATION, ScoreAmount.pointOnKill);
            }

        }
        else
        {
            throw new System.Exception("The score logic when the player is the seeker has not been implemented.");
        }

        if(roundsWon == 5)
        {
            AddFinalPoint(finalBlueRoundWonUI, finalRoundWon);
        }
        else
        {
            AddPoint(blueRoundWonUI, blueSide[4-roundsWon]);
        }
    }

    public void AddRedPoint()
    {
        if(pointGiven)
            return;

        int roundsWon =  PlayerPrefs.GetInt("SeekerRounds") + 1;        
        PlayerPrefs.SetInt("SeekerRounds", roundsWon);

        if(playerIsHider)
        {
            float totTime = timer.startTime;
            float currentTime = Mathf.RoundToInt(timer.GetTime());
            int penaltyOnDeath = -Mathf.RoundToInt(ScoreAmount.penaltyOnDeath  + (ScoreAmount.penaltyOnDeath *(currentTime/totTime)));

            AddScore("HiderScore", penaltyOnDeath);
            scoreNotification.Play(ScoreType.KILLED, penaltyOnDeath);
        }
        else
        {
            throw new System.Exception("The score logic when the player is the seeker has not been implemented.");
        }

        if(roundsWon == 5)
        {
            AddFinalPoint(finalRedRoundWonUI, finalRoundWon);
        }
        else
        {
            AddPoint(redRoundWonUI, redSide[4-roundsWon]);
        }
    }

    private void AddPoint(Sprite scoreUI, GameObject scoreHolder)
    {
        GameObject.FindGameObjectWithTag("Enemy").GetComponent<Controller>().Freeze();

        timer.Stop();

        // Animations
        scoreHolder.transform.DOPunchRotation(Vector3.forward * 270, 0.4f, 10, 10);
        scoreHolder.transform.DOPunchScale(Vector2.one, 0.5f, 8, 4);

        // Important for this to be in this order
        pointGiven = true;
        scoreHolder.GetComponent<Image>().sprite = scoreUI;
        LevelLoader.i.LoadLevel(1f, () => { pointGiven = false; timer.Reset(); });
    }

    private void AddFinalPoint(Sprite scoreUI, GameObject scoreHolder)
    {
        timer.Stop();
        // Animations
        scoreHolder.transform.DOPunchRotation(Vector3.forward * 270, 0.4f, 10, 10);
        scoreHolder.transform.DOPunchScale(Vector2.one, 0.5f, 8, 4);
        
        pointGiven = true;
        scoreHolder.GetComponent<Image>().sprite = scoreUI;

        // Pop End Screen UI
        GameObject.FindGameObjectWithTag("Enemy").GetComponent<Controller>().Freeze();
        endScreenUI.SetActive(true);
    }

    public static void Reset()
    {
        PlayerPrefs.SetInt("HiderRounds", 0);
        PlayerPrefs.SetInt("HiderScore", 0);
        PlayerPrefs.SetInt("SeekerRounds", 0);
        PlayerPrefs.SetInt("SeekerScore", 0);
    }

    public void DiamondUp()
    {
        if(pointGiven)
            return;

        if(playerIsHider)
        {
            scoreNotification.Play(ScoreType.GEM, ScoreAmount.pointOnGem);
            AddScore("HiderScore", ScoreAmount.pointOnGem);
        }
        else
        {
            throw new System.Exception("The score logic when the player is the seeker has not been implemented.");
        }
    }

    private void AddScore(string to, int amount)
    {
        int score =  Mathf.Clamp(PlayerPrefs.GetInt(to) + amount, 0, 150);
        PlayerPrefs.SetInt(to, score);
        scoreHUD.Play(score);
    }

    // private void OnGUI()
    // {
    //     GUI.TextArea(new Rect(50,100,50,50), "Score: " + PlayerPrefs.GetInt("HiderScore"));
    // }
}