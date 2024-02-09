using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleDisplayer : MonoBehaviour
{
  [Header("SRT/TXT File")]
  public TextAsset Subtitle;
  [Header("Audio File")]
  private AudioSource audioSource;

  [Header("Target Text Objects")]
  public TextMeshProUGUI Text;
  public TextMeshProUGUI Text2;

  [Header("Subtitle Animation Controls")]
  [Range(0, 1)]
  public float FadeTime;

  /*private bool _isPaused = false;
  private bool _isPausedTimeSet;
  private float _pausedTime;
*/
  public float elapsed;
  public float startTime;

  public SubtitleBlock currentSubtitle;
  public SubtitleBlock subtitle;


void Start()
{
  audioSource = GetComponent<AudioSource>();
}
  void Update()
  {
     
  }
  
  public IEnumerator Begin()
  {

    //assign the text and fadetext objects
    var currentlyDisplayingText = Text;
    var fadedOutText = Text2;

    //empty the text fields
    currentlyDisplayingText.text = string.Empty;
    fadedOutText.text = string.Empty;

    //ensure both gameobjects are active
    currentlyDisplayingText.gameObject.SetActive(true);
    fadedOutText.gameObject.SetActive(true);

    yield return FadeTextOut(currentlyDisplayingText);
    yield return FadeTextOut(fadedOutText);

    var parser = new SRTParser(Subtitle);

    startTime = Time.time;
    //var difference = Time.time - target;
    currentSubtitle = null;
    while (true)
    {
      /* while (_isPaused)
      {
        if (!_isPausedTimeSet)
        {
          _pausedTime = Time.time;
          _isPausedTimeSet = true;
        }
        
        yield return null;
      }

      if (_isPausedTimeSet)
      {
        startTime += Time.time - _pausedTime;
        _isPausedTimeSet = false;
      } */

      elapsed = audioSource.time;
      //Debug.Log("Current SRT Time is " +  elapsed);
      subtitle = parser.GetForTime(elapsed);
      if (subtitle != null)
      {
        if (!subtitle.Equals(currentSubtitle))
        {
          currentSubtitle = subtitle;

          // Swap references around
          var temp = currentlyDisplayingText;
          Text = fadedOutText;
          fadedOutText = temp;

          // Switch subtitle text
          Text.text = currentSubtitle.Text;

          // And fade out the old one. Yield on this one to wait for the fade to finish before doing anything else.
          StartCoroutine(FadeTextOut(fadedOutText));

          // Yield a bit for the fade out to get part-way
          yield return new WaitForSeconds(FadeTime / 3);

          // Fade in the new current
          yield return FadeTextIn(Text);
        }
        yield return null;
      }
      else
      {
        //Debug.Log("Subtitles ended");
        StartCoroutine(FadeTextOut(Text));
        yield return FadeTextOut(fadedOutText);
        Text.gameObject.SetActive(false);
        fadedOutText.gameObject.SetActive(false);
        yield break;
      }
    }
  }

  void OnValidate()
  {
    FadeTime = (int)(FadeTime * 10) / 10f;
  }

  public IEnumerator FadeTextOut(TextMeshProUGUI text)
  {
    var toColor = text.color;
    toColor.a = 0;
    yield return null;
  }

  public IEnumerator FadeTextIn(TextMeshProUGUI text)
  {
    var toColor = text.color;
    toColor.a = 1;
    yield return null;
  }

  IEnumerator Fade(TextMeshProUGUI text, Color toColor, Ease ease)
  {
    yield return DOTween.To(() => text.color, color => text.color = color, toColor, FadeTime).SetEase(ease).WaitForCompletion();
  }

  public void Seek()
  {
    StopAllCoroutines();
    StartCoroutine(Begin());
  }
}
