using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Vector2 move;

    public InputAction MoveAction;

    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        Debug.Log(move);
    }
    void FixedUpdate() 
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * 10.0f * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
}
