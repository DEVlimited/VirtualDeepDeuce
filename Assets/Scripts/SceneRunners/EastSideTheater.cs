using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class EastSideTheater : MonoBehaviour
{
    //narrative pieces
    private float currentTime;
    public float textAdvanceDelay;
    private float timeToAdvance;

    private int currentNarrativeScene = 0;

    public string[] narrativeTextChoices;

    public TMP_Text narrativeText;

    public Material imageMaterial;
    public Texture[] images;

    //object handling
    [SerializeField]
    private Transform curtain;
    [SerializeField]
    private Transform closedPos;
    [SerializeField]
    private Transform openPos;

    public float speed = 1.0f;

    private float startTime;

    private float journeyLength;

    private bool isOpening = false;
    private bool isClosing = false;
    private bool isClosed = true;
    private bool isOpen = false;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        timeToAdvance = textAdvanceDelay;


        startTime = Time.time;
        isClosed = true;
        journeyLength = Vector3.Distance(closedPos.position, openPos.position);
        curtain = closedPos;
        isOpening = true;

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        timeToAdvance -= Time.deltaTime;

        if (timeToAdvance < 0)
        {
            AdvanceNarrative(currentNarrativeScene);
        }
        if (currentTime > 45)
        {
            SceneManager.LoadScene(1);
        }
        CurtainOpenClose();
    }

    public void CurtainOpenClose()
    {

        if (isClosing)
        {
            Lerp(openPos, closedPos);

        }
        if (isOpening)
        {
            Lerp(closedPos, openPos);

        }
        if (curtain.transform.position == openPos.position)
        {
            isClosed = false;
            isOpen = true;
            isClosing = false;
            isOpening = false;
        }
        if (curtain.transform.position == closedPos.position)
        {
            isClosed = true;
            isOpen = false;
            isClosing = false;
            isOpening = false;
        }
    }
    public void Lerp(Transform start, Transform end) // move object from 'start' to 'end'
    {
        journeyLength = Vector3.Distance(closedPos.position, openPos.position);
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        curtain.transform.position = Vector3.Lerp(start.position, end.position, fractionOfJourney);
    }
    public void AdvanceNarrative(int scene)
    {
        Debug.Log("Advance called");
        //update time
        timeToAdvance = textAdvanceDelay;

        //update text
        narrativeText.text = narrativeTextChoices[scene];

        //update image
        imageMaterial.SetTexture("_MainTex", images[scene]);

        //update scene
        currentNarrativeScene += 1;

    }
}
