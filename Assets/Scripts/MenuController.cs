using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button StartButton;
    public Button QuitButton;
    [SerializeField] GameObject levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        StartButton.GetComponent<Button>().onClick.AddListener(StartGame);
        QuitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    void StartGame()
    {
        levelLoader.GetComponent<LevelLoader>().LoadNextLevel();
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
