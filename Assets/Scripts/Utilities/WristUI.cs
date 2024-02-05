using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class WristUI : MonoBehaviour
{
    /// <summary>
    /// The parent game object that contain all of the UI menus, attached to the left wrist.
    /// </summary>
    public GameObject UIInterface;
    public InputActionReference MenuPressed;
    public XRInteractorLineVisual leftHandLine;
    public XRInteractorLineVisual rightHandLine;

    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject imagePanelL;
    public GameObject imagePanelR;

    public GameObject tutorialPanelL;
    public GameObject tutorialPanelR;

    public TextMeshProUGUI textL;
    public TextMeshProUGUI textR;

    public SRTParser parser;

    // Start is called before the first frame update
    void Start()
    {
        //if the UI is disable before the scene starts, Start() is never called and 
        //OnMenuPressed is never subscribed. Subscribe first, then hide the UI.
        MenuPressed.action.started += OnMenuPressed;
        //WristUI.SetActive(true);

        if (PlayerPrefs.GetString("Hand") == "Right")
        {
            leftHand.SetActive(false);
            rightHand.SetActive(true);
            FindObjectOfType<SubtitleDisplayer>().Text = textR;
            FindObjectOfType<SubtitleDisplayer>().Text2 = textR; 
        }
        else
        {
            leftHand.SetActive(true);
            rightHand.SetActive(false);
            FindObjectOfType<SubtitleDisplayer>().Text = textL;
            FindObjectOfType<SubtitleDisplayer>().Text2 = textL; 
        }
        //Debug.Log("Hand is " + onRight.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SwitchHands()
    {
        if (PlayerPrefs.GetString("Hand") == "Right")
        {
            textL.text = FindAnyObjectByType<SubtitleDisplayer>().Text.text;
            //Debug.Log("Switching from right to left");
            leftHand.SetActive(true);
            rightHand.SetActive(false);
            PlayerPrefs.SetString("Hand", "Left");
            FindObjectOfType<SubtitleDisplayer>().Text = textL;
            FindObjectOfType<SubtitleDisplayer>().Text2 = textL; 

        }
        else
        {
            textR.text = FindAnyObjectByType<SubtitleDisplayer>().Text.text;
            //Debug.Log("Switching from left to right");
            leftHand.SetActive(false);
            rightHand.SetActive(true);
            PlayerPrefs.SetString("Hand", "Right");
            FindObjectOfType<SubtitleDisplayer>().Text = textR;
            FindObjectOfType<SubtitleDisplayer>().Text2 = textR; 

            
        } 

    }

    private void OnMenuPressed(InputAction.CallbackContext obj)
    {
        //toggle the menu off/on with the menu button
        bool enable = !UIInterface.activeInHierarchy;
        UIInterface.SetActive(enable);
        leftHandLine.enabled = !enable;
        Debug.Log("Menu");
    }

    public void Tutorial()
    {
        if (PlayerPrefs.GetString("Hand") == "Right")
        {
            if (tutorialPanelR.activeInHierarchy)
            {
                tutorialPanelR.SetActive(false);
            }
            else
            {
                tutorialPanelR.SetActive(true);
            }
        }
        if (PlayerPrefs.GetString("Hand") == "Left")
        {
            if (tutorialPanelL.activeInHierarchy)
            {
                tutorialPanelL.SetActive(false);
            }
            else
            {
                tutorialPanelL.SetActive(true);
            }
        }
    }

}
