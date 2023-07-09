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

    public GameObject bathAveButton;
    public GameObject archiveButton;

    public GameObject debugTextObject;
    public bool debugMode = false;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("Hand") == "")
        {
            PlayerPrefs.SetString("Hand", "Right");
            Debug.Log("Hand was null in PlayerPrefs. Setting as 'Right'");
        }
        if(PlayerPrefs.GetString("Menu First Time") == "")
        {
            PlayerPrefs.SetString("Menu First Time", "true");
        }
        if (PlayerPrefs.GetString("Menu First Time") == "true")
        {
            Tutorial();
            PlayerPrefs.SetString("Menu First Time", "false");
        }
        if(PlayerPrefs.GetString("Debug Mode") == "")
        {
            PlayerPrefs.SetString("Debug Mode", "off");
        }
        if(PlayerPrefs.GetString("Debug Mode") == "on")
        {
            DebugModeSwitch();
        }
        devImage = logos[1].GetComponent<Image>();
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


