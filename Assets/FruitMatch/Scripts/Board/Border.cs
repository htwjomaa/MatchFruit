using System.Collections.Generic;
using UnityEngine;

public sealed class Border : MonoBehaviour
{
    public static void GenerateAllBorders(int width, int height, float padding, BackgroundTile[,] allTiles, bool[,] blankedSpaces, GameObject borderPrefab, GameObject cornerBorderPrefab)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allTiles[i, j] != null )
                {
                    List<Directions> emptyNeighbours = GetEmptyNeighbours(i, j, width, height, allTiles, blankedSpaces);
                    if(emptyNeighbours.Count > 1) SpawnCornerBorder(emptyNeighbours, allTiles, allTiles[i, j].transform, padding, borderPrefab,cornerBorderPrefab);
                    else
                    {
                        foreach(Directions direction in emptyNeighbours)
                        {
                            SpawnSingleBorder(allTiles[i, j].transform, direction, padding, borderPrefab);
                        }
                    }
                }
            }
        }

      //  AnalyseBlankSpaces(width,height, blankedSpaces,padding, cornerBorderPrefab, allTiles);
    }
  
    private static List<Directions> GetEmptyNeighbours(int column, int row, int width, int height,
        BackgroundTile[,] allTiles, bool[,] blankedSpaces)
    {
     
        List<Directions> emptyNeighbours = new List<Directions>();
                //blank spaces
            if (column < width-1 && (allTiles[column + 1, row] == null || blankedSpaces[column + 1, row]))
                emptyNeighbours.Add(Directions.right);
            if (column > 0 && (allTiles[column - 1, row] == null || blankedSpaces[column - 1, row]))
                emptyNeighbours.Add(Directions.left);
            if (row < height-1 && (allTiles[column, row + 1] == null || blankedSpaces[column, row + 1])) emptyNeighbours.Add(Directions.top);
            if (row > 0 && (allTiles[column, row - 1] == null || blankedSpaces[column, row - 1]))
                emptyNeighbours.Add(Directions.bottom);

            //border
            if (!blankedSpaces[column, row])
            {
                if(row == height-1)   emptyNeighbours.Add(Directions.top);
                if(column == width-1)  emptyNeighbours.Add(Directions.right); 
                if(column == 0)  emptyNeighbours.Add(Directions.left);
                if(row  == 0)  emptyNeighbours.Add(Directions.bottom);
            }
   
  
        return emptyNeighbours;
    }
    private static void SpawnSingleBorder(Transform parent, Directions direction, float padding, GameObject borderPrefab)
    {
        var startPos = parent.position;
        Quaternion quat = new Quaternion();
        switch (direction)
        {
            case Directions.right:
                startPos = new Vector3(parent.position.x+padding, startPos.y, startPos.z);
                quat.eulerAngles = new Vector3(0, 0, 180);
                break;
            case Directions.left:
                startPos = new Vector3(parent.position.x-padding, startPos.y, startPos.z);
                break;
            case Directions.top: 
                startPos = new Vector3(parent.position.x, startPos.y+padding, startPos.z);
                quat.eulerAngles = new Vector3(0, 0, -90);
                //rotate
                break;
            case Directions.bottom: 
                startPos = new Vector3(parent.position.x, startPos.y-padding, startPos.z);
                quat.eulerAngles = new Vector3(0, 0, 90);
                //rotate
                break;
        }
        
         Instantiate(borderPrefab, startPos, quat, parent);
    }
    
    private static void SpawnCornerBorder(List<Directions> emptyBorders, BackgroundTile[,] allTiles, Transform parent, float padding,  GameObject borderPrefab, GameObject cornerBorderPrefab)
    {
        var startPos = parent.position;
        Quaternion quat = new Quaternion();

        bool right = false;
        bool left = false;
        bool top = false;
        bool bottom = false;
        
        foreach (Directions dir in emptyBorders)
        {
            switch (dir)
            {
                case Directions.right:
                    right = true;
                    break;
                case Directions.left:
                    left = true;
                    break;
                case Directions.top:
                    top = true;
                    break;
                case Directions.bottom:
                    bottom = true;
                    break;
            }
        }
        
        if (left && bottom)
        {
            startPos = new Vector3(parent.position.x - padding, startPos.y-padding, startPos.z);
        }
        
         if (left && top)
        {
            startPos = new Vector3(parent.position.x - padding, startPos.y+padding, startPos.z);
            quat.eulerAngles = new Vector3(0, 0, -90);
        }

        
         if (right && bottom)
        {
            startPos = new Vector3(parent.position.x + padding, startPos.y-padding, startPos.z);
            quat.eulerAngles = new Vector3(0, 0, 90);
        }

        
        if (right && top)
        {
            startPos = new Vector3(parent.position.x + padding, startPos.y+padding, startPos.z);
            quat.eulerAngles = new Vector3(0, 0, 180);
        }
        
        if (top && bottom)
        {
            foreach(Directions direction in emptyBorders)
            {
                SpawnSingleBorder(parent, direction, padding, borderPrefab);
            }

            return;
        }
        if (left && right)
        {
            foreach(Directions direction in emptyBorders)
            {
                SpawnSingleBorder(parent, direction, padding, borderPrefab);
            }

            return;
        }
        Instantiate(cornerBorderPrefab, startPos, quat, parent);
    }


    public static void AnalyseBlankSpaces(int width, int height, bool[,] blankSpaces, float padding, GameObject cornerBorderPrefab, BackgroundTile[,] allTiles)
    {

        for (int i = 0; i < width-2; i++)
        {
            for (int j = 0; j < height-2; j++)
            {

                if (blankSpaces[i, j])
                {
                    if (allTiles[i + 1, j] != null && allTiles[i - 1, j] != null && allTiles[i, j + 1] && allTiles[i, j - 1] != null)
                    {
                        Destroy(allTiles[i + 1, j].transform.GetChild(0).gameObject);
                        Destroy(allTiles[i - 1, j].transform.GetChild(0).gameObject);
                        Destroy(allTiles[i, j + 1].transform.GetChild(0).gameObject);
                        Destroy(allTiles[i, j - 1].transform.GetChild(0).gameObject);

                        List<Directions> fakeDirections = new List<Directions>();
                        fakeDirections.Add(Directions.left);
                        fakeDirections.Add(Directions.bottom);

                        var distX = MathLibrary.CalculateDistance(allTiles[i - 1, j].transform.gameObject,
                            allTiles[i + 1, j].transform.gameObject);
                        var xPos = allTiles[i - 1, j].transform.position.x + distX / 2;
                        
                        var distY = MathLibrary.CalculateDistance(allTiles[i, j- 1].transform.gameObject,
                            allTiles[i, j+ 1].transform.gameObject);
                        var yPos = allTiles[i - 1, j].transform.position.x + distY / 2;

                        var position = new Vector3(xPos, yPos, allTiles[i - 1, j].transform.position.z);
                        
                        GameObject test = new GameObject();
                        Instantiate(test, position, Quaternion.identity);
                        
     
                        List<Directions> fakeDirections2 = new List<Directions>();
                        fakeDirections2.Add(Directions.right);
                        fakeDirections2.Add(Directions.top);
                        
                        
                     ///   SpawnCornerBorder(fakeDirections , test.transform, padding, cornerBorderPrefab);
                     //   SpawnCornerBorder(fakeDirections2 , test.transform, padding, cornerBorderPrefab);
                        
                        
                    }
                }
            }
        }
    }
}
