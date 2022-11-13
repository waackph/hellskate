using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;
    public float transitionSeconds = 1f;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // Start animation
        transition.SetTrigger("Start");

        // Wait some seconds
        yield return new WaitForSeconds(transitionSeconds);

        // Switch level
        SceneManager.LoadScene(levelIndex);
    }
}
