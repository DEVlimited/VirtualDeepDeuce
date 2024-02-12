using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class UISandbox : MonoBehaviour
{
    private int currentButtonStyle = 0;
    private int currentBackgroundStyle = 0;
    public GameObject[] buttons;
    public GameObject[] backgrounds;
    private GameObject currentButton;
    private GameObject currentBackground;
    public TextMeshProUGUI bgButtonText;

    // Start is called before the first frame update
    void Start()
    {
        currentButton = buttons[currentButtonStyle];
        currentBackground = backgrounds[currentBackgroundStyle];
        currentButton.SetActive(true);
        currentBackground.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonStyleSwitch()
    {
        //turn off current button
        currentButton.SetActive(false);
        //set next button int
        if(currentButtonStyle >= buttons.Length - 1)
        {
            currentButtonStyle = 0;
        }
        else
        {
            currentButtonStyle += 1;
        }
        
        //swap out button
        currentButton = buttons[currentButtonStyle];
        //turn on button
        currentButton.SetActive(true);
        //Debug.Log("Current Button is " + currentButtonStyle + ". Switch called.");
    }

    public void BackgroundStyleSwitch()
    {
        //turn off current background
        currentBackground.SetActive(false);
        //set next background int
        if(currentBackgroundStyle >= backgrounds.Length - 1)
        {
            currentBackgroundStyle = 0;
        }
        else
        {
            currentBackgroundStyle += 1;
        }
        
        //swap out background
        currentBackground = backgrounds[currentBackgroundStyle];
        //turn on background
        currentBackground.SetActive(true);
        bgButtonText.text = "Current background is " + backgrounds[currentBackgroundStyle].name;
        //Debug.Log("Current background is " + currentBackgroundStyle + ". Switch called.");
    }

    public void SceneSwitch(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
