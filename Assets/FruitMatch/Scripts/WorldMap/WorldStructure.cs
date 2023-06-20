using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class WorldStructure
{
  public DateInformation DateInformation;
  public double UniqueIdentifier;
  public WorldNode FirstNode;
  public List<WorldNode> WorldNodes;

  public WorldStructure(DateInformation dateInformation, double uniqueIdentifier, WorldNode firstNode,
    List<WorldNode> worldNodes)
  {
    DateInformation = dateInformation;
    UniqueIdentifier = uniqueIdentifier;
    FirstNode = firstNode;
    WorldNodes = worldNodes;
  }

  public WorldStructure()
  {
    DateInformation = SaveUtil.GetDateInformation();
    UniqueIdentifier = 0;
    FirstNode = WorldMap.firstNode;
    WorldNodes = new List<WorldNode>();
  }
}
