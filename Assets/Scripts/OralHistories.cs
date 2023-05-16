using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OralHistories : MonoBehaviour
{
    public Slider slider1;
    public Slider slider2;

    public Button clip1Button;
    public Button clip2Button;

    public AudioClip[] fullLengthHistoryClips;
    public AudioSource clip1;
    public AudioSource clip2;
    public Slider trackProgress1;
    public Slider trackProgress2;

    private bool clip1IsPlaying;
    private bool clip2IsPlaying;
    private bool slider1engaged = false;
    private bool slider2engaged = false;

    public GameObject objectContainer;

    // Start is called before the first frame update
    void Start()
    {
        clip1IsPlaying = false;
        clip2IsPlaying = false;
        slider1.maxValue = clip1.clip.length;
        slider2.maxValue = clip2.clip.length;
        trackProgress1.maxValue = clip1.clip.length;
        trackProgress2.maxValue = clip2.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        if (clip1IsPlaying & !slider1engaged)
        {
            trackProgress1.value = clip1.time;
        }
        if (clip2IsPlaying & !slider2engaged)
        {
            trackProgress2.value = clip2.time;
        }
    }

    public void Play(int clip)
    {
        if(clip == 1)
        {
            if (clip1IsPlaying)
            {
                clip1.Pause();
            }
            else
            {
                clip2IsPlaying = false;
                clip2.Pause();
                clip1.Play();
                clip1IsPlaying = true;
            }
            Debug.Log("Playing " + clip1.clip.name);
        }
        if(clip == 2)
        {
            if (clip2IsPlaying)
            {
                clip2.Pause();
            }
            else
            {
                clip1IsPlaying = false;
                clip1.Pause();
                clip2.Play();
                clip2IsPlaying = true;
            }
            Debug.Log("Playing " + clip2.clip.name);
        }
    }

    public void StartMouseDrag(int slider)
    {
        if(slider == 1)
        {
            slider1engaged = true;
            
        }
        else
        {
            slider2engaged = true;
            
        }
        
    }

    public void StopMouseDrag(int slider)
    {
        if (slider == 1)
        {
            slider1engaged = false;
           //Debug.Log("Clip time is " + clip1.time + "/" + clip1.clip.length);
            clip1.time = slider1.value;
        }
        else
        {
            slider2engaged = false;
            //Debug.Log("Clip time is " + clip2.time + "/" + clip1.clip.length);
            clip2.time = slider2.value;
        }
    }
}
