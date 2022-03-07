using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] GameObject laneSystem;
    [SerializeField] float movementSpeed;
    [SerializeField] float moveImpact;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpGravityUp;
    [SerializeField] float jumpGravityDown;
    [SerializeField] float buttonTime;
    Rigidbody2D rigidBody;

    Lane currentLane;

    bool isUpper = false;
    float jumpTime;

    // State (switch lane, jump) variables
    bool isInSwitchMode = false;
    bool isJumpingUp = false;
    bool isInJumpMode = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        currentLane = laneSystem.GetComponent<LaneController>().CurrentLane;
        HandleLaneCollision();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInSwitchMode && !isInJumpMode)
        {
            checkJumpWithVelocity();
        }
        if(!isInJumpMode && !isInSwitchMode)
        {
            checkSwitchLanes();
        }
    }

    void FixedUpdate()
    {
        movePlayer();
        if(isInSwitchMode)
        {
            doMoveToLane();
        }
        if(isInJumpMode)
        {
            doJump();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("YOU DIED!");
    }

    void movePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(movementSpeed + moveX * moveImpact, rigidBody.velocity.y);
    }

    void checkSwitchLanes()
    {
        float moveY = Input.GetAxis("Vertical");
        if(moveY > 0)
        {
            isUpper = true;
            switchLane(laneUp:isUpper);
        }
        else if(moveY < 0)
        {
            isUpper = false;
            switchLane(laneUp:isUpper);
        }
    }

    void switchLane(bool laneUp)
    {
        isInSwitchMode = true;
        currentLane = laneSystem.GetComponent<LaneController>().SwitchLane(laneUp);
        HandleLaneCollision();
    }

    void HandleLaneCollision()
    {
        if(currentLane.Name == "Lane1")
        {
            Physics2D.IgnoreLayerCollision(0, 6, false);
            Physics2D.IgnoreLayerCollision(0, 7, true);
            Physics2D.IgnoreLayerCollision(0, 8, true);
        }
        else if(currentLane.Name == "Lane2")
        {
            Physics2D.IgnoreLayerCollision(0, 6, true);
            Physics2D.IgnoreLayerCollision(0, 7, false);
            Physics2D.IgnoreLayerCollision(0, 8, true);
        }
        else if(currentLane.Name == "Lane3")
        {
            Physics2D.IgnoreLayerCollision(0, 6, true);
            Physics2D.IgnoreLayerCollision(0, 7, true);
            Physics2D.IgnoreLayerCollision(0, 8, false);
        }
    }

    void doMoveToLane()
    {
        if((currentLane.LaneYPosition <= GetComponent<Transform>().position.y && isUpper) || (currentLane.LaneYPosition >= GetComponent<Transform>().position.y && !isUpper))
        {
            isInSwitchMode = false;
            GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, currentLane.LaneYPosition, 0);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        }
        else
        {
            if(isUpper)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, movementSpeed);
            else
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, -movementSpeed);
        }
    }

    void checkJumpWithVelocity()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GetComponent<Transform>().position.y == currentLane.LaneYPosition) 
        {
            isJumpingUp = true;
            isInJumpMode = true;
            jumpTime = 0;
        }
    }

    void doJump()
    {
        if((Input.GetKeyUp(KeyCode.Space) | jumpTime > buttonTime) && GetComponent<Transform>().position.y != currentLane.LaneYPosition)
        {
            isJumpingUp = false;
            rigidBody.gravityScale = jumpGravityUp;
        }
        if(isJumpingUp)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);
            jumpTime += Time.deltaTime;
        }
        if(GetComponent<Transform>().position.y < currentLane.LaneYPosition && isInJumpMode)
        {
            rigidBody.gravityScale = 0;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, currentLane.LaneYPosition, 0);
            isInJumpMode = false;
        }
    }

    void jumpWithForce()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumpingUp)
        {
            isJumpingUp = true;
            rigidBody.gravityScale = jumpGravityUp;
            float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rigidBody.gravityScale));
            // rigidBody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        if(rigidBody.velocity.y <= 0 && isJumpingUp)
        {
            if(GetComponent<Transform>().position.y <= currentLane.LaneYPosition)
            {
                rigidBody.gravityScale = 0;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
                GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, currentLane.LaneYPosition, 0);
                isJumpingUp = false;
            }
            else
            {
                rigidBody.gravityScale = jumpGravityDown;
            }
        }
    }
}
