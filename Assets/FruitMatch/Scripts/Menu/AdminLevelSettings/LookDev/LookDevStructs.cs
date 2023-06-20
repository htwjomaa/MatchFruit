using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct BgImageSet
{
    public LevelCategory LevelCategory;
    public List<BgImage> BgImageList;

    public BgImageSet(LevelCategory levelCategory, List<BgImage> bgImageList)
    {
        LevelCategory = levelCategory;
        BgImageList = bgImageList;
    }
}

[Serializable]
public struct BgImage
{  
    public string BGName;

    [SerializeField] public Sprite BGImage;
    public Vector3 BackGroundPos;
    public Vector3 BackGroundScale;

    public BgImage(string bgName, LevelCategory levelCategory, Sprite bgImage, Vector3 backGroundPos, Vector3 backGroundScale)
    {
        BGName = bgName;
        BGImage = bgImage;
        BackGroundPos = backGroundPos;
        BackGroundScale = backGroundScale;
    }
    public BgImage(string bgName,LevelCategory levelCategory, Sprite bgImage, Vector3 backGroundPos)
    {
        BGName = bgName;
        BGImage = bgImage;
        BackGroundPos = backGroundPos;
        BackGroundScale = new Vector3(1.534595f, 1.534595f, 1.534595f);
    }
    public BgImage(string bgName, Sprite bgImage)
    {
        BGName = bgName;
        BGImage = bgImage;
        BackGroundPos = new Vector3(4.87027f, 0.9639063f, 18f);
        BackGroundScale = new Vector3(1.534595f, 1.534595f, 1.534595f);
    }
}