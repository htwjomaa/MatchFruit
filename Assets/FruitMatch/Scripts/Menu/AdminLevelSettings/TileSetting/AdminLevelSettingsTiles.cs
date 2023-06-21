using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class AdminLevelSettingsTiles : MonoBehaviour
{
    private byte _currentRow;
    private byte _currentField;
    private byte _currentSetting;
    private static TKind _currentTileKind = TKind.Tiles;
    public delegate void LoadRow();
    public static event LoadRow LoadCurrentRow;
    public delegate void LoadSettings();
    public static event LoadSettings LoadCurrentSetting;
    public delegate void ResetSettings();
    public static event ResetSettings ResetSetting;
    public delegate void LoadFields();
    public static event LoadFields LoadFieldsEvent;
    public delegate void LoadFromDiskE();
    public static event LoadFromDiskE LoadFromDiskEvent;
    
    public static event Action LoadInformationEvent;
    public static event Action ShowItem;
    public int CurrentColumn;
    
    public static bool isSettingMode;
    
    public List<Sprite> TKindSprites = new();
    public List<Sprite> TTypeSprites = new();
    public List<Sprite> DirectionSprites = new();
    public List<Sprite> TeleportSprites = new();
    public byte CurrentRow
    {
        get => _currentRow;
        set
        {
            _currentRow = value;
            RowChanged();
        }
    }
    public byte CurrentSetting
    {
        get => _currentSetting;
        set
        {
            _currentSetting = value;
            SettingsChanged();
        }
    }
    public TKind CurrentTileKind
    {
        get => _currentTileKind;
        set
        {
            _currentTileKind = value;
            LoadTKind();
        }
    }

    public static void ResetSettingsInvoke() => ResetSetting?.Invoke();

    public void ClickSectionIcon()
    {
       // Invoke(nameof(ClickSection), 0.05f);
       
        Invoke(nameof(ClickSection), 0.06f);
    }
    
    private void ClickSection()
    {
        ResetSetting?.Invoke();
        LoadField();
        Rl.BoardPreview.ResetLastClicked();
        Rl.BoardPreview.StartDrawBoard();
        LoadFromDisk(Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber].TileSettingFieldConfigs);
    }

    public void ClipBoardToTiles(byte column, ref TT tileType, ref Directions directions, ref sbyte teleportTarget)
    {
        tileType = Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[GetID(_currentRow, column)].TileType;
        directions = Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[GetID(_currentRow, column)].Direction;
        teleportTarget = Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[GetID(_currentRow, column)].TeleportTarget;
    }
    public void ClickHandler(byte column, ref TT tileType, ref Directions directions, ref sbyte teleportTarget, TextMeshProUGUI text, Image image, Load load = Load.Only)
    {
        switch (_currentTileKind)
        {
            case TKind.Tiles:
                GetTileType(column, ref tileType, text, image, load);
                break;
            case TKind.Vectors:
                GetDirection(column, ref directions, text, image, load);
                break;
            case TKind.Teleports:
                GetTeleport(column, ref teleportTarget, text, image, load);
                break;
        }
    }
    [SerializeField] private Image TileKindImage;
    public void ClickNextTileKind()
    {
        CurrentTileKind = NextTileKind((int)CurrentTileKind);
        LoadFieldsEvent?.Invoke();
    }
    public void InvokeLoadFieldsEvent() => LoadFieldsEvent?.Invoke();
    public void InvokeShowItem() => ShowItem?.Invoke();
    public void GetTileType(byte column, ref TT tiletype, TextMeshProUGUI text, Image image, Load load = Load.Only)
    {
        switch (load)
        {
            case  Load.Next:
                tiletype = NextTileType((int)tiletype);
                break;
            case Load.Prev:
                tiletype = PrevTileType((int)tiletype);
                break;
        }

        Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[GetID(_currentRow, column)].TileType = tiletype;
        image.sprite = TTypeSprites[(int)tiletype];
        text.text = LocalisationSystem.GetLocalisedString(GenericSettingsFunctions.EnumToString(typeof(TT), (int)tiletype));
    }


    private void ResizeTileConfig(ref TileSettingFieldConfig[] tileSettingFieldConfigs)
    {
        for (int i = 0; i < tileSettingFieldConfigs.Length; i++)
        {
            if(tileSettingFieldConfigs[i].TileSettingConfigs == null || tileSettingFieldConfigs[i].TileSettingConfigs.Length != 81)
                Array.Resize(ref tileSettingFieldConfigs[i].TileSettingConfigs, 81);
        }
    }
    public void LoadFromDisk(TileSettingFieldConfig[] tileSettingFieldConfigs)
    {
        if(tileSettingFieldConfigs == null || tileSettingFieldConfigs.Length != SaveFileLevelConfigs.Fields) Array.Resize(ref tileSettingFieldConfigs , SaveFileLevelConfigs.Fields);
        if(Rl.saveClipBoard.TileSettingFieldConfigs.Length != SaveFileLevelConfigs.Fields) Array.Resize(ref Rl.saveClipBoard.TileSettingFieldConfigs , SaveFileLevelConfigs.Fields);

        ResizeTileConfig(ref tileSettingFieldConfigs);
        ResizeTileConfig(ref Rl.saveClipBoard.TileSettingFieldConfigs);
        Rl.saveClipBoard.TileSettingFieldConfigs = (TileSettingFieldConfig[])GenericSettingsFunctions.GetDeepCopy(tileSettingFieldConfigs);
        LoadFromDiskEvent?.Invoke();
    }
    public TileSettingFieldConfig[] SaveTileSettings() 
        => (TileSettingFieldConfig[])GenericSettingsFunctions.GetDeepCopy(Rl.saveClipBoard.TileSettingFieldConfigs);

    private int GetID(byte row, byte column) => (row * 9) + column;

    public bool IsAlsoTeleportFrom(byte column)
    {
        if (Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[GetID(_currentRow, column)].TeleportTarget == -1) return false;
        return true;
    }
    public void GetDirection(byte column, ref Directions direction, TextMeshProUGUI text, Image image, Load load = Load.Only)
    { 
        switch (load)
        {
            case  Load.Next:
                direction = NextDirections((int)direction);
                break;
            case Load.Prev:
                direction = PrevDirections((int)direction);
                break;
        }
        
        Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[GetID(_currentRow, column)].Direction = direction;
        int directionStartAdd = 0;
        if (Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField]
            .TileSettingConfigs[GetID(_currentRow, column)].IsDirectionStart)
            directionStartAdd += 4;
        image.sprite = DirectionSprites[(int)direction+directionStartAdd];
        text.text = LocalisationSystem.GetLocalisedString(GenericSettingsFunctions.EnumToString(typeof(Directions), (int)direction));
    }
    public void GetTeleport(byte column, ref sbyte teleportTarget, TextMeshProUGUI text, Image image, Load load = Load.Only)
    {
        switch (load)
        {
            case  Load.Next:
              //  teleportTarget = NextTeleport(teleportTarget);
                break;
            case Load.Prev:
              //  teleportTarget = PrevTeleport(teleportTarget);
                break;
        }
        
        Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[GetID(_currentRow, column)].TeleportTarget = teleportTarget;
        
        if (teleportTarget == -1)  //-1 means no teleport
        {
            image.sprite = TeleportSprites[0];
            text.text = LocalisationSystem.GetLocalisedString("NoTeleport");
        }
        else
        {
            image.sprite = TeleportSprites[1];
            int rowPrint = Mathf.FloorToInt((float)teleportTarget / 9)+1;
            int columnPrint = teleportTarget / rowPrint;
            text.text = "To - R: " + rowPrint + " | C: " + (columnPrint+1);
            startTeleportFrom = true;
        }
 
    }

    private bool startTeleportFrom;
    public Dictionary<int, int> validTargets = new Dictionary<int,int>();
    public static event Action TeleportFromEvent;

    public IEnumerator SetTeleportFrom(float waitSec)
    {
        yield return new WaitForSeconds(waitSec);

        validTargets.Clear(); //to - from 
        int lowerRange = _currentRow * 9-1; //0*9-1  = -1
        int higherRange = lowerRange + 10;  //-1 +9 =  8
        
        for (int i = 0; i < Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs.Length; i++)
        {
            if (Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[i].TeleportTarget != -1)
            {
                sbyte tileTarget = Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[i].TeleportTarget;
                if (tileTarget > lowerRange && tileTarget < higherRange)
                {
                    validTargets.TryAdd(tileTarget%9, i);
                }
            }
        }
        TeleportFromEvent?.Invoke();
    }


    private void LateUpdate()
    {
        if (!startTeleportFrom) return;  //bool is not needed, could check if queue but its cheaper to compute
        startTeleportFrom = false;
        StartCoroutine(SetTeleportFrom(0.02f));
    }

    private void RowChanged()
    {
        StartCoroutine(SetTeleportFrom(0.02f));
        LoadCurrentRow?.Invoke();
    }

    private void SettingsChanged()
    {
        StartCoroutine(SetTeleportFrom(0.02f));
        LoadCurrentSetting?.Invoke();
    }
    private void LoadField ()
    {
        StartCoroutine(SetTeleportFrom(0.02f));
        LoadFieldsEvent?.Invoke();
        SetAlphaForRows();
    }

    private void SetAlphaForRows()
    {
        
    }
    private void LoadTKind ()
    {
        TileKindImage.sprite = TKindSprites[(int)_currentTileKind];
        GenericSettingsFunctions.SmallJumpAnimation(TileKindImage.transform);
        switch (_currentTileKind)
        {
            case TKind.Tiles:
                LoadTiles();
                break;
            
            case TKind.Teleports:
                LoadTeleports();
                break;
            
            case TKind.Vectors:
                LoadVectors();
                break;
        }
        Rl.BoardPreview.StartDrawBoard();
    }

    private void LoadTiles()
    {
        
    }

    private void LoadTeleports()
    {
        
    }
    private void LoadVectors()
    {
        
    }

    private static TT NextTileType(int tileType)
    {
        tileType += 1;
        TT gameTypeEnum = TT.Normal;
        if (tileType > EnumCountTilekind() - 1) tileType = 0;

        int counter = 0;
        foreach (TT searchForTileKind in Enum.GetValues(typeof(TT)))
        {
            if (tileType == counter) gameTypeEnum = searchForTileKind;
            counter++;
        }
        return gameTypeEnum;
    }
    private static TT PrevTileType(int tiletype)
    {
        tiletype -= 1;
        TT gameTypeEnum = TT.Normal;
        if (tiletype < 0) tiletype = EnumCountTilekind() - 1;

        int counter = 0;
        foreach (TT searchForTileKind in Enum.GetValues(typeof(TT)))
        {
            if (tiletype == counter) gameTypeEnum = searchForTileKind;
            counter++;
        }
        return gameTypeEnum;
    }

    /*private static Teleport NextTeleport(int teleport)
    {
        teleport += 1;
        Teleport nextTeleportEnum = Teleport.NoTeleport;
        if (teleport > EnumCountTeleport() - 1) teleport = 0;

        int counter = 0;
        foreach (Teleport teleportEnum in Enum.GetValues(typeof(Teleport)))
        {
            if (teleport == counter) nextTeleportEnum = teleportEnum;
            counter++;
        }
        return nextTeleportEnum;
    }
    private static Teleport PrevTeleport(int teleport)
    {
        teleport -= 1;
        Teleport prevTeleportEnum = Teleport.NoTeleport;
        if (teleport < 0) teleport = EnumCountTeleport() - 1;

        int counter = 0;
        foreach (Teleport teleportEnum in Enum.GetValues(typeof(Teleport)))
        {
            if (teleport == counter) prevTeleportEnum = teleportEnum;
            counter++;
        }
        return prevTeleportEnum;
    }*/
    private static TKind NextTileKind(int tileKind)
    {
        tileKind += 1;
        TKind nextTileKind = TKind.Tiles;
        if (tileKind > EnumCountTKind() - 1) tileKind = 0;

        int counter = 0;
        foreach (TKind searchForTileKind in Enum.GetValues(typeof(TKind)))
        {
            if (tileKind == counter) nextTileKind = searchForTileKind;
            counter++;
        }
        return nextTileKind;
    }
    /*private TKind PrevTileKind(int tileKind)
    {
        tileKind -= 1;
        TKind prevTileKind = TKind.Tiles;
        if (tileKind < 0) tileKind = EnumCountTKind() - 1;

        int counter = 0;
        foreach (TKind searchForTileKind in Enum.GetValues(typeof(TKind)))
        {
            if (tileKind == counter) prevTileKind = searchForTileKind;
            counter++;
        }
        return prevTileKind;
    }*/
    private static Directions NextDirections(int direction)
    {
        direction += 1;
        Directions nextTileKind = Directions.bottom;
        if (direction > EnumCountDirections() - 1) direction = 0;

        int counter = 0;
        foreach (Directions searchForDirection in Enum.GetValues(typeof(Directions)))
        {
            if (direction == counter) nextTileKind = searchForDirection;
            counter++;
        }
        if (nextTileKind == Directions.none) return NextDirections((int)nextTileKind);
        return nextTileKind;
    }
    private static Directions PrevDirections(int direction)
    {
        direction -= 1;
        Directions prevTileKind = Directions.bottom;
        if (direction < 0) direction = EnumCountDirections() - 1;

        int counter = 0;
        foreach (Directions searchForDirection in Enum.GetValues(typeof(Directions)))
        {
            if (direction == counter) prevTileKind = searchForDirection;
            counter++;
        }

        if (prevTileKind == Directions.none) return PrevDirections((int)prevTileKind);
        return prevTileKind;
    }
    private static int EnumCountTilekind() => GenericSettingsFunctions.EnumCount(typeof(TT));
   // private static int EnumCountTeleport() => GenericSettingsFunctions.EnumCount(typeof(Teleport));
    private static int EnumCountTKind() => GenericSettingsFunctions.EnumCount(typeof(TKind));
    private static int EnumCountDirections() => GenericSettingsFunctions.EnumCount(typeof(Directions));
    
   // private string StringTilekind(IConvertible enumType) => Enum.GetName(typeof(enumType), (int)enumType);
   public void LoadCurrentField()
   {
       LoadInformationEvent?.Invoke();
       ResetSetting?.Invoke();
       LoadFieldsEvent?.Invoke();
       LoadCurrentRow?.Invoke();
       Rl.BoardPreview.ResetLastClicked();
       Rl.BoardPreview.StartDrawBoard();
   }
   
   public void CopyField(byte fieldOne, byte fieldTwo)
   {
       Rl.saveClipBoard.TileSettingFieldConfigs[fieldTwo].TileSettingConfigs =
           (TileSettingConfig[])GenericSettingsFunctions.GetDeepCopy(Rl.saveClipBoard.TileSettingFieldConfigs[fieldOne].TileSettingConfigs);
       LoadCurrentField();
   }
}