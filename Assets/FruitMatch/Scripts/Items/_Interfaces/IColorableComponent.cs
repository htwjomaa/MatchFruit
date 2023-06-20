using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.Items;
using FruitMatch.Scripts.Level;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class IColorableComponent : MonoBehaviour /* , IPoolable */
{
    public int color;

    public List<SpritePerLevel> Sprites;

    public SpriteRenderer directSpriteRenderer;

    private void Awake()
    {
  
      //  if (Sprites.Count > 1)
     //   {
           // Sprites[0].Sprites = LoadingManager.loadedMarmaladeSprites;
      //      Sprites[1].Sprites = LoadingManager.loadedMarmaladeSprites;
      //  }
      //  else
      //  {
           if(GetComponent<ItemSimple>())Sprites[0].Sprites = LoadingManager.loadedSprites;
           else if (GetComponent<ItemMarmalade>())
           {
               Sprites[0].Sprites = LoadingManager.loadedMarmaladeSprites;
                    Sprites[1].Sprites = LoadingManager.loadedMarmaladeSprites;
           }
           else if (GetComponent<ItemStriped>())
           {
               if (GetComponent<ItemStriped>().currentType == ItemsTypes.HORIZONTAL_STRIPED)
               {
                   Sprites[0].Sprites = LoadingManager.loadedHorStriped;
                   Sprites[1].Sprites = LoadingManager.loadedHorStriped;
               }
                   
               else
               {
                   Sprites[0].Sprites = LoadingManager.loadedVertStriped;
                   Sprites[1].Sprites = LoadingManager.loadedVertStriped;
                   
               }

               GetComponentInChildren<SpriteRenderer>().sprite = Sprites[0].Sprites[color];
           }
            // if (GetComponent<ItemStriped>())
            // {
            //     //.type  horizontal  get Horizontal
            //     //.type vertical  get Vertical
            //     ;
            // }
            //  else if(GetComponent<ItemSimple>()) S;

      //  }
        // List<Sprite> allSprites = new List<Sprite>();
      //  GameObject[] allDotPrefabs =LoadingManager.GetDots(Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
         //   .LevelConfigs[LoadingManager.CurrentLevelToload].BoardDimensionsConfig, Rl.world);
        //   for (int i = 0; i < allDotPrefabs.Length; i++)
        //   {
        //       allSprites.Add(TranslateDotsToRandomColors(allDotPrefabs[Random.Range(0, allDotPrefabs.Length)]
        //           .GetComponent<Dot>()));
        //   }
        // //  Array.Resize(ref Sprites[0].Sprites, allSprites.Count);
        //   for (int i = 0; i < allSprites.Count; i++)
        //   {
        //       Sprites[0].Sprites[i] = allSprites[i];
        //   }
        // Sprites[0] = allSprites;
       // Debug.LogError("allDotPrefabs.Length" + allDotPrefabs.Length);
        itemComponent = GetComponent<Item>();
     

  
        iColorableComponents = GetComponentsInChildren<IColorableComponent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        iColorChangables = GetComponentsInChildren<IColorChangable>();
    }

    // public IHighlightableComponent IHighlightableComponent;
    // public IDestroyableComponent IDestroyableComponent;
    // public ItemSound ItemSound;
    private void OnEnable()
    {

        if (itemComponent && !itemComponent.Combinable)
        {
            color = GetHashCode();
//            Debug.Log("color: " + color);
        }
        
    }

    // [HideInInspector]
    // public bool colorGenerated;
    public bool RandomColorOnAwake = true;
    private Item itemComponent;
    private IColorableComponent[] iColorableComponents;
    private SpriteRenderer spriteRenderer;
    private IColorChangable[] iColorChangables;

    public void SetColor(int _color)
    {
        if (_color < 0 || _color >= GetSprites(LevelManager.THIS.currentLevel).Length) return;
        // colorGenerated = true;
        var component = itemComponent;
        if (component != null && component.currentType != ItemsTypes.MULTICOLOR)
            color = _color;
        if (directSpriteRenderer == null) directSpriteRenderer = spriteRenderer;

        if (GetSprites(LevelManager.THIS.currentLevel).Length > 0 && directSpriteRenderer)
            directSpriteRenderer.sprite = GetSprites(LevelManager.THIS.currentLevel)[_color];
        foreach (var i in iColorChangables) i.OnColorChanged(_color);
    }

    public void RandomizeColor()
    {


   // color = Random.Range(0, 2);
       color = ColorGenerator.GenColor(itemComponent.square);
        
        SetColor(color);
        foreach (IColorableComponent i in iColorableComponents) i.SetColor(color);
    }
    
    

    private Sprite TranslateDotsToRandomColors(Dot dot)
    {
    
        
        switch (dot.tag)
        {
            case "Green Dot":
               return Sprites[0].Sprites[0];
            case "Dark Green Dot":
                return Sprites[0].Sprites[1];
            case "Red Dot":
                return Sprites[0].Sprites[2];;
            case "Salmon Dot":
                return Sprites[0].Sprites[3];
            case "Yellow Dot":
                return Sprites[0].Sprites[4];
            case "Indigo Dot":
                return Sprites[0].Sprites[5];
            case "Teal Dot":
                return Sprites[0].Sprites[6];
            case "Orange Dot":
                return Sprites[0].Sprites[7];
        }

        return   Sprites[0].Sprites[0];
    }

    private Sprite[] GetLoadedSprites()
    {
        List<Sprite> allSprites = new List<Sprite>();
        GameObject[] allDotPrefabs =LoadingManager.GetDots(Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
            .LevelConfigs[LoadingManager.CurrentLevelToload].BoardDimensionsConfig, Rl.world);
        for (int i = 0; i < allDotPrefabs.Length; i++)
        {
            allSprites.Add(TranslateDotsToRandomColors(allDotPrefabs[Random.Range(0, allDotPrefabs.Length)]
                .GetComponent<Dot>()));
        }

        return allSprites.ToArray();
    }
    public Sprite[] GetSprites(int level)
    {
     
        SpritePerLevel spritePerLevel = null;
        if (level == 0) level = 1;
        if (Sprites.Any(i => i.level == level))
            return Sprites.First(i => i.level == level).Sprites;

        return Sprites[0].Sprites;
    }

    public Sprite GetSprite(int level, int color)
    {      List<Sprite> allSprites = new List<Sprite>();
         GameObject[] allDotPrefabs =LoadingManager.GetDots(Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
          .LevelConfigs[LoadingManager.CurrentLevelToload].BoardDimensionsConfig, Rl.world);
          for (int i = 0; i < allDotPrefabs.Length; i++)
           {
               allSprites.Add(TranslateDotsToRandomColors(allDotPrefabs[Random.Range(0, allDotPrefabs.Length)]
                   .GetComponent<Dot>()));
          }
        // //  Array.Resize(ref Sprites[0].Sprites, allSprites.Count);
        //   for (int i = 0; i < allSprites.Count; i++)
        //   {
        //       Sprites[0].Sprites[i] = allSprites[i];
        //   }
        // Sprites[0] = allSprites;
        // Debug.LogError("allDotPrefabs.Length" + allDotPrefabs.Length);
        //var list = GetSprites(level);
        var list = allSprites.ToArray();
       Debug.LogWarning("T:T:T:T:T:T:list.Length:::::::::: " +list.Length); 
        if (color < list.Length) return list[color];
        else if (list.Any()) return list[0];
        return null;
    }

    public Sprite[] GetSpritesOrAdd(int level)
    {
        if (Sprites.All(i => i.level != level))
        {
            var sprites = Sprites[0].Sprites;
            var other = new Sprite[sprites.Length];
            for (var i = 0; i < sprites.Length; i++) other[i] = sprites[i];

            Sprites.Add(new SpritePerLevel {level = level, Sprites = other});
        }

        return GetSprites(level);
    }
}