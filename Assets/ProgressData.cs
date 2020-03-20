using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class ProgressData
{
    // Data to save/Serialize
    public int[] levelScore;  // List of levelstars/score. Array index number represent level number, and index value represent number of stars/points from 0 - 3;
    public float[] levelTimer;
    public string playerName;
    public int totalStars = 0;

    public ProgressData(MainData mainData)
    {
        // TODO:

        levelScore = mainData.levelScore;
        levelTimer = mainData.levelTimer;
        playerName = mainData.playerName;
        totalStars = mainData.totalStars;

    
    }
}
