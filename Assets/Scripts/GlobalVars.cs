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