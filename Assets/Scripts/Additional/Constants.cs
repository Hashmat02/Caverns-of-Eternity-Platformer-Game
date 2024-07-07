static public class Constants {
    // Scene Names
    public const string SCENE_MAIN_MENU = "MainMenu";
    public const string SCENE_LEVEL_SELECT = "LevelSelect"; // only available in dev mode
    public const string SCENE_GAMEPLAY = "Level1";
    public const string SCENE_SHOP = "ShopScene";
    public const string SCENE_INVENTORY = "InventoryScene";
    public const string SCENE_ACHIEVEMENTS = "Achievement";


    // Player Prefs
    public const int NUMBER_OF_LEVELS = 1;
    public const string PREF_LEVEL_NUMBER = "LevelNumber";
    public const string PREF_LEVEL_STATES = "LevelStates";
    public const string PREF_CRYSTALS = "Crystals";
	public const string PREF_POWERUPS_SPEED = "SpeedBoost";
	public const string PREF_POWERUPS_SHIELD = "Shield";
	public const string PREF_POWERUPS_REBIRTH = "CrystalRebirth";
	public const string PREF_POWERUPS_JUMP = "DoubleJump";
	public const string PREF_POWERUPS_RIDDLE = "RiddleGuide";

	// Audio Mixer
	public const int MIXER_GROUP_COUNT = 3;
	public const string MIXER_MASTER = "MasterMixer";
	public const string MIXER_MUSIC = "MusicMixer";
	public const string MIXER_SFX = "SFXMixer";
	public const string MIXER_MUTED_ALL = "AllMixerMuted";

    // Object Tags
	public const string TAG_PLAYER = "Player";
	public const string TAG_GROUND = "Ground";
	public const string TAG_TRAP_KILLER = "KillerTrap";
	public const string TAG_COLLECTIBLE_CRYSTAL = "CollectibleCrystal";

    // Project Layers
    public const int LAYER_DEFAULT = 0;
    
    // Crystals
    public const int CRYSTALS_VALUE_EACH = 5; // amount of crystals added per coin picked up

    // Player Movement Parameters
	public const float DEF_GRAVITY_SCALE = 1.0f;
    public const float PLAYER_GRAVITY = 10.0f; // acceleration

	public const float PLAYER_POSITION_SAVE_TIMER = 5.0f; // seconds
	public const float PLAYER_INVINCIBILITY_TIME = 3.0f; // seconds

	// Powerupds
	public const float POWERUPS_SPEED_DURATION = 10.0f; // seconds;
}
