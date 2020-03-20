using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainData : MonoBehaviour
{
    public static MainData instance;
    // Main data class containing essential variables needed to save game

    public int[] levelScore;  // List of levelstars/score. Array index number represent level number, and index value represent number of stars/points from 0 - 3;
    public float[] levelTimer;
    public string playerName;
    public int totalStars = 0;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    void Update()
    {
        
    }

    public void UpdateTotalStars()  // Calculates Player stars total
    {
        int score = 0;
        for(int i=0; i<levelScore.Length; i++)
        {
            score += levelScore[i];
        }
        totalStars = score;
    }

    public void SaveData()
    {
        SaveSystem.SaveGameData(this);
    }

    public void LoadData()
    {
        ProgressData progressData = SaveSystem.LoadGameData();

        levelScore = progressData.levelScore;
        playerName = progressData.playerName;
        UpdateTotalStars();
    }
}
