using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float movementSpeed;
    [SerializeField] float moveImpact;
    [SerializeField] float laneYPosition;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpGravityUp;
    [SerializeField] float jumpGravityDown;
    [SerializeField] float buttonTime;
    Rigidbody2D rigidBody;
    bool isJumping = false;
    float jumpTime;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpWithVelocity();
    }

    void FixedUpdate()
    {
        movePlayer();
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("YOU DIED!");
    }

    void movePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(movementSpeed + moveX * moveImpact, rigidBody.velocity.y);
    }

    void jumpWithVelocity()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GetComponent<Transform>().position.y == laneYPosition) 
        {
            isJumping = true;
            jumpTime = 0;
        }
        if(isJumping)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);
            jumpTime += Time.deltaTime;
        }
        if((Input.GetKeyUp(KeyCode.Space) | jumpTime > buttonTime) && GetComponent<Transform>().position.y != laneYPosition)
        {
            isJumping = false;
            rigidBody.gravityScale = jumpGravityUp;
        }
        if(GetComponent<Transform>().position.y < laneYPosition)
        {
            rigidBody.gravityScale = 0;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, laneYPosition, 0);
        }
    }

    void jumpWithForce()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            rigidBody.gravityScale = jumpGravityUp;
            float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rigidBody.gravityScale));
            // rigidBody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        if(rigidBody.velocity.y <= 0 && isJumping)
        {
            if(GetComponent<Transform>().position.y <= laneYPosition)
            {
                rigidBody.gravityScale = 0;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
                GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, laneYPosition, 0);
                isJumping = false;
            }
            else
            {
                rigidBody.gravityScale = jumpGravityDown;
            }
        }
    }
}
