using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingNextScene : MonoBehaviour
{
    public static string nextScene;
    public Slider LoadingBar;
    public Text LoadingTxt;

    void Start()
    {
        StartCoroutine(TansitionNextScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator TansitionNextScene()
    {
        //저장된 씬을 비동기 혀익으로 로드
        AsyncOperation ao = SceneManager.LoadSceneAsync(nextScene);

        //로드되는 씬의 모습이 화면에 보이지 않게 됨
        ao.allowSceneActivation = false;

        float timer = 0.0f;
        while (!ao.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (ao.progress < 0.9f)
            {
                LoadingBar.value = Mathf.Lerp(LoadingBar.value, ao.progress, timer);
                if (LoadingBar.value >= ao.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                LoadingBar.value = Mathf.Lerp(LoadingBar.value, 1f, timer);
                if (LoadingBar.value == 1.0f)
                {
                    ao.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    // Start is called before the first frame update




    // Update is called once per frame
    void Update()
    {
        
    }
}
