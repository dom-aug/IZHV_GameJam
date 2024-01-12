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


    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerBoxCollider = GetComponent<BoxCollider>();
        playerAnimator = GetComponentInChildren<Animator>();
        if (playerAnimator == null) {
            Debug.LogError("PlayerAnimator is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        var jumpMovement = Input.GetButtonDown("Jump");
        
        if (jumpMovement && isOnGround()) {
            playerRigidbody.velocity = - Physics.gravity * jumpForce;
            playerAnimator.SetBool("Jump", true);
            playerAnimator.SetBool("Grounded", false);
        }

        if (isOnGround()) {
            playerAnimator.SetBool("Jump", false);
        }
        playerAnimator.SetBool("Grounded", isOnGround());

        if (horizontalMovement != 0) {            
            transform.position += new Vector3(horizontalMovement * speed, 0, 0);
        }
        playerAnimator.SetFloat("MovementDirection", horizontalMovement);


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

        var hit = Physics.Raycast(boundsBottom, Vector3.down, 0.1f, groundLayer);

        return hit;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle") {            
            var boundsSize = playerBoxCollider.bounds.size;
            var boundsCenter = playerBoxCollider.bounds.center;

            var boundsBottom = new Vector3(boundsCenter.x, boundsCenter.y - boundsSize.y / 2, boundsCenter.z);

            var hit = Physics.Raycast(boundsBottom, Vector3.forward, 0.5f, obstacleLayer);

            if (hit) {
                Debug.Log("hit obstacle");
                //game over
            }
            
        }
    }
}
