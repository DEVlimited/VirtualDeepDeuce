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

    public GameObject tutorialPanelR;
    public GameObject tutorialPanelL;    

    [Header("Artifacts")]
    private GameObject currentArtifact;
    public GameObject aerialViews;
    public GameObject SanfordRichardsonArtifact;
    public GameObject SanfordObitL;
    public GameObject SanfordObitR;
    public GameObject blackDispatchClippingsContainer;
    public GameObject[] blackDispatchClippings;
    public int currentClipping = 0;

    [Header("Oral Histories")]
    public GameObject oralHistoryMenu;
    public GameObject ohBackButton;
    public GameObject anitaArnold;
    public GameObject[] anitaArnoldButtons;
    public GameObject avisMelvaFranklin;
    public GameObject[] avisMelvaFranklinButtons;
    public GameObject eloiseCarbajal;
    public GameObject[] eloiseCarbajalButtons;
    public GameObject georgeRichardson;
    public GameObject[] georgeRichardsonButtons;
    public GameObject hobertSutton;
    public GameObject[] hobertSuttonButtons;
    public GameObject hurettaWalkerDobbs;
    public GameObject[] hurettaWalkerDobbsButtons;
    public GameObject jwSanford;
    public GameObject[] jwSanfordButtons;
    public GameObject sandraRichards;
    public GameObject[] sandraRichardsButtons;



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
        if (PlayerPrefs.GetString("Archive First Time") == "")
        {
            PlayerPrefs.SetString("Archive First Time", "true");
        }
        if (PlayerPrefs.GetString("Archive First Time") == "true")
        {
            //launch tutorial if this is the first time in the scene
            Tutorial();
            PlayerPrefs.SetString("Archive First Time", "false");
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

    public void BackToHistories()
    {
        oralHistoryMenu.SetActive(true);
        ohBackButton.SetActive(false);
        anitaArnoldButtons[0].SetActive(false);
        anitaArnoldButtons[1].SetActive(false);
        avisMelvaFranklinButtons[0].SetActive(false);
        eloiseCarbajalButtons[0].SetActive(false);
        georgeRichardsonButtons[0].SetActive(false);
        hobertSuttonButtons[0].SetActive(false);
        hurettaWalkerDobbsButtons[0].SetActive(false);
        jwSanfordButtons[0].SetActive(false);
        jwSanfordButtons[1].SetActive(false);
        sandraRichardsButtons[0].SetActive(false);
        anitaArnold.SetActive(false);
    }

    public void CloseAll()
    {
        oralHistoryMenu.SetActive(false);
        ohBackButton.SetActive(false);
        anitaArnoldButtons[0].SetActive(false);
        anitaArnoldButtons[1].SetActive(false);
        avisMelvaFranklinButtons[0].SetActive(false);
        eloiseCarbajalButtons[0].SetActive(false);
        georgeRichardsonButtons[0].SetActive(false);
        hobertSuttonButtons[0].SetActive(false);
        hurettaWalkerDobbsButtons[0].SetActive(false);
        jwSanfordButtons[0].SetActive(false);
        jwSanfordButtons[1].SetActive(false);
        sandraRichardsButtons[0].SetActive(false);
        anitaArnold.SetActive(false);
        archiveMenu.SetActive(false);
    }

    public void LoadArtifact(string artifact)
    {
        
        activeHand = PlayerPrefs.GetString("Hand");
        if (artifact == "Aerial Views")
        {
            MenuSwitch();
            aerialViews.SetActive(true);
        }
        if (artifact == "SanfordRichardson")
        {
            MenuSwitch();
            SanfordRichardsonArtifact.SetActive(true);
            currentArtifact = SanfordRichardsonArtifact;
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
            MenuSwitch();
            currentClipping = 0;
            blackDispatchClippingsContainer.SetActive(true);
            previousButton.SetActive(true);
            nextButton.SetActive(true);
        }
        if(artifact == "AnitaArnold")
        {
            backButton.SetActive(false);
            ohBackButton.SetActive(true);
            oralHistoryMenu.SetActive(false);
            anitaArnold.SetActive(true);
            anitaArnoldButtons[1].SetActive(true);
            anitaArnoldButtons[0].SetActive(true);

        }
        if (artifact == "AvisMelvaFranklin")
        {
            backButton.SetActive(false);
            ohBackButton.SetActive(true);
            oralHistoryMenu.SetActive(false);
            avisMelvaFranklin.SetActive(true);
            avisMelvaFranklinButtons[0].SetActive(true);
        }
        if (artifact == "EloiseCarbajal")
        {
            backButton.SetActive(false);
            ohBackButton.SetActive(true);
            oralHistoryMenu.SetActive(false);
            eloiseCarbajal.SetActive(true);
            eloiseCarbajalButtons[0].SetActive(true);
        }
        if (artifact == "GeorgeRichardson")
        {
            backButton.SetActive(false);
            ohBackButton.SetActive(true);
            oralHistoryMenu.SetActive(false);
            georgeRichardson.SetActive(true);
            georgeRichardsonButtons[0].SetActive(true);
        }
        if (artifact == "HobertSutton")
        {
            backButton.SetActive(false);
            ohBackButton.SetActive(true);
            oralHistoryMenu.SetActive(false);
            hobertSutton.SetActive(true);
            hobertSuttonButtons[0].SetActive(true);
        }
        if (artifact == "HurettaWalker")
        {
            backButton.SetActive(false);
            ohBackButton.SetActive(true);
            oralHistoryMenu.SetActive(false);
            hurettaWalkerDobbs.SetActive(true);
            hurettaWalkerDobbsButtons[0].SetActive(true);
        }
        if (artifact == "JWSanford")
        {
            backButton.SetActive(false);
            ohBackButton.SetActive(true);
            oralHistoryMenu.SetActive(false);
            jwSanford.SetActive(true);
            jwSanfordButtons[0].SetActive(true);
            jwSanfordButtons[1].SetActive(true);
        }
        if (artifact == "SandraRichards")
        {
            backButton.SetActive(false);
            ohBackButton.SetActive(true);
            oralHistoryMenu.SetActive(false);
            sandraRichards.SetActive(true);
            sandraRichardsButtons[0].SetActive(true);
        }
    }

    public void ArtifactSwitch()
    {
        aerialViews.SetActive(false);
        SanfordRichardsonArtifact.SetActive(false);
        blackDispatchClippingsContainer.SetActive(false);
        anitaArnoldButtons[0].SetActive(false);
        anitaArnoldButtons[1].SetActive(false);
        avisMelvaFranklinButtons[0].SetActive(false);
        eloiseCarbajalButtons[0].SetActive(false);
        georgeRichardsonButtons[0].SetActive(false);
        hobertSuttonButtons[0].SetActive(false);
        hurettaWalkerDobbsButtons[0].SetActive(false);
        jwSanfordButtons[0].SetActive(false);
        jwSanfordButtons[1].SetActive(false);
        sandraRichardsButtons[0].SetActive(false);
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

    public void Tutorial()
    {
       if(PlayerPrefs.GetString("Hand") == "Right")
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
