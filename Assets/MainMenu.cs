using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject[] panels;
    

    void Start()
    {
        
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


}
