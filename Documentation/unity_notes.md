## Screen Space & World Space
- using `Input.mousePosition` returns position in screen space. This leads to reacurring positions (world space is separated in camera widths).
- to get position in world space we need to transform the coordinates to world space with `camera.ScreenToWorldPoint(mousePosition)`

## Hitting an object by click
- Use raycast in 2D to check if mouse click hit an object in world using `Raycasthit2D().collider` object returned by `Physics2D.Raycast(Vec2 Pos, Vec2.zero Origin)`

## Update vs. FixedUpdate
- Update differs between devices, because different devices can play different amount of frames per second
- FixedUpdate has always the same amount of calls in a time unit. e.g. for movement depending on update should be managed in FixedUpdate.

## Move Player
- If for some reason we do not want a RigidBody2D attached (which is recommended when making use of collision detection and movement), the transform can be used to move the player `transform.position + moveSpeed * Time.deltaTime`.
- [x] Using a RigidBody2D we need to use its velocity instead of manipulating the transform directly (this would cause unpredictable behavior). `rigidBody2D.velocity = new Vector2(moveX, moveY) * movementSpeed;`. For this to work in a 2D game with top down (no ground collision object), we will need to set the gravity to 0 and set a constraint to rotation.

## Moving the Camera
- To move the camera we could manipulate the transforms position itself (like for a player without RigidBody), using direct Matrix Operations we may better use `transform.Translate(new Vector3(cameraSpeed, 0, 0) * Time.deltaTime)`

## Input Movement
- To get direct movement input independet of input device use: `Input.GetAxis("Horizontal")` and `Input.GetAxis("Vertical")` 

## Player Jump
- A jump with a rigidBody should be implemented using the AddForce Function `rigidBody.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse)`. Because the gravity is set to 0, we need to set it higher so the force (adding velocity) is reduced by gravity again. We need to detect when the jump is finished (the ground is hit (a specific y-position)) and need to set the y-Position to the ground (because checking just the y-position and turning off gravity will likely not lead to a precise positioning but can be below or above the aimed y-Position). <br>
This is a workaround - not sure if it will lead to problems..
- [x] We could also use the `rigidBody.velocity.y` directly to make a jump
- Furthermore we can use the transform.position.y to make a jump, but with a rigidBody attached this may lead to issues. In this case we would have to calculate our own gravity and so on.

For further information on implementing the different ways in unity, see: https://gamedevbeginner.com/how-to-jump-in-unity-with-or-without-physics/
