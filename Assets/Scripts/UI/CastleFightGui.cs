using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleFightGui : MonoBehaviour
{
    public static CastleFightGui instance;

    [SerializeField] Text infoPopupText;
    float popUpTimerCurrent;
    [SerializeField] float popUpTimer = 2f;
    bool timerActive;

    [Header("In-Game Data:")]
    [SerializeField] GameObject topPanel;
    [SerializeField] GameObject inGamePanel;
    [SerializeField] GameObject inGameMenu;
    [SerializeField] GameObject backgroundOverlay;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject gameWonPanel;
    [SerializeField] GameObject startButton;
    [SerializeField] Text timeUsedText;
    [SerializeField] Text unitsText;

    [Header("World Overview Data:")]
    [SerializeField] GameObject worldOverviewPanel;
    [SerializeField] GameObject levelSelectedPanel;
    [SerializeField] GameObject upgradesPanel;
    [SerializeField] GameObject unitUpgradesPanel;
    [SerializeField] GameObject bountyStorePanel;
    [SerializeField] Text levelHeader;
    [SerializeField] Text levelDescription;
    [SerializeField] Image levelImage;
    [SerializeField] Text starsText;
    [SerializeField] Text bountyText;
    [SerializeField] Text playerNameText;

    [Header("Main Menu Data:")]
    [SerializeField] GameObject menuPanel;

    string selectedScene;
    [SerializeField] WorldLevelBannerScript worldLevel;

    // Start is called before the first frame update
    void Start()
    {
        
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if(infoPopupText != null) infoPopupText.enabled = false;

        SetPlayerName();
        SetStarsText();
        SetBountyText();
        /*
        if(Scenes.instance.GetSceneString() == "LevelOverview")
        {
            worldOverviewPanel.SetActive(true);
            inGamePanel.SetActive(false);
        }
        */
    }

    public void SetPlayerName()
    {
        string playerName = PlayerPrefs.GetString("currentSession");
        playerNameText.text = "Playing As: " + playerName;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            InfoTextTimer();
        }
    }

    public void SetInfoText(string infoText)
    {
        infoPopupText.enabled = true;
        infoPopupText.text = infoText;
        ResetTimer();
    }

    private void ResetTimer()
    {
        popUpTimerCurrent = 0f;
        timerActive = true;
    }

    private void InfoTextTimer()
    {
        if(popUpTimerCurrent < popUpTimer)
        {
            popUpTimerCurrent += Time.deltaTime;
        }
        else
        {
            infoPopupText.enabled = false;
            timerActive = false;
        }
    }

    public void ShowSelectedLevel(WorldLevelBannerScript worldLevel)
    {
        
        worldOverviewPanel.SetActive(true);
        levelSelectedPanel.SetActive(true);
        levelHeader.text = worldLevel.GetLevelName();
        this.levelDescription.text = worldLevel.GetLevelDescription();
        levelImage.sprite = worldLevel.GetLevelSprite();
        selectedScene = worldLevel.GetLevelName();
        this.worldLevel = worldLevel;

    }

    public void ReloadScene()
    {
        Scenes.instance.ReloadScene();
    }

    public void HideSelectedLevel()
    {
        levelSelectedPanel.SetActive(false);
    }

    public void EnterSelectedLevel()
    {
        levelSelectedPanel.SetActive(false);
        worldOverviewPanel.SetActive(false);
        inGamePanel.SetActive(true);
        CastleFightData.instance.ResetGame();
        CastleFightData.instance.SetCurrentLevel(worldLevel);
        Scenes.instance.SceneToLoad(selectedScene, worldLevel.GetCameraSize());
    }

    public void ShowGameMenu(bool value)
    {
        inGameMenu.SetActive(value);
    }



    public void LoadWorldLevel()
    {
        inGameMenu.SetActive(false);
        inGamePanel.SetActive(false);
        menuPanel.SetActive(false);
        worldOverviewPanel.SetActive(true);
        levelSelectedPanel.SetActive(false);
        starsText.gameObject.SetActive(true);
        bountyText.gameObject.SetActive(true);
        SetStarsText();
        SetBountyText();
        Scenes.instance.SceneToLoad("Level Overview", 21.4f);
        
    }

    public void LoadMainMenuScene()
    {
        inGameMenu.SetActive(false);
        inGamePanel.SetActive(false);
        menuPanel.SetActive(true);
        worldOverviewPanel.SetActive(false);
        Scenes.instance.SceneToLoad("Main Menu", 13.46f);
    }

    public void ExitGame()
    {
        
    }

    public void GameLost(int bountyToAdd, int bountyCurrent)
    {
        backgroundOverlay.SetActive(true);
        gameOverPanel.SetActive(true);
        topPanel.SetActive(false);

    }

    public void GameWon(float roundTime, int score, int bountyToAdd, int bountyCurrent)
    {
        topPanel.SetActive(false);
        backgroundOverlay.SetActive(true);
        gameWonPanel.SetActive(true);
        Debug.Log("Round Time: " + roundTime);
        Debug.Log("Score: " + score);
    }

    public void ResetGame()
    {
        startButton.SetActive(true);
        backgroundOverlay.SetActive(false);
        gameWonPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        topPanel.SetActive(true);
    }

    public void SetStarsText()
    {
        starsText.text = MainData.instance.totalStars.ToString();
    }

    public void SetBountyText()
    {
        bountyText.text = MainData.instance.totalBounty.ToString();
    }

    public void SetTimeUsedText(float time)
    {
        int minutes;
        int seconds;

        if (time <= 59)
        {
            minutes = 0;
            seconds = Mathf.RoundToInt(time);
        }
        else
        {
            minutes = Mathf.RoundToInt(time / 60);
            seconds = Mathf.RoundToInt(time % 60);
        }

        if(seconds < 10)
        {
            timeUsedText.text = minutes.ToString() + ":0" + seconds.ToString();
        }
        else
        {
            timeUsedText.text = minutes.ToString() + ":" + seconds.ToString();
        }
    }

    public void GlobalUpgradesPanelButton()
    {
        upgradesPanel.SetActive(!upgradesPanel.activeSelf);
    }

    public void UnitUpgradesPanelButton()
    {
        unitUpgradesPanel.SetActive(!unitUpgradesPanel.activeSelf);
    }

    public void BountyStorePanelButton()
    {
        bountyStorePanel.SetActive(!bountyStorePanel.activeSelf);
    }

    public void SetNumberOfUnitsText(string numberOfUnits)
    {
        unitsText.text = numberOfUnits;
    }
}
