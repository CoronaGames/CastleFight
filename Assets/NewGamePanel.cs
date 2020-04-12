using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGamePanel : MonoBehaviour
{
    [SerializeField] Button startGameButton;
    [SerializeField] InputField sessionInputField;
    [SerializeField] Text feedBackText;
    // Start is called before the first frame update
    void Start()
    {
        startGameButton.interactable = false;
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckInputString()
    {
        string text = sessionInputField.text;
        if(text.Length >= 3 && text.Length < 20)
        {
            feedBackText.text = "";
            startGameButton.interactable = true;
        }
        else if (text.Length >= 20)
        {
            feedBackText.text = "Session name can't be longer than 20 characters";
            startGameButton.interactable = false;
        }
        else
        {
            feedBackText.text = "You need at least 3 characters";
            startGameButton.interactable = false;
        }
    }

    public void StartGame()
    {
        string session = sessionInputField.text;
        PlayerPrefs.SetString("currentSession", session);
        SceneManager.LoadScene("Level Overview");
    }
}
