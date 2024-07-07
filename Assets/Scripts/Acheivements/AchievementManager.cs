// AchievementManager.cs
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    private Dictionary<string, bool> achievements = new Dictionary<string, bool>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAchievements();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetAchievement(string achievementId, bool achieved)
    {
        if (achievements.ContainsKey(achievementId))
        {
            achievements[achievementId] = achieved;
        }
        else
        {
            achievements.Add(achievementId, achieved);
        }
    }

    public bool GetAchievement(string achievementId)
    {
        if (achievements.ContainsKey(achievementId))
        {
            return achievements[achievementId];
        }
        return false;
    }

    public void SaveAchievements()
    {
        foreach (var achievement in achievements)
        {
            PlayerPrefs.SetInt(achievement.Key, achievement.Value ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void LoadAchievements()
    {
        foreach (var achievement in achievements.Keys)
        {
            achievements[achievement] = PlayerPrefs.GetInt(achievement, 0) == 1;
        }
    }
}
