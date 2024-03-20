using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System;
using Unity.VisualScripting.Antlr3.Runtime;

public class GuidedBathAvenue : MonoBehaviour
{
    [Header("Time Elements")]
    private float currentTime;
    public TextMeshProUGUI currentTimeText;
    public float eventDelay = 3f;
    public float timeSinceEventChange;
    public int currentEventNumber;
    private bool eventAvailable = true;
    private bool stopTime = false;
    private int nextViewSwitch;
    private int nextItemSwitch;
    public float[] startTimes;
    public float[] endTimes;

    [Header("Utilities")]
    public Fader fader;
    public GameObject xRRig;
    public ImageResize imageResize;
    public CSVtoNarrativeParser parser;


    private int currentNarrativeClip = 0;
    private int currentView = 0;
    private float nextEventTime;
    private int eventNumber;

    private float bobbyVolume = 0.25f;
    private float quietSpeakerVolume = 1f;
    private float loudSpeakerVolume = 0.5f;

    public Transform[] viewLocations;
    public string[] narrativeTextChoices;
    private int currentNarrativeString = 0;
    public AudioClip audioClip;
    public AudioSource audioSource;

    [Header("Wrist UI Elements")]
    public GameObject continueButton;
    public GameObject continueTextL;
    public GameObject continueTextR;
    

    private int currentImage = 0;
    [SerializeField]
    private int[] minMaxClamp;
    public GameObject imagePanelL;
    public GameObject imagePanelR;
    public GameObject panel;
    public RawImage narrativeImageL;
    public RawImage narrativeImageR;
    public Texture[] narrativeTexture;
    public GameObject[] guidedImages;

    public InputActionReference consolePrint;

    void Start()
    {
        //LoadNarrative();
        InputActionsSetup();

        currentView = 0;
        ViewSwitch(0);
        currentTime = 0;
        nextEventTime = 5.4f;
        audioSource.volume = bobbyVolume;  
        //check playerprefs and run missing setups/tutorials
        if (PlayerPrefs.GetString("Hand") == "")
        {
            PlayerPrefs.SetString("Hand", "Right");
            Debug.Log("Hand was null in PlayerPrefs. Setting as 'Right'");
        }
        if (PlayerPrefs.GetString("Guided First Time") == "")
        {
            PlayerPrefs.SetString("Guided First Time", "true");
        }
        if (PlayerPrefs.GetString("Guided First Time") == "true")
        {
            //launch tutorial
            //Tutorial();
            PlayerPrefs.SetString("Guided First Time", "false");
        }

        //begin subtitles
        StartCoroutine(FindObjectOfType<SubtitleDisplayer>().Begin());

    }

    void Update()
    {
        if (!stopTime)
        {
            currentTime += Time.deltaTime;
        }
        if (stopTime)
        {
            continueTextL.SetActive(true);
            continueTextR.SetActive(true);
        }
        
        timeSinceEventChange += Time.deltaTime;

       

        if (currentTime > nextEventTime && eventAvailable)
        {
            EventSwitch();
        }
        if(timeSinceEventChange > eventDelay)
        {
            EventCooldown();
        }   
        //for timing/testing purposes - displays the time since scene start
        currentTimeText.text = eventNumber + " | " + currentTime;
    }

    private void LoadNarrative()
    {
        startTimes = new float[parser.grid.Length];
        //set startTimes
            //pull them from csv parsed to table. start times begin on row 1 column 1; see for statment to see implementation
       for (int i = 1; i < parser.grid.Length - 1; i++)
       {
            Debug.Log(i);
            parser.CellContentsCall(i, 1);
            var temp = i;
            temp -= 1;
            Debug.Log(parser.resultText + " is being set to startTime" + temp);
            Debug.Log("Setting startTime" + startTimes[temp] + " to " + parser.resultText + " | " + Convert.ToInt32(parser.resultText));
            startTimes[temp] = Convert.ToInt32(parser.resultText);
            Debug.Log(startTimes[temp]);
       }
    }

    private void ConsolePrint_performed(InputAction.CallbackContext obj)
    {
        //Debug.Log("Console Print called");
        ConsoleStatusPrint();
    }
    private void EventCooldown()
    {
        timeSinceEventChange = 0;
        eventAvailable = true;
    }
    private void ConsoleStatusPrint()
    {
        Debug.Log("Console Status Print Called");
        if(PlayerPrefs.GetString("Debug Mode") == "on")
        {
            Debug.LogError("Event number is " + (eventNumber - 1) + " | Time is " + currentTime + " | current clip is " + currentNarrativeClip + " | current narrative string is " + currentNarrativeString);
        }
        
    }
    private void InputActionsSetup()
    {
        consolePrint.action.started += ConsolePrint_performed;
    }

    public void NewEventSwitch(int eventNumber = 0, int direction = 0, int destination = 0)
    {
        //set currentEventNumber as working eventNumber
        eventNumber = currentEventNumber;

        //if there is a desired destination, go there
            //if not, use direction
        if(destination == 0)
        {
            //go to adjacent event using 'direction
            eventNumber += direction;
        }
        else
        {
            //go to specific event using 'destination'
            eventNumber = destination;
        }
        //check if viewSwitch is needed
        if(eventNumber == nextViewSwitch)
        {
            //go to specific View using 'destination'
            ViewSwitch(0, eventNumber);
        }
        //check if itemSwitch is needed
        if(eventNumber == nextItemSwitch)
        {
            //go to specific View using 'destination'
            ItemSwitch(0, eventNumber);
        }
        
        currentEventNumber = eventNumber;

    }
    public void EventSwitch(int direction = 1)
    {
        //turn off WristUI image panels if they are on
        if(imagePanelL.activeInHierarchy)
        {
            panel.SetActive(true);
            imagePanelL.SetActive(false);
        }
        if (imagePanelR.activeInHierarchy)
        {
            panel.SetActive(true);
            imagePanelR.SetActive(false);
        }
        
        //set eventAvailable to false to prevent switching
        eventAvailable = false;
        
        //comments show what text is handled in events below
            //The Fairgrounds neighborhood had the approximate borders of NE 8th Street on the north to the Rock Island railroad tracks at the south, and Stonewall or Lottie Avenues to the West and Eastern Avenue (now MLK) to the east
        if (eventNumber == 0)
        {
            currentTime = 5.4f;
            nextEventTime = 24.5f;
            //TextSwitch(direction);
            ViewSwitch(direction);
        }

        //Today’s Douglass High School (900 N MLK) was the site of the first State Fairgrounds.
        if (eventNumber == 1)
        {
            currentTime = 24.5f;
            nextEventTime = 32f;
            //TextSwitch(direction);
        }

        //The area was near the path of the unchallenged North Canadian River and was inclined to flooding.
        if (eventNumber == 2)
        {
            currentTime = 32f;
            nextEventTime = 38f;
            //TextSwitch(direction);
        }

        //The Fairgrounds was an incredibly dense, predominantly Black neighborhood.
        if (eventNumber == 3)
        {
            currentTime = 38f;
            nextEventTime = 43f;
           // TextSwitch(direction);
        }

        //Because of redlining and other systemically racist house practices, the majority of African American residents of Oklahoma City primarily lived in a few specific neighborhoods.
        if (eventNumber == 4)
        {
            currentTime = 43;
            nextEventTime = 53f;
           // TextSwitch(direction);
        }

        //Take a look at these aerial photos...
        if (eventNumber == 5)
        {
            //stop time to allow for interaction
            stopTime = true;
            //normalize time
            currentTime = 54f;
            nextEventTime = 54.5f;
            
            //activate image in world
            guidedImages[0].SetActive(true);

            //clamp images selectable through wristUI
            minMaxClamp[0] = 0;
            minMaxClamp[1] = 3;

            //set image on wristUI
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);
            ImageSwitch(narrativeImageL, narrativeTexture[currentImage]);
            ImageSwitch(narrativeImageR, narrativeTexture[currentImage]);
            //TextSwitch(direction);
        }

        //The East Side Theater and its neighboring...
        if (eventNumber == 6)
        {
            currentTime = 54.5f;
            nextEventTime = 68f;

           // TextSwitch(direction);
        }

        //DJS: He had the idea of Blacks, Negros, doing things...
        if (eventNumber == 7)
        {
            audioSource.volume = quietSpeakerVolume;
            currentTime = 68f;
            nextEventTime = 155f;

            //activate image in world
            guidedImages[0].SetActive(false);
            guidedImages[1].SetActive(true);

            
            //skipping portion with missing audio -> audio is missing because I don't have timestamps
           // TextSwitch(direction * 2);

        }

        //MH:  I asked Mr. Richardson, I said I was thinking...
        if (eventNumber == 8)
        {
            currentTime = 155f;
            nextEventTime = 221.5f;

           // TextSwitch(direction);

        }

        //Construction began on the commercial strip...
        if (eventNumber == 9)
        {
            audioSource.volume = bobbyVolume;
            //stop time to allow for interaction
            stopTime = true;
            
            //normalize time
            currentTime = 221.5f;
            nextEventTime = 222.5f;

            //activate image in world
            guidedImages[1].SetActive(false);
            guidedImages[2].SetActive(true);          

            //clamp images selectable through wristUI
            minMaxClamp[0] = 6;
            minMaxClamp[1] = 7;

            currentImage = 6;
            //set image on wristUI
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);
            ImageSwitch(narrativeImageL, narrativeTexture[currentImage]);
            ImageSwitch(narrativeImageR, narrativeTexture[currentImage]);
           // TextSwitch(direction);

        }

        //Browse through these clippings from The Black Dispatch...
        if (eventNumber == 10)
        {
            //stop time to allow for interaction
            stopTime = true;
            //normalize time
            currentTime = 222.5f;
            nextEventTime = 223f;

            //activate image in world
            guidedImages[2].SetActive(false);
            guidedImages[3].SetActive(true);

            //clamp images selectable through wristUI
            minMaxClamp[0] = 8;
            minMaxClamp[1] = 37;

            //set image on wristUI
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);
            currentImage = 8;
            ImageSwitch(narrativeImageL, narrativeTexture[currentImage]);
            ImageSwitch(narrativeImageR, narrativeTexture[currentImage]);
           // TextSwitch(direction);
        }

        //So why is the East Side Theater gone?
        if (eventNumber == 11)
        {
            audioSource.volume = bobbyVolume;
            currentTime = 223f;
            nextEventTime = 233f;

           // TextSwitch(direction);

        }

        //Women:  Do you ever hear anyone talk about the East Side Theater?...
        if(eventNumber == 12)
        {
            audioSource.volume = loudSpeakerVolume;
            currentTime = 233f;
            nextEventTime = 243f;

           // TextSwitch(direction);
        }
        //Oral histories from former and current residents of the Fairgrounds describe the types of business and activities...
        if(eventNumber == 13)
        {
            audioSource.volume = bobbyVolume;
            currentTime = 243f;
            nextEventTime = 255f;

            //TextSwitch(direction);
        }

        //706 N Bath  Louie's Garage...
        if(eventNumber == 14)
        {
            currentTime = 253f;
            nextEventTime = 267f;

            ViewSwitch(direction);
           // TextSwitch(direction);
        }

        //…and on down, there was a man, they called it Buck’s
        if (eventNumber == 15)
        {
            currentTime = 267f;
            nextEventTime = 320f;

           // TextSwitch(direction);

        }

        //710 N Bath Sound of diner, dishes, water running.  
        if (eventNumber == 16)
        {
            //stop time to allow for interaction
            stopTime = true;
            //normalize time
            currentTime = 300f;
            nextEventTime = 300.5f;

            //activate image in world
            guidedImages[3].SetActive(false);
            guidedImages[4].SetActive(true);

            //clamp images selectable through wristUI NOT RIGHT
            minMaxClamp[0] = 38;
            minMaxClamp[1] = 38;

            //set image on wristUI
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);
            currentImage = 38;
            ImageSwitch(narrativeImageL, narrativeTexture[currentImage]);
            ImageSwitch(narrativeImageR, narrativeTexture[currentImage]);
           // TextSwitch(direction);
            ViewSwitch(direction);

        }

        //710B was the home of Melody Records...  
        if (eventNumber == 17)
        {
            //interaction continues through multiple events so...
                //continue button is disabled till the last one
                //time continues until last audio clip starts
            continueButton.SetActive(false);

            //normalize time
            currentTime = 300.5f;
            nextEventTime = 308f;

            //activate image in world
            guidedImages[4].SetActive(false);
            guidedImages[5].SetActive(true);

            //clamp images selectable through wristUI
            minMaxClamp[0] = 39;
            minMaxClamp[1] = 41;

            //set image on wristUI
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);
            currentImage = 39;
            ImageSwitch(narrativeImageL, narrativeTexture[currentImage]);
            ImageSwitch(narrativeImageR, narrativeTexture[currentImage]);
            //TextSwitch(direction);

        }

        //But there was a coffee shop and we’re talking about that same side of the street...
        if (eventNumber == 18)
        {
            //normalize time
            currentTime = 308f;
            nextEventTime = 362f;

            //TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //ANITA ARNOLD 08-17-07 20’29-21.01
        if (eventNumber == 19)
        {
            //reactivate continue from persistent interaction
            continueButton.SetActive(true);
            //stop time to allow for interaction
            stopTime = true;
            //normalize time
            currentTime = 355f;
            nextEventTime = 356f;

            //TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);
        }

        //712 Bath  In a time before appliances...
        if (eventNumber == 20)
        {
            currentTime = 356f;
            nextEventTime = 386f;

           // TextSwitch(direction);
            ViewSwitch(direction);
        }

        //714 Bath In the early days of the block...
        if (eventNumber == 21)
        {
            stopTime = true;
            //normalize time
            currentTime = 376f;
            nextEventTime = 377f;

            //activate image in world
            guidedImages[4].SetActive(false);
            guidedImages[5].SetActive(true);

            //clamp images selectable through wristUI
            minMaxClamp[0] = 42;
            minMaxClamp[1] = 42;

            //set image on wristUI
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);
            currentImage = 42;
            ImageSwitch(narrativeImageL, narrativeTexture[currentImage]);
            ImageSwitch(narrativeImageR, narrativeTexture[currentImage]);
            //TextSwitch(direction);
            ViewSwitch(direction);
        }

        //By the end of the 1960s, 714 Bath...
        if (eventNumber == 22)
        {
            currentTime = 385f;
            nextEventTime = 400f;

            //TextSwitch(direction);
            ViewSwitch(direction);
        }

        //718 Bath Listed as “under construction”...
        if (eventNumber == 23)
        {
            //stop time to allow for interaction
            stopTime = true;
            //normalize time
            currentTime = 400f;
            nextEventTime = 401f;

            //activate image in world
            guidedImages[3].SetActive(false);
            guidedImages[4].SetActive(true);

            //clamp images selectable through wristUI
            minMaxClamp[0] = 43;
            minMaxClamp[1] = 48;

            //set image on wristUI
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);
            currentImage = 43;
            ImageSwitch(narrativeImageL, narrativeTexture[currentImage]);
            ImageSwitch(narrativeImageR, narrativeTexture[currentImage]);
            //TextSwitch(direction);
            ViewSwitch(direction);
        }

        //We’re also lucky to have several pics...
        if (eventNumber == 24)
        {
            //stop time to allow for interaction
            stopTime = true;
            //normalize time
            currentTime = 402f;
            nextEventTime = 403f;

            //clamp images selectable through wristUI
            minMaxClamp[0] = 49;
            minMaxClamp[1] = 52;

            //set image on wristUI
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);
            currentImage = 49;
            ImageSwitch(narrativeImageL, narrativeTexture[currentImage]);
            ImageSwitch(narrativeImageR, narrativeTexture[currentImage]);
            //TextSwitch(direction);
        }

        //718B Bath This address was also home to the Glamour-Manor...
        if (eventNumber == 25)
        {
            //stop time to allow for interaction
            stopTime = true;
            //normalize time
            currentTime = 403f;
            nextEventTime = 404f;

            //clamp images selectable through wristUI
            minMaxClamp[0] = 53;
            minMaxClamp[1] = 57;

            //set image on wristUI
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);
            currentImage = 53;
            ImageSwitch(narrativeImageL, narrativeTexture[currentImage]);
            ImageSwitch(narrativeImageR, narrativeTexture[currentImage]);
            //TextSwitch(direction);
        }

        //720-722 Bath East Side Theater Listen...
        if (eventNumber == 26)
        {
            //normalize time
            currentTime = 394f;
            nextEventTime = 404f;
            //TextSwitch(direction);
            ViewSwitch();
        }

        //AA- And then down on Bath, on 7th and Bath...
        if (eventNumber == 27)
        {
            currentTime = 404f;
            nextEventTime = 435f;

           // TextSwitch(direction);

        }

        //AA- It wasn’t fair and I was the youngest...
        if (eventNumber == 28)
        {
            currentTime = 435f;
            nextEventTime = 472f;

           // TextSwitch(direction);

        }

        //728 Bath Jimmy’s Shine Parlor In an era of 
        if (eventNumber == 29)
        {
            audioSource.volume = bobbyVolume;
            currentTime = 475f;
            nextEventTime = 489f;

           // TextSwitch(direction);
            ViewSwitch(direction);
        }

        //730 Bath This location was home 
        if (eventNumber == 30)
        {
            //stop time to allow for interaction
            stopTime = true;
            //normalize time
            currentTime = 487f;
            nextEventTime = 488f;

            //clamp images selectable through wristUI
            minMaxClamp[0] = 58;
            minMaxClamp[1] = 58;

            //set image on wristUI
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);
            currentImage = 58;
            ImageSwitch(narrativeImageL, narrativeTexture[currentImage]);
            ImageSwitch(narrativeImageR, narrativeTexture[currentImage]);
           // TextSwitch(direction);
        }

        //Urban Renewal
        if (eventNumber == 31)
        {
            currentTime = 500f;
            nextEventTime = 532f;

           // TextSwitch(direction);
            ViewSwitch(direction);
            

        }

        //Oklahoma City
        if (eventNumber == 32)
        {
            currentTime = 532f;
            nextEventTime = 592f;

           // TextSwitch(direction);
        }

        //SB: What do you remember about when
        if(eventNumber == 33)
        {
            audioSource.volume = quietSpeakerVolume;
            //interaction continues through multiple events so...
                //continue button is disabled till the last one
                //time continues until last audio clip starts
            continueButton.SetActive(false);

            //normalize time
            currentTime = 592f;
            nextEventTime = 637f;

            //clamp images selectable through wristUI
            minMaxClamp[0] = 59;
            minMaxClamp[1] = 84;

            //set image on wristUI
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);
            currentImage = 59;
            ImageSwitch(narrativeImageL, narrativeTexture[currentImage]);
            ImageSwitch(narrativeImageR, narrativeTexture[currentImage]);
           // TextSwitch(direction);
        }

        //MH: You found that throughout the neighborhood that...
        if (eventNumber == 34)
        {
            //normalize time
            currentTime = 637f;
            nextEventTime = 660.5f;

            //TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //MH: Just part of the neighborhood....
        if (eventNumber == 35)
        {
            //normalize time
            currentTime = 660.5f;
            nextEventTime = 684f;

           // TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //MH: We were all right there together....
        if (eventNumber == 36)
        {
            //normalize time
            currentTime = 684f;
            nextEventTime = 707.5f;

            //TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //DJS: You were on that subject of the neighborhood....
        if (eventNumber == 37)
        {
            //normalize time
            currentTime = 707.5f;
            nextEventTime = 731f;

            //TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //DJS: DJS: You’d know who those people were....
        if (eventNumber == 38)
        {
            //normalize time
            currentTime = 731f;
            nextEventTime = 773f;

            //TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //MH: It was our neighborhood. DJS: Yes it was....
        if (eventNumber == 39)
        {
            //normalize time
            currentTime = 754f;
            nextEventTime = 777.5f;

            //TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //PG: What has changed the most to you....
        if (eventNumber == 40)
        {
            //normalize time
            currentTime = 777.5f;
            nextEventTime = 854f;

            //TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //PG:Do you know what happened to those beautiful....
        if (eventNumber == 41)
        {
            //normalize time
            currentTime = 915f;
            nextEventTime = 942f;

            //extSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //AF: The interesting part about it is that as the city....
        if (eventNumber == 42)
        {
            //normalize time
            currentTime = 942f;
            nextEventTime = 955f;

            //TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //HS: There was a Urban Renewal that came....
        if (eventNumber == 43)
        {
            audioSource.volume = bobbyVolume;
            //normalize time
            currentTime = 955f;
            nextEventTime = 985f;

            //TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //CT: Do you know how long that project lasted?....
        if (eventNumber == 44)
        {
            //normalize time
            currentTime = 985f;
            nextEventTime = 1185f;

            //TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //SR: You see that whole northwest area that’s now a....
        if (eventNumber == 45)
        {
            audioSource.volume = quietSpeakerVolume;
            //normalize time
            currentTime = 1185f;
            nextEventTime = 1242f;

            //TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //KF-When you think back...
        if (eventNumber == 46)
        {
            audioSource.volume = loudSpeakerVolume;
            //reactivate continue from persistent interaction
            continueButton.SetActive(true);
            //stop time to allow for interaction
            stopTime = true;
            //normalize time
            currentTime = 1242f;
            nextEventTime = 1243f;

            //TextSwitch(direction);
            imagePanelL.SetActive(true);
            imagePanelR.SetActive(true);

        }

        //credits
        if (eventNumber == 47)
        {
            audioSource.volume = bobbyVolume;
            currentTime = 1243f;
            nextEventTime = 1303f;

            ViewSwitch(direction);

            //activate image in world
            guidedImages[5].SetActive(false);
            guidedImages[6].SetActive(true);
        }

        //back to menu
        if(eventNumber == 48)
        {
            SceneManager.LoadScene(0);
        }


        eventNumber += direction;
    }
    public void ImageSwitch(RawImage image, Texture texture, int direction = 1)
    {
        image.texture = texture;
        currentImage += direction;

        if (currentImage > minMaxClamp[1])
        {
            currentImage = minMaxClamp[1];
        }
        if (currentImage < minMaxClamp[0])
        {
            currentImage = minMaxClamp[0];
        }
        image.SetNativeSize();
        //image.rectTransform.sizeDelta = new Vector3(image.rectTransform.sizeDelta.x / reduction, image.rectTransform.sizeDelta.y / reduction);
        imageResize.ImageResizer(image, texture);
    }
    public void WristImageSwitch(int direction = 1)
    {
        ImageSwitch(narrativeImageL, narrativeTexture[currentImage], direction);
        ImageSwitch(narrativeImageR, narrativeTexture[currentImage], direction);
    }
     public void ItemSwitch(int direction = 1, int destination = 0)
    {
        //if there is a desired destination, go there
            //if not, use direction
        if(destination == 0)
        {
            //go to adjacent item using 'direction'
            currentImage += direction;
        }
        else
        {
            //go to a specific item using 'desitination'
            currentImage = destination;
        }

    }
    public void ViewSwitch(int direction = 1, int destination = 0)
    {
        //fade out camera
        fader.FadeOut();

        //if there is a desired destination, go there
            //if not, use direction
        if(destination == 0)
        {
            //go to adjacent view using 'direction'
            currentView += direction;
        }
        else
        {
            //go to a specific view using 'desitination'
            currentView = destination;
        }

        //set camera position and rotation to the new location
        xRRig.transform.position = viewLocations[currentView].position;
        xRRig.transform.rotation = viewLocations[currentView].rotation;

        //fade in camera at new location
        fader.FadeIn();

    }
    public void Continue()
    {
        if (!audioSource.isPlaying)
        {
            stopTime = false;
        }
        continueTextL.SetActive(false);
        continueTextR.SetActive(false);


    }
    public void Pause()
    {
        stopTime = true;
    }

public void Seek(int eventNumber)
{
    //get start time for target event
    var target = startTimes[eventNumber];
    
    //Debug.Log("Target is " + target);
    /* Debug.Log("dspTime is " + AudioSettings.dspTime);
    double time = target + AudioSettings.dspTime;
    Debug.Log("TargetDouble is " + time); */
    //set audio file to the target time (start time)
    audioSource.time = target;
    Debug.Log("Clip playing at " + audioSource.time);
    //set subtitle parser to the target time which is read from the audioSource inside of the SubtitleDisplayer class
    Debug.Log("Subtitle at: " + FindAnyObjectByType<SubtitleDisplayer>().elapsed);
    //FindObjectOfType<SubtitleDisplayer>().Seek();
    Debug.Log("Updated subtitle at: " + FindAnyObjectByType<SubtitleDisplayer>().elapsed);
    //sets currentTime to the target; Depricated but not rooted out yet
    currentTime = target;
    
}    
}

