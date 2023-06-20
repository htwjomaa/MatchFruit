using System;
using FruitMatch.Scripts.Core;
using TMPro;
using UnityEngine; 
public enum GameType
{
    Moves,
    Time,
    Nothing,
    EmptyMoves,
    NoEmptyMoves
}
[Serializable]
public sealed class EndGameRequirements
{
    public GameType GameType;
    public float CounterValue;
}

public sealed class EndGameManager : MonoBehaviour
{
    public GameObject label;
    public TextMeshProUGUI counter;
    public EndGameRequirements requirements;
    public int currentCounterValue;
    private float timerSeconds;
    public GameObject youWinPanel;
    public GameObject tryAgainPanel;

    // Start is called before the first frame update
    void Start()
    {
        //GetGameType();
       // SetGameType();
      //  SetupGame();
    }

    public void GetGameTypeP2()
    {
        Debug.Log("P2-Started");
        GameType gameTypeP2 =  Rl.saveFileLevelConfigManagement
            .AllSaveFileLevelConfigs.LevelConfigs[Rl.board.level]
            .BoardDimensionsConfig.GameTypeP2.GameType;

        float counterValueP2 =    Rl.saveFileLevelConfigManagement
            .AllSaveFileLevelConfigs.LevelConfigs[Rl.board.level]
            .BoardDimensionsConfig.GameTypeP2.CounterValue;
        
        Rl.world.levels[Rl.board.level].endGameRequirements.CounterValue = GenericSettingsFunctions.GetConstvaluesMovesTime(counterValueP2, gameTypeP2);
        Rl.world.levels[Rl.board.level].endGameRequirements.GameType = gameTypeP2;
        requirements.GameType = gameTypeP2;
        
        //Removes this once it is implemented-----------
        if (Rl.world.levels[Rl.board.level].endGameRequirements.GameType is GameType.Nothing
            or GameType.EmptyMoves or GameType.NoEmptyMoves)
            Rl.world.levels[Rl.board.level].endGameRequirements.GameType = GameType.Moves;
        //-----------------------------------------------
        SetGameType();
        SetupGame();
    }
    private static void GetGameType()
    {

        GameType gameTypeP1 =  Rl.saveFileLevelConfigManagement
            .AllSaveFileLevelConfigs.LevelConfigs[Rl.board.level]
            .BoardDimensionsConfig.GameTypeP1.GameType;

        float counterValueP1 =    Rl.saveFileLevelConfigManagement
            .AllSaveFileLevelConfigs.LevelConfigs[Rl.board.level]
            .BoardDimensionsConfig.GameTypeP1.CounterValue;
        
        Rl.world.levels[Rl.board.level].endGameRequirements.CounterValue = GenericSettingsFunctions.GetConstvaluesMovesTime(counterValueP1, gameTypeP1);
        Rl.world.levels[Rl.board.level].endGameRequirements.GameType = gameTypeP1;
      
        
        //Removes this once it is implemented-----------
        if (Rl.world.levels[Rl.board.level].endGameRequirements.GameType is GameType.Nothing
            or GameType.EmptyMoves or GameType.NoEmptyMoves)
            Rl.world.levels[Rl.board.level].endGameRequirements.GameType = GameType.Moves;
        //-----------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        if (requirements.GameType == GameType.Time && currentCounterValue > 0)
        {
            timerSeconds -= Time.deltaTime;
            if (timerSeconds <= 0)
            {
                DecreaseCounterValue();
                timerSeconds = 1;
            }
        }
    }

    private float _minutes;
    private float _seconds;
    public void DecreaseCounterValue()
    {
        if (Rl.board.currentState == GameState.pause) return;
        currentCounterValue--;
        if (requirements.GameType == GameType.Time)
        {
            _minutes = currentCounterValue / 60;
            _seconds = currentCounterValue - (_minutes * 60);
            counter.text = _minutes + "min  " + _seconds + "sec";
        }
        else
        {
            counter.text = currentCounterValue.ToString();
        }
    
        if (currentCounterValue <= 0)
            LoseGame();
    }

    void changeLabel()
    {
        Debug.Log("Requirements " + requirements.GameType);
        if (requirements.GameType == GameType.Moves)
        {
            label.GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedValue("moves");
        }

        else if (requirements.GameType == GameType.Time)
        {
            timerSeconds = 1;
            label.GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedValue("time");
        }
  
        
    }
    void SetupGame()
    {
        // CONVERT IT TO THE RIGHT VALUE 
        currentCounterValue = (int)requirements.CounterValue;
        changeLabel();
        currentCounterValue++;
        DecreaseCounterValue();
        counter.text = "" + currentCounterValue;
    }
  public void SetGameType()
  {
      if (Rl.board.level >= Rl.board.world.levels.Length && Rl.board.world.levels[Rl.board.level] == null) return;
      requirements = Rl.board.world.levels[Rl.board.level].endGameRequirements;
  }
    public void WinGame()
    {
        youWinPanel.SetActive(true);
        Rl.board.currentState = GameState.win;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        Rl.fadePanelController.GameOver();
    }
    
    public void LoseGame()
    {
        tryAgainPanel.SetActive(true);
        Rl.board.currentState = GameState.lose;
        Debug.Log("You Lose!");
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        Rl.fadePanelController.GameOver();
    }
}