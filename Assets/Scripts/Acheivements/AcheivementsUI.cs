using UnityEngine;
using TMPro;

public class AchievementsUI : MonoBehaviour
{
    public TMP_Text achievementText;

    void Start()
    {
        UpdateAchievementText();
    }

    void OnEnable()
    {
        UpdateAchievementText();
    }

    void UpdateAchievementText()
    {
        bool allCrystalsCollected = DataPersistenceManager.loadInt("AllCrystalsCollected") == 1;
        achievementText.text = "All Crystals Collected: " + (allCrystalsCollected ? "Completed" : "Not Completed");
        Debug.Log("Achievement status displayed: All Crystals Collected - " + (allCrystalsCollected ? "Completed" : "Not Completed"));
    }
}
