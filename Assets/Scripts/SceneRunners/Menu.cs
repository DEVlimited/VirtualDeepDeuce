using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject[] logos;
    public GameObject panel;
    public GameObject tutorialPanel;
    private Image devImage;

    private int lastScene;

    public GameObject bathAveButton;
    public GameObject archiveButton;

    public GameObject debugTextObject;
    public bool debugMode = false;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Hand Check Beginning");
        if (PlayerPrefs.GetString("Hand") == "")
        {
            PlayerPrefs.SetString("Hand", "Right");
            Debug.Log("Hand was null in PlayerPrefs. Setting as 'Right'");
        }
        //Debug.Log("Hand Check Complete");
        //Debug.Log("Beginning First Time Check");
        if(PlayerPrefs.GetString("Menu First Time") == "")
        {
            PlayerPrefs.SetString("Menu First Time", "true");
        }
        if (PlayerPrefs.GetString("Menu First Time") == "true")
        {
            Tutorial();
            PlayerPrefs.SetString("Menu First Time", "false");
        }
        //Debug.Log("First Time Check Complete");
        //Debug.Log("Debug Check Beginning");
        if(PlayerPrefs.GetString("Debug Mode") == "")
        {
            PlayerPrefs.SetString("Debug Mode", "off");
        }
        if(PlayerPrefs.GetString("Debug Mode") == "on")
        {
            PlayerPrefs.SetString("Debug Mode", "off");
        }
        //Debug.Log("Debug Check Complete");
        //Debug.Log("Logo Assignment Beginning");
        devImage = logos[1].GetComponent<Image>();
        //Debug.Log("Logo Assignment Complete");

       /*  if(PlayerPrefs.GetString("Last Scene") != "")
        {
            //unload previous scene
            Debug.Log("Unloading Previous Scene Beginning");
            lastScene = PlayerPrefs.GetInt("Last Scene Build Index");
            SceneManager.UnloadSceneAsync(lastScene);
            Debug.Log("Unloading Previous Scene Complete");
            Debug.Log("Deleting Build Index from PlayerPrefs Beginning");
            //delete record of last scene
            PlayerPrefs.DeleteKey("Last Scene Build Index");
        }
        else
        {
            Debug.Log("No previous scene to release.");    
        } */
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DebugModeSwitch()
    {
        if (!debugMode)
        {
            bathAveButton.SetActive(true);
            archiveButton.SetActive(true);
            debugTextObject.SetActive(true);
            debugMode = true;
            PlayerPrefs.SetString("Debug Mode", "on");

            devImage.color = Color.red;
        }
        else
        {
            bathAveButton.SetActive(false);
            archiveButton.SetActive(false);
            debugTextObject.SetActive(false);
            debugMode = false;
            PlayerPrefs.SetString("Debug Mode", "off");

            devImage.color = Color.white;
        }
    }
    public void SwitchScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void AppExit()
    {
        Application.Quit();
    }

    public void LogosOff()
    {
        for (int i = 0; i < logos.Length; i++)
        {
            logos[i].SetActive(false);
        }
    }

    public void Tutorial()
    {
        if (tutorialPanel.activeInHierarchy)
        {
            tutorialPanel.SetActive(false);
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
            tutorialPanel.SetActive(true);
        }
        }
    }


