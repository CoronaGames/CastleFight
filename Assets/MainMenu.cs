using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject[] panels;
    [SerializeField] Button continueButton;
    [SerializeField] bool isGameSaves = false;
    

    void Start()
    {
        CheckForGameSaves();
    }

    public void CheckForGameSaves()
    {
        ProgressData progressData = SaveSystem.LoadGameData();
        if(progressData != null)
        {
            isGameSaves = progressData.hasSaved;
        }
        else
        {
            isGameSaves = false;
        }
        continueButton.interactable = isGameSaves;
    }

    public void ButtonClicked(int index)
    {
        mainPanel.SetActive(false);
        for (int i=0; i<panels.Length; i++)
        {
            if (i == index)
            {
                panels[i].SetActive(true);
            }
            else
            {
                panels[i].SetActive(false);
            }
        }
    }

   

    public void ExitButtonClicked()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        mainPanel.SetActive(true);
        for(int i=0; i<panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Level Overview");
    }
}
