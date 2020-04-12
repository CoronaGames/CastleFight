using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldLevelBannerScript : MonoBehaviour

        , IPointerEnterHandler
        , IPointerExitHandler
{
    public Button button;
    public GameObject partilceSystem;
    [SerializeField] int levelIndex = 0;
    [SerializeField] int startMoney = 0;
    [SerializeField] int maxUnits = 10;
    [SerializeField] Sprite[] starSprites;
    [SerializeField] SpriteRenderer starRenderer;
    [SerializeField] Sprite[] buttonSprites; // Index 0: PointerExited, Index 1: PointerEntered;
    [SerializeField] Image buttonImage;

    [Header("LevelSelectedPanelData")]
    [SerializeField] string mapName;
    [SerializeField] string mapDescription;
    [SerializeField] Sprite levelSprite;
    [SerializeField] float mainCameraSize = 12f;
   

    [Header("LevelData:")]
    [SerializeField] float[] scoreTimer;
    public bool destroyMainBase;
    public bool waveSurvival;
    public bool timeSurvival;

    // Start is called before the first frame update
    void Start()
    {
        SetStars();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)button).OnPointerEnter(eventData);

        if(partilceSystem != null)
        {
            partilceSystem.SetActive(true);
        }
        buttonImage.sprite = buttonSprites[1];
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)button).OnPointerExit(eventData);
    
        if (partilceSystem != null)
        {
            partilceSystem.SetActive(false);
        }
        buttonImage.sprite = buttonSprites[0];
    }

    public void ButtonClicked()
    {
        CastleFightGui.instance.ShowSelectedLevel(this);
    }

    public string GetLevelName()
    {
        return mapName;
    }

    public string GetLevelDescription()
    {
        return mapDescription;
    }

    public float GetCameraSize()
    {
        return mainCameraSize;
    }

    public Sprite GetLevelSprite()
    {
        return levelSprite;
    }

    public void SetStars()
    {
        int starSpriteIndex = MainData.instance.levelScore[levelIndex];
        if (starSpriteIndex <= starSprites.Length)
        {
            starRenderer.sprite = starSprites[starSpriteIndex];
        }
    }

    public float[] GetScoreTimer()
    {
        return scoreTimer;
    }

    public int GetLevelIndex()
    {
        return levelIndex;
    }

    public int GetStartMoney()
    {
        return startMoney;
    }

    public int GetMaxUnits()
    {
        return maxUnits;
    }
}
