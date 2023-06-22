using System;
using System.Collections.Generic;
using System.Linq;
using FruitMatch.Scripts.Blocks;
using FruitMatch.Scripts.Items;
using FruitMatch.Scripts.Level;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public sealed class LoadingHelper : MonoBehaviour
{
 [SerializeField] public List<int> TargetSequence = new List<int>(3);
  public GameObject[] BlockPrefabs;
 public GameObject[] ItemPrefabs;
 public ParticleSystem[] VanishParticleObjects;
 public Sprite[] Sprites;
 public Sprite[] Marmalades;
 public Sprite[] HorStriped;
 public Sprite[] VertStriped;
 public Sprite[] MultiColor;
 public Sprite[] TargetIcon;
 public List<ObjectiveSettings> ObjectiveSettingsDebug;
 public static LoadingHelper THIS;
 public bool sideDotStart = true;
 public bool sideDotBool = true;
 public GameObject ItemPrefab;
 public GameObject tilePrefab;
 public Sprite tileBackgroundDark;
 public Sprite tileBackgroundBright;
 public GameObject[] sideDotIcons;
 public Transform boardTransform;
 [SerializeField] public float topOffset =1.3f;
 [SerializeField] public float bottomOffset = 1.2f;
 [SerializeField] public float leftOffset = 1.55f;
 [SerializeField] public float rightOffset = 1.55f;
 [SerializeField] public bool  topActive = false;
 [SerializeField] public bool  bottomActive = true;
 [SerializeField] public bool  leftActive = true;
 [SerializeField] public bool  rightActive = true;
 public List<BackGroundTileSideList> bgTiles = new List<BackGroundTileSideList>(5);
 public bool[,] abc = new bool[0,0];
 public BackgroundTile[,] alltiles = new BackgroundTile[0, 0];
 public GameObject[,] allDots = new GameObject[0, 0];
 public GameObject[] allDotPrefabs;
 public GameObject FieldParent;
 public FieldBoard FieldBoard;
 public int height;
  public int width;
  public List<GameObject> DEBUGSQUAREARRAY = new List<GameObject>();
  public GameObject ItemParent;
  public int testY = 0;
  public int testX = 0;
  
  [Button] private void DebugTestRowColumn() => ScaleUpTest(testX, testY);
  public Sprite[] loadedSpritesDebug;
  public Sprite[] loadedSpritesDebugMarmalade;
  private void ScaleUpTest(int targetColumn, int targetRow)
  {
 
    ConvertToTwoDimensionalArray(LoadingHelper.THIS.FieldBoard, ref LoadingHelper.THIS.allDots);
   
  // LoadingHelper.THIS.allDots[targetColumn, targetRow].gameObject.transform.localScale = new Vector3(5f, 5, 5f);
  Debug.Log("Name: " + LoadingHelper.THIS.allDots[targetColumn, targetRow].gameObject.name);
  }
  
  public void SideDotScaler()
  {
   foreach (var n in FindObjectsOfType<SideDot>())
   {
    n.transform.parent = FieldParent.transform;
    n.transform.localScale = new Vector3(0.42f, 0.42f, 0.42f);
   }
 
  }

  public GameObject GetBlockPrefab(SquareTypes squareType)
  {
   if (squareType == SquareTypes.NONE) return null;
   return BlockPrefabs[(int)squareType - 1];
  }
  
  public GameObject GetItemPrefab(ItemsTypes itemType)
  {
   if (itemType == ItemsTypes.NONE) return null;
   return ItemPrefabs[(int)itemType - 1];
  }
  /*public GameObject GetBlockPrefab(SquareTypes squareType)
  {
   switch (squareType)
   {
    case SquareTypes.NONE:
     break;
    case SquareTypes.EmptySquare:
     break;
    case SquareTypes.SugarSquare:
     break;
    case SquareTypes.WireBlock:
     break;
    case SquareTypes.SolidBlock:
     break;
    case SquareTypes.ThrivingBlock:
     break;
    case SquareTypes.JellyBlock:
     break;
    case SquareTypes.SpiralBlock:
     break;
    case SquareTypes.UndestrBlock:
     break;
    case SquareTypes.BigBlock:
     break;
    default:
     throw new ArgumentOutOfRangeException(nameof(squareType), squareType, null);
   }

   return BlockPrefabs
  }*/
    public  void ConvertToTwoDimensionalArray(FieldBoard fieldBoard, ref GameObject[,] allDots)
    {
     List<GameObject> objList = new();  // List you want to Check
     
     HashSet<GameObject> objHashSet = new();  //This is the Datatype you should be using
     for (int i = 0; i < objList.Count; i++)
     {
      if(!objHashSet.Contains(objList[i]))
       objHashSet.Add(objList[i]);
     else Debug.Log("Duplicate found: " + objList[i].name);
     }
     
     
        fieldBoard = FieldParent.GetComponentInChildren<FieldBoard>();
        int counter = fieldBoard.squaresArray.Length - width; //81 - 9 = 72
        int rowCounter = height;
        
        allDots = new GameObject[width, height];
        /*for (int i = 0; i < LoadingHelper.THIS.width; i++)
        {
            for (int j = 0; j < LoadingHelper.THIS.height; j++)
            {
                if (counter == rowCounter*LoadingHelper.THIS.width)  //81 reset // 72 reset
                {
                    rowCounter--;  //8
                    counter = rowCounter * LoadingHelper.THIS.width - LoadingHelper.THIS.width;  //72-9 = 63
                }
                allDots[i, j] = fieldBoard.squaresArray[counter].gameObject;
                counter++;
            }
        }*/
        List<Square> squareList = fieldBoard.squaresArray.ToList();
        
        squareList = squareList.OrderBy(x => x.transform.position.x).ThenBy(x => x.transform.position.y).ToList();

        int counterList = 0;
        for (int i = 0; i < LoadingHelper.THIS.width; i++)
        {
         for (int j = 0; j < LoadingHelper.THIS.height; j++)
         {
          allDots[i, j] = squareList[counterList].gameObject;
          counterList++;
         }
        }
        
     
        /*or (int i = 0; i < LoadingHelper.THIS.width; i++)
        {
         for (int j = 0; j < LoadingHelper.THIS.height; j++)
         {
          if (!allDots[i, j].GetComponent<SpriteRenderer>().enabled)
           allDots[i, j] = null;
         }
        }*/
      
       // for(int i = 0; i < )
        
        LoadingHelper.THIS.DEBUGSQUAREARRAY.Clear();
        int counterDotsTransfered = 0;
        for (int i = 0; i < LoadingHelper.THIS.width; i++)
        {
            for (int j = 0; j < LoadingHelper.THIS.height; j++)
            {
              //  if (allDots[i, j] != null)
               // {
                    LoadingHelper.THIS.DEBUGSQUAREARRAY.Add(allDots[i, j]);
                    counterDotsTransfered++;
              //  }
            }
        }
        Debug.Log("VALID ITEMS:: " + counterDotsTransfered);
        
       
    }
 private void Awake()
 {
  THIS = this;
  
 }

 public int ColorHelper(int colorableColor)
 {

  Sprite[] loadedSprites = LoadingManager.GetLoadedSprites(Sprites);
  if (colorableColor > loadedSprites.Length-1) colorableColor = loadedSprites.Length-1 ;

  try
  {
   return ColorHelper(loadedSprites[colorableColor]);
  }
  catch (Exception e)
  {
   return ColorHelper(loadedSprites[0]);   //IT SOMETIMES GOES OUT OF ARRAY. MAYBE NEED FIX SOMEWHERE?
  }

 }
 public int ColorHelper(Sprite sprite)
 {
  int counter = 0;
  for (int i = 0; i < Sprites.Length; i++)
  {
   if (sprite == Sprites[i])
   {
    counter = i;
    break;
   }
  }

  return GetTheRightColor(counter);
 }
 
 private int GetTheRightColor(int spriteColor)
 {
  switch (spriteColor)
  {
   case 0:
    spriteColor = 2;
    break;
   case 1:
    spriteColor = 2;
    break;
   case 2:
    spriteColor = 0;
    break;
   case 3:
    spriteColor = 7;
    break;
   case 4:
    spriteColor = 4;
    break;
   case 5:
    spriteColor = 3;
    break;
   case 6:
    spriteColor = 4;
    break;
   case 7:
    spriteColor = 0;
    break;
  }

  return spriteColor;
 }
}
