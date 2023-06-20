using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening.Plugins.Core.PathCore;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class WorldMap : MonoBehaviour
{
   public Sprite defaultActiveSprite;
   public Sprite defaultDectiveSprite;
   
   public Sprite defaultDeactivatedLineSprite;
   public Sprite defaultActivatedLineSprite;
   
   public Sprite controlPointSprite;
   
   public static Dictionary<ushort, WorldNode> allUniqueIdentifier;
   public static WorldNode firstNode;
   public  WorldStructure worldStructure;
   public WorldNode currentNode;
   public delegate void GetHashId();

   public static GetHashId HashId;
   public WorldNode testNode1;
   public WorldNode testNode2;
   public Material LineRendererMat;
   
   public void Start()
   {
      allUniqueIdentifier = new Dictionary<ushort, WorldNode>();
      HashId?.Invoke();
   }

   [SerializeField] private int testInt = 0;
   [Button()] private void TestAddWorldNodeAtEnd() => AddWorldNodeToEnd(ref currentNode);
   [Button()] private void TestAddWorldNodeAtStart() => AddWorldNodeAtStart(ref currentNode);
   [Button()] private void TestInsertWorldNodeAfter() => InsertWorldNodeAfter(ref testNode1, ref testNode2);
   [Button()] private void TestInsertWorldNodeBefore() => InsertWorldNodeBefore(ref testNode1, ref testNode2);
   
   [Button()] private void TestInsertAfter() => InsertAfter(ref currentNode);
   [Button()] private void TestInsertBefore() => InsertBefore(ref currentNode);
   [Button()] private void TestAddAdditionalWorldNode() => AddAdditionalNode(ref currentNode);

   [Button()]
   private void TestDrawline() => DrawLines(true);

   private bool RenewControlPointParents(WorldNode node, bool forceRenew)
   {
      bool destroyChildren = false;
      if (node.NextWorldLines != null && node.transform.childCount != node.NextWorldLines.Count || forceRenew)
      {
         if(GenericSettingsFunctions.DestroyAllChildren(node.gameObject)) destroyChildren = true;
         for (int j = 0; j < node.NextWorldLines?.Count; j++)
         {
            GameObject newControlParent = new GameObject("Control Point Parent");
            newControlParent.transform.SetParent(node.transform);
         }
      }

      return destroyChildren;
   }
   private void DrawLinesFalseDestroyChildren() => DrawLines(false);

   public void DrawLines(bool destroyChildren, WorldNode providedNode = null)
   {
      WorldNode currentNode = this.currentNode;
      if (providedNode != null) currentNode = providedNode;
      if (destroyChildren)
      {
         destroyChildren = RenewControlPointParents(currentNode, false);
         if (destroyChildren) Invoke(nameof(DrawLinesFalseDestroyChildren), 0.03f);
      }
    
      for (int i = 0; i < currentNode.NextWorldLines?.Count; i++)
      {
         DrawWorldLine(currentNode.NextWorldLines[i], null, i, testInt);
      }
   }

   private void OnDestroy()
   {
      firstNode = null;
      allUniqueIdentifier = null;
   }

   public void InsertAfter(ref WorldNode worldNode)
   {
      WorldNode dummyWorldNode = null;
      InsertAfter(ref worldNode, ref dummyWorldNode);
   }
   public void InsertBefore(ref WorldNode worldNode)
   {
      WorldNode dummyWorldNode = null;
      InsertBefore(ref worldNode, ref dummyWorldNode);
   }
   
   public void InsertAfter(ref WorldNode worldNode, ref WorldNode nextWorldNode)
   { //if there is no otherWorld node, it is the end, otherwise if the 2nd Args is null it trys to get the nextWorldnode, otherwise if both are there it works too.
      if(worldNode.NextWorldLines == null || worldNode.NextWorldLines.Count == 0 || worldNode.NextWorldLines[0].NextWorldNode == null) AddWorldNodeToEnd(ref worldNode);
      else if (nextWorldNode == null) InsertWorldNodeAfter(ref worldNode, ref worldNode.NextWorldLines[0].NextWorldNode);
      else InsertWorldNodeAfter(ref worldNode, ref nextWorldNode);
   }
   public void InsertBefore(ref WorldNode worldNode, ref WorldNode preWorldNode)
   { //if there is no otherWorld node, it is the end, otherwise if the 2nd Args is null it trys to get the nextWorldnode, otherwise if both are there it works too.
      if(worldNode.PrevWorldLines == null || worldNode.PrevWorldLines.Count == 0 || worldNode.PrevWorldLines[0].NextWorldNode == null) AddWorldNodeAtStart(ref worldNode);
      else if (preWorldNode == null) InsertWorldNodeBefore(ref worldNode, ref worldNode.PrevWorldLines[0].PrevWorldNode);
      else InsertWorldNodeBefore(ref worldNode, ref preWorldNode);
   }

   public bool IsFirstNode(WorldNode worldNode)
   {
      if (worldNode.PrevWorldLines.Count == 0) return true;
      return false;
   }
   public bool IsLastNode(WorldNode worldNode)
   {
      if (worldNode.NextWorldLines.Count == 0) return true;
      return false;
   }

   public void AddAdditionalNode(ref WorldNode worldNode)
   {
      Debug.Log("Adding Additonal Node");
      if (worldNode == null) return;
      WorldNode newWorldNode = InstantiateWorldNode(ref worldNode);
      if (worldNode.NextWorldLines == null) 
      {
         worldNode.NextWorldLines = new List<WorldLine>(); //if the nextNodeList list is null, give me a new one
      }

      WorldLine newWorldLine = InstantiateWorldLine(worldNode, newWorldNode);
      worldNode.NextWorldLines.Add(newWorldLine);
      newWorldNode.PrevWorldLines[0].NextWorldNode = newWorldNode;  // the prev worldLines has most likely zero entries, so I had the prev node to it
      newWorldNode.PrevWorldLines[0].PrevWorldNode = worldNode;
      newWorldNode.NextWorldLines = null;  
      
     
      DrawLines(true);
   }
   
   public void AddWorldNodeToEnd(ref WorldNode prevWorldNode)
   {
      Debug.Log("Adding WorldNode to an ENDPoint");
      if (prevWorldNode == null) return;  //security return
      WorldNode newWorldNode = InstantiateWorldNode(ref prevWorldNode); //gets the new node with the lines
      if (prevWorldNode.NextWorldLines == null) 
      {
         prevWorldNode.NextWorldLines = new List<WorldLine>(); //if the nextNodeList list is null, give me a new one
      }
      newWorldNode.PrevWorldLines[0].NextWorldNode = newWorldNode;  // the prev worldLines has most likely zero entries, so I had the prev node to it
      newWorldNode.PrevWorldLines[0].PrevWorldNode = prevWorldNode;
      prevWorldNode.NextWorldLines.Add(newWorldNode.PrevWorldLines[0]); //add the new line to the prev List
      newWorldNode.NextWorldLines = null;  //null because it is the end
      
      DrawLines(true);
   }

   private int FindEntryWorldLine(WorldNode nodeA, WorldNode nodeB, Line line)
   {
      int entryId = int.MaxValue;
      if (line == Line.NextLine)
      {
         for (int i = 0; i < nodeA.NextWorldLines.Count; i++)
         {
            if(nodeA.NextWorldLines[i].PrevWorldNode == nodeA && nodeA.NextWorldLines[i].NextWorldNode == nodeB)
            {
               return i;
            }
         }
      }
      else if (line == Line.PrevLine)
      {
         for (int i = 0; i < nodeA.PrevWorldLines.Count; i++)
         {
            if(nodeA.PrevWorldLines[i].PrevWorldNode == nodeB && nodeA.PrevWorldLines[i].NextWorldNode == nodeA)
            {
               return i;
            }
         }
      }

      
      return entryId;
   }

   public void InsertWorldNodeBefore(ref WorldNode middleWorldNode, ref WorldNode prevWorldNode) 
      => InsertWorldNodeAfter(ref prevWorldNode, ref middleWorldNode, Line.PrevLine);

   public void InsertWorldNodeAfter(ref WorldNode middleWorldNode, ref WorldNode nextWorldNode, Line line = Line.NextLine)
   {
      Debug.Log(line == Line.PrevLine ? "Inserting Node Before" : "Inserting Node After");
      if (middleWorldNode == null || nextWorldNode == null) return;
      WorldNode newWorldNode = InstantiateWorldNode(ref middleWorldNode, ref nextWorldNode);
  
      
      if (middleWorldNode.NextWorldLines == null) middleWorldNode.NextWorldLines = new List<WorldLine>(); //if the nextNodeList list is null, give me a new one
      if (middleWorldNode.PrevWorldLines == null) middleWorldNode.PrevWorldLines = new List<WorldLine>(); //if the PrevNodeList is null, give me a new one

      int entryForNextWorldLine = FindEntryWorldLine(middleWorldNode, nextWorldNode, Line.NextLine);
      int entryForPrevWorldLine = FindEntryWorldLine(  nextWorldNode, middleWorldNode, Line.PrevLine);
      
      
      if (entryForNextWorldLine != int.MaxValue)
      {
         // I HAVE TO MAKE A NEW WORLDLINE BECAUSE C# MAKES A SHALLOW COPY OF NEWWORLDNODE AND IT GETS OVERWRITTEN OTHERWISE. YES....
         middleWorldNode.NextWorldLines.Remove(middleWorldNode.NextWorldLines[entryForNextWorldLine]);
         //TODO: Copy the smoothing and the default sprites the right way

         WorldLine newWorldLine = InstantiateWorldLine(middleWorldNode, newWorldNode);
         middleWorldNode.NextWorldLines.Add(newWorldLine);
      }
 
      if ( entryForPrevWorldLine != int.MaxValue)
      {
         nextWorldNode.PrevWorldLines.Remove(nextWorldNode.PrevWorldLines[entryForPrevWorldLine]);
         //TODO: Copy the smoothing and the default sprites the right way
         WorldLine newWorldLine = InstantiateWorldLine(newWorldNode, nextWorldNode);
         nextWorldNode.PrevWorldLines.Add(newWorldLine);
      }

   //    newWorldNode.PrevWorldLines[0].PrevWorldNode = middleWorldNode;
   //  newWorldNode.PrevWorldLines[0].NextWorldNode = newWorldNode;
   //    
   //  newWorldNode.NextWorldLines[0].PrevWorldNode = newWorldNode;
   // newWorldNode.NextWorldLines[0].NextWorldNode = nextWorldNode;
   
   newWorldNode.PrevWorldLines[0].PrevWorldNode = middleWorldNode;
   newWorldNode.PrevWorldLines[0].NextWorldNode = newWorldNode;
      
   newWorldNode.NextWorldLines[0].PrevWorldNode = newWorldNode;
   newWorldNode.NextWorldLines[0].NextWorldNode = nextWorldNode;
   
  // RenewControlPointParents(middleWorldNode, true);
   //RenewControlPointParents(newWorldNode, true);
  // RenewControlPointParents(nextWorldNode, true);
   
   //new, next, middle
  // DrawLines(true, newWorldNode);
   //DrawLines(true, middleWorldNode);
   DrawLines(true, nextWorldNode);
   }
   
   public void AddWorldNodeAtStart(ref WorldNode nextWorldNode)
   {   
      Debug.Log("Adding WorldNode to an STARTPoint");
      if (nextWorldNode == null) return; //security return
      WorldNode newWorldNode = InstantiateWorldNode(ref nextWorldNode); //gets the new node with the lines
      if (nextWorldNode.PrevWorldLines == null)
         nextWorldNode.PrevWorldLines = new List<WorldLine>(); //if the PrevNodeList is null, give me a new one
      
      newWorldNode.NextWorldLines[0].PrevWorldNode = newWorldNode;  // the prev node of the next worldLine has no entry, so I add it here
      newWorldNode.NextWorldLines[0].NextWorldNode = nextWorldNode;
      nextWorldNode.PrevWorldLines.Add(newWorldNode.NextWorldLines[0]); //add the new line to the prev List
      newWorldNode.PrevWorldLines = null; //null because it is the start
  
      
      DrawLines(true, newWorldNode);
   }
   
   private void MakeFirstNode(bool isFirstNode)
   {
      if (!isFirstNode) return;
      
   }

   private void FindAndDestroyBrokenWorldLines()
   {
      Debug.Log("Finding and Destroying broken World LINES");
      List<WorldNode> allWorldNodes = GetAllWorldNodes();

      for (int i = 0; i < allWorldNodes.Count; i++)
      {
         for (int j = 0; j < allWorldNodes[i].PrevWorldLines.Count; j++)
         {
            if (allWorldNodes[i].PrevWorldLines[j].NextWorldNode == null &&
                allWorldNodes[i].PrevWorldLines[j].PrevWorldNode == null)
               allWorldNodes[i].PrevWorldLines.Remove(allWorldNodes[i].PrevWorldLines[j]);
         }
         for (int k = 0; k < allWorldNodes[i].NextWorldLines.Count; k++)
         {
            if (allWorldNodes[i].NextWorldLines[k].NextWorldNode == null &&
                allWorldNodes[i].NextWorldLines[k].PrevWorldNode == null)
               allWorldNodes[i].NextWorldLines.Remove(allWorldNodes[i].NextWorldLines[k]);
         }
      }
   }
   
   private void FindAndDestroyBrokenWorldNodes()
   {
      Debug.Log("Finding and Destroying broken World NODES");
      List<WorldNode> allWorldNodes = GetAllWorldNodes();

      for (int i = 0; i < allWorldNodes.Count; i++)
      {
         if(allWorldNodes[i].NextWorldLines == null && allWorldNodes[i].PrevWorldLines == null)
            Destroy(allWorldNodes[i]);
      }
   }

   private List<WorldNode> GetAllWorldNodes()
   {
      List<WorldNode> allWorldNodes = new List<WorldNode>();
      foreach (WorldNode worldNode in FindObjectsByType<WorldNode>(FindObjectsSortMode.None))
         allWorldNodes.Add(worldNode);
      return allWorldNodes;
   }
   public void DeleteWorldNode(ref WorldNode nodeToDelete)
   {
      if (nodeToDelete == null) return;

      if (!IsFirstNode(nodeToDelete))
      {
         //deletes all Entries to the Lines in prev nodes
         for (int i = 0; i < nodeToDelete.PrevWorldLines.Count; i++)
         {
            for(int j = 0; j < nodeToDelete.PrevWorldLines[i].PrevWorldNode.PrevWorldLines.Count;j++)
               if (nodeToDelete.PrevWorldLines[i] == nodeToDelete.PrevWorldLines[i].PrevWorldNode.PrevWorldLines[j])
                  nodeToDelete.PrevWorldLines[i].PrevWorldNode.PrevWorldLines
                     .Remove(nodeToDelete.PrevWorldLines[i].PrevWorldNode.PrevWorldLines[j]);
         }
      }
        
         if (!IsLastNode(nodeToDelete))
         {
            
            //Todo: connect the prev nodes to the next nodes
            // for (int i = 0; i < nodeToDelete.NextWorldLines.Count; i++)
            // {
            //    for(int j = 0; j < nodeToDelete.NextWorldLines[i].PrevWorldNode.NextWorldLines.Count;j++)
            //       if (nodeToDelete.NextWorldLines[i] == nodeToDelete.NextWorldLines[i].NextWorldNode.NextWorldLines[j])
            //       {
            //          nodeToDelete.NextWorldLines[i].NextWorldNode.NextWorldLines
            //             .Remove(nodeToDelete.NextWorldLines[i].NextWorldNode.NextWorldLines[j]);
            //       }
            //       
            // }
         }
         allUniqueIdentifier.Remove(nodeToDelete.UniqueIdentifier);
         Destroy(nodeToDelete);
   }

   private Vector3 GenerateNullNodePos(WorldNode aNode, float transformY)
      => new (aNode.gameObject.transform.position.x, aNode.gameObject.transform.position.y+transformY, aNode.gameObject.transform.position.z);

   public WorldNode InstantiateWorldNode(ref WorldNode prevWorldNode)
   {
      WorldNode dummyNode = null;
      return InstantiateWorldNode(ref prevWorldNode, ref dummyNode);
   }

   public WorldNode InstantiateWorldNode(ref WorldNode prevWorldNode, ref WorldNode nextWorldNode)
   {
         //if one is null, it still gets a position
        // Debug.Log("prevWorldNode: " + prevWorldNode.WorldPos.);
         Vector3 prevPos = prevWorldNode == null ? GenerateNullNodePos(nextWorldNode, 8f) : prevWorldNode.transform.position;
         Vector3 nextPos = nextWorldNode == null ? GenerateNullNodePos(prevWorldNode, -8f) : nextWorldNode.transform.position;
         Vector3 worldPos = MathLibrary.FindTheMidPoint(prevPos, nextPos);
    
         
         //Every Node needs a Unique identifier
         ushort uniqueIdentifier = FindNextViableUniqueIdentifier(allUniqueIdentifier);
         
         //Generates a new Object and prepares it
         GameObject newNode = new GameObject();
         newNode.transform.position = worldPos;
         newNode.name = $"NodeID: {uniqueIdentifier}";
         newNode.AddComponent<WorldNode>();
         newNode.transform.parent = transform;
         //Generates the worldLines. Null is okay here
         WorldNode worldNode = newNode.GetComponent<WorldNode>();
         WorldLine prevWorldLine = InstantiateWorldLine(prevWorldNode, nextWorldNode);
         WorldLine nextWorldLine = InstantiateWorldLine(prevWorldNode, nextWorldNode);

         //It creates it with just one connection but for later connections we need to add Lists, not only single Worldlines
         List<WorldLine> prevWorldLineDummyList = new List<WorldLine>();
         List<WorldLine> nextWorldLineDummyList = new List<WorldLine>();
         prevWorldLineDummyList.Add(prevWorldLine);
         nextWorldLineDummyList.Add(nextWorldLine);

         //Add all to the WorldNode and returns it
         worldNode.UniqueIdentifier = uniqueIdentifier;
         worldNode.PrevWorldLines = prevWorldLineDummyList;
         worldNode.NextWorldLines = nextWorldLineDummyList;
         worldNode.DeactivedSprite = defaultDectiveSprite;
         worldNode.ActivatedSprite = defaultActiveSprite;
         worldNode.Degree = 0;
         worldNode.WorldPos = worldPos;
         newNode.AddComponent<SpriteRenderer>().sprite = worldNode.ActivatedSprite;
         allUniqueIdentifier.Add(uniqueIdentifier,worldNode);

         newNode.AddComponent<BoxCollider>();
         
         GameObject controlPointParent = new GameObject("Control Point Parent");
         controlPointParent.transform.SetParent(newNode.transform);
         
         return worldNode;
   }

   private WorldLine InstantiateWorldLine(WorldNode prevWorldNode, WorldNode nextWorldNode)
   {
      List<Vector3> controlpointList = new List<Vector3>();
      controlpointList.Add(new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
      return new(prevWorldNode, controlpointList, nextWorldNode, 1,
         defaultDeactivatedLineSprite, defaultActivatedLineSprite);
   }

   public ushort FindNextViableUniqueIdentifier(Dictionary<ushort, WorldNode> allUniqueIdentifier)
   {
     //ToDo: better search algo if levels get high (like 4k levels)
      for (ushort i = 0; i  < ushort.MaxValue;i++ )
      {
         if (!allUniqueIdentifier.ContainsKey(i))
            return i;
      }
      return ushort.MaxValue;
   }

   [SerializeField] private GameObject TestLineObj;

   private void AddComponentsToMidPointObj(ref GameObject midPointObj, WorldLine worldLine, Vector3 midPoint, int controlPointParentIndex)
   { //or in other words the control Point
     
      midPointObj.transform.parent = worldLine.PrevWorldNode.transform.GetChild(controlPointParentIndex).transform;
      midPointObj.transform.position = midPoint;
      midPointObj.AddComponent<WorldLineCurvePoints>().parentWorldLine = worldLine;
      midPointObj.AddComponent<SpriteRenderer>().sprite = controlPointSprite;
      midPointObj.AddComponent<BoxCollider>();
      midPointObj.GetComponent<WorldLineCurvePoints>().enabled = true;
   }

   public void DrawWorldLine(WorldLine worldLine, Transform callbackTransform, int controlPointParent,
      int additonalControlPointsCount)
   {
      
      if (worldLine.PrevWorldNode == null || worldLine.NextWorldNode == null)
         return; //return if not two nodes are provided
      if (callbackTransform == null && worldLine.PrevWorldNode.transform.childCount != worldLine.PrevWorldNode.NextWorldLines.Count)
      {
         GenericSettingsFunctions.DestroyAllChildren(worldLine.PrevWorldNode.gameObject);
         for (int i = 0; i < worldLine.PrevWorldNode.NextWorldLines.Count; i++)
         {
            GameObject newControlParent = new GameObject("Control Point Parent");
            newControlParent.transform.SetParent(worldLine.PrevWorldNode.transform);
         }
      }

      //Get the ControlPointCount
      if (additonalControlPointsCount < 1) additonalControlPointsCount = 1; //need at least one additionalControl point
      // if (callbackTransform != null) additonalControlPointsCount = worldLine.PrevWorldNode.transform.GetChild(0).childCount;
      int controlPointsCount = additonalControlPointsCount + 2;


      //prepare the container
      Vector3[] controlPoints = new Vector3[controlPointsCount]; //making new control Points Array to Use
      Vector3[] additionalControlPointsArray = worldLine.ControlPoints.ToArray(); //getting the controlPoints
      Array.Resize(ref additionalControlPointsArray,
         additonalControlPointsCount); //and resizing it in case there are now less control points
      List<Vector3>
         additionalControlPoints =
            additionalControlPointsArray
               .ToList(); //now we have the control Points that get additionaly drawn. at least 1!
      worldLine.ControlPoints = additionalControlPoints; //moving it back so it also resizes

      GameObject additonalControlPointObj;

      //Make new points in case there werent any
      if (callbackTransform == null)
      {
         if (worldLine.PrevWorldNode.transform.GetChild(controlPointParent).GetComponent<LineRenderer>() == null)
            worldLine.PrevWorldNode.transform.GetChild(controlPointParent).gameObject.AddComponent<LineRenderer>();
         //here we get new positions so they dont stack on one another
         for (int i = 0;
              i < additonalControlPointsCount && Math.Abs(additionalControlPoints[i].x - float.MaxValue) < 0.1f;
              i++)
         {
            additionalControlPoints[i] = MathLibrary.FindTheMidPoint(worldLine.PrevWorldNode.transform.position,
               worldLine.NextWorldNode.transform.position);
            additionalControlPoints[i] = new Vector3(additionalControlPoints[i].x - 0.5f * i,
               additionalControlPoints[i].y - 0.5f * i, additionalControlPoints[i].z);
         }

         worldLine.ControlPoints = additionalControlPoints; //new points get made and shuffed into the array

         //here we instantiate the new points
         for (int i = 0; i < additonalControlPointsCount; i++)
         {
            additonalControlPointObj = new GameObject("Additonal ControlPoint of: " + worldLine.PrevWorldNode.name +
                                                      " and " + worldLine.NextWorldNode.name + " ::: " + i);
            AddComponentsToMidPointObj(ref additonalControlPointObj, worldLine, additionalControlPoints[i],
               controlPointParent); //adds all the components
            controlPoints[i + 1] = additonalControlPointObj.transform.position;
            additonalControlPointObj.transform.parent =
               worldLine.PrevWorldNode.transform.GetChild(controlPointParent).transform;
            //TODO: CHECK FOR DUPLICATE DOES NOT WORK THIS WAY
            //if (CheckForDuplicates(additonalControlPointObj))
            // {
            //  Destroy(additonalControlPointObj);
            //return;
            //}
         }
      }
   else for (int i = 0; i < additionalControlPoints.Count; i++) controlPoints[i + 1] = additionalControlPoints[i];
      

      //Assign first and last Control Point
      controlPoints[0] = worldLine.PrevWorldNode.transform.position;
      controlPoints[controlPoints.Length-1] = worldLine.NextWorldNode.transform.position;
      
      //Make Splines and make a length table
      CatmullRom catmullRom = new CatmullRom(controlPoints, worldLine.Smoothing, false);
      CatmullRom.CatmullRomPoint[] catmullRomPoints =  catmullRom.GenerateSplinePoints();
   
     WorldCurvesLengthTable lengthTable = new WorldCurvesLengthTable(catmullRomPoints);

     LineRenderer lineRenderer;
    // try
    // {
        lineRenderer = worldLine.PrevWorldNode.transform.GetChild(controlPointParent).GetComponent<LineRenderer>();
    // }
     // catch (Exception e)
     // {
     //    lineRenderer = worldLine.PrevWorldNode.transform.GetChild(worldLine.PrevWorldNode.transform.childCount ).GetComponent<LineRenderer>();
     // }

    AdjustLineRenderer(ref lineRenderer, catmullRomPoints.Length-1);
    
  
    List<Vector3> positions = new List<Vector3>();
     for (int i = 0; i < catmullRomPoints.Length-2; i++)
     {
        float percent = (float)i / ((float)catmullRomPoints.Length-3f);
        positions.Add( lengthTable.ToPoints(percent));
     }
     
     positions.Add(worldLine.NextWorldNode.transform.position);
     lineRenderer.SetPositions(positions.ToArray());
     
   }

   public void ReDrawLine(WorldLine worldLine, Transform callbackTransform, int controlPointParentIndex) 
      => DrawWorldLine(worldLine, callbackTransform, controlPointParentIndex, worldLine.PrevWorldNode.transform.GetChild(controlPointParentIndex).childCount);


   private void AdjustLineRenderer(ref LineRenderer lineRenderer, int positionCount)
   {
      lineRenderer.alignment = LineAlignment.View;
      lineRenderer.textureMode = LineTextureMode.Tile;
      lineRenderer.positionCount = positionCount;
      lineRenderer.material = LineRendererMat;
      lineRenderer.sortingOrder = -1;
      lineRenderer.receiveShadows = false;
   }

   private bool CheckForDuplicates(GameObject objectToCheck)
   {
      int hashToCheck = objectToCheck.GetComponent<WorldLineCurvePoints>().parentWorldLine.PrevWorldNode.GetHashCode()+
                        objectToCheck.GetComponent<WorldLineCurvePoints>().parentWorldLine.NextWorldNode.GetHashCode();

      byte counter = 0;
      for (int i = 0; i < objectToCheck.transform.parent.childCount; i++)
      {
         WorldLine childObject = objectToCheck.transform.parent.GetChild(i).GetComponent<WorldLineCurvePoints>().parentWorldLine;
         if (childObject.PrevWorldNode.GetHashCode() + childObject.NextWorldNode.GetHashCode() == hashToCheck)
            counter++;
      }

      if (counter > 1) return true;
      return false;
   }
   private List<LineRenderer> AdjustLineRenderers(ref WorldNode node)
   {
      List<LineRenderer> lineRenderers = new List<LineRenderer>();
      foreach(LineRenderer lineRenderer in node.GetComponents<LineRenderer>())
         lineRenderers.Add(lineRenderer);

      int lineRenderersCount = lineRenderers.Count - node.NextWorldLines.Count+1;

      for (int i = 0; i < lineRenderersCount; i++)
      {
         LineRenderer lineRenderer = node.gameObject.AddComponent<LineRenderer>();
         lineRenderers.Add(lineRenderer );
      }

      return lineRenderers;
   }
}

[Serializable]
public enum Line
{
   PrevLine,
   NextLine
}