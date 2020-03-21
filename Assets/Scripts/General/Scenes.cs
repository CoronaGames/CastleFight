using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public static Scenes instance;
    [SerializeField] Camera main;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SceneToLoad(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SceneToLoad(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void SceneToLoad(string name, float cameraSize)
    {
        SetOrthographicSize(cameraSize);
        SceneToLoad(name);
    }

    public string GetSceneString()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void ReloadScene()
    {
        // Reset game
        CastleFightData.instance.ResetGame();
        SceneToLoad(GetSceneString());
    }

    public void SetOrthographicSize(float cameraSize)
    {
        main.orthographicSize = cameraSize;
    }
}
