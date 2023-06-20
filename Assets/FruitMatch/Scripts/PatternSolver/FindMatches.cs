using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class FindMatches : MonoBehaviour {

    private Board board;
    public HashSet<GameObject> currentMatches = new HashSet<GameObject>();
    public List<Sequence> SequenceToMatch = new List<Sequence>();
    [Space] 
    [Header("MatchStyle")]
    public bool MatchDiagonal = true;
    public bool MatchDiagonalRightUp = true;
    public bool MatchDiagonalLeftUp = true;
    public bool MatchRow = true;
    public bool MatchRowOnlyVertical = true;
    public bool MatchRowOnlyHorizontal = true;
    public bool MatchBlock = true;
    public bool MatchCorner = true;    
    public bool MatchEnitreRow = true;
    public bool MatchEnitreRowOnlyHorizontal = true;
    public bool MatchEnitreRowOnlyVertical = true;
    [Space]
    [Header("Match after")]
    public bool MatchFruit = true;
    public bool MatchColors = true;
    public bool MatchSequence = true;

    public bool startMatching = false;
	// Use this for initialization
	void Start ()
    {
        board = Rl.board;
        startMatching = false;
    }

    public void FindAllMatches()
    {
        startMatching = true;
    }

    private void LateUpdate()
    {
        if (!startMatching) return;
        StartCoroutine(FindAllMatchesCo());
        startMatching = false;
    }

    private List<GameObject> IsAdjacentBomb(Dot dot1, Dot dot2, Dot dot3){
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot1.column, dot1.row));
        }

        if (dot2.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot2.column, dot2.row));
        }

        if (dot3.isAdjacentBomb)
        { 
            currentMatches.Union(GetAdjacentPieces(dot3.column, dot3.row));
        }
        return currentDots;
    }

    private List<GameObject> IsRowBomb(Dot dot1, Dot dot2, Dot dot3){
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot1.row));
            BombClass.BombRow(dot1.row, board.width, ref board.concreteTiles);
        }

        if (dot2.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot2.row));
            BombClass.BombRow(dot2.row, board.width, ref board.concreteTiles);
        }

        if (dot3.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot3.row));
            BombClass.BombRow(dot3.row, board.width, ref board.concreteTiles);
        }
        return currentDots;
    }

    private List<GameObject> IsColumnBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot1.column));
            BombClass.BombColumn(dot1.column, board.width, board.height, ref board.concreteTiles);
        }

        if (dot2.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot2.column));
            BombClass.BombColumn(dot2.column, board.width, board.height, ref board.concreteTiles);
        }

        if (dot3.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot3.column));
            BombClass.BombColumn(dot3.column, board.width, board.height, ref board.concreteTiles);
        }
        return currentDots;
    }

    private void AddToListAndMatch(GameObject dot)
    {
       // if (!currentMatches.Contains(dot)) currentMatches.Add(dot);
        dot.GetComponent<Dot>().isMatched = true;
    }

    private void GetNearbyPieces(params GameObject[] dotMatches)
    {
        foreach (var dot in dotMatches) 
            AddToListAndMatch(dot);
    }

    private IEnumerator FindAllMatchesCo()
    {
      
        //yield return new WaitForSeconds(.2f);
       MatchFinder();
        yield return null;
    }
    
 private void DisableList() => DisableEnableList(true, MatchDiagonal, MatchDiagonalRightUp,
     MatchDiagonalLeftUp, MatchRow, MatchRowOnlyVertical, MatchRowOnlyHorizontal, MatchBlock, MatchCorner, MatchEnitreRow);
private void EnableList() => DisableEnableList(false, MatchDiagonal, MatchDiagonalRightUp,
     MatchDiagonalLeftUp, MatchRow, MatchRowOnlyVertical, MatchRowOnlyHorizontal, MatchBlock, MatchCorner, MatchEnitreRow);
    private void DisableEnableList(bool disable, params bool[] toModifiy)
    {
        Debug.Log("hi");
        if (disable)
        {
            for (var index = 0; index < toModifiy.Length; index++) toModifiy[index] = false;
            return;
        }
     
           for (var index = 0; index < toModifiy.Length; index++)
               toModifiy[index] = true;
    }
   
   private void EntireRowHelperMethod(int length, Directions directions)
   {
       List<GameObject> helperList = new List<GameObject>();
       for (int i = 0; i < length; i++)
       {
           if (directions == Directions.bottom)
           {
               AddAllToHashSet(board.AllDots[i, 0]);
               helperList.Add(board.AllDots[i,0]);
           }
           if (directions == Directions.top)
           {
               AddAllToHashSet(board.AllDots[i, board.height-1]);
               helperList.Add(board.AllDots[i,board.height-1]);
           }
       }
       for (int i = 0; i < length; i++)
       {
           if (directions == Directions.left)
           {
               AddAllToHashSet(board.AllDots[0, i]);
               helperList.Add(board.AllDots[0,i]);
           }
           if (directions == Directions.right)
           {
               AddAllToHashSet(board.AllDots[board.width-1, i]);
               helperList.Add(board.AllDots[board.width-1,i]);
           }
       }

       CheckForMatch(helperList.ToArray());
       currentMatches.Clear();
   }
    private void MatchFinder()
    {
        if (MatchCorner)
        {
            AddAllToHashSet(board.AllDots[0, 0], board.AllDots[board.width - 1, 0], board.AllDots[0, board.height - 1],
                board.AllDots[board.width - 1, board.height - 1]);
            CheckForMatch(board.AllDots[0, 0], board.AllDots[board.width - 1, 0], board.AllDots[0, board.height - 1],
                 board.AllDots[board.width - 1, board.height - 1]);
            currentMatches.Clear();
        }

        if (MatchEnitreRow || MatchEnitreRowOnlyHorizontal)
        {
            EntireRowHelperMethod(board.width, Directions.bottom);
            EntireRowHelperMethod(board.width, Directions.top);
        }

        if (MatchEnitreRow || MatchEnitreRowOnlyVertical)
        {
            EntireRowHelperMethod(board.height, Directions.left);
            EntireRowHelperMethod(board.height, Directions.right);
        }
      
        
        for (int i = 0; i < board.width; i ++)
        {
            for (int j = 0; j < board.height; j ++)
            {
                currentMatches.Clear();
                GameObject currentDot = board.AllDots[i, j];
                if (currentDot == null) continue;

                GameObject leftDot = null;
                GameObject rightDot = null;
                GameObject upDot = null;
                GameObject downDot = null;
                GameObject leftUp = null;
                GameObject leftDown= null;
                GameObject rightUp = null;
                GameObject rightDown = null;
                
                AddAllToHashSet(currentDot);
                    if(i > 0 && i < board.width - 1)
                    {
                        leftDot = board.AllDots[i - 1, j];
                         rightDot = board.AllDots[i + 1, j];
                        if (leftDot == null || rightDot == null) continue;

                        AddAllToHashSet(leftDot, rightDot);
                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        upDot = board.AllDots[i, j + 1];
                         downDot = board.AllDots[i, j - 1];

                        if (upDot == null || downDot == null) continue;
                        AddAllToHashSet(upDot, downDot);
                    }
                    if (j > 0 && j < board.height - 1 && i > 0 && i < board.width - 1)
                    {
                         leftUp = board.AllDots[i-1, j + 1];
                        leftDown = board.AllDots[i-1, j - 1];
                         rightUp = board.AllDots[i+1, j + 1];
                         rightDown= board.AllDots[i+1, j -1];
                         if (leftUp == null ||  leftDown == null ||  rightUp == null ||  rightDown == null) continue;
                        AddAllToHashSet(leftUp, leftDown, rightUp, rightDown);
                    }

                    if (MatchBlock)
                    {
                        CheckForMatch(currentDot, leftDot, upDot, leftUp);
                        CheckForMatch(currentDot, leftDot, downDot, leftDown);
                        CheckForMatch(currentDot, rightDot, upDot, rightUp);
                        CheckForMatch(currentDot, rightDot, downDot, rightDown);
                    }


                    if (MatchRow || MatchRowOnlyHorizontal) CheckForMatch(currentDot, leftDot, rightDot);
                    if (MatchRow || MatchRowOnlyVertical) CheckForMatch(currentDot, upDot, downDot);
                    

                    if (MatchDiagonal || MatchDiagonalRightUp) CheckForMatch(currentDot, leftDown, rightUp);
                    if (MatchDiagonal || MatchDiagonalLeftUp) CheckForMatch(currentDot, leftUp, rightDown);
                    
            }
        }
    }
    private void CheckForMatch(params GameObject[] firstDotThenAllDot)
    {
        if (firstDotThenAllDot[0] == null) return;
        if (MatchColors)
        {
            Colors fruitColorToMatch = firstDotThenAllDot[0].GetComponent<Dot>().FruitColor;
            foreach (GameObject item in firstDotThenAllDot)
            {
                if (item == null) return;
                if (!currentMatches.Contains(item) || fruitColorToMatch != item.GetComponent<Dot>().FruitColor) return;
            }
               
            GetNearbyPieces(firstDotThenAllDot);
            Debug.Log("Match found");
            DebugLogDots(firstDotThenAllDot);
        }
        if (MatchSequence)
        {
            /*Colors fruitColorToMatch = firstDotThenAllDot[0].GetComponent<Dot>().FruitColor;
            string tag = firstDotThenAllDot[0].tag;
            
            foreach (GameObject item in firstDotThenAllDot)
            {
                if (item == null) return;
                if (!currentMatches.Contains(item) || fruitColorToMatch != item.GetComponent<Dot>().FruitColor) return;
            }
               
            GetNearbyPieces(firstDotThenAllDot);
            Debug.Log("Match found");
            DebugLogDots(firstDotThenAllDot);*/
   
        }
        if (MatchFruit)
        {
            string tag = firstDotThenAllDot[0].tag;
            foreach (GameObject item in firstDotThenAllDot)
            {
                if (item == null) return;
                if (!currentMatches.Contains(item) || !item.CompareTag(tag)) return;
            }
               
     
            GetNearbyPieces(firstDotThenAllDot);
            Debug.Log("Match found");
            DebugLogDots(firstDotThenAllDot);
        }
    }
    /*private void CheckForMatchBlock(GameObject dotOne, GameObject dotTwo, GameObject dotThree, GameObject dotFour)
    {
        if (!currentMatches.Contains(dotOne) || !currentMatches.Contains(dotTwo) || !currentMatches.Contains(dotThree) || !currentMatches.Contains(dotFour)) return;
        
        string tag = dotOne.tag;
        if (!dotTwo.CompareTag(tag) || !dotThree.CompareTag(tag) || !dotFour.CompareTag(tag)) return;
        /*if (dotOne == dotTwo || dotOne == dotThree || dotFour
            || dotTwo == dotThree || dotTwo == dotFour || dotThree == dotFour) return#1#;
        GetNearbyPieces(dotOne, dotTwo, dotThree, dotFour);
        Debug.Log("Match found");
        DebugLogDots(dotOne, dotTwo, dotThree, dotFour);
    }*/

    private void DebugLogDots(params  GameObject[] dots)
    {
      //  Debug.Log("------- Start ------");
        for (var index = 0; index < dots.Length; index++)
        {
            var t = dots[index];
            var n = t.GetComponent<Dot>();
//            Debug.Log("TAG::: " + t.tag+" ::: - Dot: [" + index + "] -- column: " + (n.column+1).ToString() + " row: " + (n.row+1).ToString() + "  ||Fruitcolor ~"+ n.FruitColor +"~ ");
        }
     //   Debug.Log("------- End ------");
    }
    private void AddAllToHashSet(params GameObject[] gameObjects)
    {
        foreach (GameObject item in gameObjects) currentMatches.Add(item);
    }
    private void Match3()
    {
        //yield return new WaitForSeconds(.2f);
        for (int i = 0; i < board.width; i ++)
        {
            for (int j = 0; j < board.height; j ++)
            {
                GameObject currentDot = board.AllDots[i, j];
                if (currentDot == null) continue;
                Dot currentDotDot = currentDot.GetComponent<Dot>();
                
                    if(i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.AllDots[i - 1, j];

                        GameObject rightDot = board.AllDots[i + 1, j];

                        if (leftDot == null || rightDot == null) continue;
                       
                            Dot rightDotDot = rightDot.GetComponent<Dot>();
                            Dot leftDotDot = leftDot.GetComponent<Dot>();
                            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                currentMatches.Union(IsRowBomb(leftDotDot, currentDotDot, rightDotDot));
                                currentMatches.Union(IsColumnBomb(leftDotDot, currentDotDot, rightDotDot));
                                currentMatches.Union(IsAdjacentBomb(leftDotDot, currentDotDot, rightDotDot));
                                GetNearbyPieces(leftDot, currentDot, rightDot);
                            }
                        
                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.AllDots[i, j + 1];
                        GameObject downDot = board.AllDots[i, j - 1];

                        if (upDot == null || downDot == null) continue;
                        Dot downDotDot = downDot.GetComponent<Dot>();
                        Dot upDotDot = upDot.GetComponent<Dot>();
                        if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                        {
                            currentMatches.Union(IsColumnBomb(upDotDot, currentDotDot, downDotDot));

                            currentMatches.Union(IsRowBomb(upDotDot, currentDotDot, downDotDot));

                            currentMatches.Union(IsAdjacentBomb(upDotDot, currentDotDot, downDotDot));

                            GetNearbyPieces(upDot, currentDot, downDot);
                        }
                    }
                
            }
        }
    }

    public void MatchPiecesOfColor(string color)
    {
        for (int i = 0; i < board.width; i ++)
        {
            for (int j = 0; j < board.height; j ++)
            {
                //Check if that piece exists
                if(board.AllDots[i, j] != null)
                {
                    //Check the tag on that dot
                    if(board.AllDots[i, j].tag == color)
                    {
                        //Set that dot to be matched
                        board.AllDots[i, j].GetComponent<Dot>().isMatched = true;
                    }
                }
            }
        }
    }

    List<GameObject> GetAdjacentPieces(int column, int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = column - 1; i <= column + 1; i++)
        {
            for (int j = row - 1; j <= row + 1; j++)
            {
                //Check if the piece is inside the board
                if (i >= 0 && i < board.width && j >= 0 && j < board.height){
					if (board.AllDots[i, j] != null)
					{
						dots.Add(board.AllDots[i, j]);
						board.AllDots[i, j].GetComponent<Dot>().isMatched = true;
					}
                }
            }
        }
        return dots;
    }

    List<GameObject> GetColumnPieces(int column){
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.height; i ++){
            if(board.AllDots[column, i]!= null){
				Dot dot = board.AllDots[column, i].GetComponent<Dot>();
				if(dot.isRowBomb)
				{
					dots.Union(GetRowPieces(i)).ToList();
				}

                dots.Add(board.AllDots[column, i]);
                dot.isMatched = true;
            }
        }
        return dots;
    }

    List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            if (board.AllDots[i, row] != null)
            {
				Dot dot = board.AllDots[i, row].GetComponent<Dot>();
				if (dot.isColumnBomb)
                {
					dots.Union(GetColumnPieces(i)).ToList();
                }
                dots.Add(board.AllDots[i, row]);
				dot.isMatched = true;
            }
        }
        return dots;
    }

    public void CheckBombs(MatchType matchType){
        //Did the player move something?
        if(board.currentDot != null){
            //Is the piece they moved matched?
            if (board.currentDot.isMatched && board.currentDot.tag == matchType.color)
            {
                //make it unmatched
                board.currentDot.isMatched = false;
                //Decide what kind of bomb to make
                /*
                int typeOfBomb = Random.Range(0, 100);
                if(typeOfBomb < 50){
                    //Make a row bomb
                    board.currentDot.MakeRowBomb();
                }else if(typeOfBomb >= 50){
                    //Make a column bomb
                    board.currentDot.MakeColumnBomb();
                }
                */
                if((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45)
                   ||(board.currentDot.swipeAngle < -135 || board.currentDot.swipeAngle >= 135)){
                    board.currentDot.MakeRowBomb();
                }else{
                    board.currentDot.MakeColumnBomb();
                }
            }
            //Is the other piece matched?
            else if(board.currentDot.otherDot != null){
                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();
                //Is the other Dot matched?
                if(otherDot.isMatched && otherDot.tag == matchType.color){
                    //Make it unmatched
                    otherDot.isMatched = false;
                    /*
                    //Decide what kind of bomb to make
                    int typeOfBomb = Random.Range(0, 100);
                    if (typeOfBomb < 50)
                    {
                        //Make a row bomb
                        otherDot.MakeRowBomb();
                    }
                    else if (typeOfBomb >= 50)
                    {
                        //Make a column bomb
                        otherDot.MakeColumnBomb();
                    }
                    */
                    if ((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45)
                   || (board.currentDot.swipeAngle < -135 || board.currentDot.swipeAngle >= 135))
                    {
                        otherDot.MakeRowBomb();
                    }
                    else
                    {
                        otherDot.MakeColumnBomb();
                    }
                }
            }
            
        }
    }

}
