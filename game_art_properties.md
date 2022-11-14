
Resolution:
- Target: 1920x1080
- Pixel Art Resolution Reduction (1920/3): 640x360
- Camera rotation buffer-space: 120
- Overall Pixelart Resoultion: 640x480

Level Composition:
- Split: 1/3 background (1px-120px) and 2/3 playground (121px-360px ~ 240px)
- Lanes: playground/4 => 60px per lane, floor off-set top and button each 30px

==> An obstacle can be of height 60px (1 lane) or 120px (2 lanes) or 180 (3 lanes)

==> Player is of height 60px-100px


Idea to implement Transition from street to hell:
- we use a transition backround that moves up with the same speed as the camera. It needs therefore to be twice the normal height (=> 480x2=960px).

