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
        //����� ���� �񵿱� �������� �ε�
        AsyncOperation ao = SceneManager.LoadSceneAsync(nextScene);

        //�ε�Ǵ� ���� ����� ȭ�鿡 ������ �ʰ� ��
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
