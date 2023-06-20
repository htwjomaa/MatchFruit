using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AnimationItem : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Animation")]
    [SerializeField] public List<FruitSprites> FruitSprites;
    [SerializeField] public FruitAnimationState currentAnimationState;
    [MinMaxSlider(0,30f)] [SerializeField]   private Vector2 animationStart;
    [MinMaxSlider(0.05f,30f)] [SerializeField]   private Vector2 eyesClosed;
    [MinMaxSlider(0.05f,30f)] [SerializeField]   private Vector2 eyesOpened;
    [MinMaxSlider(0.05f,0.6f)] [SerializeField]   private Vector2 slowBlink;
    [MinMaxSlider(0.001f,0.01f)] [SerializeField]   private Vector2 fastBlink;
    [MinMaxSlider(0.05f,0.3f)] [SerializeField]   private Vector2 toLiftUp2;
    
    
    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartAnimation();
    }

    private bool _closingEyes;

    
    private Sprite GetSpriteFromList(FruitAnimationState fruitAnimationState)
    {
        for (int i = 0; i < FruitSprites.Count; i++)
        {
            //yes dictionary would be faster but this way I can use the Inspector
            if (FruitSprites[i].animationState == fruitAnimationState)
            {
                return FruitSprites[i].AssociatedSprite;
            }
        }

        return null;
    }
int _defaultAnimationStartTime = 0;

public void MoveUp(float durationSec)
{
    StopAllCoroutines();
    StartCoroutine(AnimatenCenterCo(durationSec, FruitAnimationState.LiftUp));
}
private float GetRandomNumber(Vector2 vec2) => Random.Range(vec2.x, vec2.y);
private bool _slowBlink;
[Button]public void StartAnimation()
    {
       // Debug.Log("Starting animation");
        _closingEyes = true;
        StartCoroutine(AnimatenCenterCo(GetRandomNumber(animationStart), FruitAnimationState.EyesOpen));
    }
    public IEnumerator AnimatenCenterCo(float secondsBetweenAnimations, FruitAnimationState fruitAnimationState)
    {
        Sprite spriteFromList = GetSpriteFromList(fruitAnimationState);
        currentAnimationState = fruitAnimationState;
        if(spriteFromList != null && _spriteRenderer != null) _spriteRenderer.sprite = spriteFromList;
        yield return new WaitForSeconds(secondsBetweenAnimations);
        switch (currentAnimationState)
        {
            case FruitAnimationState.EyesOpen:
                //Close Eye
                _closingEyes = true;
                _slowBlink = Random.Range(0, 5) > 1;
                
                if (_slowBlink)
                {
                    StartCoroutine(AnimatenCenterCo(GetRandomNumber(slowBlink), FruitAnimationState.Inbetween));
                }
                else
                {
                    StartCoroutine(AnimatenCenterCo(GetRandomNumber(fastBlink), FruitAnimationState.Inbetween));
                }

                break;
            case FruitAnimationState.Inbetween:

                if (_closingEyes)
                {
                    if (_slowBlink)
                        StartCoroutine(AnimatenCenterCo(GetRandomNumber(eyesClosed), FruitAnimationState.EyesClosed));
                    else
                    {
                        StartCoroutine(AnimatenCenterCo(GetRandomNumber(slowBlink), FruitAnimationState.EyesClosed));
                    }
                }
                else
                {
                    if (_slowBlink)
                        StartCoroutine(AnimatenCenterCo(GetRandomNumber(eyesOpened), FruitAnimationState.EyesOpen));
                    else
                    {
                        StartCoroutine(AnimatenCenterCo(GetRandomNumber(slowBlink), FruitAnimationState.EyesOpen));
                    }
                }

                break;
            case FruitAnimationState.EyesClosed:
                _closingEyes = false;
                if (_slowBlink)
                {
                    StartCoroutine(AnimatenCenterCo(GetRandomNumber(slowBlink), FruitAnimationState.Inbetween));
                }

                else
                {
                    StartCoroutine(AnimatenCenterCo(GetRandomNumber(fastBlink), FruitAnimationState.Inbetween));
                }
                
                break;
            case FruitAnimationState.LiftUp:
                StartCoroutine(AnimatenCenterCo(GetRandomNumber(toLiftUp2), FruitAnimationState.LiftUp2));
                break;
            case FruitAnimationState.LiftUp2:
                StartCoroutine(AnimatenCenterCo(GetRandomNumber(eyesOpened), FruitAnimationState.EyesOpen));
                break;
            case FruitAnimationState.LiftDown:
                
                break;
            case FruitAnimationState.Special1:
                
                break;
            case FruitAnimationState.Special2:
                
                break;
            case FruitAnimationState.Special3:
                
                break;
        }
    }
    public void CloseEyes()
    {
        
    }
}
[Serializable]
public enum FruitAnimationState
{
    EyesOpen,
    Inbetween,
    EyesClosed,
    LiftUp,
    LiftUp2,
    LiftDown,
    Special1,
    Special2,
    Special3,
    Special4
}
[Serializable]
public struct FruitSprites
{
    [FormerlySerializedAs("SpriteCategory")] public FruitAnimationState animationState;
    public Sprite AssociatedSprite;

    public FruitSprites(FruitAnimationState animationState, Sprite associatedSprite)
    {
        this.animationState = animationState;
        AssociatedSprite = associatedSprite;
    }
}