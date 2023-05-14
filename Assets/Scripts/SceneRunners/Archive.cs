using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Archive : MonoBehaviour
{
    private WristUI wristUI;
    private string activeHand;
    private bool menuOn = true;
    public GameObject wristButtonsL;
    public GameObject wristButtonsR;

    public GameObject archiveMenu;
    public GameObject backButton;
    public GameObject nextButton;
    public GameObject previousButton;
    public GameObject rightHand;
    public GameObject leftHand;
    

    [Header("Artifacts")]
    public GameObject aerialViews;
    public GameObject SanfordRichardsonArtifact;
    public GameObject SanfordObitL;
    public GameObject SanfordObitR;
    public GameObject blackDispatchClippingsContainer;
    public GameObject[] blackDispatchClippings;
    public int currentClipping = 0;
    [Header("Oral Histories")]
    public GameObject oralHistoryMenu;
    public GameObject anitaArnold;
    public GameObject[] anitaArnoldButtons;

    // Start is called before the first frame update
    void Start()
    {
        activeHand = PlayerPrefs.GetString("Hand");
        if(activeHand == "Right")
        {
            wristUI = rightHand.GetComponent<WristUI>();
        }
        else
        {
            wristUI = leftHand.GetComponent<WristUI>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSwitchHands()
    {
        activeHand = PlayerPrefs.GetString("Hand");
        if (activeHand == "Right")
        {
            wristUI = rightHand.GetComponent<WristUI>();
        }
        if(activeHand == "Left")
        {
            wristUI = leftHand.GetComponent<WristUI>();
        }
    }

    public void LoadArtifact(string artifact)
    {
        MenuSwitch();
        activeHand = PlayerPrefs.GetString("Hand");
        if (artifact == "Aerial Views")
        {
            aerialViews.SetActive(true);
        }
        if (artifact == "SanfordRichardson")
        {
            SanfordRichardsonArtifact.SetActive(true);
            if(activeHand == "Right")
            {
                SanfordObitR.SetActive(true);
                wristButtonsR.SetActive(false);
            }
            if(activeHand == "Left")
            {
                SanfordObitL.SetActive(true);
                wristButtonsL.SetActive(false);
            }
        }
        if(artifact == "BlackDispatch")
        {
            currentClipping = 0;
            blackDispatchClippingsContainer.SetActive(true);
            previousButton.SetActive(true);
            nextButton.SetActive(true);
        }
        if(artifact == "AnitaArnold")
        {
            anitaArnold.SetActive(true);
            anitaArnoldButtons[1].SetActive(true);
            anitaArnoldButtons[0].SetActive(true);
        }
        if (artifact == "AvisMelvaFranklin")
        {

        }
        if (artifact == "EloiseCarbajal")
        {

        }
        if (artifact == "GeorgeRichardson")
        {

        }
        if (artifact == "HobertSutton")
        {

        }
        if (artifact == "HurettaWalker")
        {

        }
        if (artifact == "JWSanford")
        {

        }
        if (artifact == "SandraRichards")
        {

        }
        if (artifact == "AvisMelvaFranklin")
        {

        }
    }

    public void ArtifactSwitch()
    {
        aerialViews.SetActive(false);
        SanfordRichardsonArtifact.SetActive(false);
        blackDispatchClippingsContainer.SetActive(false);
        anitaArnold.SetActive(false);
        if (activeHand == "Right")
        {
            SanfordObitR.SetActive(false);
            wristButtonsR.SetActive(true);
        }
        if(activeHand == "Left")
        {
            SanfordObitL.SetActive(false);
            wristButtonsL.SetActive(true);
        }
    }
    public void MenuSwitch()
    {
        if (menuOn)
        {
            archiveMenu.SetActive(false);
            backButton.SetActive(true);
            menuOn = false;
        }
        else
        {
            ArtifactSwitch();
            archiveMenu.SetActive(true);
            backButton.SetActive(false);
            previousButton.SetActive(false);
            nextButton.SetActive(false);
            anitaArnoldButtons[0].SetActive(false);
            anitaArnoldButtons[1].SetActive(false);
            menuOn = true;
        }
    }

    public void OralHistoryMenuSwitch(bool ohmOpen)
    {
        if (!ohmOpen)
        {
            archiveMenu.SetActive(false);
            oralHistoryMenu.SetActive(true);
        }
        else
        {
            archiveMenu.SetActive(true);
            oralHistoryMenu.SetActive(false);
        }
        
    }

    public void Next()
    {
       
        blackDispatchClippings[currentClipping].SetActive(false);
        if (currentClipping < (blackDispatchClippings.Length - 1))
        {
            currentClipping += 1;
        }
        else
        {
            currentClipping = 0;
        }
        //Debug.Log("Current clipping is " + currentClipping);
        blackDispatchClippings[currentClipping].SetActive(true);


    }

    public void Previous()
    {
        
        blackDispatchClippings[currentClipping].SetActive(false);
        if (currentClipping > 0)
        {
            currentClipping -= 1;
        }
        else
        {
            currentClipping = blackDispatchClippings.Length - 1;
        }
        //Debug.Log("Current clipping is " + currentClipping);
        blackDispatchClippings[currentClipping].SetActive(true);
        
    }
}
