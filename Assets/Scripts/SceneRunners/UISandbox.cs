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
    public GameObject interfacePanel;
    private int currentButtonStyle = 0;
    private int currentBackgroundStyle = 0;
    public GameObject[] buttons;
    public GameObject[] backgrounds;
    public AudioClip[] melodyTracks;
    public TMP_Dropdown melodyDropdown;
    private AudioSource melodyAudioSource;
    public GameObject melodyJukebox;
    public GameObject carousel;
    public RectTransform simpleScrollPanelContainer;
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
        
        //set AudioSource
        melodyAudioSource = GetComponent<AudioSource>();
        //clear any options stored in the dropdown
        melodyDropdown.ClearOptions();  
        //create list to store them titles in
        List<TMP_Dropdown.OptionData> titleList = new List<TMP_Dropdown.OptionData>();
        //pull drop down options from audiofiles titles in folder and add them to the list
        for (int i = 0; i < melodyTracks.Length; i++)
        {
            var newOption = new TMP_Dropdown.OptionData();
            newOption.text = melodyTracks[i].name;
            titleList.Add(newOption);
        }
        //add each title to the dropdown
        foreach (TMP_Dropdown.OptionData title in titleList)
        {
            //add the title
            melodyDropdown.options.Add(title);

        }
        //Add listener for when the value of the Dropdown changes, to take action
        melodyDropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(melodyDropdown);
        });

        //simpleScrollPanelContainer.position = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        //Debug.Log(change.value);
        if(melodyAudioSource.isPlaying)
        {
            melodyAudioSource.Stop();
        }
        melodyAudioSource.clip = melodyTracks[change.value];
        melodyAudioSource.Play();
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

    public void JukeboxSwitch()
    {
        if(melodyJukebox.activeInHierarchy)
        {
            melodyJukebox.SetActive(false);
        }
        else
        {
            melodyJukebox.SetActive(true);
        }
    }

    public void CarouselSwitch()
    {
        if(carousel.activeInHierarchy)
        {
            carousel.SetActive(false);
            //simpleScrollPanelContainer.position = new Vector3(0, 0, 0);
            interfacePanel.SetActive(true);
        }
        else
        {
            carousel.SetActive(true);
            //simpleScrollPanelContainer.position = new Vector3(0, 0, 0);
            interfacePanel.SetActive(false);
        }
    }

    public void SceneSwitch(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
