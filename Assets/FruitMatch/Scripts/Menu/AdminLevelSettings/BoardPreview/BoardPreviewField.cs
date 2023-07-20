using System;
using UnityEngine;
using UnityEngine.UI;

public class BoardPreviewField : MonoBehaviour
{
    int depth = 0;
    private Transform parentTransform;

    private void Start()
    {
        if (!transform.parent.transform.parent.GetComponent<BoardPreview>() &&
            !transform.parent.transform.parent.GetComponent<BoardPreviewSideDot>())
        {
            transform.parent.transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
            transform.parent.transform.parent.GetComponent<Button>().onClick.AddListener(ClickOnPreviewField);
            parentTransform = transform.parent.transform.parent.transform;
        }
        else
        {
            parentTransform = transform.parent.transform;
        }

        transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
        transform.parent.GetComponent<Button>().onClick.AddListener(ClickOnPreviewField);
        GetComponent<Button>().onClick.AddListener(ClickOnPreviewField);

    }

    private void ClickOnPreviewField()
    {
        int siblingIndex = parentTransform.GetSiblingIndex();
        //
        //     int maxRow = (int)Rl.adminLevelSettingsBoard.BoardHeightSlider.value;
        if (GetComponentInParent<BoardPreview>() != null)
        {
            Rl.boardPreviewDragNDrop.IsFieldValid();
        int maxColumn = (int)Rl.adminLevelSettingsBoard.BoardWidthSlider.value;
    
        // 35      --   maxC = 6   --- maxR = 9
        float calRow = (float)siblingIndex / (float)maxColumn; //35:6 = 5.8
        int row = Mathf.FloorToInt(calRow); // = 5
        int column = siblingIndex - row * maxColumn;
        Rl.adminLevelSettingsTiles.CurrentColumn = column;
        int differenceColumn = 9 - maxColumn; //3
        int addMissing = differenceColumn * row; //3*5 = 15
        int finalTile = addMissing + siblingIndex; // 40

        if (Rl.boardPreviewDragNDrop.CurrentSelectedItem != null)
        {
            switch (Rl.adminLevelSettingsTiles.CurrentTileKind)
            {
                case TKind.Tiles:
                    TT tileType = EvaluateObjects(Rl.boardPreviewDragNDrop.CurrentSelectedItem.Identifier);
                    Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[finalTile].TileType = tileType;
                    break;
                case TKind.Vectors:
                    Directions direction;
                    bool isDirectionStart = true;
                    direction = EvaluateArrow(Rl.boardPreviewDragNDrop.CurrentSelectedItem.Identifier);
                    if (direction == Directions.none) direction = EvaluteArrowSpawn(Rl.boardPreviewDragNDrop.CurrentSelectedItem.Identifier);
                    else isDirectionStart = false;

                    if (Rl.boardPreviewDragNDrop.CurrentSpecialSelected.Identifier == "one")
                    {
                        Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[finalTile].IsDirectionStart = isDirectionStart;
                        Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[finalTile].Direction = direction;
                    }
                    else
                    {
                        for (int i = 0;
                             i < Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs.Length; i++)
                        {
                            Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[i].IsDirectionStart = isDirectionStart;
                            Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[i].Direction = direction;
                        }
                    }

                    break;
                case TKind.Teleports:

                    if (EvaluateIsTeleport(Rl.boardPreviewDragNDrop.CurrentSelectedItem.Identifier))
                    {
                        if (Rl.boardPreviewDragNDrop.teleportFrom != -128) 
                            Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[Rl.boardPreviewDragNDrop.teleportFrom].TeleportTarget = (sbyte)finalTile;
                    
                        Rl.boardPreviewDragNDrop.ToggleTeleportTarget((sbyte)finalTile);
                    }
                    else
                    {
                        Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[finalTile].TeleportTarget = -1;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        Rl.adminLevelSettingsTiles.CurrentRow = (byte)row;
        if (Rl.boardPreviewDragNDrop.CurrentSelectedItem != null) Rl.BoardPreview.StartDrawBoard(0.155f);
        GenericSettingsFunctions.SmallShake(0.155f, 1, transform);
        if (FieldState.CurrentSection == CopySection.TileSettings) Rl.adminLevelSettingsTiles.InvokeShowItem();
        }

        else
        {
            Directions sideDotBoardDirection = GetComponentInParent<BoardPreviewSideDot>().direction;
            int sideDotMultiplier = 0;
            switch (sideDotBoardDirection)
            {
                case Directions.bottom:
                    sideDotMultiplier = 0;
                    break;
                case Directions.top:
                    sideDotMultiplier = 1;
                    break;
                case Directions.right:
                    sideDotMultiplier = 2;
                    break;
                case Directions.left:
                    sideDotMultiplier = 3;
                    break;
                case Directions.none:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            int finalTile = (sideDotMultiplier * 9) + siblingIndex;
              if (Rl.boardPreviewDragNDrop.CurrentSelectedItem != null)
        {
            switch (Rl.adminLevelSettingsTiles.CurrentTileKind)
            {
                case TKind.Tiles:
                    SideDotTile tileType = EvaluateObjectsForSideDot(Rl.boardPreviewDragNDrop.CurrentSelectedItem.Identifier);
                    Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField]
                            .SideFruitsSettings[finalTile] =
                        new SideFruitsSetting(tileType,
                            Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField]
                                .SideFruitsSettings[finalTile].IsActivate,
                            Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField]
                                .SideFruitsSettings[finalTile].ActivatesAfterTimeOrMoves,
                            Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField]
                                .SideFruitsSettings[finalTile].SideDotTypeSettings);
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

              Rl.adminLevelSettingsSideDots.CurrentBar = (byte)sideDotMultiplier;
              Rl.adminLevelSettingsTiles.CurrentColumn = siblingIndex;
              if (Rl.boardPreviewDragNDrop.CurrentSelectedItem != null) Rl.BoardPreview.StartDrawBoard(0.155f);
              GenericSettingsFunctions.SmallShake(0.155f, 1, transform);
            //  if (FieldState.CurrentSection == CopySection.SideDotSettings) Rl.adminLevelSettingsTiles.InvokeShowItem();
        }
    }

    private Directions EvaluteArrowSpawn(string Identifier)
    {
        switch (Identifier)
        {
            case "arrow_down_spawn":
                return Directions.bottom;
            case "arrow_up_spawn":
                return Directions.top;
            case "arrow_left_spawn":
                return Directions.left;
            case "arrow_right_spawn":
                return Directions.right;
        }

        return Directions.none;
    }

    private Directions EvaluateArrow(string Identifier)
    {
        switch (Identifier)
        {
            case "arrow_down":
                return Directions.bottom;
            case "arrow_up":
                return Directions.top;
            case "arrow_left":
                return Directions.left;
            case "arrow_right":
                return Directions.right;
        }

        return Directions.none;
    }

    private TT EvaluateObjects(string Identifier)
    {

        switch (Identifier)
        {
            case "normal":
                return TT.Normal;
            case "emptytile":
                return TT.EmptyTile;
            case "jelly":
                return TT.Jelly;
            case "lock":
                return TT.Lock;
            case "chest":
                return TT.Chest;
            case "lockedchest":
                return TT.LockedChest;
            case "bubble":
                return TT.Bubble;
            case "bigblock":
                return TT.BigBlock;
            case "ingredient":
                return TT.Ingredient;
            case "samecolorbomb":
                return TT.SameColorBomb;
        }

        return TT.Normal;
    }
    
    private SideDotTile EvaluateObjectsForSideDot(string Identifier)
    {

        switch (Identifier)
        {
            case "normal":
                return SideDotTile.Normal;
            case "emptytile":
                return SideDotTile.EmptyTile;
            case "jelly":
                return SideDotTile.Jelly;
            case "lock":
                return SideDotTile.Lock;
            case "chest":
                return SideDotTile.Chest;
            case "lockedchest":
                return SideDotTile.LockedChest;
            case "ingredient":
                return SideDotTile.Ingredient;
            case "onlyline":
                return SideDotTile.Package;
            case "onlyturna":
                return SideDotTile.HorizontalBomb;
            case "onlyturnb":
                return SideDotTile.VerticalBomb;
            case "samecolorbomb":
                return SideDotTile.SameColorBomb;
            case "undestroyable":
                return SideDotTile.SuchBombe;
        }

        return SideDotTile.Normal;
    }
    
    private bool EvaluateIsTeleport(string Identifier)
    {
        if (Identifier == "teleport") return true;
        return false;
    }
}