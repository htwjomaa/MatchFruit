using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class BackgroundTile : MonoBehaviour
{
    public int hitPoints;
    private SpriteRenderer sprite;
    public GameObject AssosiatedGameObject;
    public GameObject child;
    void Start() => sprite = GetComponent<SpriteRenderer>();

    private void Awake()
    {
        //GenericSettingsFunctions.DestroyAllChildren(gameObject);
        //child = Instantiate(gameObject, transform);
       // child.AddComponent<SpriteRenderer>();
    }
    
    
    private void Update()
    {
        if (hitPoints > 0) return;
        
            if(Rl.goalManager != null)
            {
                Rl.goalManager.CompareGoal(this.gameObject.tag);
                Rl.goalManager.UpdateGoals();
            }
            
            List<TileType> helperCopy = Rl.board.boardLayout.ToList();
            for (int i = 0; i < Rl.board.boardLayout.Length; i++)
            {
                TileType tile =   Rl.board.boardLayout[i];
                if (tile.x == (int)this.transform.position.x && tile.y == (int)this.transform.position.y &&
                    tile.tileKind == TileKind.Breakable) helperCopy.RemoveAt(i);
            }

            Rl.board.boardLayout = helperCopy.ToArray();
            Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        if (hitPoints == 2147483647) return;
        hitPoints -= damage;
        MakeLighter();
        ChangeColor();
        Rl.GameManager.PlayAudio(Rl.soundStrings.TakeDamageAudio, Random.Range(0,4), Rl.settings.GetSFXVolume, Rl.effects.audioSource);
    }

    void MakeLighter()
    {
        //take the current color
        Color color = sprite.color;
        //get the current colors alpha value and cut it in half.
        float newAlpha = color.a * .85f;
        sprite.color = new Color(color.r, color.g, color.b, newAlpha);
    }
    void ChangeColor()
    {
        //take the current color
        Color color = sprite.color;
        sprite.color = new Color(color.r, color.g*.7f, color.b*.7f, color.a);
    }
}