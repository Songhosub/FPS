using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginSceneManager : MonoBehaviour
{
    public Button gameStart;
    public Button gameQuit;

    public Button guestStart;
    public Button loginStart;

    public GameObject LoginPop;
    public GameObject StartPop;

    // Start is called before the first frame update
    void Start()
    {
        LoginPop.SetActive(false);
        StartPop.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoginPopSetUp()
    {
        LoginPop.SetActive(true);
        guestStart.gameObject.SetActive(false);
        loginStart.gameObject.SetActive(false);
    }

    public void StartPopSetUp()
    {
        StartPop.SetActive(true);
        gameStart.gameObject.SetActive(false);
        gameQuit.gameObject.SetActive(false);
    }

    public void LoginPopSetDown()
    {
        LoginPop.SetActive(false);
        guestStart.gameObject.SetActive(true);
        loginStart.gameObject.SetActive(true);
    }

    public void StartPopSetDown()
    {
        StartPop.SetActive(false);
        gameStart.gameObject.SetActive(true);
        gameQuit.gameObject.SetActive(true);
    }

    public void GameStart()
    {
        LoadingNextScene.LoadScene("Scene");
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
