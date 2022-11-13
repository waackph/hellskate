using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] float fallingSpeed;
    [SerializeField] GameObject levelLoader;

    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator animator;

    Lane currentLane;

    bool isUpper = false;
    bool playerIsOnObstacle = false;
    float jumpTime;

    // Variables to wait for combinations
    float combiTime;
    bool isInCombiMode = false;
    List<KeyCode> combiKeyList;

    // State (switch lane, jump) variables
    bool isInSwitchMode = false;
    bool isJumpingUp = false;
    bool isInJumpMode = false;
    float jumpStartPosition;

    bool levelIsFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        combiKeyList = new List<KeyCode>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentLane = laneSystem.GetComponent<LaneController>().CurrentLane;
        jumpStartPosition = currentLane.LaneYPosition;
        HandleLaneCollision(currentLane.Name);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInCombiMode)
        {
            checkAnyKeyPressed();
        }
        animator.SetBool("IsJumping", isInJumpMode);
    }

    void FixedUpdate()
    {
        movePlayer();
        if(!levelIsFinished)
        {
            if(isInSwitchMode && !isInJumpMode && !playerIsOnObstacle)
            {
                doMoveToLane();
            }
            // Idea: Maybe we should try to use colliders for lanes, 
            // then having less gravity issues, more unity-like solution maybe
            // and make use of full physics feature.
            // But on the other hand, switching lanes using colliders might be a pain 
            // and may be also unprecise, not sure. 
            // Other non-hacky solution would be to completely drop phyisics...
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
        // TODO: Decide if died or not depending on entry point of collision 
        // (e.g. in front, from below and not jump = death)
        if(collision.gameObject.tag.Equals("Obstacle"))
        {
            Vector2 hit = collision.contacts[0].normal;
            float angle = Vector2.Angle(hit, Vector2.up);

            if(Mathf.Approximately(angle, 0))
            {
                Debug.Log("Up");
                playerIsOnObstacle = true;
            }
            if(Mathf.Approximately(angle, 180))
            {
                Debug.Log("Down");
                GameOver();
            }
            if(Mathf.Approximately(angle, 90))
            {
                // Sides of the obstacle
                // Perpendicular = senkrecht (to the other two vectors, similar to 3D Cross Product)
                Vector2 perpendicularVec = Vector2.Perpendicular(hit);
                Debug.Log(Vector2.Perpendicular(hit));
                // Vector3 cross = Vector3.Cross(Vector3.forward, new Vector3(hit.x, hit.y, 0));
                // Debug.Log(cross);
                if(perpendicularVec.y > 0)
                {
                    Debug.Log("Right");
                    GameOver();
                }
                else
                {
                    Debug.Log("Left");
                    GameOver();
                }
            }
        }
        else if(collision.gameObject.tag.Equals("DeadlyObstacle"))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        levelLoader.GetComponent<LevelLoader>().LoadMenu();
        GlobalVars.CurrentGameState = GlobalVars.GameState.Loose;
    }

    void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.gameObject.tag.Equals("NextLevel"))
        {
            levelIsFinished = true;
            // SceneManager.LoadScene("HellLevel");
            levelLoader.GetComponent<LevelLoader>().LoadNextLevel();
        }
        if(col.gameObject.tag.Equals("Finish"))
        {
            // TODO: Load Menu with new score
            levelLoader.GetComponent<LevelLoader>().LoadMenu();
            GlobalVars.CurrentGameState = GlobalVars.GameState.Win;
        }
     }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Obstacle"))
        {
            if(playerIsOnObstacle)
            {
                Debug.Log("Player exited obstacle");
                playerIsOnObstacle = false;
            }
        }
    }

    void movePlayer()
    {
        if(levelIsFinished)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, -movementSpeed);
        }
        else
        {
            float moveX = Input.GetAxis("Horizontal");
            rigidBody.velocity = new Vector2(movementSpeed + moveX * moveImpact, rigidBody.velocity.y);
        }
    }

    void checkSwitchLanes(KeyCode key)
    {
        if(key == KeyCode.UpArrow)
        {
            isUpper = true;
            switchLane(laneUp:isUpper);
        }
        else if(key == KeyCode.DownArrow)
        {
            isUpper = false;
            switchLane(laneUp:isUpper);
        }
    }

    void switchLane(bool laneUp)
    {
        isInSwitchMode = true;
        currentLane = laneSystem.GetComponent<LaneController>().SwitchLane(laneUp);
        HandleLaneCollision(currentLane.Name);
    }

    void HandleLaneCollision(string currentLayer)
    {
        spriteRenderer.sortingLayerName = currentLayer;
        if(currentLayer == "Lane1")
        {
            gameObject.layer = LayerMask.NameToLayer("Lane1");
            // Physics2D.IgnoreLayerCollision(0, 6, false);
            // Physics2D.IgnoreLayerCollision(0, 7, true);
            // Physics2D.IgnoreLayerCollision(0, 8, true);
        }
        else if(currentLayer == "Lane2")
        {
            gameObject.layer = LayerMask.NameToLayer("Lane2");
            // Physics2D.IgnoreLayerCollision(0, 6, true);
            // Physics2D.IgnoreLayerCollision(0, 7, false);
            // Physics2D.IgnoreLayerCollision(0, 8, true);
        }
        else if(currentLayer == "Lane3")
        {
            gameObject.layer = LayerMask.NameToLayer("Lane3");
            // Physics2D.IgnoreLayerCollision(0, 6, true);
            // Physics2D.IgnoreLayerCollision(0, 7, true);
            // Physics2D.IgnoreLayerCollision(0, 8, false);
        }
    }

    void doMoveToLane()
    {
        if((currentLane.LaneYPosition <= rigidBody.transform.position.y && isUpper) || (currentLane.LaneYPosition >= rigidBody.transform.position.y && !isUpper))
        {
            isInSwitchMode = false;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            rigidBody.transform.position = new Vector3(rigidBody.transform.position.x, currentLane.LaneYPosition, 0);

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
        // Initilize jump start
        isJumpingUp = true;
        isInJumpMode = true;
        jumpTime = 0;
        jumpStartPosition = rigidBody.transform.position.y;
    }

    void checkForCombi()
    {
        combiTime += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.X))
        {
            combiKeyList.Add(KeyCode.X);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            combiKeyList.Add(KeyCode.Space);
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            combiKeyList.Add(KeyCode.UpArrow);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            combiKeyList.Add(KeyCode.DownArrow);
        }

        if(combiTime > waitForCombi)
        {
            isInCombiMode = false;
            evaluatedCombi();
        }
    }

    void evaluatedCombi()
    {
        // When player is on a lane and not jumping already or 
        // player is on an obstacle (while in jumpmode because we are not on the lane itself) 
        // the player is able to jump
        if(!isInJumpMode | playerIsOnObstacle)
        {
            if(combiKeyList.Contains(KeyCode.Space))
            {
                checkJumpWithVelocity();
                Debug.Log(combiKeyList.Count);
                if(combiKeyList.Contains(KeyCode.X))
                {
                    Debug.Log("Combi!");
                    Debug.Log(combiKeyList.Count);
                }
            }
        }
        if(!isInSwitchMode)
        {
            if(combiKeyList.Contains(KeyCode.UpArrow))
            {
                checkSwitchLanes(KeyCode.UpArrow);
            }
            else if(combiKeyList.Contains(KeyCode.DownArrow))
            {
                checkSwitchLanes(KeyCode.DownArrow);
            }
        }
    }

    void doJump()
    {
        if((!Input.GetKey(KeyCode.Space) | jumpTime > buttonTime) && Mathf.Abs((Mathf.Abs(jumpStartPosition) - Mathf.Abs(rigidBody.transform.position.y))) >= minHeight && rigidBody.transform.position.y != currentLane.LaneYPosition)
        {
            isJumpingUp = false;
            rigidBody.gravityScale = jumpGravityUp;
            if(isInSwitchMode)
            {
                // Enable collisions again
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        if(isJumpingUp)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);
            jumpTime += Time.deltaTime;
            if(isInSwitchMode && GetComponent<BoxCollider2D>().enabled)
            {
                // Disable collisions
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        if((rigidBody.transform.position.y <= currentLane.LaneYPosition) && isInJumpMode && !isJumpingUp)
        {
            isInJumpMode = false;
            rigidBody.gravityScale = 0;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            rigidBody.transform.position = new Vector3(rigidBody.transform.position.x, currentLane.LaneYPosition, 0);
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
