using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreenBarSystem : MonoBehaviour {

    public GameObject bar;
    private int currentSceneBuildIndexInt;
    public Text loadingText;
    public bool backGroundImageAndLoop;
    public float LoopTime;
    public GameObject[] backgroundImages;
    [Range(0,1f)]public float vignetteEfectValue; // Must be a value between 0 and 1
    AsyncOperation async;
    Image vignetteEfect;


    public void loadingScreen (int sceneNo)
    {
        gameObject.SetActive(true);
        StartCoroutine(Loading(sceneNo));
    }

    // Used to beta test the loading bar. Delete the comment lines (25 and 36). Use "Space" in the playtest to fill the load value.
    /*
    public void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            bar.transform.localScale += new Vector3(0.001f,0,0);

            if (loadingText != null)
                loadingText.text = "%" + (100 * bar.transform.localScale.x).ToString("####");
        }
    }
    */

    private void Start()
    {
        currentSceneBuildIndexInt = SceneManager.GetActiveScene().buildIndex;
        vignetteEfect = transform.Find("VignetteEfect").GetComponent<Image>();
        vignetteEfect.color = new Color(vignetteEfect.color.r,vignetteEfect.color.g,vignetteEfect.color.b,vignetteEfectValue);

        if (backGroundImageAndLoop)
            StartCoroutine(transitionImage());
    }


    // The pictures change according to the time of
    IEnumerator transitionImage ()
    {
        for (int i = 0; i < backgroundImages.Length; i++)
        {
            yield return new WaitForSeconds(LoopTime);
            for (int j = 0; j < backgroundImages.Length; j++)
                backgroundImages[j].SetActive(false);
            backgroundImages[i].SetActive(true);           
        }
    }
    // Activate the scene 
    IEnumerator Loading (int sceneNo)
    {
        if(PlayerPrefs.GetString("Menu First Time") == "true") 
        {
            //Debug.Log("Tutorial Called from loading screen class");
            PlayerPrefs.SetString("Menu First Time", "false");
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            async = SceneManager.LoadSceneAsync(sceneNo);
        async.allowSceneActivation = false;
        // Continue until the installation is completed
        float asyncProgress = 0.0f;
        while (async.isDone == false)
        {
            bar.transform.localScale = new Vector3(async.progress,0.9f,1);

            if (loadingText != null)
                loadingText.text = "%" + (100 * bar.transform.localScale.x).ToString("####");

            if (async.progress > 0.0f)
            {
                asyncProgress = async.progress;
                bar.transform.localScale = new Vector3(1, asyncProgress, 1);
                async.allowSceneActivation = true;
            }
            yield return null;
        }
        }
    }

}
