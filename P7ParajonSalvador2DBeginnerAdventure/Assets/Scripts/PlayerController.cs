using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Variables for movement
    public InputAction MoveAction;
    public float speed = 10.0f;
    Rigidbody2D rigidbody2d;
    Vector2 move;

    // Variables for Health
    public int maxHealth = 5;
    public int health { get {  return currentHealth; }}
    int currentHealth;

    // Variables for i-frames
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    //Animation Variables
    Animator animator;
    Vector2 moveDirection = new Vector2(1,0);

    //Projectile Variables
    public GameObject projectilePrefab;

    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        move = MoveAction.ReadValue<Vector2>();
        if (isInvincible)
        {
            {

                damageCooldown -= Time.deltaTime;
                if (damageCooldown < 0)
                isInvincible = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
    }
    void FixedUpdate() 
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * 10.0f * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }
    void Launch()
    { 
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);
        animator.SetTrigger("Shoot");
    }
}
