using UnityEngine;
using UnityEngine.UI;

/// <summary>Class <c>MenuController</c> handles loading the menu, given the game state.
/// It furthermore handles the execution of menu button events.</summary>
///
public class MenuController : MonoBehaviour
{
    public Button StartButton;
    public Button QuitButton;
    [SerializeField] GameObject levelLoader;
    [SerializeField] GameObject gameStateText;

    // Start is called before the first frame update
    void Start()
    {
        StartButton.GetComponent<Button>().onClick.AddListener(StartGame);
        QuitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
        SetGameStateText();
    }

    void SetGameStateText()
    {
        Debug.Log(GlobalVars.CurrentGameState);
        if(GlobalVars.CurrentGameState == GlobalVars.GameState.Win)
        {
            gameStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "You won!";
        }
        else if(GlobalVars.CurrentGameState == GlobalVars.GameState.Loose)
        {
            gameStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "You lost!";
        }
        else
        {
            gameStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
    }

    void StartGame()
    {
        levelLoader.GetComponent<LevelLoader>().LoadNextLevel();
        GlobalVars.CurrentGameState = GlobalVars.GameState.Init;
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
