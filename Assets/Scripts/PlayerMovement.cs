using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float friction;
    [SerializeField] private float groundRadius;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Tilemap modifiableTilemap;
    [SerializeField] private Tilemap groundTilemap;

    private AudioManager audioManager;

    private CameraBehaviour cameraBehaviour;
    
    private Animator animator;

    private Transform groundCheck;
    
    private Rigidbody2D rb;

    private bool grounded;
    private float xInputRaw;
    
    [HideInInspector] public float xInput;
    
    public bool lockWalking;
    public bool lockJumping;
    [HideInInspector] public bool hasUsedAutoJump;

    private bool facingRight;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundCheck = transform.GetChild(0);
        audioManager = FindObjectOfType<AudioManager>();

        facingRight = true;

        StartCoroutine(WalkSound());
    }
    
    private Vector3Int[] vectorsToCheck = {
        new(1, 0),
        new(1, 1),
        new(0, 1)
    };

    private void Update() {
        xInputRaw = Input.GetAxisRaw("Horizontal");
        xInput = Mathf.Lerp(xInput, xInputRaw, Time.deltaTime * friction);

        if (facingRight && xInput < 0) {
            facingRight = false;
            Flip();
        } else if (!facingRight && xInput > 0) {
            facingRight = true;
            Flip();
        }

        if (lockWalking) {
            xInput = 0;
        }

        rb.velocity = new Vector2(xInput * speed, rb.velocity.y);    
        
        Collider2D collision = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);

        grounded = collision != null;

        if (!lockJumping) {
            if (Input.GetButtonDown("Jump") && grounded) {
                audioManager.PlaySound("Jump");
                rb.velocity = new Vector2(0, jumpVelocity);
            }
        }
        
        animator.SetFloat(Animator.StringToHash("xVelocity"), Mathf.Abs(xInput));
        animator.SetFloat(Animator.StringToHash("yVelocity"), rb.velocity.y);
        animator.SetBool(Animator.StringToHash("Grounded"), grounded);
        
        Vector3Int position = groundTilemap.WorldToCell((transform.position + new Vector3(facingRight ? -0.5f : 0.5f, -0.5f, 0)));

        if (((Utils.IsCellFull(position + vectorsToCheck[0], groundTilemap) &&
            !Utils.IsCellFull(position + vectorsToCheck[1], groundTilemap) &&
            !Utils.IsCellFull(position + vectorsToCheck[2], groundTilemap)) ||
            (Utils.IsCellFull(position + vectorsToCheck[0], modifiableTilemap) &&
             !Utils.IsCellFull(position + vectorsToCheck[1], modifiableTilemap) &&
             !Utils.IsCellFull(position + vectorsToCheck[2], modifiableTilemap))) && grounded && Mathf.Abs(xInput) > 0.1f) {
            rb.velocity = new Vector2(0, jumpVelocity);
            audioManager.PlaySound("Jump");
            hasUsedAutoJump = true;
        }
    }

    private void Flip() {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        vectorsToCheck[0].x *= -1;
        vectorsToCheck[1].x *= -1;
    }

    private IEnumerator WalkSound() {
        while (true) {
            if (Mathf.Abs(xInput) > 0.1f && grounded) {
                audioManager.PlaySound("Walk1");
                yield return new WaitForSeconds(0.2f);
            }
            if (Mathf.Abs(xInput) > 0.1f && grounded) {    
                audioManager.PlaySound("Walk2");
                yield return new WaitForSeconds(0.2f); 
            }

            yield return null;
        }
    }
}