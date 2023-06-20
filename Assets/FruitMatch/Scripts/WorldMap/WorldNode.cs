using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public sealed class WorldNode : MonoBehaviour
{
    public List<WorldLine> PrevWorldLines;
    public List<WorldLine> NextWorldLines;
    
    public ushort UniqueIdentifier;
    public Vector3 WorldPos; 
    public Sprite ActivatedSprite; 
    public Sprite DeactivedSprite;
    [Range(0,359)]public ushort Degree;
    public WorldNode(List<WorldLine> prevWorldLines, List<WorldLine> nextWorldLines, ushort uniqueIdentifier,
        Vector3 worldPos, Sprite activatedSprite, Sprite deactivedSprite, ushort degree)
    {
        PrevWorldLines = prevWorldLines;
        NextWorldLines = nextWorldLines;
        UniqueIdentifier = uniqueIdentifier;
        WorldPos = worldPos;
        ActivatedSprite = activatedSprite;
        DeactivedSprite = deactivedSprite;
        Degree = degree;
    }
    public WorldNode(List<WorldLine> prevWorldLines, ushort uniqueIdentifier, Vector3 worldPos, Sprite activatedSprite, Sprite deactivedSprite, ushort degree)
    {
        PrevWorldLines = prevWorldLines;
        NextWorldLines = null;
        UniqueIdentifier = uniqueIdentifier;
        WorldPos = worldPos;
        ActivatedSprite = activatedSprite;
        DeactivedSprite = deactivedSprite;
        Degree = degree;
    }
    public WorldNode(List<WorldLine> prevWorldLines, List<WorldLine> nextWorldLines, ushort uniqueIdentifier, Vector3 worldPos, ushort degree)
    {
        PrevWorldLines = prevWorldLines;
        NextWorldLines = nextWorldLines;
        UniqueIdentifier = uniqueIdentifier;
        WorldPos = worldPos;
        ActivatedSprite = Rl.worldMap.defaultActiveSprite;
        DeactivedSprite = Rl.worldMap.defaultDectiveSprite;
        Degree = degree;
    }
    public WorldNode(List<WorldLine> prevWorldLines, ushort uniqueIdentifier, Vector3 worldPos, ushort degree)
    {
        PrevWorldLines = prevWorldLines;
        NextWorldLines = null;
        UniqueIdentifier = uniqueIdentifier;
        WorldPos = worldPos;
        ActivatedSprite = Rl.worldMap.defaultActiveSprite;
        DeactivedSprite = Rl.worldMap.defaultDectiveSprite;
        Degree = degree;
    }


    public void Awake()
    {
        WorldMap.HashId += LoadIds;
        _cachedTimer = _timer;
        _childObjects = new Queue<Transform>();
        _controlPointCashedPos= new Queue<Vector3>();
    }

    private void OnDestroy() => WorldMap.HashId -= LoadIds;
    private void LoadIds() => WorldMap.allUniqueIdentifier.Add(UniqueIdentifier, this);

    private float _timer = 0.125f;
    private float _cachedTimer;
    private Queue<Transform> _childObjects = new Queue<Transform>();
    private Queue<Vector3> _controlPointCashedPos= new Queue<Vector3>();
    private void Update()
    {
        if (_timer > -2) _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            // for (int i = 0; i< _childObjects.Count; i++)
            // {
            //     _childObjects.Dequeue().transform.parent = transform;
            // }
            if (MoveLevelElements.selectedGameObject == gameObject)
            {
                for (int i = 0; i < _controlPointCashedPos.Count; i++)
                {
                    for (int j = 0; j < transform.childCount; j++)
                    {
                        for (int k = 0; k < transform.GetChild(j).childCount; k++)
                        {
                            transform.GetChild(j).GetChild(k).transform.position = _controlPointCashedPos.Dequeue();
                        }
                    }
                }
            }
            _controlPointCashedPos.Clear();

            if (MoveLevelElements.selectedGameObject == gameObject)
                {
                for (int i = 0; i< transform.childCount; i++)
                {
                    for (int j = 0; j < transform.GetChild(i).childCount; j++)
                    {
                        _controlPointCashedPos.Enqueue(transform.GetChild(i).GetChild(j).transform.position);
                    }
                }
            }
            
            if (transform.hasChanged)
            {
                _timer = _cachedTimer;
                transform.hasChanged = false;
                ReDrawLines();
            }
        }
    
    }



    private void ReDrawLines()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                transform.GetChild(i).GetChild(j).GetComponent<WorldLineCurvePoints>().timer = _cachedTimer/2;
                transform.GetChild(i).GetChild(j).transform.hasChanged = false;
            }

            if (transform.GetChild(i).childCount > 0)
            {
                transform.GetChild(i).GetChild(0).GetComponent<WorldLineCurvePoints>().timer = _cachedTimer/2;
                transform.GetChild(i).GetChild(0).transform.hasChanged = true;
            }
           
            //  if(transform.GetChild(i) != null && transform.GetChild(i).childCount > 0) 
          //     transform.GetChild(i).transform.GetChild(0).transform.hasChanged = true;
        }

        if (PrevWorldLines != null)
        {
            for (int i = 0; i < PrevWorldLines.Count; i++)
            {
                for (int j = 0; j < PrevWorldLines[i].PrevWorldNode.transform.childCount; j++)
                {
                    if (PrevWorldLines[i].PrevWorldNode.transform.GetChild(j) != null &&
                        PrevWorldLines[i].PrevWorldNode.transform.GetChild(j).transform.childCount > 0)
                    {
                        PrevWorldLines[i].PrevWorldNode.transform.GetChild(j).GetChild(0).transform.hasChanged = true;
                        PrevWorldLines[i].PrevWorldNode.transform.GetChild(j).GetChild(0).GetComponent<WorldLineCurvePoints>().timer = _cachedTimer/2;
                    }
                }
            }
        }
    }
}

public class ParentClass
{
    
}