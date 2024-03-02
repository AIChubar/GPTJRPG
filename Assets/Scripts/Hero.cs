using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    // public Animator animator;

    public bool movementsEnabled = true;

    public GroupData allyGroup;
 
    
    private Rigidbody2D rb;
    
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }
    
    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
    
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

    }
    void FixedUpdate()
    {
        if (movementsEnabled)
        {
            var movement = playerInput.Player.Move.ReadValue<Vector2>();
            rb.MovePosition(rb.position + movement * (3.0f * Time.fixedDeltaTime));
        }
    }
}
