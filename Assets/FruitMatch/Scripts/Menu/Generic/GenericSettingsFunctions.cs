using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public sealed class GenericSettingsFunctions : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private bool onMouseUpSound;
    public void OnPointerUp(PointerEventData eventData)
    {
        // if (!onMouseUpSound) return;
        Rl.GameManager.PlayAudio(Rl.soundStrings.SliderHandleSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    }
    private static float tweenDuration = 0.16f;
    private static Vector3 strength = new Vector3(-0.26f, -0.3f, 0);
    private static int vibrato = 13;
     private static float randomness = 20.7f;
    private static Ease ease = Ease.InOutBounce;
    public static void SmallJumpAnimation(params Transform[] transforms) => SmallJumpAnimation(tweenDuration, transforms);

    public static void SmallJumpAnimation(float duration, params Transform[] transforms)
    {
        for (int i = 0; i < transforms.Length; i++)
            transforms[i].DOPunchScale(strength, duration, vibrato, randomness).SetEase(ease);
    }
    public static void SmallShake(params Transform[] transforms) => SmallShake(tweenDuration, 2, transforms);
    public static void SmallShake(float duration, int jumps, params Transform[] transforms)
    {
        for (int i = 0; i < transforms.Length; i++)
            transforms[i]
                .DOJump(new Vector3(transforms[i].position.x, transforms[i].position.y, transforms[i].position.z),
                    50f, jumps , duration*2.5f, true);
    }
    public static Color ResetColorAlmostFull() => new Color(255, 5, 5, 0.95f);
    public static Color SetColorAndAlpha(Color newColor, float newAlpha) =>
        new Color(newColor.r, newColor.g, newColor.b, newAlpha);
    public static void ActivateGameObject(GameObject parentObj, byte switchValue)
    {
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            if (switchValue != parentObj.transform.GetChild(i).GetSiblingIndex())
            {
                parentObj.transform.GetChild(i).transform.gameObject.SetActive(true);
            }
            else if (switchValue == parentObj.transform.GetChild(i).GetSiblingIndex())
            {
                parentObj.transform.GetChild(i).transform.gameObject.SetActive(false);
            }
        }
    }
    public static string EnumToString(Type type, int i) => Enum.GetName(type, i);
    public static void Addlisteners(UnityAction<string> method, params TMP_InputField[] tmpInputField)
    {
        for (int i = 0; i < tmpInputField.Length; i++)
        {
            tmpInputField[i].onEndEdit.AddListener(method);
        }
    }
    public static void Addlisteners(UnityAction<float> method, params  Slider[] sliders)
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].onValueChanged.AddListener(method);
        }
    }
    public static Vector3 FindTheMidPoint(params GameObject[] midPointObj)
    {
        float xPos = 0, yPos = 0, zPos = 0, objCounter = 0;
        foreach (GameObject Object in midPointObj)
        {
            xPos += Object.transform.position.x;
            yPos += Object.transform.position.y;
            zPos += Object.transform.position.z;
            objCounter++;
        }
        xPos = xPos/ objCounter; yPos = yPos/ objCounter; zPos = zPos/ objCounter;

        return new Vector3(xPos, yPos, zPos);
    }

    static T Cast<T>(object obj, T type)
    {
        return (T)obj;
    }
    public static void CheckFieldArrays<T>( ref IEnumerable<T>  list)
    {
        var enumerable = list as T[] ?? list.ToArray();
        if(enumerable.Count() != SaveFileLevelConfigs.Fields)
     {
         T[] array = enumerable.ToArray();
         Array.Resize(ref array, SaveFileLevelConfigs.Fields);
         list = array;
     }
    }
   
    public static void RemoveListeners(params TMP_InputField[] tmpInputField)
    {
        for (int i = 0; i < tmpInputField.Length; i++)
            tmpInputField[i].onValueChanged.RemoveAllListeners();
    }
    public static void RemoveListeners(params Slider[] slider)
    {
        for (int i = 0; i < slider.Length; i++)
            slider[i].onValueChanged.RemoveAllListeners();
    }

    public static void UpdateTextFields(ref List<TextMeshProUGUI> text, params Slider[] sliders)
    {
        for (int i = 0; i < text.Count; i++) 
            text[i].text = sliders[i].value.ToString(CultureInfo.InvariantCulture);
    }
    public static void UpdateTextFields(ref List<TextMeshProUGUI> text, params string[] slidervalues)
    {
        for (int i = 0; i < text.Count; i++)
            text[i].text = slidervalues[i];
    }
    public static void UpdateTextFields(ref List<TMP_InputField> text, params string[] slidervalues)
    {
        for (int i = 0; i < text.Count; i++)
            text[i].text = slidervalues[i];
    }

    public static void ActivateGameObject(Transform transform, byte SwitchValue)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (SwitchValue != transform.GetChild(i).GetSiblingIndex())
            {
                transform.GetChild(i).transform.gameObject.SetActive(false);
            }
            else if (SwitchValue == transform.GetChild(i).GetSiblingIndex())
            {
                transform.GetChild(i).transform.gameObject.SetActive(true);
            }
        }
    }
    public static List<TextMeshProUGUI> AddToValueDisplayList(params TextMeshProUGUI[] valueDisplay)
    {
        List<TextMeshProUGUI> valueDisplayList = new List<TextMeshProUGUI>(valueDisplay.Length);
        for (int i = 0; i < valueDisplay.Length; i++) 
            valueDisplayList.Add(valueDisplay[i]);
        return valueDisplayList;
    }
    public static List<TMP_InputField> AddToValueDisplayList(params TMP_InputField[] valueDisplay)
    {
        List<TMP_InputField> valueDisplayList = new List<TMP_InputField>(valueDisplay.Length);
        for (int i = 0; i < valueDisplay.Length; i++) 
            valueDisplayList.Add(valueDisplay[i]);
        return valueDisplayList;
    }
    public static bool DestroyAllChildren(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++) Destroy(parent.transform.GetChild(i).gameObject);
        return true;
    }
    public static object GetDeepCopy(object input)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, input);
            stream.Position = 0;
            return formatter.Deserialize(stream);
        }
    }
private static ObjectiveSettings[]  InitObjectSettings()
{
    ObjectiveSettings[] objectiveSettingsArray = new ObjectiveSettings[enumCountObjectiveNumber()];
    for(int i = 0; i < enumCountObjectiveNumber(); i++)
    {
        objectiveSettingsArray[i].ObjectiveNumber = GetObjectiveNumber(i);
        if (i == 0) objectiveSettingsArray[i].Enabled = true;
        else objectiveSettingsArray[i].Enabled = false;
        objectiveSettingsArray[i].AllowSimilar = false;
        objectiveSettingsArray[i].PhaseGoalArray = InitPhaseGoals();
    }

    return objectiveSettingsArray;
}
private static PhaseGoal[] InitPhaseGoals()
{

    PhaseGoal[] phaseGoalArray = new PhaseGoal[enumCountGoalNumber()];
    for(int i = 0; i< enumCountGoalNumber(); i++)
    {
        phaseGoalArray[i].GoalNumber = GetGoalNumber(i);
        phaseGoalArray[i].GoalColor = Colors.AlleFarben;
        phaseGoalArray[i].GoalFruit = FruitType.AlleFrüchte;
    }

    return phaseGoalArray;
}
private static PhaseGoals InitPhaseGoals(PhaseNumber phaseNumber) => new (phaseNumber,  InitObjectSettings());

private static GoalNumber GetGoalNumber(int goalNumber)
    {
        GoalNumber goalNumberEnum = GoalNumber.Goal1;
        if (goalNumber > enumCountGoalNumber() - 1) goalNumber = 0;

        int counter = 0;
        foreach (GoalNumber searchForGoalNumber in Enum.GetValues(typeof(GoalNumber)))
        {
            if (goalNumber== counter) goalNumberEnum  = searchForGoalNumber;
            counter++;
        }
        
        return goalNumberEnum;
    }
    
    private static int enumCountGoalNumber()
    {
        int counter = 0;
        foreach (GoalNumber doesNotMatterAtAll in Enum.GetValues(typeof(GoalNumber )))
            counter++;
        
        return counter;
    }

    public static ObjectiveNumber GetObjectiveNumber(int goalNumber)
    {
        ObjectiveNumber objectiveNumberEnum = ObjectiveNumber.ObjectiveNumberOne;
        if (goalNumber > enumCountObjectiveNumber() - 1) goalNumber = 0;

        int counter = 0;
        foreach (ObjectiveNumber searchForObjectiveNumber in Enum.GetValues(typeof(ObjectiveNumber)))
        {
            if (goalNumber== counter) objectiveNumberEnum  = searchForObjectiveNumber;
            counter++;
        }
        
        return objectiveNumberEnum;
    }
    private static int enumCountObjectiveNumber()
    {
        int counter = 0;
        foreach (ObjectiveNumber doesNotMatterAtAll in Enum.GetValues(typeof(ObjectiveNumber )))
            counter++;
        
        return counter;
    }

    public static void InitPhaseGoalList(ref List<PhaseGoals> phaseGoalsList)
    {
        phaseGoalsList = new List<PhaseGoals>();
        phaseGoalsList.Add(InitPhaseGoals(PhaseNumber.P1));
        phaseGoalsList.Add(InitPhaseGoals(PhaseNumber.P2));
    }

    public static int GetConstvaluesMovesTime(float percentValue, GameType gameType)
    {
        int calcualtedValue = 0;
        switch (gameType)
        {
            case GameType.Moves:
                calcualtedValue = (int)MathLibrary.Remap(0, 1, 0, ConstValues.MAXVALUEMOVES, percentValue);
                break;
            case GameType.Time:
             
                calcualtedValue = (int)MathLibrary.Remap(0, 1, 0, ConstValues.MAXVALUETIME, percentValue);
                break;
            case GameType.EmptyMoves:
                calcualtedValue = (int)MathLibrary.Remap(0, 1, 0, ConstValues.MAXVALUEMOVES, percentValue);
                break;
            case GameType.Nothing:
                calcualtedValue = 0;
                break;
            case GameType.NoEmptyMoves:
                calcualtedValue = (int)MathLibrary.Remap(0, 1, 0, ConstValues.MAXVALUEMOVES, percentValue);
                break;
        }

        if (calcualtedValue == 0 && gameType != GameType.Nothing)
        {
            if (gameType is GameType.Time) calcualtedValue = 30;
            else calcualtedValue = 0;
        }
        return calcualtedValue;
    }
    
    public static int GetConstValuesPoints(float percentValue)
    {
        int calculatedValue = (int)MathLibrary.Remap(0, 1, 0, ConstValues.MAXVALUEPOINTS, percentValue);
        return (calculatedValue+ 50) / 500 * 500;
    }
    public static int GetConstValuesTrophyEM(float percentValue)
    {
        return  (int)MathLibrary.Remap(0, 1, 0, 200, percentValue);
    }
    public static float GetFloatValueFromConstValuePoints(int constValue) => MathLibrary.Remap(0,ConstValues.MAXVALUEPOINTS, 0, 1, constValue);

    public static string TranslateTimeConstValues(int calculatedValue, GameType gameType)
    {
        if (gameType !=  GameType.Time) return calculatedValue.ToString() ;

        string formattedTime = "";
        float t = MathLibrary.Remap(0, ConstValues.MAXVALUETIME, 0, (int)(ConstValues.MAXVALUETIME/30), calculatedValue);
        if (calculatedValue > 30)
        {
            formattedTime += Mathf.FloorToInt(t / 2) + "m ";
            formattedTime += (int)t % 2 == 1 ? "30s" : "0s";
        }
        else formattedTime += "0m 0s";

        return formattedTime;
    }
    public static int EnumCount(Type enumType)
    {
        int counter = 0;
        foreach (var countUp in Enum.GetValues(enumType))
            counter++;
        
        return counter;
    }
}

