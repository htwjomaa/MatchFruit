/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using TMPro;

public class Player : MonoBehaviour
{
    public float speed = 1f;
    [SerializeField] private float maxHealth = 10f;
    public InputActionReference movementAction;
    public InputActionAsset InputActions;
    public Rigidbody2D rigidbody;
    public Image healthBar;
    public Vector2 Direction { get; private set;} = Vector2.right;
    private float currentHealth;
    private int experiencePoints = 0;
    private Animator animator;
    public static Player THIS;

    private void Awake() 
    {
        InputActions.Enable();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        var movementVector = movementAction.action.ReadValue<Vector2>();
        //transform.Translate(movementVector * speed * Time.deltaTime);

        if (movementVector.magnitude > 0)
        {
            Direction = movementVector.normalized;
        }
        animator.SetBool("Walking", movementVector.magnitude > 0);

        rigidbody.MovePosition(rigidbody.position + movementVector * speed * Time.deltaTime);
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = Math.Clamp(currentHealth / maxHealth, 0, 1);

        if (currentHealth <= 0) Debug.Log("You Died");
    }
    
    public static void AddExperience(int amount) => experiencePoints += amount;
    // You can also update your UI to reflect the new experience points here if needed.
    //Debug.Log("XP Collected: " + amount);
}
public class PlayerLevelSystem : MonoBehaviour
{
    public Image xpBarImage; // Reference to the XP-Bar UI Slider.
    public TextMeshProUGUI levelText; // Reference to a TextMeshProUGUI element to display the player's level.

    private int experiencePoints = 0;
    private int currentLevel = 1;
    private int xpToNextLevel = 100; // Adjust this as needed for your leveling system.
    private void Start()
    {
        Debug.Log("PlayerLevelSystem Start() called");
        // Initialize the XP-Bar and level UI.
        UpdateXPBarUI();
        UpdateLevelUI();
    }
    public void AddExperience(int amount)
    {
        experiencePoints += amount;
        Debug.Log("XP Collected: " + amount);

        // Check if the player has enough XP to level up.
        if (experiencePoints >= xpToNextLevel) LevelUp();

        // Update the XP-Bar UI.
        UpdateXPBarUI();
    }
    private void LevelUp()
    {
        Debug.Log("LEVEL UP");

        currentLevel++;
        experiencePoints -= xpToNextLevel;
        xpToNextLevel += 100; // Increase the XP requirement for the next level.

        // Update the level UI.
        UpdateLevelUI();
    }
    private void UpdateXPBarUI()
    {
        // Calculate the fill amount based on XP progress.
        float fillAmount = (float)experiencePoints / xpToNextLevel;
        // Update the fill amount of the UI Image.
        xpBarImage.fillAmount = fillAmount;
        Debug.Log("Bar is updating");
    }
    private void UpdateLevelUI()
    {
        Debug.Log("LEVEL TEXT UPDATED");
        // Update the player's level in the TextMeshProUGUI component.
        levelText.text = "Level " + currentLevel.ToString();
    }

    public int mal2(int input)
    {
        return input * 2;
    }

    public void mal2(ref int input)
    {
        input *= 2;
    }
}
public class XPOrb : MonoBehaviour
{
    public int xpValue = 10; // The amount of experience points this orb gives.

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collected the XP-Orb.
        if (other.CompareTag("Player"))
        {
            // Increase the player's experience points.
            Player player = other.GetComponent<Player>();
            //if (player != null)
           // {
                Player.AddExperience(xpValue);
                
           // }

            // Destroy the XP-Orb.
            Destroy(gameObject);
        }
    }
}*/