using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Class <c>LevelLoader</c> handles loading scenes which are levels or menus.
/// It also plays an animation while switching scenes.</summary>
///
public class LevelLoader : MonoBehaviour
{

    public Animator transition;
    public float transitionSeconds = 1f;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("StartMenu");
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
