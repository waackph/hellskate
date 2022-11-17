/// <summary>Class <c>GlobalVars</c> stores globally accessible variables, such as the game state.</summary>
///

public static class GlobalVars
{
    public enum GameState 
    {
        Init,
        Win,
        Loose
    }

    public static GameState CurrentGameState { get; set; }
}