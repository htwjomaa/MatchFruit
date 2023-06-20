using UnityEngine;
public sealed class Curtain : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float alphaSpeed = 0.005f;
    void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
    }
    void Update()
    {
        if (spriteRenderer.color.a != 0)
            spriteRenderer.color = new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - alphaSpeed);
        else
        {
            Destroy(gameObject);
        }
    }
}