using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using DG.Tweening;
using NaughtyAttributes;
using FruitMatch.Scripts.Core;
using UnityEngine.PlayerLoop;

public sealed class Dot : MonoBehaviour 
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public bool isMatched = false;
    public Colors FruitColor = Colors.AlleFarben;
    public GameObject otherDot;
    
    private Vector2 firstTouchPosition = Vector2.zero;
    private Vector2 finalTouchPosition = Vector2.zero;
    private Vector2 tempPosition;

    [Header("Swipe Stuff")]
    public float swipeAngle = 0;
    public float swipeResist = 1f;

    [Header("Powerup Stuff")]
    public bool isColorBomb ;
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isAdjacentBomb;

  
    //This is for testing and Debug only.
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
        {
            isAdjacentBomb = true;
            GameObject marker = Instantiate(Rl.dotPrefabs.adjacentMarker, transform.position, Quaternion.identity);
            marker.transform.parent = this.transform;
        }
    }

    // private IEnumerator Start()
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     if(Rl.board.allDots[column,row] == null)Destroy(this.gameObject);
    //     StartCoroutine(Start());
    // }
    private int _firstObjectSortingOrder, _secondObjectSortingOrder;
    private bool _finishedSwappingFirstDot, _finishedDestroyingFirstDot;
    IEnumerator CheckIfMoveAgain()
    {
        if (_finishedSwappingFirstDot && _finishedDestroyingFirstDot)
        {
            Rl.board.currentState = GameState.move;
            yield break;
        }

        yield return new WaitForFixedUpdate();
        StartCoroutine(CheckIfMoveAgain());
    }
    private IEnumerator StartAnimationCo(GameObject firstObject, GameObject secondObject, float duration)
    {
       //yes this is totally messy  because of all the null checks, but it works and ensures that an object can be destroyed at any given time.
       if (GetComponent<AnimationItem>() != null) GetComponent<AnimationItem>().MoveUp(duration*0.8f);
        if (firstObject != null)  _firstObjectSortingOrder = firstObject.GetComponent<SpriteRenderer>().sortingOrder;
        if (secondObject != null) _secondObjectSortingOrder = secondObject.GetComponent<SpriteRenderer>().sortingOrder;
        
        if (firstObject != null)  firstObject.GetComponent<SpriteRenderer>().sortingOrder = 6;
        if (secondObject != null) secondObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
        
        if (firstObject != null)  StartCoroutine(AnimateSwapCo(Rl.board.swapBiggerNumber, duration*0.4f, firstObject));
        if (secondObject != null) yield return StartCoroutine(AnimateSwapCo(Rl.board.swapSmallerNumber , duration*0.4f, secondObject));
        
        if (firstObject != null)  firstObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
        if (secondObject != null) secondObject.GetComponent<SpriteRenderer>().sortingOrder = 6;
        
        if (firstObject != null)  StartCoroutine(AnimateSwapCo(1f, duration, firstObject));
        if (secondObject != null) yield return StartCoroutine(AnimateSwapCo(1.15f, duration*0.65f, secondObject));
        if (secondObject != null) StartCoroutine(AnimateSwapCo(1f, duration*0.4f, secondObject));
        
        if (firstObject != null)  firstObject.GetComponent<SpriteRenderer>().sortingOrder = _firstObjectSortingOrder;
        if (secondObject != null) secondObject.GetComponent<SpriteRenderer>().sortingOrder = _secondObjectSortingOrder;

        _finishedSwappingFirstDot = true;
        yield break;
        // yield return StartCoroutine(AnimateSwapCo(1f, duration, objectToIncrease));
    }
    private IEnumerator AnimateSwapCo(float endValue, float duration, GameObject firstObject)
    {
        if (firstObject == null) yield break;
        // Vector2 middlePos = firstObject.transform.position + (firstObject.transform.position - secondObject.transform.position).normalized * 0.5f;
        firstObject.transform.DOScale(endValue, duration).SetEase(Rl.board.swapbackEase);
        yield return new WaitForSeconds(duration);
    }
    private void Update()
    {
        if (Mathf.Abs(column - transform.position.x) > Rl.board.dotSnapValue)
        {
            //Move Towards the target
            tempPosition = new Vector2(column, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition,0.1f);
            if (Rl.board.AllDots[column, row] != this.gameObject)
            {
                Rl.board.AllDots[column, row] = this.gameObject;
                Rl.findMatches.FindAllMatches();
            }
            
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(column, transform.position.y);
            transform.position = tempPosition;
        }
        if (Mathf.Abs(row - transform.position.y) >Rl.board.dotSnapValue)
        {   //Move Towards the target
            tempPosition = new Vector2(transform.position.x, row);
            transform.position = Vector2.Lerp(transform.position, tempPosition,0.1f);
            if (Rl.board.AllDots[column, row] != gameObject)
            {
                Rl.board.AllDots[column, row] = gameObject;
                Rl.findMatches.FindAllMatches();
            }
   
        }
        else
        {   //Directly set the position
            tempPosition = new Vector2(transform.position.x, row);
            transform.position = tempPosition;
        }
	}
    
    public IEnumerator CheckMoveCo()
    {
        /*if(isColorBomb)
        {
            //This piece is a color bomb, and the other piece is the color to destroy
            Rl.findMatches.MatchPiecesOfColor(otherDot.tag);
            isMatched = true;
        }
        else if(otherDot.GetComponent<Dot>().isColorBomb)
        {
            //The other piece is a color bomb, and this piece has the color to destroy
            Rl.findMatches.MatchPiecesOfColor(this.gameObject.tag);
            otherDot.GetComponent<Dot>().isMatched = true;
        }*/
        //Add a check here to see if they're both color bombs
        yield return new WaitForSeconds(.5f);
        if (otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = previousRow;
                column = previousColumn;
               // Rl.GameManager.PlayAudio(SwapdotAudio, Random.Range(0, 4), Rl.settings.GetSFXVolume,
              //      Rl.effects.audioSource);
              Rl.GameManager.PlayAudio(Rl.soundStrings.SwitchSound , Random.Range(0, 4), Rl.settings.GetSFXVolume,
                  Rl.effects.audioSource);
                yield return new WaitForSeconds(.5f);
                Rl.board.currentDot = null;
              //  Rl.board.currentState = GameState.move;
              _finishedDestroyingFirstDot = true;

            }
            else
            {
                if (Rl.endGameManager != null)
                {
                    if (Rl.endGameManager.requirements.GameType == GameType.Moves)
                    {
                        Rl.endGameManager.DecreaseCounterValue();
                    }
                }

                Rl.board.DestroyMatches();
            }
            //otherDot = null;
        }
    }

    /*
    [SerializeField] private float _distance;
    [SerializeField]  private Vector2 _currentPos;

    private void Start()
    {
        _distance = GetDistance()/4;
    }

    private float GetDistance()
    {
       return MathLibrary.CalculateDistance(Rl.board.allDots[0, 0].transform.gameObject,
            Rl.board.allDots[0, 1].transform.gameObject);
    }*/

    // private void OnMouseDrag()
    // {
    //     _currentPos = (Vector2)Rl.Cam.ScreenToWorldPoint(Input.mousePosition);
    //     if (MathLibrary.CalculateDistance(firstTouchPosition, _currentPos ) > _distance)
    //     {
    //         OnMouseUpFunc();
    //     }
    // }
    [HideInInspector]
    public Vector3 mousePos;
    private void OnMouseDown()
    {
        if (Rl.board.currentState == GameState.pause) return;
        //Destroy the hint
        if ((BoardUtil.CheckTileBelow(column, row) is TileKind.Breakable or TileKind.Lock or TileKind.Chest) && Rl.board.currentState != GameState.move) return;
       // deltaPos = mousePos - GetMousePosition();
       // SwitchDirection(deltaPos);
        firstTouchPosition = Rl.Cam.ScreenToWorldPoint(Input.mousePosition);
        Rl.board.SetFocusPos(this.transform.position);
        StartCoroutine(CalculateFinalTouchPosCO());
        if(Rl.hintManager !=null) Rl.hintManager.DestroyHint();
    }


    IEnumerator CalculateFinalTouchPosCO()
    {
        if(Rl.board.currentState != GameState.move) yield break;
        finalTouchPosition = Rl.Cam.ScreenToWorldPoint(Input.mousePosition);
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist ||
            Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            Rl.board.currentState = GameState.wait;

            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            Rl.board.currentDot = this;
            yield break;
        }
        else Rl.board.currentState = GameState.move;
        yield return new WaitForFixedUpdate();
        StartCoroutine(CalculateFinalTouchPosCO());
       
    }

 
    private void OnMouseUpFunc()
    {
        if (BoardUtil.CheckTileBelow(column, row) is TileKind.Breakable or TileKind.Lock or TileKind.Chest && Rl.board.currentState != GameState.move) return;
        //Rl.board.currentTouchedDot = null;
        finalTouchPosition = Rl.Cam.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    private void OnMouseUp()
    {
        Rl.board.ResetFocusPos();
        StopCoroutine(CalculateFinalTouchPosCO());
    }

    void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            Rl.board.currentState = GameState.wait;
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            Rl.board.currentDot = this;
        }
        else Rl.board.currentState = GameState.move;
    }

 
    void MovePiecesActual(Vector2 direction)
    {
        otherDot = Rl.board.AllDots[column + (int)direction.x, row + (int)direction.y];
        previousRow = row;
        previousColumn = column;
        if (Rl.board.lockTiles[column, row] == null && Rl.board.lockTiles[column + (int)direction.x, row + (int)direction.y] == null)
        {
            if (otherDot != null)
            {
                Rl.GameManager.PlayAudio(Rl.soundStrings.SwitchSound , Random.Range(0, 4), Rl.settings.GetSFXVolume,
                    Rl.effects.audioSource);
                 _finishedSwappingFirstDot = false;
                 _finishedDestroyingFirstDot = false;
                StartCoroutine(CheckIfMoveAgain());
                StartCoroutine(StartAnimationCo(gameObject, otherDot, 0.7f));
                otherDot.GetComponent<Dot>().column += -1 * (int)direction.x;
                otherDot.GetComponent<Dot>().row += -1 * (int)direction.y;
                column += (int)direction.x;
                row += (int)direction.y;
                StartCoroutine(CheckMoveCo());
            }
            else
            {
                Rl.board.currentState = GameState.move;
            }
        }
        else
        {
            Rl.board.currentState = GameState.move;
        }
    }
    
    public Vector3 deltaPos;
    //direction of switching this item
    [HideInInspector]
    public Vector3 switchDirection;
    public Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public void SwitchDirection(Vector3 delta)
        {
            deltaPos = delta;
            if (Vector3.Magnitude(deltaPos) > 0.1f)
            {
                //LevelManager.THIS.DragBlocked = true;
              //  switchItem = null;
                if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y) && deltaPos.x > 0 /*&& !tutorialItem*/)
                    switchDirection.x = 1;
                else if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y) && deltaPos.x < 0 /* && !tutorialItem*/)
                    switchDirection.x = -1;
                else if (Mathf.Abs(deltaPos.x) < Mathf.Abs(deltaPos.y) && deltaPos.y > 0)
                    switchDirection.y = 1;
                else if (Mathf.Abs(deltaPos.x) < Mathf.Abs(deltaPos.y) && deltaPos.y < 0)
                    switchDirection.y = -1;

              
                    if (switchDirection.x > 0)
                    {
                        MovePiecesActual(Vector2.left);
                    }
                    else if (switchDirection.x < 0)
                    {
                        MovePiecesActual(Vector2.right);
                    }
                    
                    else  if (switchDirection.y > 0)
                    {
                        MovePiecesActual(Vector2.down);
                    }
                    else if (switchDirection.y < 0)
                    {
                        MovePiecesActual(Vector2.up);
                    }
                
             
                /*if (switchDirection.x > 0)
                {
                    neighborSquare = square.GetNeighborLeft();
                }
                else if (switchDirection.x < 0)
                {
                    neighborSquare = square.GetNeighborRight();
                }
                else if (switchDirection.y > 0)
                {
                    neighborSquare = square.GetNeighborBottom();
                }
                else if (switchDirection.y < 0)
                {
                    neighborSquare = square.GetNeighborTop();
                }

                if (neighborSquare != null)
                    switchItem = neighborSquare.Item;
                if (switchItem != null)
                {
                    if (switchItem.square.GetSubSquare().CanGoOut())
                        LevelManager.THIS.StartCoroutine(Switching());
                    else if (((currentType != ItemsTypes.NONE || switchItem.currentType != ItemsTypes.NONE) && (currentType != ItemsTypes.INGREDIENT && switchItem.currentType != ItemsTypes.INGREDIENT)) &&
                             switchItem.square.GetSubSquare().CanGoOut())
                        LevelManager.THIS.StartCoroutine(Switching());
                    else
                        ResetDrag(); //1.6.1
                }
                else
                    ResetDrag();*/
            }
        }
    void MovePieces()
    {
        Rl.board.ResetFocusPos();
        if (swipeAngle > -45 && swipeAngle <= 45 && column < Rl.board.width - 1) MovePiecesActual(Vector2.right);
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < Rl.board.height - 1) MovePiecesActual(Vector2.up);
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0) MovePiecesActual(Vector2.left);
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0) MovePiecesActual(Vector2.down);
        else Rl.board.currentState = GameState.move;
        
        if (row > 0 && row < Rl.board.height - 1)
        {
            // GameObject upDot1 = Rl.board.allDots[column, row + 1];
            // GameObject downDot1 = Rl.board.allDots[column, row - 1];
            // if (upDot1 != null && downDot1 != null)
            // {
            //     if (upDot1.tag == this.gameObject.tag && downDot1.tag == gameObject.tag)
            //     {
            //         upDot1.GetComponent<Dot>().isMatched = true;
            //         downDot1.GetComponent<Dot>().isMatched = true;
            //         isMatched = true;
            //     }
            // }
        }
    }
 
    public void MakeRowBomb()
    {
        if (!isColumnBomb && !isColorBomb && !isAdjacentBomb)
        {
            isRowBomb = true;
            GameObject arrow = Instantiate(Rl.dotPrefabs.rowArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;
        }
    }
 
    public void MakeColumnBomb()
    {
        if (!isRowBomb && !isColorBomb && !isAdjacentBomb)
        {
            isColumnBomb = true;
            GameObject arrow = Instantiate(Rl.dotPrefabs.columnArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;
        }
    }
    
    public void MakeColorBomb()
    {
        if (!isColumnBomb && !isRowBomb && !isAdjacentBomb)
        {
            isColorBomb = true;
            GameObject color = Instantiate(Rl.dotPrefabs.colorBomb, transform.position, Quaternion.identity);
            color.transform.parent = this.transform;
            this.gameObject.tag = "Color";
        }
    }
    
    public void MakeAdjacentBomb()
    {
        if (!isColumnBomb && !isRowBomb && !isColorBomb)
        {
            isAdjacentBomb = true;
            GameObject marker = Instantiate(Rl.dotPrefabs.adjacentMarker, transform.position, Quaternion.identity);
            marker.transform.parent = this.transform;
        }
    }
}
