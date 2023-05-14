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

    public AudioClip[] historyClips;
    public AudioSource clip1;
    public AudioSource clip2;

    private bool clip1IsPlaying;
    private bool clip2IsPlaying;

    // Start is called before the first frame update
    void Start()
    {
        clip1IsPlaying = false;
        clip2IsPlaying = false;
        slider1.maxValue = clip1.clip.length;
        slider2.maxValue = clip2.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        if (clip1IsPlaying)
        {
            slider1.value = clip1.time;
        }
        if (clip2IsPlaying)
        {
            slider2.value = clip2.time;
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
            
        }
    }
  
    public void LoadClip(int clipNumber1, int clipNumber2 = -1)
    {
        clip1.clip = historyClips[clipNumber1];
        slider1.maxValue = clip1.clip.length;

        if(clipNumber2 > 0)
        {
            clip2.clip = historyClips[clipNumber2];
            slider2.maxValue = clip2.clip.length;
        }
    }

    public void OnSliderChange1(float value)
    {

    }
    public void OnSliderChange2(float value)
    {

    }
}
