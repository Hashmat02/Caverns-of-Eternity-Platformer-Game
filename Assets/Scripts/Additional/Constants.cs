static public class Constants {
    // Scene Names
    public const string SCENE_MAIN_MENU = "MainMenu";
    public const string SCENE_LEVEL_SELECT = "LevelSelect"; // only available in dev mode
    public const string SCENE_GAMEPLAY = "Gameplay";

    // Player Prefs
    public const int NUMBER_OF_LEVELS = 1;
    public const string PREF_LEVEL_NUMBER = "LevelNumber";
    public const string PREF_LEVEL_STATES = "LevelStates";
    public const string PREF_CRYSTALS = "Crystals";

    // Object Tags
    public const string TAG_PLAYER = "Player";
	public const string TAG_GROUND = "Ground";

    // Project Layers
    public const int LAYER_DEFAULT = 0;
    
    // Crystals
    public const int CRYSTALS_VALUE_EACH = 5; // amount of crystals added per coin picked up

    // Player Movement Speeds
    public const float PLAYER_GRAVITY = 10.0f; // acceleration
}
