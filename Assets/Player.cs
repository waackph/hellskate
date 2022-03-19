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
    [SerializeField] float waitForCombi;
    [SerializeField] float minHeight;

    Rigidbody2D rigidBody;

    Lane currentLane;

    bool isUpper = false;
    float jumpTime;

    // Variables to wait for combinations
    float combiTime;
    bool isInCombiMode = false;
    List<string> combiKeyList;

    // State (switch lane, jump) variables
    bool isInSwitchMode = false;
    bool isJumpingUp = false;
    bool isInJumpMode = false;
    float jumpStartPosition;

    // Start is called before the first frame update
    void Start()
    {
        combiKeyList = new List<string>();
        rigidBody = GetComponent<Rigidbody2D>();
        currentLane = laneSystem.GetComponent<LaneController>().CurrentLane;
        jumpStartPosition = currentLane.LaneYPosition;
        HandleLaneCollision();
    }

    // Todo: Wait 0.5 seconds to allow key combinations (switch lane while jump) 
    // look at jumpTime to see how to wait for some time
    // Update is called once per frame
    void Update()
    {
        if(!isInJumpMode)
        {
            checkJumpWithVelocity();
        }
        if(!isInSwitchMode)
        {
            checkSwitchLanes();
        }
        if(!isInCombiMode)
        {
            checkAnyKeyPressed();
        }
    }

    void FixedUpdate()
    {
        movePlayer();
        if(isInSwitchMode && !isInJumpMode)
        {
            doMoveToLane();
        }
        if(isInJumpMode)
        {
            doJump();
        }
        // TODO: Execute an appropriate animation and score when combi is evaluated.
        if(isInCombiMode)
        {
            checkForCombi();
        }
    }

    void checkAnyKeyPressed()
    {
        if(Input.anyKey)
        {
            // Initilize combi mode start
            combiTime = 0f;
            isInCombiMode = true;
            combiKeyList.Clear();
            checkForCombi();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: Decide if died or not depending on entry point of collision (e.g. in front, from below and not jump = death)
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
        if((currentLane.LaneYPosition <= rigidBody.transform.position.y && isUpper) || (currentLane.LaneYPosition >= rigidBody.transform.position.y && !isUpper))
        {
            isInSwitchMode = false;
            rigidBody.transform.position = new Vector3(rigidBody.transform.position.x, currentLane.LaneYPosition, 0);
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
        if (Input.GetKeyDown(KeyCode.Space))  // && rigidBody.transform.position.y == currentLane.LaneYPosition) 
        {
            // Initilize jump start
            isJumpingUp = true;
            isInJumpMode = true;
            jumpTime = 0;
            jumpStartPosition = rigidBody.transform.position.y;
        }
    }

    void checkForCombi()
    {
        combiTime += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.X))
        {
            combiKeyList.Add(KeyCode.X.ToString());
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            combiKeyList.Add(KeyCode.Space.ToString());
        }

        if(combiTime > waitForCombi)
        {
            isInCombiMode = false;
            evaluatedCombi();
        }
    }

    void evaluatedCombi()
    {
        if(combiKeyList.Contains(KeyCode.Space.ToString()))
        {
            if(combiKeyList.Contains(KeyCode.X.ToString()))
            {
                Debug.Log("Combi!");
                Debug.Log(combiKeyList.Count);
            }
        }
    }

    void doJump()
    {
        if((Input.GetKeyUp(KeyCode.Space) | jumpTime > buttonTime) && Mathf.Abs((Mathf.Abs(jumpStartPosition) - Mathf.Abs(rigidBody.transform.position.y))) >= minHeight && rigidBody.transform.position.y != currentLane.LaneYPosition)
        {
            isJumpingUp = false;
            rigidBody.gravityScale = jumpGravityUp;
        }
        if(isJumpingUp)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);
            jumpTime += Time.deltaTime;
        }
        if(rigidBody.transform.position.y <= currentLane.LaneYPosition && isInJumpMode && !isJumpingUp)
        {
            rigidBody.gravityScale = 0;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            rigidBody.transform.position = new Vector3(rigidBody.transform.position.x, currentLane.LaneYPosition, 0);
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
            if(rigidBody.transform.position.y <= currentLane.LaneYPosition)
            {
                rigidBody.gravityScale = 0;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
                rigidBody.transform.position = new Vector3(rigidBody.transform.position.x, currentLane.LaneYPosition, 0);
                isJumpingUp = false;
            }
            else
            {
                rigidBody.gravityScale = jumpGravityDown;
            }
        }
    }
}
