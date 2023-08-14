using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.Level;
using UnityEngine;
using UnityEngine.Assertions.Must;



//This class is half depricated, but still needed for the code to function. It is for the TargetGoals
public sealed class GoalManager : MonoBehaviour
{
    public Queue<BlankGoal[]> _goalsP1Queue;
    public Queue<BlankGoal[]> _goalsP2Queue;
    public GameObject goalPrefab;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;
    public List<GoalPanel> currentGoals = new List<GoalPanel>();
    [SerializeField] public List<ObjectiveSettings> listPhase1 = new List<ObjectiveSettings>();
    [SerializeField] public List<ObjectiveSettings> listPhase2 = new List<ObjectiveSettings>();

    public void TranslateGoals()
    {
        // bool nullCheckGoalEntry = Rl.board == null && Rl.board.world == null &&
        //                           6 > Rl.board.world.levels.Length &&
        //                           Rl.board.world.levels[6] == null;
        // if (nullCheckGoalEntry) return;   //early out
        
        
        // destroy = destroy the given amount or destroy as much as possible
        // avoid = try not to make this combo
        // empty = empty all goals of this color
        // nothing = no goal here;
        
       listPhase1?.Clear();
       listPhase2?.Clear();

       int level = LevelManager.THIS.currentLevel - 1;
       
       listPhase1 = AddAllEnabledObjectiveSettings(listPhase1, level, PhaseNumber.P1);
       listPhase2 = AddAllEnabledObjectiveSettings(listPhase2, level, PhaseNumber.P2);

       listPhase1 = SetAllToAdditive(listPhase1);
       listPhase2 = SetAllToAdditive(listPhase2);
       
       listPhase1 = RemoveAllNothingObjectives(listPhase1);
       listPhase2 = RemoveAllNothingObjectives(listPhase2);

       listPhase1 = RemoveEmptyGoals(listPhase1);
       listPhase2 = RemoveEmptyGoals(listPhase2);
       
       List<List<ObjectiveSettings>> objectiveSettingsPhase1Clumped = ClumpObjectSettings(listPhase1);
       List<List<ObjectiveSettings>> objectiveSettingsPhase2Clumped = ClumpObjectSettings(listPhase2);

       List<PhaseGoal[]> phase1GoalsClumped = ClumpPhaseGoals(objectiveSettingsPhase1Clumped);
       List<PhaseGoal[]> phase2GoalsClumped = ClumpPhaseGoals(objectiveSettingsPhase2Clumped);
       List<List<BlankGoal>>  blankGoalPhase1List = GetBlankGoalLists(phase1GoalsClumped);
       List<List<BlankGoal>>  blankGoalPhase2List = GetBlankGoalLists(phase2GoalsClumped);

       SetBlankGoalQueues(blankGoalPhase1List, ref _goalsP1Queue);
       SetBlankGoalQueues(blankGoalPhase2List, ref _goalsP2Queue);

       LoadNextGoals();
    }
    
    //----Remove this later once  goal only and additive are enabled
    private List<ObjectiveSettings> SetAllToAdditive( List<ObjectiveSettings> objectiveSettings)
    {
        for (int i = 0; i < objectiveSettings.Count; i++)
        {
            objectiveSettings[i] = new ObjectiveSettings(objectiveSettings[i].PhaseGoalArray, objectiveSettings[i].ObjectiveNumber,
                objectiveSettings[i].Enabled, objectiveSettings[i].AllowSimilar, false, true);
        }

        return objectiveSettings;
    }
    //------------------------------------------------------------
    // private void ControlAllColors()
    // {
    //     //remove this method once colors are in the game
    //     for (int i = 0; i < Rl.world.levels[6].levelGoals.Length; i++)
    //         Rl.world.levels[6].levelGoals[i].fruitColors = Colors.AlleFarben;
    // }
    public void LoadNextGoals()
    {
        if(_goalsP1Queue.Count > 0) Rl.world.levels[6].levelGoals = _goalsP1Queue.Dequeue();
        else Rl.world.levels[6].levelGoals = _goalsP2Queue.Dequeue();
    }
    /*public void LoadGoals()
    {
        List <BlankGoal> blankGoalHelperList = Rl.world.levels[6].levelGoals.ToList();
        for (int i = 0; i < Rl.world.levels[6].levelGoals.Length; i++)
        {
            if(Rl.world.levels[6].levelGoals[i].numberCollected >=
                Rl.world.levels[6].levelGoals[i].numberNeeded)
                
                blankGoalHelperList.Remove(blankGoalHelperList[i]);
        }

        while (blankGoalHelperList.Count < 4 || _goalsP1Queue.Count > 0)
        {
            var n = _goalsP1Queue.Dequeue().ToList()
            blankGoalHelperList.Add(.ToList());
        }
    }*/

    private void SetBlankGoalQueues(List<List<BlankGoal>> allBlankGoals, ref Queue<BlankGoal[]> goalQueue)
    {
        goalQueue = new Queue<BlankGoal[]>();
        if (allBlankGoals.Count == 0) return;
        for (int i = 0; i < allBlankGoals.Count; i++)
        {
            goalQueue.Enqueue(allBlankGoals[i].ToArray());
        }
       
    }
    private List<List<BlankGoal>>  GetBlankGoalLists( List<PhaseGoal[]> phaseGoalsClumped)
    {
        List<List<BlankGoal>> allBlankGoals = new List<List<BlankGoal>>();
        
        for (int i = 0; i < phaseGoalsClumped.Count; i++)
        {
            List<BlankGoal> blankGoals = new List<BlankGoal>();
            for (int j = 0; j < phaseGoalsClumped[i].Length; j++)
            {
       
                BlankGoal blankGoal = new BlankGoal();
                blankGoal.numberNeeded = (int)phaseGoalsClumped[i][j].CollectionAmount;
                blankGoal.collectionStyle = phaseGoalsClumped[i][j].CollectionStyle;
                blankGoal.fruitType = phaseGoalsClumped[i][j].GoalFruit;
                blankGoal.fruitColors = phaseGoalsClumped[i][j].GoalColor;
                blankGoal.goalSprite = Rl.world.GetGoalSprite(phaseGoalsClumped[i][j].GoalFruit);
                blankGoals.Add(blankGoal);
            }
       
            allBlankGoals.Add(blankGoals);
        }
        return allBlankGoals;
    }
    private List<PhaseGoal[]> ClumpPhaseGoals( List<List<ObjectiveSettings>> objectiveSettingClumped)
    {
        List<PhaseGoal[]> clumpedPhaseGoals = new List<PhaseGoal[]>();
        
        for (int i = 0; i < objectiveSettingClumped.Count; i++)
        {
            List<PhaseGoal> phaseGoals = new List<PhaseGoal>();
            for (int j = 0; j < objectiveSettingClumped[i].Count; j++)
            {
               
                for (int k = 0; k < objectiveSettingClumped[i][j].PhaseGoalArray.Length; k++)
                {
                
                    if(objectiveSettingClumped[i][j].PhaseGoalArray[k].CollectionStyle != CollectionStyle.Nothing)
                        phaseGoals.Add(objectiveSettingClumped[i][j].PhaseGoalArray[k]);
                }
            }
            
            clumpedPhaseGoals.Add(phaseGoals.ToArray());
            phaseGoals.Clear();
        }

        return clumpedPhaseGoals;
    }
    private List<List<ObjectiveSettings>> ClumpObjectSettings(List<ObjectiveSettings> objectiveSettingsList)
    {List<List<ObjectiveSettings>> allObjectiveSettingsClumped = new List<List<ObjectiveSettings>>();

        List<ObjectiveSettings> objectSettingClump = new List<ObjectiveSettings>();
        bool addClump = false;
        bool lastMember = false;
        for (int i = 0; i < objectiveSettingsList.Count; i++)
        {
            if (i == objectiveSettingsList.Count - 1) 
                lastMember = true;
            if (objectiveSettingsList[i].Additive)
                objectSettingClump.Add(objectiveSettingsList[i]);
            else addClump = true;

            if (addClump)
            {
                addClump = false;
                allObjectiveSettingsClumped.Add(objectSettingClump);
                objectSettingClump = new List<ObjectiveSettings>();
                objectSettingClump.Add(objectiveSettingsList[i]);
                if(lastMember) allObjectiveSettingsClumped.Add(objectSettingClump);
            }
        }
        allObjectiveSettingsClumped.Add(objectSettingClump);
        return allObjectiveSettingsClumped;
    }
    
    private List<ObjectiveSettings> RemoveEmptyGoals(List<ObjectiveSettings> objectiveSettingsList)
    {
        for (int i = 0; i < objectiveSettingsList.Count; i++)
        {
            bool allNothing = true;
            for (int j = 0; j < objectiveSettingsList[i].PhaseGoalArray.Length; j++)
            {
                if (objectiveSettingsList[i].PhaseGoalArray[j].CollectionStyle != CollectionStyle.Nothing )
                    allNothing = false;
            }

            if (allNothing) objectiveSettingsList.Remove(objectiveSettingsList[i]);
        }
        
        return objectiveSettingsList;
    }

    private List<ObjectiveSettings> RemoveAllNothingObjectives(List<ObjectiveSettings> objectiveSettingsList)
    {
        for (int i = 0; i < objectiveSettingsList.Count; i++)
        {
            bool everyThingIsNothing = true;
            for (int j = 0; j < objectiveSettingsList[i].PhaseGoalArray.Length; j++)
            {
            
                if (objectiveSettingsList[i].PhaseGoalArray[j].GoalFruit != FruitType.Nothing)
                    everyThingIsNothing = false;
            }

            if (everyThingIsNothing) objectiveSettingsList.Remove(objectiveSettingsList[i]);
        }

        return objectiveSettingsList;
    }

    private List<ObjectiveSettings> AddAllEnabledObjectiveSettings(List<ObjectiveSettings> objectiveSettingsList, int level, PhaseNumber phaseNumber)
    {
        for (int i = 0;
             i < Rl.saveFileLevelConfigManagement
                 .AllSaveFileLevelConfigs.LevelConfigs[level]
                 .GoalConfig.PhaseGoalsList[(int)phaseNumber].ObjectiveSettingsArray.Length;
             i++)

        {
            if (Rl.saveFileLevelConfigManagement
                .AllSaveFileLevelConfigs.LevelConfigs[level]
                .GoalConfig.PhaseGoalsList[(int)phaseNumber].ObjectiveSettingsArray[i].Enabled)
                objectiveSettingsList.Add(Rl.saveFileLevelConfigManagement
                    .AllSaveFileLevelConfigs.LevelConfigs[level]
                    .GoalConfig.PhaseGoalsList[(int)phaseNumber].ObjectiveSettingsArray[i]);
        }
        return objectiveSettingsList;
    }
//     private void Start()
//     {
//         TranslateGoals();
//        // SetupGoals(true);
//         //ResetGoalsColleted();
//      //   StartCoroutine(AddASecondCo());
//       //  StartCoroutine(AddSecondTodayPlayed());
// //        Rl.world.totalTimeAppUsedSec = Rl.world.totalTimePlayedInGame + Rl.world.totalTimeInMenu;
//         _localistedDoneString = LocalisationSystem.GetLocalisedString(localistedDoneString);
//         //InvokeRepeating(nameof(ControlAllColors), 2f,5f);
//     }

    private void GoalNullCheck()
    {
        if (Rl.world.LevelToLoad.scoreGoals.Length > 0) return;
        
        Array.Resize(ref Rl.world.LevelToLoad.scoreGoals, 3);
        Rl.world.LevelToLoad.scoreGoals[0] = 10000;
        Rl.world.LevelToLoad.scoreGoals[0] = 20000;
        Rl.world.LevelToLoad.scoreGoals[0] = 30000;
    }
    public void CompareStarGoals(float newScore)   //Update Stars if target hit
    {
        GoalNullCheck();
        if (newScore > Rl.world.LevelToLoad.scoreGoals[2])
            Rl.world.UpdateStars(Rl.world.LevelToLoad, new[] { true, true, true });
        else if (newScore > Rl.world.LevelToLoad.scoreGoals[1])
            Rl.world.UpdateStars(Rl.world.LevelToLoad, new[] { true, true, false });
        else if (newScore > Rl.world.LevelToLoad.scoreGoals[0])
            Rl.world.UpdateStars(Rl.world.LevelToLoad, new[] { true, false, false });
    }
    private void OnDestroy()
    {
        StopCoroutine(AddASecondCo());
        Rl.world.totalTimeAppUsedSec = Rl.world.totalTimePlayedInGame + Rl.world.totalTimeInMenu;
    }

    private void OnApplicationQuit()
    {
        StopCoroutine(AddASecondCo());
        Rl.world.totalTimeAppUsedSec = Rl.world.totalTimePlayedInGame + Rl.world.totalTimeInMenu;
    }

    public IEnumerator AddASecondCo()
    {
        yield return new WaitForSeconds(1);
        Rl.world.totalTimePlayedInGame++;
        StartCoroutine(AddASecondCo());
    }
 
    public IEnumerator AddSecondTodayPlayed()
    {
        if (Rl.world.LoadedDay != (byte)System.DateTime.Today.Day)
        {
            Rl.world.LoadedDay = (byte)System.DateTime.Today.Day;
            Rl.world.totalTimePlayedToday = 0;
        }
        yield return new WaitForSeconds(1);
        Rl.world.totalTimePlayedToday++;
        StartCoroutine(AddSecondTodayPlayed());
    }
    public void ResetGoalsColleted()
    {
        foreach (BlankGoal levelGoal in Rl.world.levels[6].levelGoals)
            levelGoal.numberCollected = 0;
    }
   
    void SetupGoals(bool isStartPhase)
    {
        for (int i = 0; i < goalGameParent.transform.childCount; i++)
        {
            currentGoals.Remove(goalGameParent.transform.GetChild(i).GetComponent<GoalPanel>());
            Destroy(goalGameParent.transform.GetChild(i).transform.gameObject);
        }
        for(int i = 0; i< Rl.world.levels[6].levelGoals.Length; i ++)
        {
            GameObject goal;
            GoalPanel panel;
          
            if (isStartPhase)
            {
                //Create a new Goal Panel at the goalIntroParent position
             goal = Instantiate(goalPrefab, goalIntroParent.transform.position, Quaternion.identity);
            goal.transform.SetParent(goalIntroParent.transform, false); 
            
            //Set the Image of the goal
             panel = goal.GetComponent<GoalPanel>();
            panel.thisSprite = Rl.world.levels[6].levelGoals[i].goalSprite;
            // panel.thisString = LevelGoals[i].numberNeeded - LevelGoals[i].numberCollected > 0 
            //     ? (LevelGoals[i].numberNeeded - LevelGoals[i].numberCollected).ToString() 
            //     : 0.ToString();

            panel.thisTextRect.localScale = new Vector3(1,1,1);
      
            }
            GameObject gameGoal = Instantiate(goalPrefab, goalGameParent.transform.position, Quaternion.identity);
            gameGoal.transform.SetParent(goalGameParent.transform, false); 
            
            //Set the Image of the goal
            panel = gameGoal.GetComponent<GoalPanel>();
            currentGoals.Add(panel);
            panel.thisSprite = Rl.world.levels[6].levelGoals[i].goalSprite;
             panel.thisString = Rl.world.levels[6].levelGoals[i].numberNeeded - Rl.world.levels[6].levelGoals[i].numberCollected > 0 
                ? (Rl.world.levels[6].levelGoals[i].numberNeeded - Rl.world.levels[6].levelGoals[i].numberCollected).ToString() 
                 : 0.ToString();
        }
    }

    [SerializeField] private string localistedDoneString = "done";
    private string _localistedDoneString = "";
 
    public void UpdateGoals()
    {
        /*int goalsCompleted = 0;
        for (int i = 0; i < Rl.world.levels[6].levelGoals.Length; i++)
        {
            currentGoals[i].thisText.text = Rl.world.levels[6].levelGoals[i].numberNeeded - Rl.world.levels[6].levelGoals[i].numberCollected > 0
                ? (Rl.world.levels[6].levelGoals[i].numberNeeded - Rl.world.levels[6].levelGoals[i].numberCollected).ToString()
                : _localistedDoneString;
            
            if (Rl.world.levels[6].levelGoals[i].numberCollected >= Rl.world.levels[6].levelGoals[i].numberNeeded)
                goalsCompleted++;
        }

        if (goalsCompleted >= Rl.world.levels[6].levelGoals.Length) 
            SetupNewGoals();

        if (_goalsP1Queue.Count == 0 && goalsCompleted >= Rl.world.levels[6].levelGoals.Length)
        {
            if(_goalsP2Queue.Count > 0) StartP2();
            else
            {
                if (Rl.endGameManager != null)
                {
                    Rl.endGameManager.WinGame();
                    Debug.Log("You win");
                }
                  
            }
        }*/
    }
    
    private void SetupNewGoals()
    {
        if (_goalsP1Queue.Count < 1) return;
        Rl.world.levels[6].levelGoals = _goalsP1Queue.Dequeue();
        SetupGoals(false);
        ResetGoalsColleted();
    }
    private void StartP2()
    {
        _goalsP1Queue.Clear();
        for (int i = 0; i < _goalsP2Queue.Count; i++)
        {
            _goalsP1Queue.Enqueue(_goalsP2Queue.Dequeue());
        }
        Rl.endGameManager.GetGameTypeP2();
        SetupNewGoals();
    }
    public void CompareGoal(string tag)
    {
        CompareGoal(Rl.world.GetFruitTypeFromTag(tag), Colors.AlleFarben);
    }

    public void CompareGoal(string tag, Colors fruitColorToCompare) => CompareGoal(Rl.world.GetFruitTypeFromTag(tag), fruitColorToCompare);

    public void CompareGoal( FruitType fruitTypeToCompare, Colors fruitColorToCompare)
    {
        //yes lot of ifs....  should make a better solution
        for (int i = 0; i < Rl.world.levels[6].levelGoals.Length; i++)
        {
            if (Rl.world.levels[6].levelGoals[i].fruitType == FruitType.Bubble 
                || Rl.world.levels[6].levelGoals[i].fruitType == FruitType.Jelly 
                || Rl.world.levels[6].levelGoals[i].fruitType == FruitType.Lock 
                || Rl.world.levels[6].levelGoals[i].fruitType == FruitType.Truhe)
            {
                if (fruitTypeToCompare == Rl.world.levels[6].levelGoals[i].fruitType)
                {
                    Rl.world.levels[6].levelGoals[i].numberCollected++;
                }
            }
            else
            {
                if (
                    (fruitColorToCompare == Rl.world.levels[6].levelGoals[i].fruitColors  || Rl.world.levels[6].levelGoals[i].fruitColors == Colors.AlleFarben)
                    && (fruitTypeToCompare == Rl.world.levels[6].levelGoals[i].fruitType || Rl.world.levels[6].levelGoals[i].fruitType == FruitType.AlleFrüchte)
                    )
                {
                    Debug.Log("Collected: " +  Rl.world.levels[6].levelGoals[i].fruitType);
                    Rl.world.levels[6].levelGoals[i].numberCollected++;
                }
            }
          
        }
    }
    
    /*private List<List<ObjectiveSettings>> ClumpObjectSettings(List<ObjectiveSettings> objectiveSettingsList)
   {
       List<List<ObjectiveSettings>> allObjectiveSettingsClumped = new List<List<ObjectiveSettings>>();

       List<ObjectiveSettings> objectSettingClump = new List<ObjectiveSettings>();
       int clumpCounter = 0;
       for (int i = 0; i < objectiveSettingsList.Count; i++)
       {
           if (objectiveSettingsList[i].Additive)
           {
               // objectSettingClump.Add(objectiveSettingsList[i]);
               objectSettingClump[clumpCounter] = objectiveSettingsList[i];
           }
           else if (!objectiveSettingsList[i].Additive)
           {
               allObjectiveSettingsClumped.Add(objectSettingClump);
               objectSettingClump.Clear();
               objectSettingClump.Add(objectiveSettingsList[i]);
               // if (objectSettingClump.Count == 0)
               // {
               //     ;
               // }
               //
           }
       }
       allObjectiveSettingsClumped.Add(objectSettingClump);

       return allObjectiveSettingsClumped;
   }*/
}