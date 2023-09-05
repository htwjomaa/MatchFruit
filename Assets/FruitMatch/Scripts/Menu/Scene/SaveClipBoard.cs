using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class SaveClipBoard : ScriptableObject
{
    [Header("General Settings")]
    public bool defaultModeText;
    public Language languageTextDefault;
    public bool[] levelNameDefaultTextBool;
    public bool[] leveldescriptionDefaultTextBool;
    public bool[] modeDefaultTextBool;

    [Header("Graphic Settings")]
    
    public LevelCategory LevelCategory = LevelCategory.NightJungle;
    public string BGName = "Night Safari";
    [FormerlySerializedAs("WholeCateogry")] public bool WholeCategory = false;
    
    [Header("Goal Settings")] 
    public List<PhaseGoals> PhaseGoalsList = new List<PhaseGoals>();

    private void Awake()
    {
        if (FruitClipboardParent.Length == 0)
            CreateFruitClipBoardForSaveClipBoard();
    }

    [Button()]
    public void InitPhaseGoalList() => GenericSettingsFunctions.InitPhaseGoalList(ref PhaseGoalsList);

    [Header("Config Settings")] 
    public LevelConfig LevelConfig;
    
    [Header("Board Settings")]
    public int[] BoardWidth;
    public int[] BoardHeight;
    public bool[] AllowTip;
    public bool[] DestroyTargetOnly;
    public bool[] AddToLastField;
    public bool[] NoMatches;
    public EndGameRequirements[] GameTypeP1;
     public int[] TipDelay;
    public bool BorderGraphic;
    
    [Header("TileKinds")] 
    
    public TyleTypeSetting[,,] TyleTypeSettings;


    [Header("Match rules")] 
    public Row[] Row;
    public Diagonal[] Diagonal;
    public Pattern[] Pattern1;
    public Pattern[] Pattern2;
    public bool[] BlockCombine;
    public int[] penalty;
    public bool[] SequenceEnabled;
    public uint[] RowMatchValue;
    public uint[] DiagonalMatchValue;
    public uint[] Pattern1MatchValue;
    public uint[] Pattern2MatchValue;
    
    public Phase[] RowPhase;
    public Phase[] DiagonalPhase;
    public Phase[] Pattern1Phase;
    public Phase[] Pattern2Phase;

    public FruitType[] GoalFruitOne;
    public Colors[] GoalColorOne;
    public FruitType[] GoalFruitTwo;
    public Colors[] GoalColorTwo;
    public FruitType[] GoalFruitThree;
    public Colors[] GoalColorThree;

    [Header("Archievment Settings")]
    public bool Star1Enabled;
    [FormerlySerializedAs("Star1NextLevel")] public bool Star1NeededNextLevel;
    public float Star1Value;
    
    public bool Star2Enabled;
    [FormerlySerializedAs("Star2NextLevel")] public bool Star2NeededNextLevel;
    public float Star2Value;
    
    public bool Star3Enabled;
    [FormerlySerializedAs("Star3NextLevel")] public bool Star3NeededNextLevel;
    public float Star3Value;
    
    public bool TrophyEnabled;
    [FormerlySerializedAs("TrophyNextLevel")] public bool TrophyNeededNextLevel;
    public bool TrophyNoEmpty;
    public bool TrophyNumberAreMovesOrTime;
    public float TrophyValue;
    
    
    [Header("Bombs Settings")]
    public bool horizontalBomb;
    public bool verticalBomb;
    [FormerlySerializedAs("diagonalBomb")] public bool searchBomb;
    public bool fruitBomb;
    public bool colorBomb;
    public bool allBombs;

    
    public MatchStyle matchStyleHorBomb;
    public ushort horBombMatchNumber;
    
    public MatchStyle matchStyleVertBomb;
    public ushort vertBombMatchNumber;
    
    [FormerlySerializedAs("matchStyleDiagonalBomb")] public MatchStyle matchStyleSearchBomb;
    [FormerlySerializedAs("diagonalBombMatchNumber")] public ushort searchBombMatchNumber;
    
    public MatchStyle matchStyleFruitBomb;
    public ushort fruitBombMatchNumber;
    
    public MatchStyle matchStyleColorBomb;
    public ushort colorBombMatchNumber;
    
    
    [Header("Fruit Settings ")]

    public FruitsConfigParent[] FruitClipboardParent;

    [Header("Luck Settings ")]
    
    [Range(0, 1)] public float[] NeededPieces;
    [Range(0, 1)] public float[] NeededPiecesOverTime;
    [Range(0, 1)] public float[] BeneficialExtras;
    [Range(0, 1)] public float[] BeneficialExtrasOverTime;
    [Range(0, 1)] public float[] MaliciousExtras;
    [Range(0, 1)] public float[] MaliciousExtrasOverTime;
    [Range(0, 1)] public float[] Overall;
    public bool[] NeededPiecesOnlyStart;
    public bool[] BeneficialExtrasOnlyStart;
    public bool[] MaliciousExtrasOnlyStart;

    [Header("Layout Settings ")] 
    [Range(0, 100)] public float[] ZoomOutValue;
    [Range(0, 100)] public float[] TopPaddingValue;
    [Range(0, 100)] public float[] LeftPaddingValue;
    [Range(0, 100)] public float[] BottomPaddingValue;
    [Range(0, 100)] public float[] RightPaddingValue;


    [Header("SideDot ")]
    public SideFruitsFieldConfig SideFruitsFieldConfigs = new SideFruitsFieldConfig();
    public TileSettingFieldConfig[] TileSettingFieldConfigs  = new TileSettingFieldConfig[SaveFileLevelConfigs.Fields];
    
    private static int enumCountFruitType()
    {
        int counter = 0;
        foreach (FruitType fruitType in Enum.GetValues(typeof(FruitType)))
            counter++;
        
        return counter;
    }

    [Button()]
    private void CreateFruitClipBoardForSaveClipBoard() => CreateFruitClipboardParentList(ref FruitClipboardParent);
    public void CreateFruitClipboardParentList(ref FruitsConfigParent[] fruitParent )
    {

        int fruitTypeCount = 9;
        Debug.Log("fruitTypeCount " + fruitTypeCount);
        Array.Clear(fruitParent,0,fruitParent.Length);
        fruitParent = new FruitsConfigParent[fruitTypeCount];
        
        for (int i = 0; i < fruitTypeCount-1; i++)
        {
            FruitsConfig[] fruitClipboards = new FruitsConfig[fruitTypeCount];
            for (int j = 0; j < fruitTypeCount; j++)
            {
                fruitClipboards[j] = new FruitsConfig(GetFruitColor(j), false, 0, 0, 100, 0, false,
                    Phase.InP1AndP2);
            }

            fruitParent[i] = new FruitsConfigParent(GetFruitType(i), true, fruitClipboards);
        }
    }
    
    private FruitType GetFruitType(int fruitTypeInt)
    {
        FruitType fruitType = FruitType.BlaueFrucht;
        
        int counter = 0;
        foreach (FruitType type in Enum.GetValues(typeof(FruitType)))
        {
            if (fruitTypeInt == counter) fruitType = type;
            counter++;
        }

        return fruitType;
    }
    private Colors GetFruitColor(int fruitColorTypeInt)
    {
        Colors fruitColor = Colors.AlleFarben;
        
        int counter = 0;
        foreach (Colors colors in Enum.GetValues(typeof(Colors)))
        {
            if (fruitColorTypeInt == counter) fruitColor = colors;
            counter++;
        }

        return fruitColor;
    }
    
}

