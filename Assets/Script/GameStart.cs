using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameStart : MonoBehaviour {
    
    public void OnClick()
    {
        AudioSource au = GetComponent<AudioSource>();
        au.Play();

        Invoke("NextScene", 2.0f);
    }

    private void NextScene()
    {
        SceneManager.LoadScene("PlayingScene");
    }

    public void restart_Button()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
