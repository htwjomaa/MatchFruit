using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FruitMatch.Scripts.Blocks;
using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.GUI.Boost;
using FruitMatch.Scripts.Items;
using FruitMatch.Scripts.Level;
using FruitMatch.Scripts.System.Combiner;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

/// <summary>
/// A SideDot is a Dot that sits on the Side lines and is not part of the main Grid.
/// A Dot is a Dot in the main grid.
/// </summary>
public sealed class SideDot : MonoBehaviour
{
    // ---- public Properties---- //
    public int columnSideDot; //x
    public int rowSideDot;
    
    // ---- private Properties---- //
    private bool onlyDot = false;
    internal List<DotObject> changeSquares;
    internal Queue<MoveTowardsTarget> collectedMovePositions = new Queue<MoveTowardsTarget>();
    private Queue<OtherSideDotPos> otherSideDotsPosQueue = new Queue<OtherSideDotPos>();
    private List<SideDotComponent> needSideDotComponent = new List<SideDotComponent>();
    [SerializeField] public SideDotType sideDotType = SideDotType.turnA;

    public string ClickonsidedotSound = "clickonsidedot";
    [SerializeField] private string noSideDotMovementPossibleSound = "clickonsidedot";
    private GameObject destroyablelastDot;
    private GameObject lastObjMoveNextDot;
    private GameObject otherSideDot = null;
    public GameObject iconObj;
   public static GameObject OtherSideDotForPrepapre;

    // --------------------------------------------  private Unity functions -------------------------------------------- //
    private void Awake()
    {
        collectedMovePositions = null;
        if (!GetComponent<Square>())
        {
            transform.localScale = new Vector3(0.42f, 0.42f, 0.42f);
        }
       
    }
    private void Start() =>
        //        Debug.Log("I have been added!!!");
        PrepareSideDotDelayed(0.33f);

    public void PrepareSideDotDelayed(float seconds)
    {
        StartCoroutine(DelayPrepareSideDotCo(seconds));
    }
    IEnumerator DelayPrepareSideDotCo(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PrepareSideDot();
      //  LoadingHelper.THIS.sideDotStart = false;
    }
    public void PrepareSideDot()
    {
        if(iconObj != null) Destroy(iconObj);
        iconObj = new GameObject();
        iconObj.name = "Icon Object";
        
        SpriteRenderer spriteRenderer = new SpriteRenderer();
        if (GetComponent<SpriteRenderer>())
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        else GetComponentInChildren<SpriteRenderer>();
        
        if(spriteRenderer != null) SideDotInit.UpdateSpriteAlpha(spriteRenderer);
        
        SideDotTypeSolver(); 
        SetIconIdentifier();
        
    }
    [Button] public void SetIconIdentifier() => SideDotInit.SetIconIdentifier(columnSideDot, rowSideDot, sideDotType, ref iconObj, transform);
    public void SideDotTypeSolver() =>  SideDotInit.SideDotTypeSolver(columnSideDot, rowSideDot, ref sideDotType);

    public void OnMouseDown()
    {
        LoadingHelper.THIS.sideDotStart = false;
       if (LevelManager.GetGameStatus() == GameState.RegenLevel || LevelManager.THIS.findMatchesStarted || !LoadingHelper.THIS.sideDotAndSwitchFinished) return;
       LoadingHelper.THIS.sideDotAndSwitchFinished = false;
       // if (GetComponent<AnimationItem>() != null) GetComponent<AnimationItem>().MoveUp(0.33f);

        if (LoadingHelper.THIS.FieldBoard == null)
        {
            LoadingHelper.THIS.FieldBoard = LoadingHelper.THIS.FieldParent.GetComponentInChildren<FieldBoard>();
            LoadingHelper.THIS.ConvertToTwoDimensionalArray(LoadingHelper.THIS.FieldBoard,
                ref LoadingHelper.THIS.allDots);
        }
        
        if (LevelManager.THIS.ActivatedBoost.type != BoostType.FreeMove)
        {
            if (LevelManager.THIS.levelData.limitType == LIMIT.MOVES)
                LevelManager.THIS.levelData.Limit--;
            
            LevelManager.THIS.moveID++;
        }

        if (LevelManager.THIS.ActivatedBoost.type == BoostType.FreeMove)
            LevelManager.THIS.ActivatedBoost = null;

        Destroy(iconObj);
        Rl.GameManager.PlayAudio(Rl.soundStrings.Clickonsidedot, Random.Range(0,5), Rl.settings.GetSFXVolume, Rl.effects.audioSource);
        StartActionSideDot(columnSideDot, rowSideDot, LoadingHelper.THIS.width, LoadingHelper.THIS.height,
            ref LoadingHelper.THIS.allDots);
    }

    private SpriteRenderer[] sprtRendererOtherSideDot;
    private bool particlePlayed = false;
    private IEnumerator CheckIfOtherSideDotStoppedMovingCo(float sec)
    {
//        Debug.Log("Checking");
        var pos = OtherSideDotForPrepapre.transform.position;
        yield return new WaitForSeconds(sec);
        if (OtherSideDotForPrepapre.transform.position != pos)
            StartCoroutine(CheckIfOtherSideDotStoppedMovingCo(sec));
        else
        {
            if(sideDotTeleported) yield break;
          //  Debug.Log("Same Pos");
            sprtRendererOtherSideDot = otherSideDot.GetComponentsInChildren<SpriteRenderer>();
            foreach (var t in sprtRendererOtherSideDot)
            {
                var color = t.color;
                color = new Color(color.r, color.g, color.b, 0f);
                t.color = color;
            }
           
            playVanishParticle(0);

        }
    }

    private void playVanishParticle(int particleNumber)
    {
      // LoadingHelper.THIS.VanishParticleObjects[particleNumber].gameObject.SetActive(false);
        LoadingHelper.THIS.VanishParticleObjects[particleNumber].transform.position = OtherSideDotForPrepapre.transform.position;
        if (!particlePlayed)
        {
            LoadingHelper.THIS.VanishParticleObjects[particleNumber].gameObject.SetActive(true);
            Rl.GameManager.PlayAudio(Rl.soundStrings.Vanish, Random.Range(0,5), Rl.settings.GetSFXVolume, Rl.effects.audioSource);
            StartCoroutine(ParticleActiveSecurityCo(2f));
        }
    }
    IEnumerator ParticleActiveSecurityCo(float sec)
    {
        particlePlayed = true;
        yield return new WaitForSeconds(sec);
        particlePlayed = false;
    }
    private bool sideDotTeleported = false;
    void Update()
    { 
       // if(Icon == null) SetIconIdentifier();
      if (!SideDotInit.CheckCorners(columnSideDot, rowSideDot, LoadingHelper.THIS.width, LoadingHelper.THIS.height, ref sideDotType)) sideDotType = SideDotType.moveLine;
       if (collectedMovePositions is null) return;   //for now guarding, later enclose it  | null as a bool
       if(!sideDotTeleported) StartCoroutine(CheckIfOtherSideDotStoppedMovingCo(0.05f));
       if (collectedMovePositions.Count > 0)
       { 
           SideDotAnimation.AnimateAllDots(ref collectedMovePositions);  // this one gets activated  // as long as there are still dots that need animation
           return;   //guarding  the code below
       }
       
       if (otherSideDotsPosQueue.Count > 0)
       {
           OtherSideDotMovingTeleportingAndMoving(ref otherSideDotsPosQueue);
           sideDotTeleported = true;
          StopCoroutine(CheckIfOtherSideDotStoppedMovingCo(0.05f));
          return;  //guarding again the code blow
       }

       SideDotBugSecurity.ChangeSquareRefs(ref changeSquares);  
       if (otherSideDot != null && otherSideDot.GetComponent<SideDot>() != null)
          // otherSideDot.GetComponent<SideDot>().PrepareSideDot();
       
       for (int i = 0; i < needSideDotComponent.Count; i++)
       {
           if (!needSideDotComponent[i].HasSideDot)
               needSideDotComponent[i].GameObject.AddComponent<SideDot>();

         //  if (!needSideDotComponent[i].HasSideDot)   Destroy(needSideDotComponent[i].GameObject.GetComponent<SideDot>().Icon);
           needSideDotComponent[i].GameObject.GetComponent<SideDot>().columnSideDot = needSideDotComponent[i].Column;
           needSideDotComponent[i].GameObject.GetComponent<SideDot>().rowSideDot = needSideDotComponent[i].Row;
           if (!needSideDotComponent[i].HasSideDot)
           {
           //    needSideDotComponent[i].GameObject.GetComponent<SideDot>().PrepareSideDot();
           }
           else
           {
               needSideDotComponent[i].GameObject.GetComponent<SideDot>().sideDotType =
                   needSideDotComponent[i].SideDotType;
               //  needSideDotComponent[i].GameObject.GetComponent<SideDot>().SetIconIdentifier();
           }
       }
       LevelManager.THIS.FindMatches();
       LoadingHelper.THIS.sideDotAndSwitchFinished = true;
       //CheckIfItemsAreStillValid();

       if (OtherSideDotForPrepapre.GetComponent<SideDot>() == null)
       {
           OtherSideDotForPrepapre.AddComponent<SideDot>();
           OtherSideDotForPrepapre.GetComponent<SideDot>().columnSideDot = columnSideDot;
           OtherSideDotForPrepapre.GetComponent<SideDot>().rowSideDot = rowSideDot;
       }
       else
       {
           OtherSideDotForPrepapre.GetComponent<SideDot>().PrepareSideDot();
       }
      
    // if(OtherSideDotForPrepapre != null && OtherSideDotForPrepapre.GetComponent<SideDot>() != null) OtherSideDotForPrepapre.GetComponent<SideDot>().PrepareSideDot();
     // else
    //  {
       //   SideDot[] n = FindObjectsOfType<SideDot>();
       //   for (int i = 0; i < n.Length; i++)
       //  {
         //     if(n[i].iconObj == null) n[i].PrepareSideDot();
         // }
     // }
      if(!onlyDot) Destroy(GetComponent<SideDot>());
      else collectedMovePositions = null;
    }

    private void CheckIfItemsAreStillValid()
    {
        ItemSimple[] itemSimples = FindObjectsByType<ItemSimple>(FindObjectsSortMode.None);
        for (int i = 0; i < itemSimples.Length; i++)
        {
            if (itemSimples[i].enabled && itemSimples[i] != itemSimples[i].square.item)
            {
                itemSimples[i].square = null;
                itemSimples[i].DestroyItem(false, false);
            }
          
        }
    }
    
    
    private void OtherSideDotMovingTeleportingAndMoving(ref Queue<OtherSideDotPos> otherSideDotPos)
   {
       while (otherSideDotPos.Count > 0)
       {  
           OtherSideDotPos tempObject = otherSideDotPos.Dequeue();  //moves them from one que to another
           tempObject.moveTowardsTarget.objectToMove.transform.position = tempObject.teleportPos; // but uses the teleport position
           collectedMovePositions.Enqueue(tempObject.moveTowardsTarget); //before adding it to the other queue with less information

           if (sprtRendererOtherSideDot == null) return;
           foreach (SpriteRenderer spriteRenderer in sprtRendererOtherSideDot)
           {
               SideDotInit.UpdateSpriteAlpha(spriteRenderer);
           }
           //playVanishParticle(1);
       }
   }

    public static bool CheckForItems (GameObject square)
    {
        var item = square.GetComponent<Square>().item;
        if (item.GetComponent<ItemSimple>() || item.GetComponent<ItemStriped>() ||
            item.GetComponent<ItemMarmalade>()) return true;
        return false;
    }
   public static void ItemEnableDisable(ref GameObject item, bool enable)
   {
       if (item == null) return;
       if (item.GetComponent<ItemSimple>() && !enable) item.GetComponent<ItemSimple>().square = null;
       if(item.GetComponent<ItemSimple>() )  item.GetComponent<ItemSimple>().enabled = enable;
       if(item.GetComponent<ItemStriped>() )  item.GetComponent<ItemStriped>().enabled = enable;
       if(item.GetComponent<ItemMarmalade>() )  item.GetComponent<ItemMarmalade>().enabled = enable;
       if(item.GetComponent<ItemPackage>() )  item.GetComponent<ItemPackage>().enabled = enable;
       if(item.GetComponent<ItemIngredient>() )  item.GetComponent<ItemIngredient>().enabled = enable;
       if(item.GetComponent<ItemMulticolor>() )  item.GetComponent<ItemMulticolor>().enabled = enable;
       if(item.GetComponent<ItemSpiral>() )  item.GetComponent<ItemSpiral>().enabled = enable;
       //if(item.GetComponent<Item>() )  item.GetComponent<Item>().enabled = enable;
       if(item.GetComponent<IColorableComponent>() )  item.GetComponent<IColorableComponent>().enabled = enable;
       if(item.GetComponent<Animator>())  item.GetComponent<Animator>().enabled = enable;
       
       if(item.GetComponent<ItemStriped>())  item.GetComponent<ItemStriped>().enabled = enable;
       if(item.GetComponent<ItemCombineBehaviour>())  item.GetComponent<ItemCombineBehaviour>().enabled = enable;

     
   }
// --------------------------------------------  private methods -------------------------------------------- //
private void StartActionSideDot(int column, int row, int width, int height, ref GameObject[,] allDots)
    {  // ---- Coordinater ---- //
     //   Rl.board.currentState = GameState.wait;
        //coordinates the passed variables
        ClearQueues();
        
        switch(SideDotUtil.CheckDirection(column, row, width, height))
        {  
          case Directions.top:
              SwipeActionsStack(column, 0, width, height, Directions.top, ref allDots);
              break;
          case Directions.bottom:
              SwipeActionsStack(column, height-1, width, height, Directions.bottom, ref allDots);
              break;
          case Directions.left:
              SwipeActionsStack(width-1, row, width, height, Directions.left, ref allDots);
              break;
          case Directions.right:
              SwipeActionsStack(0, row, width, height, Directions.right, ref allDots);
              break;
        }
    }

private void ClearQueues()
{
changeSquares?.Clear();
needSideDotComponent?.Clear();
collectedMovePositions?.Clear();
otherSideDotsPosQueue?.Clear();
destroyablelastDot = null;
lastObjMoveNextDot = null;
otherSideDot = null;
LevelManager.THIS.findMatchesStarted = false;
}

private void SwipeActionsStack(int column, int row,int width, int height, Directions direction, ref GameObject[,] allDots)
   { //Coordinates the method order    ---- more or less the "main function"
       //Preparation
      // Destroy(Icon);
       changeSquares = new List<DotObject>();
       FirstSquareWayPoints = new List<GameObject>();
       otherSideDotsPosQueue = new Queue<OtherSideDotPos>();
       bool firstDot = true;
       int thisDotX = columnSideDot, thisDotY = rowSideDot;
       int modX = 0,  modY = 0;
       onlyDot = false;
       Queue<MoveTowardsTarget> helperQueue = new Queue<MoveTowardsTarget>();
       FirstSquareWayPoints.AddRange(GetFirstViableDot(column, row,ref modX, ref modY, ref allDots, direction,
           ref onlyDot));
       otherSideDot = SideDotOtherSideDot.GetOtherSideDot(0,0, columnSideDot, rowSideDot , ref direction, ref allDots, sideDotType, ref lastObjMoveNextDot);
       int targetColumn = -99999, targetRow = -99999;
  
      if((onlyDot && FirstSquareWayPoints.Count> 1) || !onlyDot) 
          SideDotNextDot.MoveToNextDot(FirstSquareWayPoints[^1], direction, allDots, ref lastObjMoveNextDot, ref helperQueue, ref changeSquares);
      else HandleEdgeCaseOnlyDot(ref FirstSquareWayPoints, direction, allDots);

  
      SideDotLastDot.MoveLastDot(lastObjMoveNextDot,ref otherSideDot, allDots, direction, ref needSideDotComponent, ref helperQueue);
        SideDotOtherSideDot.MoveOtherSideDot(otherSideDot, width, height, thisDotX, thisDotY, sideDotType, direction, transform, ref helperQueue, ref otherSideDotsPosQueue, ref needSideDotComponent);
        if (!onlyDot) MoveFirstSideDot(FirstSquareWayPoints, ref helperQueue);
        else MoveEdgeCaseOnlyDot(ref FirstSquareWayPoints, otherSideDot, ref helperQueue, ref needSideDotComponent);
        
       //helperQueue = ChangeHelperQueueOrder(direction, helperQueue);
      SideDotBugSecurity.ConvertToRightQueue(ref collectedMovePositions,ref helperQueue, ref collectedMovePositions); //helper-queue so we dont get an Update tick in between  
     // Icon.GetComponent<IconScript>().sideDotRef = null;
     Destroy(iconObj);
   }

private void MoveEdgeCaseOnlyDot(ref List<GameObject>FirstSquareWayPoints, GameObject otherSideDot, ref Queue<MoveTowardsTarget> helperQueue,ref List<SideDotComponent> needSideDotComponent)
{
    Debug.Log("MOVE EDGE CASE ONLY DOT");
    GameObject dummyObj = new GameObject();
    SideDot otherDot = otherSideDot.GetComponent<SideDot>();
    Destroy(otherDot.iconObj);
    dummyObj   = Instantiate(dummyObj, otherSideDot.transform.position, quaternion.identity);
    dummyObj.AddComponent<DummyObj>();
    FirstSquareWayPoints.Add(dummyObj.gameObject );
    
   needSideDotComponent.Add(new SideDotComponent(gameObject,
       otherDot .columnSideDot,
       otherDot .rowSideDot,
       true,
       otherDot.sideDotType
       ));
   helperQueue.Enqueue(new MoveTowardsTarget(gameObject, FirstSquareWayPoints, Rl.settings.GetAnimationSpeedLinePush));
}
private void HandleEdgeCaseOnlyDot(ref List<GameObject>FirstSquareWayPoints, Directions direction, GameObject[,] allDots)
{
 
    if (FirstSquareWayPoints.Count == 1)
    {
        Vector2Int pos = SideDotUtil.GetXYFromNormalXYOBJ(FirstSquareWayPoints[0], allDots);
        int column = pos.x;
        int row = pos.y;
        switch (direction)
        {
            case Directions.left:
                column--;
                break;
            case Directions.right:
                column++;
                break;
            case Directions.bottom:
                row--;
                break;
            case Directions.top:
                row++;
                break;
        }
        FirstSquareWayPoints.Add(allDots[column, row]);
    }
    
}
private Queue<MoveTowardsTarget> ChangeHelperQueueOrder(Directions directions,Queue<MoveTowardsTarget> helperQueue)
{
    List<MoveTowardsTarget> helperQueueList = helperQueue.ToList();

    switch (directions)
    {
        case Directions.left:
            helperQueueList = helperQueueList.OrderBy(x => x.objectToMove.transform.position.x).ToList();
            break;
        case Directions.right: 
            helperQueueList = helperQueueList.OrderBy(x => x.objectToMove.transform.position.y).ToList();
            break;
        case Directions.bottom:
            helperQueueList = helperQueueList.OrderBy(y => y.objectToMove.transform.position.y).ToList();
            break;
        case Directions.top: 
            helperQueueList = helperQueueList.OrderBy(y => y.objectToMove.transform.position.y).ToList();
            break;
    }

    Queue<MoveTowardsTarget> newQueue = new Queue<MoveTowardsTarget>();
    for (int i = 0; i < helperQueueList.Count; i++) newQueue.Enqueue(helperQueueList[i]);

    return newQueue;
}
private void MoveFirstSideDot(List<GameObject> firstDotWayPts, ref Queue<MoveTowardsTarget> helperQueue)
    {
        helperQueue.Enqueue(new MoveTowardsTarget(gameObject, firstDotWayPts, Rl.settings.GetAnimationSpeedLinePush));
        //Change Alphas
        SpriteRenderer sprtRndr = new SpriteRenderer();
        if (GetComponent<SpriteRenderer>())
            sprtRndr = GetComponent<SpriteRenderer>();
        else sprtRndr = GetComponentInChildren<SpriteRenderer>();
        
        Color sprtRndrSideDotColor = sprtRndr.color;
        sprtRndr.color = new Color(sprtRndrSideDotColor.r, sprtRndrSideDotColor.g, sprtRndrSideDotColor.b, 1);
       // Vector2 pos = SideDotUtil.GetXYFromNormalXYOBJ(firstDotWayPts[^1], allDots)
     // if(Math.Abs(gameObject.transform.localScale.x - 0.535f) < 0.01f) ScaleDots(gameObject, new Vector3(0.515f, 0.515f, 0.515f));
      //else ScaleDots(gameObject, new Vector3(0.43f, 0.43f, 0.43f));
     // else if(Math.Abs(gameObject.transform.localScale.x - 0.42f) < 0.02f) ScaleDots(gameObject, new Vector3(0.515f, 0.515f, 0.515f));
     int i = 0;
      //  Debug.Log("firstDotSquare:: " + firstDotSquare);
      for (; i < firstDotWayPts.Count; i++)
      {
          if (firstDotWayPts[i].GetComponent<SpriteRenderer>().enabled)
          {
              break;
          }
            
      }
      Debug.Log("MOVING FIRST SIDEDOT: " + gameObject.name + " TO: " + firstDotWayPts[i].name);
        changeSquares.Add(new DotObject(gameObject, firstDotWayPts[i])); //Destroy SideDot because it is no longer a SideDot and became a Dot
    }

    public void ScaleDots(GameObject obj, Vector3 staticEndValue)
    {
        obj.transform.DOScale(staticEndValue, 0.5f);
    }
  
    /*private void GetFirstDotForChecking(int width, int height, int column, int row, ref bool firstDotStat, ref int firstDotColumn, ref int firstDotRow, ref SideDotStat firstSideDotStat, ref GameObject[,] allDots, ref Queue<MoveTowardsTarget> helperQueue, Directions directions)
    {   //Moves all the Dots on the dots one position ahead with a for loop and passes parameters, including what the next Dot is,  to a helper method
        switch (directions)
        {
            case Directions.right:
                for (; column < width - 1; column++) MoveToNextDotHelperMethod(column, row, +1, 0, ref firstDotStat, ref firstDotColumn,ref firstDotRow, ref firstSideDotStat, ref allDots, ref helperQueue);
                break;
            case Directions.top:
                for (; row < height - 1; row++) MoveToNextDotHelperMethod(column, row, 0, +1, ref firstDotStat, ref firstDotColumn,ref firstDotRow, ref firstSideDotStat, ref allDots, ref helperQueue);
                break;
            case Directions.left:
                for (; column > 0; column--) MoveToNextDotHelperMethod(column, row, -1, 0, ref firstDotStat, ref firstDotColumn,ref firstDotRow, ref firstSideDotStat, ref allDots, ref helperQueue);
                break;
            case Directions.bottom:
                for (; row > 0; row--) MoveToNextDotHelperMethod(column, row, 0, -1, ref firstDotStat, ref firstDotColumn,ref firstDotRow, ref firstSideDotStat, ref allDots, ref helperQueue);
                break;
        }
    }
    private void GetFirstDotForCheckingHelperMethod(int column, int row, int nextColumnMod, int nextRowMod, ref bool firstDotStat, ref int firstDotColumn, ref int firstDotRow, ref SideDotStat firstSideDotStat, ref GameObject[,] allDots, ref Queue<MoveTowardsTarget> helperQueue)
    {      
        //Destroy the Dot Component so nothing wild happens "on it's way"
        //  if(allDots[column+ nextColumnMod, row+nextRowMod] is null) 
        bool inArrayBounds = true;
        Vector2Int mod =  CheckForBlankSpaces(column, row, nextColumnMod, nextRowMod, ref inArrayBounds, ref allDots);
        if (!inArrayBounds)
        {
            lastGameObjectMoveNextDot = allDots[column, row];
            return;
        }
        needDotComponent.Add(new DotObject(allDots[column, row], allDots[column+ mod.x, row+mod.y].GetComponent<Dot>().column, allDots[column+ mod.x, row+mod.y].GetComponent<Dot>().row)); //Adds it later back
        //Get moved together later
     
        helperQueue.Enqueue( new MoveTowardsTarget(allDots[column, row].gameObject, allDots[column+ mod.x, row+mod.y].transform.position, Rl.settings.GetAnimationSpeedLinePush));
    }*/public List<GameObject> GetFirstViableDot(int column, int row, ref int modX, ref int modY, ref GameObject[,] allDots, Directions directions, ref bool onlyDot)
{   //Moves all the Dots on the dots one position ahead with a for loop and passes parameters, including what the next Dot is,  to a helper method
    List<GameObject> SqrList = new List<GameObject>();
    SqrList.Add(allDots[column, row]);
    if (allDots[column, row].transform.GetComponent<SpriteRenderer>().enabled)
    {
        return SqrList;
    }
  //  int modX = 0, modY = 0;
    switch (sideDotType)
    {
        case SideDotType.moveLine:
            switch (directions)
            {
                case Directions.right:
                    modX = 1;
                    modY = 0;
                    break;
                case Directions.top:
                    modX = 0;
                    modY = 1;
                    break;
                case Directions.left:
                    modX = -1;
                    modY = 0;
                    break;
                case Directions.bottom:
                    modX = 0;
                    modY = -1;
                    break;
            }
            break;
        case SideDotType.turnA:
            switch (directions)
            {
                case Directions.right:
                    modX = 0;
                    modY = -1;
                    break;
                case Directions.top:
                    modX = -1;
                    modY = 0;
                    break;
                case Directions.left:
                    modX = 0;
                    modY = -1;
                    break;
                case Directions.bottom:
                    modX = -1;
                    modY = 0;
                    break;
            }
            break;
        case SideDotType.turnB:
            switch (directions)
            {
                case Directions.right:
                    modX = 0;
                    modY = 1;
                    break;
                case Directions.top:
                    modX = 1;
                    modY = 0;
                    break;
                case Directions.left:
                    modX = 0;
                    modY = 1;
                    break;
                case Directions.bottom:
                    modX = 1;
                    modY = 0;
                    break;
            }
            break;
    }

    //SideDotUtil.CheckForBlankSpaces(column, row, directions, allDots, ref SqrList);
    Vector2Int newDots = SideDotUtil.CheckForBlankSpaces(column, row, modX, modY, ref allDots, ref SqrList, ref onlyDot);
    
    if(column+newDots.x < LoadingHelper.THIS.width &&
       row + newDots.y < LoadingHelper.THIS.height &&
       column+newDots.x >-1 &&
       row + newDots.y > -1)SqrList.Add(allDots[column +newDots.x, row +newDots.y]);

    if (newDots.x < -50) Debug.Log("SOmething went wrong SqrList.Count: " + SqrList.Count);
    else
    {
        Debug.Log("Everything is fine. Count is: "  + SqrList.Count);
    }
 
    return SqrList;
}

[SerializeField] private List<GameObject> FirstSquareWayPoints = new List<GameObject>();
}
public struct SideDotComponent
{
    public GameObject GameObject;
    public int Column;
    public int Row;
    public bool HasSideDot;
    public SideDotType SideDotType;

    public SideDotComponent(GameObject gameObject, int column, int row, bool hasSideDot, SideDotType sideDotType)
    {
        GameObject = gameObject;
        Column = column;
        Row = row;
        HasSideDot = hasSideDot;
        SideDotType = sideDotType;
    }
}