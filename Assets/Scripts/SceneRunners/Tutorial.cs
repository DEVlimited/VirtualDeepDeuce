using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //begin subtitles
        StartCoroutine(FindObjectOfType<SubtitleDisplayer>().Begin());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneSwitch(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
