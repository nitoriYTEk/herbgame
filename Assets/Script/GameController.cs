using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public AudioPlayer audio;

    public int score = 0;
    public Text scoreText;

    public int hitCount = 0;
    int maxHitCount = 0;
    public Text hitText;
    public float hitTime = 0;
    public Slider hitTimeSlider;
    public float maxHitTime;

    public Text scoreBonusText;

    float timeElapsed = -5.0f;
    public Text timeText;

    public float timeLimit;

    public Text fpsText;

    public Text countDownText;
    public Image[] countDownCircle;
    public Text[] resultText;

    int countState = 0;
    float sec;

    int[] scoreRankTable =
    {
        10000, 
        30000,
        90000,
        120000
    };

    int[] hitRankTable =
    {
        50,
        120,
        250,
        400
    };

    string[] rankText =
    {
        "D",
        "C",
        "B",
        "A",
        "S"
    };

    string[] totalRankText =
    {
        "D+",
        "C",
        "C+",
        "B",
        "B+",
        "A",
        "A+",
        "S",
        "S+"
    };

    // Use this for initialization
    void Start () {
        audio.Play(5);
	}
	
	// Update is called once per frame
	void Update () {

        timeElapsed += Time.deltaTime;
        timeText.text = Mathf.Min(timeLimit, Mathf.Max(0.0f, timeElapsed)).ToString("F2") + "s / " + timeLimit.ToString("F2") + "s";

        fpsText.text = (1.0f / Time.deltaTime).ToString("F1");

        if (timeElapsed < 1.0f)
        {
            sec += Time.deltaTime;
            if (sec > 1.0f)
            {
                if (countState == 0)
                {
                    audio.Play(4);
                    countDownText.text = "3";
                }
                else if (countState == 1)
                {
                    countDownText.text = "2";
                }
                else if (countState == 2)
                {
                    countDownText.text = "1";
                }
                else if (countState == 3)
                {
                    countDownText.text = "Start!";
                }
                else if (countState == 4)
                {
                    audio.PlayMusic();
                    countDownText.text = "";
                }
                sec -= 1.0f;
                countState++;
            }

            if(timeElapsed >= -4.0f && timeElapsed < -1.0f)
            {
                foreach (var circle in countDownCircle)
                {
                    circle.fillAmount = sec;
                }
            }
            else
            {
                foreach (var circle in countDownCircle)
                {
                    circle.fillAmount = 0;
                }
            }
        }
        else if(timeElapsed <= timeLimit + 3 && timeElapsed > timeLimit)
        {
            if(timeElapsed - Time.deltaTime <= timeLimit)
            {
                countDownText.text = "Finish!";
            }
        }
        else if(timeElapsed > timeLimit + 3)
        {
            if (timeElapsed - Time.deltaTime <= timeLimit + 3)
            {
                audio.Play(6);

                countDownText.text = "";

                int scoreRank = 0;
                int hitRank = 0;
                for(int i = 0; i < 4; i++)
                {
                    if (scoreRankTable[scoreRank] <= score) scoreRank++;
                    if (hitRankTable[hitRank] <= maxHitCount) hitRank++;
                }

                resultText[0].gameObject.SetActive(true);
                resultText[0].text =
                    "Result" + "\n" +
                    "Score :" + "\n" +
                    "Max HIT :" + "\n" +
                    "Total Rank :";
                resultText[1].gameObject.SetActive(true);
                resultText[1].text =
                    "" + "\n" +
                    score.ToString("#,0") + "   " + rankText[scoreRank] + "\n" +
                    maxHitCount.ToString() + "   " + rankText[hitRank] + "\n" +
                    totalRankText[scoreRank + hitRank];
                resultText[2].gameObject.SetActive(true);
            }


            if (Input.GetKeyDown("z"))
            {
                SceneManager.LoadScene("man");
            }
        }
        

        scoreText.text = "Score : " + score.ToString("#,0");

        hitTime -= Time.deltaTime;
        if (hitTime < 0)
        {
            hitTime = 0;
            hitCount = 0;
        }
        if (hitCount > 0)
        {
            hitText.text = hitCount.ToString() + " HIT！";
            scoreBonusText.text = "Score bonus : " + ((hitCount + 1000) / 10).ToString() + "%";
            hitTimeSlider.gameObject.SetActive(true);
            hitTimeSlider.value = hitTime / (maxHitTime - 1.0f);
        }
        else
        {
            hitText.text = "";
            scoreBonusText.text = "";
            hitTimeSlider.gameObject.SetActive(false);
        }
    }

    public void addScore(int score)
    {
        this.score += (score) * (hitCount + 1000) / 1000;
    }

    public void inclHit()
    {
        hitCount++;
        maxHitCount = hitCount > maxHitCount ? hitCount : maxHitCount;
        hitTime = maxHitTime;
    }

    public bool inputable()
    {
        return timeElapsed > 0 && timeElapsed < timeLimit;
    }

    public bool herbGeneratable()
    {
        return timeElapsed < timeLimit;
    }

    public float getTimeRatio()
    {
        return timeElapsed / timeLimit;
    }
}
