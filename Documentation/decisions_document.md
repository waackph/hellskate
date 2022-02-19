## Organization
- Weekly meeting (at least short online meeting to check status)

## Game Logic
- How to move in the world? <br>
Move camera and player instead of world in opposite direction, because we have finite levels and hence won't get floating point problems.
- How to implement the depth (player can move in y-direction)? <br>
Use lanes for now, because it seems to be easier to implement and also easier for player to understand which obstacles to dodge.
- Should we use physics (gravity) or implement own way of making a jump? <br>
We try to implement own simple physics because it seems to be much more complicated to use unity physics with way to much unneeded functionality.

## Art
- ...