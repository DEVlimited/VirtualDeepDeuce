using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString("Hand") == null)
        {
            PlayerPrefs.SetString("Hand", "Right");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void AppExit()
    {
        Application.Quit();
    }
}
