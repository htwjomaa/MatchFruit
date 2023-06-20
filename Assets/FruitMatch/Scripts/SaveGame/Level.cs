using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "World", menuName = "Level")]
public sealed class Level : ScriptableObject
{
  [Header("Board Dimensions")] 
  public int width;
  public int height;
  
  [Header("Starting tiles")]
  public TileType [] boardLayout;

  [Header("Available Dots")]
  public GameObject[] dots;
  
  [Header("Score Goals")]
  public double[] scoreGoals;

  [Header("EndGameRequirements")] 
  public EndGameRequirements endGameRequirements;
  public BlankGoal[] levelGoals;


  [Header("LevelInfoPanel")] 
  public string LevelNameText;
  public string LevelDescriptionText;
  public string LevelGameTypeDefaultText;
  public Sprite[] LevelGoalImagesOverlay;


  public Bomb[] Bombs;
  
  
  [Button()] 
  public void GenerateBombsLists()
  {
   
      Array.Clear(Bombs, 0, Bombs.Length);
      Bombs = new Bomb[enumCountBomb()];
      for (int j = 0; j < Bombs.Length; j++)
      {
        Bombs[j] = enumGetSpecificBombValue(j);
      }
    
  }

  private static int enumCountBomb()
  {
    //for now randomize it only
    int counter = 0;
    foreach (Bomb matchStyle in Enum.GetValues(typeof(Bomb)))
    {
      counter++;
    }

    return counter;
  }
    
  private static Bomb enumGetSpecificBombValue(int counter)
  {
    //for now randomize it only
    foreach (Bomb bomb in Enum.GetValues(typeof(Bomb)))
    {
      if (counter == (int)bomb) return bomb;
    }

    return Bomb.Horizontal;
  }
  
}
