using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField Id;
    public InputField Pw;

    public Text LoginError;

    public Button IDCreatBtn;
    public Button IDChackBtn;

    // Start is called before the first frame update
    void Start()
    {
        LoginError.text = "";

        IDCreatBtn.onClick.AddListener(IDCreat);
        IDChackBtn.onClick.AddListener(IDChack);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IDCreat()
    {
        //빈칸이 존재하는지를 확인
        if (CheckInput(Id.text, Pw.text))
        {
            //오류 시의 오류 문구 표시
            if (PlayerPrefs.HasKey(Id.text))
            {
                LoginError.text = "이미 존재하는 아이디입니다.";
            }

            //성공 시의 알림 표시
            else
            {
                PlayerPrefs.SetString(Id.text, Pw.text);
                LoginError.text = "아이디가 생성되었습니다.";
            }
        }
    }

    void IDChack()
    {
        //빈칸이 존재하는지를 확인
        if (CheckInput(Id.text, Pw.text))
        {
            //성공 시 씬 전환
            if (PlayerPrefs.HasKey(Id.text) &&
                PlayerPrefs.GetString(Id.text) == Pw.text)
            {
                LoadingNextScene.LoadScene("Scene");
            }
            //오류 시 오류 문구 표시
            else
            {
                LoginError.text = "아이디 또는 패스워드가 일치하지 않습니다.";
            }
        }
    }

    bool CheckInput(string ID, string PW)
    {
        if(ID == "" || PW == "")
        {
            LoginError.text = "아이디 또는 패스워드를 입력해주세요.";
            return false;
        }
        else
        {
            return true;
        }
    }
}
