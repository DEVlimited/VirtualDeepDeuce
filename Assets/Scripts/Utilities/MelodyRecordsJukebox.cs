using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MelodyRecordsJukebox : AudioPlayers
{
     [Header("Melody Records Jukebox")]
    public GameObject melodyJukebox;
    // Start is called before the first frame update
    void Start()
    {
        //clear any options stored in the dropdown
        dropdown.ClearOptions(); 
        //create list to store them titles in
        List<TMP_Dropdown.OptionData> titleList = new List<TMP_Dropdown.OptionData>();
        //pull drop down options from audiofiles titles in clip array and add them to the list
        for (int i = 0; i < clips.Length; i++)
        {
            var newOption = new TMP_Dropdown.OptionData();
            newOption.text = clips[i].name;
            titleList.Add(newOption);
        }
        //add each title to the dropdown
        foreach (TMP_Dropdown.OptionData title in titleList)
        {
            //add the title
            dropdown.options.Add(title);

        }
        //Add listener for when the value of the Dropdown changes, to take action
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
