using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public float score;
    public Image scoreBar;
    [SerializeField] private int CounterFPS = 15;
    [SerializeField] private float durationCount = 1.2f;
    private float _CashedValue;
    private Coroutine NumberCounter_Coroutine;
    public double oldHighScore = 0;
    [SerializeField] TMP_Text[] FindPointTMPs;

    private void Start()
    {
        oldHighScore = Rl.board.world.GetHighscore(Rl.board.world.LevelToLoad);
    }

    private void CompareAndUpdateHighScore()
    {
        if (score > oldHighScore)
        {
            Rl.board.world.UpdateHighScore(Rl.board.world.LevelToLoad, score);
            Rl.goalManager.CompareStarGoals(score);
        }
    }


    private void Update()
    {
        ScoreText.text = "" + score;
        CompareAndUpdateHighScore();
    }

    public void IncreaseScore(int amountToIncrease)
    {
        UpdateUIScore(score + amountToIncrease);
        
        if (Rl.board != null && scoreBar != null && Rl.board.scoreGoals.Length > 0)
        {
            int length = Rl.board.scoreGoals.Length;
            scoreBar.fillAmount = (float)score / (float)Rl.board.scoreGoals[length-1];
        }
    }
    
    public void UpdateUIScore(float newValue)
    {
        if (NumberCounter_Coroutine != null) StopCoroutine(NumberCounter_Coroutine);
        NumberCounter_Coroutine = StartCoroutine(CountText(newValue));
        _CashedValue = newValue;
    }
    IEnumerator CountText(float newValue)
    {
        WaitForSeconds Wait = new WaitForSeconds(1f / CounterFPS);
        float prevValue = _CashedValue, stepSpeed;
        if (newValue - prevValue < 0) stepSpeed = Mathf.FloorToInt((newValue - prevValue) / (CounterFPS * durationCount));
        else stepSpeed = Mathf.CeilToInt((newValue - prevValue) / (CounterFPS * durationCount));
        if (prevValue < newValue)
        { 
            while(prevValue < newValue)
            {
                prevValue += stepSpeed;
                if (prevValue > newValue) prevValue = newValue;
                score = (int)prevValue;
                ScoreText.SetText(prevValue.ToString());
                yield return Wait;  
            }
        }
    }
}
