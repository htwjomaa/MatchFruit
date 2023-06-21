using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SaveFileLevelConfigs
{
    public LevelConfig[] LevelConfigs ;
    public DateInformation DateInformation;
    public double UniqueIdentifier;
    public static byte Fields = 4;
    public SaveFileLevelConfigs(LevelConfig[] levelConfigs, DateInformation dateInformation, double uniqueIdentifier)
    {
        LevelConfigs = levelConfigs;
        DateInformation = dateInformation;
        UniqueIdentifier = uniqueIdentifier;
    }
}

[Serializable]
public struct LevelConfig
{
    public BoardDimensionsConfig BoardDimensionsConfig;
    public ScoreGoalsConfig ScoreGoalsConfig;
    public LevelTextConfig[] LevelTextConfig;
    public GoalConfig GoalConfig;
    public SideFruitsFieldConfig SideFruitsFieldConfig;
    public AnimationConfig AnimationConfig;
    public DateInformation DateInformation;
    public BombConfig[] BombConfigs;
    public LuckConfig LuckConfig;
    public GraphicConfig GraphicConfig;
    public MatchFinderConfig matchFinderConfig;
    public LayoutFieldConfig LayoutFieldConfigs;
    public TileSettingFieldConfig[] TileSettingFieldConfigs;
    public ExtraSettings ExtraSettings;
    public LevelConfig(BoardDimensionsConfig boardDimensionsConfig, ScoreGoalsConfig scoreGoalsConfig,
        LevelTextConfig[] levelTextConfig, GoalConfig goalConfig, SideFruitsFieldConfig sideFruitsFieldConfig, AnimationConfig animationConfig, DateInformation dateInformation,
        BombConfig[] bombConfig, LuckConfig luckConfig, GraphicConfig graphicConfig, MatchFinderConfig matchFinderConfig,
        LayoutFieldConfig layoutFieldConfigs, TileSettingFieldConfig[] tileSettingFieldConfigs, ExtraSettings extraSettings)
    {
        BoardDimensionsConfig = boardDimensionsConfig;
        ScoreGoalsConfig = scoreGoalsConfig;
        LevelTextConfig = levelTextConfig;
        GoalConfig = goalConfig;
        Array.Resize(ref sideFruitsFieldConfig.SideFruitsConfig, SaveFileLevelConfigs.Fields);
        SideFruitsFieldConfig = sideFruitsFieldConfig;
        AnimationConfig = animationConfig;
        DateInformation = dateInformation;
        BombConfigs = bombConfig;
        LuckConfig = luckConfig;
        GraphicConfig = graphicConfig;
        this.matchFinderConfig = matchFinderConfig;
        Array.Resize(ref layoutFieldConfigs.LayoutConfig, SaveFileLevelConfigs.Fields);
        LayoutFieldConfigs = layoutFieldConfigs;
        Array.Resize(ref tileSettingFieldConfigs, SaveFileLevelConfigs.Fields);
        TileSettingFieldConfigs = tileSettingFieldConfigs;
        ExtraSettings = extraSettings;
    }
}

[Serializable]
public struct GraphicConfig
{
    public bool AllowBorderGraphic;
    public LevelCategory LevelCategory;
    public string BGName;
    public bool WholeCategory;

    public GraphicConfig(bool allowBorderGraphic, LevelCategory levelCategory, string bgName, bool wholeCategory)
    {
        AllowBorderGraphic = allowBorderGraphic;
        LevelCategory = levelCategory;
        BGName = bgName;
        WholeCategory = wholeCategory;
    }
}

[Serializable]
public struct LuckConfig
{
    [Range(0, 1)] public float NeededPieces;
    [Range(0, 1)] public float NeededPiecesOverTime;
    [Range(0, 1)] public float BeneficialExtras;
    [Range(0, 1)] public float BeneficialExtrasOverTime;
    [Range(0, 1)] public float MaliciousExtras;
    [Range(0, 1)] public float MaliciousExtrasOverTime;

    public LuckConfig(float neededPieces, float neededPiecesOverTime, float beneficialExtras, float beneficialExtrasOverTime, float maliciousExtras, float maliciousExtrasOverTime)
    {
        NeededPieces = neededPieces;
        NeededPiecesOverTime = neededPiecesOverTime;
        BeneficialExtras = beneficialExtras;
        BeneficialExtrasOverTime = beneficialExtrasOverTime;
        MaliciousExtras = maliciousExtras;
        MaliciousExtrasOverTime = maliciousExtrasOverTime;
    }
}

[Serializable]
public struct Fruit
{
    public FruitType FruitType;
    public Colors FruitColor;
    public bool isActive;
    public bool isPositive;
    [Range(0, 100)] public float SpawnRate;
    [Range(-10, 10)] public int ChangeSpawnRateOverTimeOrMoves;
    public bool spawnLater;
    public bool SpawnOnlyAfterFirstGoalIsMet;
    public bool spawnOnlyInRoundTwo;
    public bool spawnOnlyAfterBoardIsFilled;

    public Fruit(FruitType fruitType, Colors fruitColor, bool isActive, bool isPositive, float spawnRate, int changeSpawnRateOverTimeOrMoves, bool spawnLater, bool spawnOnlyAfterFirstGoalIsMet, bool spawnOnlyInRoundTwo, bool spawnOnlyAfterBoardIsFilled)
    {
        FruitType = fruitType;
        FruitColor = fruitColor;
        this.isActive = isActive;
        this.isPositive = isPositive;
        SpawnRate = spawnRate;
        ChangeSpawnRateOverTimeOrMoves = changeSpawnRateOverTimeOrMoves;
        this.spawnLater = spawnLater;
        SpawnOnlyAfterFirstGoalIsMet = spawnOnlyAfterFirstGoalIsMet;
        this.spawnOnlyInRoundTwo = spawnOnlyInRoundTwo;
        this.spawnOnlyAfterBoardIsFilled = spawnOnlyAfterBoardIsFilled;
    }
}


/*
[Serializable]
public struct GoalFruit
{
    public FruitType FruitType;
    public Colors FruitColor;
    public int Goal;
    public bool CountBackwards;

    public GoalFruit(FruitType fruitType, Colors fruitColor, int goal, bool countBackwards)
    {
        FruitType = fruitType;
        FruitColor = fruitColor;
        Goal = goal;
        CountBackwards = countBackwards;
    }
}


[Serializable]
public struct Objectives
{
    public GoalFruit[] GoalFruits;
    public bool AllowSimilar;
    public bool Enabled;

    public Objectives(GoalFruit[] goalFruits, bool allowSimilar, bool enabled)
    {
        GoalFruits = goalFruits;
        Array.Resize(ref GoalFruits, 5); //hardcoded for 5
        AllowSimilar = allowSimilar;
        Enabled = enabled;
    }
}
*/

/*[Serializable]
public struct GoalConfig
{
    public Objectives[] Objectives;
    public PhaseNumber PhaseNumber;

    public GoalConfig(Objectives[] objectives, PhaseNumber phaseNumber)
    {
        Objectives = objectives;
        Array.Resize(ref Objectives, 3); //hardcoded for 3
        PhaseNumber = phaseNumber;
    }
}*/
[Serializable]
public struct ExtraSettings
{
    public bool HideTargets;

    public ExtraSettings(bool hideTargets)
    {
        this.HideTargets = hideTargets;
    }
}
[Serializable]
public struct GoalConfig
{
    public List<PhaseGoals> PhaseGoalsList;
    
    public GoalConfig(List<PhaseGoals> phaseGoalsList)
    {
        PhaseGoalsList = phaseGoalsList;
    }
}
[Serializable]
public struct TyleTypeSetting
{
    public TileType TileType;
    public bool SpawnAtStart;
    public bool SpawnLater;
    [Range(0, 100)] public float SpawnRate;
    [Range(-10, 10)] public int ChangeSpawnRateOverTimeOrMoves;
    public bool SpawnAtRandomPosition;
    public bool SpawnAtSpecificPos;
    public Vector2 SpecificPos;

    public TyleTypeSetting(TileType tileType, bool spawnAtStart, bool spawnLater, float spawnRate, int changeSpawnRateOverTimeOrMoves, bool spawnAtRandomPosition, bool spawnAtSpecificPos, Vector2 specificPos)
    {
        TileType = tileType;
        SpawnAtStart = spawnAtStart;
        SpawnLater = spawnLater;
        SpawnRate = spawnRate;
        ChangeSpawnRateOverTimeOrMoves = changeSpawnRateOverTimeOrMoves;
        SpawnAtRandomPosition = spawnAtRandomPosition;
        SpawnAtSpecificPos = spawnAtSpecificPos;
        SpecificPos = specificPos;
    }
}

[Serializable]
public struct ZoomSettings
{
    [Range(-10, 10)] public float LeftToRight;
    [Range(-10, 10)] public float TopToBottom;
    [Range(-10, 10)] public float ZoomOutIn;
}

[Serializable]
public struct SideFruitsPadding
{
    [Range(0, 10)] public float LeftBarPadding;
    [Range(0, 10)] public float RightBarPadding;
    [Range(0, 10)] public float TopBarPadding;
    [Range(0, 10)] public float BottomBarPadding;
}

[Serializable]
public struct TileSettingFieldConfig
{
    public TileSettingConfig[] TileSettingConfigs;
    public TileSettingFieldConfig(TileSettingConfig[] tileSettingConfigs)
    {
        Array.Resize(ref tileSettingConfigs, 81);
        TileSettingConfigs = tileSettingConfigs;
    }
}

[Serializable]
public struct TileSettingConfig
{
    public TT TileType;
    public TT TileTypeUnderneath;
    public sbyte TeleportTarget;
    public Directions Direction;
    public bool IsDirectionStart;
    public bool IsActive;
    public bool IsActivate;
    [Range(0, 100)] public float SpawnRate;
    [Range(-100, 100)] public int ChangeSpawnRateOverTimeOrMoves;

    public TileSettingConfig(TT tileType, TT tileTypeUnderneath, sbyte teleportTarget, Directions direction, bool isDirectionStart, bool isActive, bool isActivate, float spawnRate, int changeSpawnRateOverTimeOrMoves)
    {
        TileType = tileType;
        TileTypeUnderneath = tileTypeUnderneath;
        TeleportTarget = teleportTarget;
        Direction = direction;
        IsDirectionStart = isDirectionStart;
        IsActive = isActive;
        IsActivate = isActivate;
        SpawnRate = spawnRate;
        ChangeSpawnRateOverTimeOrMoves = changeSpawnRateOverTimeOrMoves;
    }
    public TileSettingConfig(TT tileType, TT tileTypeUnderneath, Directions direction, bool isDirectionStart, bool isActive, bool isActivate, float spawnRate, int changeSpawnRateOverTimeOrMoves)
    {
        TileType = tileType;
        TileTypeUnderneath = tileTypeUnderneath;
        TeleportTarget = -1;
        Direction = direction;
        IsDirectionStart = isDirectionStart;
        IsActive = isActive;
        IsActivate = isActivate;
        SpawnRate = spawnRate;
        ChangeSpawnRateOverTimeOrMoves = changeSpawnRateOverTimeOrMoves;
    }
}
[Serializable]
public struct SideDotTypeSetting
{
    public SideDotType SideDotType;
    [Range(0, 100)] public float SpawnRate;
    [Range(-100, 100)] public int ChangeSpawnRateOverTimeOrMoves;

    public SideDotTypeSetting(SideDotType sideDotType, float spawnRate, int changeSpawnRateOverTimeOrMoves)
    {
        SideDotType = sideDotType;
        SpawnRate = spawnRate;
        ChangeSpawnRateOverTimeOrMoves = changeSpawnRateOverTimeOrMoves;
    }
}
[Serializable]
public struct SideFruitsSetting
{
    public SideDotTile SideDotTile;
    public bool IsActivate; 
    public int ActivatesAfterTimeOrMoves;
    public SideDotTypeSetting[] SideDotTypeSettings;

    public SideFruitsSetting(SideDotTile sideDotTile,  bool isActivate, int activatesAfterTimeOrMoves, SideDotTypeSetting[] sideDotTypeSettings)
    {
        SideDotTile = sideDotTile;
        IsActivate = isActivate;
        ActivatesAfterTimeOrMoves = activatesAfterTimeOrMoves;
        Array.Resize(ref sideDotTypeSettings, 3);
        SideDotTypeSettings = sideDotTypeSettings;
    }
}

[Serializable]
public struct SideFruitsFieldConfig
{
    public SideFruitsConfig[] SideFruitsConfig;

    public SideFruitsFieldConfig(SideFruitsConfig[] sideFruitsConfig)
    {
        Array.Resize(ref sideFruitsConfig, SaveFileLevelConfigs.Fields);
        SideFruitsConfig = sideFruitsConfig;
    }
}

[Serializable]
public struct SideFruitsConfig
{
    public SideFruitsPadding SideFruitsPadding;
    public List<SideFruitsSetting> SideFruitsSettings;
    public bool TopActive;
    public bool BottomActive;
    public bool LeftActive;
    public bool RightActive;

    public SideFruitsConfig(SideFruitsPadding sideFruitsPadding, List<SideFruitsSetting> sideFruitsSettings, bool topActive, bool bottomActive, bool leftActive, bool rightActive)
    {
        SideFruitsPadding = sideFruitsPadding;
        SideFruitsSettings = sideFruitsSettings;
        TopActive = topActive;
        BottomActive = bottomActive;
        LeftActive = leftActive;
        RightActive = rightActive;
    }
}

[Serializable]
public struct AnimationConfig
{
    public bool OverrideAnimations;
    [Range(-5, 5)] public float AnimationSpeedRefillBoardModifier;
    [Range(-5, 5)] public float AnimationSpeedSwapModifier;
    [Range(-5, 5)] public float SwipeResistanceModifier;
    [Range(-5, 5)] public float SideFruitsAnimationModifier;

    public AnimationConfig(bool overrideAnimations, float animationSpeedRefillBoardModifier, float animationSpeedSwapModifier, float swipeResistanceModifier, float sideFruitsAnimationModifier)
    {
        OverrideAnimations = overrideAnimations;
        AnimationSpeedRefillBoardModifier = animationSpeedRefillBoardModifier;
        AnimationSpeedSwapModifier = animationSpeedSwapModifier;
        SwipeResistanceModifier = swipeResistanceModifier;
        SideFruitsAnimationModifier = sideFruitsAnimationModifier;
    }
}

[Serializable]
public struct MatchFinderConfig
{
    public Row Row;
    public uint RowValue;
    public Phase RowPhase;
    
    public Diagonal Diagonal;
    public uint DiagonalValue;
    public Phase DiagonalPhase;
    
    public Pattern Pattern1;
    public uint Pattern1Value;
    public Phase Pattern1Phase;
    
    public Pattern Pattern2;
    public uint Pattern2Value;
    public Phase Pattern2Phase;

    public MatchFinderConfig(Row row, uint rowValue, Phase rowPhase, Diagonal diagonal, uint diagonalValue, Phase diagonalPhase, Pattern pattern1, uint pattern1Value, Phase pattern1Phase, Pattern pattern2, uint pattern2Value, Phase pattern2Phase)
    {
        Row = row;
        RowValue = rowValue;
        RowPhase = rowPhase;
        Diagonal = diagonal;
        DiagonalValue = diagonalValue;
        DiagonalPhase = diagonalPhase;
        Pattern1 = pattern1;
        Pattern1Value = pattern1Value;
        Pattern1Phase = pattern1Phase;
        Pattern2 = pattern2;
        Pattern2Value = pattern2Value;
        Pattern2Phase = pattern2Phase;
    }
}

[Serializable]
public struct LayoutFieldConfig
{
    public LayoutConfig[] LayoutConfig;

    public LayoutFieldConfig(LayoutConfig[] layoutConfig)
    {
        LayoutConfig = layoutConfig;
        
        Array.Resize(ref LayoutConfig, SaveFileLevelConfigs.Fields);
    }
}

[Serializable]
public struct LayoutConfig
{
    public float ZoomOut;
    public float LeftPadding;
    public float RightPadding;
    [FormerlySerializedAs("Toppadding")] public float TopPadding;
    public float BottomPadding;

    public LayoutConfig(float zoomOut, float leftPadding, float rightPadding, float topPadding, float bottomPadding)
    {
        ZoomOut = zoomOut;
        LeftPadding = leftPadding;
        RightPadding = rightPadding;
        TopPadding = topPadding;
        BottomPadding = bottomPadding;
    }
}

[Serializable]
public struct BoardDimensionsConfig
{
    public int[] Width;
    public int[] Height;
    public bool[] NoMatches;
    public FruitsConfigParent[] FruitsConfigParent;
    public bool[] P1P2;
    public EndGameRequirements[] GameTypeP1;
    public EndGameRequirements[] GameTypeP2;

    public BoardDimensionsConfig(int[] width, int[] height, bool[] noMatches, FruitsConfigParent[] fruitsConfigParent, bool[] p1P2, EndGameRequirements[] gameTypeP1, EndGameRequirements[] gameTypeP2)
    {
        Width = width;
        Height = height;
        NoMatches = noMatches;
        FruitsConfigParent = fruitsConfigParent;
        P1P2 = p1P2;
        GameTypeP1 = gameTypeP1;
        GameTypeP2 = gameTypeP2;
    }
}


[Serializable]
public struct ScoreGoalsConfig
{
    public float Star1Value;
    public float Star2Value;
    public float Star3Value;
    public float TrophyValue;
    
    public bool Star1Enabled;
    public bool Star2Enabled;
    public bool Star3Enabled;
    public bool TrophyEnabled;

    public bool Star1NeededForNextLevel;
    public bool Star2NeededForNextLevel;
    public bool Star3NeededForNextLevel;
    public bool TrophyNeededForNextLevel;

    public bool TrophyNoEmpty;
    public bool TrophyNumberAreMovesOrTime;

    public ScoreGoalsConfig(float star1Value, float star2Value, float star3Value, float trophyValue, bool star1Enabled, bool star2Enabled, bool star3Enabled, bool trophyEnabled, bool star1NeededForNextLevel, bool star2NeededForNextLevel, bool star3NeededForNextLevel, bool trophyNeededForNextLevel, bool trophyNoEmpty, bool trophyNumberAreMovesOrTime)
    {
        Star1Value = star1Value;
        Star2Value = star2Value;
        Star3Value = star3Value;
        TrophyValue = trophyValue;
        Star1Enabled = star1Enabled;
        Star2Enabled = star2Enabled;
        Star3Enabled = star3Enabled;
        TrophyEnabled = trophyEnabled;
        Star1NeededForNextLevel = star1NeededForNextLevel;
        Star2NeededForNextLevel = star2NeededForNextLevel;
        Star3NeededForNextLevel = star3NeededForNextLevel;
        TrophyNeededForNextLevel = trophyNeededForNextLevel;
        TrophyNoEmpty = trophyNoEmpty;
        TrophyNumberAreMovesOrTime = trophyNumberAreMovesOrTime;
    }
}

[Serializable]
public struct LevelTextConfig
{
    public Language Language;
    public bool LevelDefaultLanguage;
    public string LevelName;
    public bool LevelNameDefaultText;
    public string LevelDescriptionText; 
    public bool LevelDescriptionDefaultText; 
    public string LevelGameTypeText; 
    public bool LevelGameTypeDefaultText;
    

    public LevelTextConfig(Language language, bool levelDefaultLanguage,string levelName,
        bool levelNameDefaultText, string levelDescriptionText, bool levelDescriptionDefaultText,
        string levelGameTypeText, bool levelGameTypeDefaultText)
    {
        Language = language;
        LevelDefaultLanguage = levelDefaultLanguage;
        LevelName = levelName;
        LevelNameDefaultText = levelNameDefaultText;
        LevelDescriptionText = levelDescriptionText;
        LevelDescriptionDefaultText = levelDescriptionDefaultText;
        LevelGameTypeText = levelGameTypeText;
        LevelGameTypeDefaultText = levelGameTypeDefaultText;
    }
}

[Serializable]
public struct BombConfig
{
    public Bomb Bomb;
    public bool Active;
    public MatchStyle MatchStyle;
    public ushort Matchnumber;

    public BombConfig(Bomb bomb, bool active, MatchStyle matchStyle, ushort matchnumber)
    {
        Bomb = bomb;
        Active = active;
        MatchStyle = matchStyle;
        Matchnumber = matchnumber;
    }
}

[Serializable]
public struct FruitsConfigParent
{
    public FruitType FruitType;
    public bool FruitEnabled;
    public FruitsConfig[] fruitClipboards;

    public FruitsConfigParent(FruitType fruitType, bool fruitEnabled, FruitsConfig[] fruitClipboards)
    {
        FruitType = fruitType;
        FruitEnabled = fruitEnabled;
        this.fruitClipboards = fruitClipboards;
    }
}
[Serializable]
public struct FruitsConfig
{
    public Colors FruitColor;
    public bool IsEnabled;
    public uint SpawnStart;
    public int SpawnEnd;
    public byte SpawnChance;
    public short SpawnChanceOverTime;
    public bool SpawnChanceNegative;

    [FormerlySerializedAs("FruitPhase")] public Phase phase;

    public FruitsConfig(Colors fruitColor, bool isEnabled, uint spawnStart, int spawnEnd, byte spawnChance, short spawnChanceOverTime, bool spawnChanceNegative, Phase phase)
    {
        FruitColor = fruitColor;
        IsEnabled = isEnabled;
        SpawnStart = spawnStart;
        SpawnEnd = spawnEnd;
        SpawnChance = spawnChance;
        SpawnChanceOverTime = spawnChanceOverTime;
        SpawnChanceNegative = spawnChanceNegative;
        this.phase = phase;
    }
}
[Serializable]
public enum Phase
{
    NotInP2,
    OnlyInP2,
    InP1AndP2
}
