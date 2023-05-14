using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeepDeuce : MonoBehaviour
{
    public Transform vrRig;
    public Transform TheaterTrigger;
    public Transform DominosTrigger;

    public float distanceToTeleport;

    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private Transform endPos;

    public float speed = 1.0f;

    private float startTime;

    private float journeyLength;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if player gets 'distanceToTeleport' from the theater they will load the theater scene
        if (Vector3.Distance(vrRig.position, TheaterTrigger.position) < distanceToTeleport)
        {
            SceneManager.LoadScene(3);
        }
        //if player gets 'distanceToTeleport' from the theater they will load the theater scene
        if (Vector3.Distance(vrRig.position, DominosTrigger.position) < distanceToTeleport)
        {
            SceneManager.LoadScene(2);
        }
    }
    public void Lerp(Transform start, Transform end)
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        vrRig.transform.position = Vector3.Lerp(start.position, end.position, fractionOfJourney);
    }
}
