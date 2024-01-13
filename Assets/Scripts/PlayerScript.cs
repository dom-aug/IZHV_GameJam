using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 10f;

    public bool isGrounded = false;

    public LayerMask groundLayer;

    public LayerMask obstacleLayer;

    public LayerMask boundaryLayer;

    private Rigidbody playerRigidbody;

    private BoxCollider playerBoxCollider;

    private Animator playerAnimator;

    private int obstacleLayerMask;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerBoxCollider = GetComponent<BoxCollider>();
        playerAnimator = GetComponentInChildren<Animator>();
        if (playerAnimator == null) {
            Debug.LogError("PlayerAnimator is null");
        }

        obstacleLayerMask = LayerMask.NameToLayer("Obstacle");
    }

    // Update is called once per frame
    void Update()
    {
        speed = GameManagerScript.Instance.gameSpeed;
        playerAnimator.SetFloat("Speed", speed/10f);

        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        var jumpMovement = Input.GetButtonDown("Jump");
        var toggleTorch = Input.GetKeyDown(KeyCode.T);
        
        if (jumpMovement && isOnGround()) {
            playerRigidbody.velocity = - Physics.gravity * jumpForce;
            playerAnimator.SetBool("Jump", true);
            playerAnimator.SetBool("Grounded", false);
        }

        if (isOnGround()) {
            playerAnimator.SetBool("Jump", false);
        }
        playerAnimator.SetBool("Grounded", isOnGround());

        if (horizontalMovement != 0 && Math.Abs(transform.position.x + horizontalMovement) < 4.5f) {            
            transform.position += new Vector3(horizontalMovement * speed / 500, 0, 0);
        }
        playerAnimator.SetFloat("MovementDirection", horizontalMovement);

        if (toggleTorch) {
            gameObject.GetComponentInChildren<Light>().enabled = !gameObject.GetComponentInChildren<Light>().enabled;
        }

    }

    void FixedUpdate()
    {
        //playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x * speed, 0, 0);
    }

    bool isOnGround() 
    {
        var boundsSize = playerBoxCollider.bounds.size;
        var boundsCenter = playerBoxCollider.bounds.center;

        var boundsBottom = new Vector3(boundsCenter.x, boundsCenter.y - boundsSize.y / 2, boundsCenter.z);

        var hit = Physics.Raycast(boundsBottom, Vector3.down, 0.2f, groundLayer);
        //var hit = Physics.BoxCast(transform.position, boundsSize, Vector3.down, transform.rotation, 0.2f, groundLayer);

        return hit;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == obstacleLayerMask) {            
            var boundsSize = playerBoxCollider.bounds.size;
            var boundsCenter = playerBoxCollider.bounds.center;

            var boundsBottom = new Vector3(boundsCenter.x, boundsCenter.y - boundsSize.y / 2, boundsCenter.z);

            var hit = Physics.Raycast(boundsCenter, Vector3.forward, 0.2f, obstacleLayer);

            //var hit = Physics.BoxCast(boundsCenter, boundsSize, Vector3.forward, transform.rotation, 0.2f, obstacleLayer);

            if (hit) {
                Debug.Log("hit obstacle");
                //game over
                GameManagerScript.Instance.LoseGame();
                playerAnimator.SetBool("GameOver", true);
            }
            
        }
    }
}
