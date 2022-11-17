using UnityEngine;

/// <summary>Class <c>Parallax</c> moves a background sprite by a given parameter value for parallex effect.
/// It furthermore moves part of the background sprite to the end, before the camera leaves the sprite.
/// It also replaces the ending part of the sprite 
/// by a alternative (for ending) after a given amount of iterations</summary>
///
public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public GameObject secondPartSprite;
    public GameObject endingSprite;
    public float parallaxEffect;
    public int amountIterations;
    private int iterations;


    // Start is called before the first frame update
    void Start()
    {
        iterations = 0;
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(iterations <= amountIterations)
        {
            if(iterations == amountIterations)
            {
                ChangeSpritesToLevelEnding();
            }
            float temp = (cam.transform.position.x * (1 - parallaxEffect));
            float dist = (cam.transform.position.x * parallaxEffect);

            transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

            if (temp > startpos + length)
            {
                startpos += length;
                iterations += 1;
            }
            else if (temp < startpos - length) 
            {
                startpos -= length;
            }
        }
    }

    // Replacing second part of background sprite by ending sprite
    void ChangeSpritesToLevelEnding()
    {
        secondPartSprite.SetActive(false);
        endingSprite.SetActive(true);
    }
}
