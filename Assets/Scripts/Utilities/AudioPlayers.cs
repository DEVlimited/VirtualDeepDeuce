using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioPlayers : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public AudioSource audioSource;
    public AudioClip[] clips;
  
    void Start()
    {
         
    }

    protected void DropdownValueChanged(TMP_Dropdown change)
    {
        //Debug.Log(change.value);

        //null check on audiosource
        if(audioSource == null)
        {
            Debug.LogError("Audiosource not set");
        }

        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = clips[change.value];
        audioSource.Play();
    }

    public void AudioPlayerSwitch()
    {
        if(gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
