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
    

    [Header("PlayerData:")]
    public int playerMoney = 0;
    [SerializeField] int startMoney = 200;
    [SerializeField] Text goldValueText;
    [SerializeField] int maxUnits = 10;
    [SerializeField] int currentUnits = 0;
    [SerializeField] bool maxUnitsReached = false;
    [SerializeField] bool pauseSpawningUnits = false;
    [SerializeField] LinkedList<Health> activeUnitsPlayer;
    [SerializeField] LinkedList<Health> activeUnitsEnemy;   // Used for global spells etc;
    public int[] amountOfUnitTypes; // Index 0 = Archer, Index 1 = Soldier, Index 2 = Wizard;
    

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
        activeUnitsPlayer = new LinkedList<Health>();
        activeUnitsEnemy = new LinkedList<Health>();
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
        gameInitiated = false;
    }

    public void GameWon()
    {
        UpdateMainData();
        CastleFightGui.instance.GameWon(timeInitiated, MainData.instance.levelScore[currentLevelIndex]);
        gameInitiated = false;
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
        timeInitiated = 0f;
        currentUnits = 0;
        timeInitiated = 0f;
        SetMoney(startMoney);
        pauseSpawningUnits = false;
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
        SetMaxUints(level.GetMaxUnits());


    }

    private void SetMaxUints(int maxUnits)
    {
        this.maxUnits = maxUnits;
    }

    public int GetMaxUnits()
    {
        return maxUnits;
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

    public void AddPlayerUnitCount()
    {
        currentUnits++;
        if(currentUnits >= maxUnits)
        {
            SetMaxUintsReached(true);
        }
        SetUnitsText();
    }

    public void RemoveOnePlayerUnitCount()
    {
        currentUnits--;
        if (currentUnits < maxUnits)
        {
            SetMaxUintsReached(false);
        }
        SetUnitsText();
    }

    private void SetUnitsText()
    {
        CastleFightGui.instance.SetNumberOfUnitsText(currentUnits + "/" + maxUnits);
    }

    public bool IsMaxUnitsReached()
    {
        return maxUnitsReached;
    }

    private void SetMaxUintsReached(bool value)
    {
        maxUnitsReached = value;
    }

    public void AddSpawnedUnit(Health unit)
    {
        if(unit.GetComponent<TeamData>().GetTeamBelonging() == Team.TeamRed)
        {
            activeUnitsPlayer.AddFirst(unit);
            UpdateUnitManager(unit, false);
        }
        else
        {
            activeUnitsEnemy.AddFirst(unit);
        }
    }

    public void RemoveUnit(Health unit)
    {
        if (unit.GetComponent<TeamData>().GetTeamBelonging() == Team.TeamRed)
        {
            if (activeUnitsPlayer.Contains(unit))
            {
                activeUnitsPlayer.Remove(unit);
                UpdateUnitManager(unit, true);
            }

        }
        else
        {
            if (activeUnitsEnemy.Contains(unit))
            {
                activeUnitsEnemy.Remove(unit);
            }
        }
    }

    // Used for dwellings to keep count of how many units are alive of each unit type;
    private void UpdateUnitManager(Health unit, bool isRemoving)    // Bool true if you should remove
    {
        if(unit.GetName() == "Archer")
        {
            if (isRemoving) amountOfUnitTypes[0]--;
            else amountOfUnitTypes[0]++;
        }
        else if(unit.GetName() == "Soldier")
        {
            if (isRemoving) amountOfUnitTypes[1]--;
            else amountOfUnitTypes[1]++;
        }
        else if(unit.GetName() == "Wizard")
        {
            if (isRemoving) amountOfUnitTypes[2]--;
            else amountOfUnitTypes[2]++;
        }
    }

    public int GetAmountOfUnit(int unitTypeIndex)
    {
        return amountOfUnitTypes[unitTypeIndex];
    }

    public bool IsPauseSpawningUnits()
    {
        return pauseSpawningUnits;
    }

    public void SetPauseSpawningUnits(bool value)
    {
        pauseSpawningUnits = value;
    }
}
