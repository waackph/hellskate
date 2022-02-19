## Screen Space & World Space
- using `Input.mousePosition` returns position in screen space. This leads to reacurring positions (world space is separated in camera widths).
- to get position in world space we need to transform the coordinates to world space with `camera.ScreenToWorldPoint(mousePosition)`

## Hitting an object by click
- Use raycast in 2D to check if mouse click hit an object in world using `Raycasthit2D().collider` object returned by `Physics2D.Raycast(Vec2 Pos, Vec2.zero Origin)`

## Update vs. FixedUpdate
- Update differs between devices, because different devices can play different amount of frames per second
- FixedUpdate has always the same amount of calls in a time unit. e.g. for movement depending on update should be manageed in FixedUpdate.

