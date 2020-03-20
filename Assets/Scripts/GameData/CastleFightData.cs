using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using UnityEngine.UI;

public class CastleFightData : MonoBehaviour
{
    // Game Mode Gameloop
    // Current level data

    public static CastleFightData instance;
    public bool gameInitiated = false;
    public int playerMoney = 0;
    [SerializeField] int startMoney = 200;
    [SerializeField] Text goldValueText;
    [SerializeField] bool gameWon = false;

    [Header("CurrentLevelData:")]
    [SerializeField] int currentLevelIndex;
    [SerializeField] float[] highScoreValues;
    [SerializeField] float timeInitiated = 0f;

    // Game mode bools/win conditions: DestroyMainBase, waveSurvival(Survive x amount of waves), timeSurvival(Survive x amount of time)?;
    [Header("Game Mode: ")]
    [SerializeField] bool destroyMainBase;
    [SerializeField] bool waveSurvival;
    [SerializeField] bool timeSurvival;

    [Header("Wave Survival:")]
    [SerializeField] int totalSpawners;
    [SerializeField] int finishedSpawners = 0;

    [Header("Time Survival:")]
    [SerializeField] float timeToSurviveInSeconds;


    [SerializeField] List<Attacker> remainingEnemies = new List<Attacker>();
    [SerializeField] bool waitForRemainingEnemies;



    void Start()
    {
        Physics2D.callbacksOnDisable = true;
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        goldValueText.text = playerMoney.ToString();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void LateUpdate()
    {
      
        if (waitForRemainingEnemies)
        {
            if(remainingEnemies.Count <= 0)
            {
                GameWon();
            }
            else
            {
                for(int i=0; i<remainingEnemies.Count; i++)
                {
                    if (remainingEnemies[i].GetComponent<Health>())
                    {
                        if (remainingEnemies[i].GetComponent<Health>() != null && remainingEnemies[i].GetComponent<Health>().IsDead())
                        {
                            remainingEnemies.Remove(remainingEnemies[i]);
                        }
                    }
                }
            }
        }

        else if (gameInitiated)
        {
          
            if (destroyMainBase)
            {

            }
            else if (waveSurvival)
            {
                if (finishedSpawners >= totalSpawners)
                {
                    CheckForRemainingEnemies();
                    //GameWon();
                }
            }
            else if (timeSurvival)
            {
                if (timeToSurviveInSeconds > timeInitiated)
                {
                    GameWon();
                }
            }
        }

        if (!gameInitiated) return;
        if(gameInitiated || waitForRemainingEnemies)
        {
            timeInitiated += Time.deltaTime;
            CastleFightGui.instance.SetTimeUsedText(timeInitiated);
        }
    }

    private void CheckForRemainingEnemies()
    {
        Attacker[] remainingUnits = FindObjectsOfType<Attacker>();
        for(int i=0; i < remainingUnits.Length; i++)
        {
            if(remainingUnits[i].GetComponent<TeamData>().GetTeamBelonging() != Team.TeamRed)
            {
                remainingEnemies.Add(remainingUnits[i]);
            }
        }

        if(remainingEnemies.Count > 0)
        {
            waitForRemainingEnemies = true;
        }
        else
        {
            GameWon();
        }
    }

    public void BaseDestroyed(MainBase mainBase)
    {
       
        if(gameInitiated && mainBase.GetComponent<TeamData>().GetTeamBelonging() == Team.TeamRed)
        {
            // Game Lost
            GameLost();
        }
        else if (gameInitiated  && mainBase.GetComponent<TeamData>().GetTeamBelonging() == Team.TeamGreen)
        {
            GameWon();
        }
        

        // Game over
    }

    public void GameLost()
    {
        CastleFightGui.instance.GameLost();
        UpdateMainData();
        gameWon = false;
        gameInitiated = false;
    }

    public void GameWon()
    {
        UpdateMainData();
        CastleFightGui.instance.GameWon(timeInitiated, MainData.instance.levelScore[currentLevelIndex]);
        gameInitiated = false;
        gameWon = true;
    }


    public void StartGame()
    {
        gameInitiated = true;
    }

    public void SetMoney(int moneyToAdd)
    {
        playerMoney = moneyToAdd;
        goldValueText.text = playerMoney.ToString();
    }

    public void SetStartMoney(int startMoney)
    {
        this.startMoney = startMoney;
        SetMoney(startMoney);
    }

    public void AddMoney(int moneyToAdd)
    {
        playerMoney += moneyToAdd;
        goldValueText.text = playerMoney.ToString();
    }

    public bool ReduceMoney(int moneyToReduce)
    {
        // Return true if transaction successful;
        if(moneyToReduce <= playerMoney)
        {
            playerMoney -= moneyToReduce;
            goldValueText.text = playerMoney.ToString();
            return true;
        }
        else
        {
           
            return false;
        }
    }

    public void ResetGame()
    {
        CastleFightGui.instance.ResetGame();
        totalSpawners = 0;
        gameInitiated = false;
        gameWon = false;
        timeInitiated = 0f;
        SetMoney(startMoney);
    }

    public void UpdateMainData()
    {
        MainData.instance.levelScore[currentLevelIndex] = CalculateScore();
        MainData.instance.levelTimer[currentLevelIndex] = timeInitiated;
        MainData.instance.UpdateTotalStars();
    }

    private int CalculateScore()
    {
        int points = 3;
        if (highScoreValues == null) return 0;
        for(int i=0; i< highScoreValues.Length; i++)
        {
            if (i > 2) break;
            if(timeInitiated < highScoreValues[i])
            {
                return points;
            }
            else
            {
                points--;
            }
        }
        return 1;
    }

    public void SetCurrentLevel(WorldLevelBannerScript level)
    {
        highScoreValues = level.GetScoreTimer();
        currentLevelIndex = level.GetLevelIndex();
        destroyMainBase = level.destroyMainBase;
        waveSurvival = level.waveSurvival;
        timeSurvival = level.timeSurvival;
        SetStartMoney(level.GetStartMoney());

    }

    public void AddSpawner()
    {
        totalSpawners++;
    }

    public void SpawnerFinished()
    {
        finishedSpawners++;
    }

    public void SetCurrentWave(int value)
    {
        finishedSpawners = value;
    }

    public void SetGameModeWaveSurvival()
    {
        waveSurvival = true;
        destroyMainBase = false;
        timeSurvival = false;
    }
}
