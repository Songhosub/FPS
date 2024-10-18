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
        //��ĭ�� �����ϴ����� Ȯ��
        if (CheckInput(Id.text, Pw.text))
        {
            //���� ���� ���� ���� ǥ��
            if (PlayerPrefs.HasKey(Id.text))
            {
                LoginError.text = "�̹� �����ϴ� ���̵��Դϴ�.";
            }

            //���� ���� �˸� ǥ��
            else
            {
                PlayerPrefs.SetString(Id.text, Pw.text);
                LoginError.text = "���̵� �����Ǿ����ϴ�.";
            }
        }
    }

    void IDChack()
    {
        //��ĭ�� �����ϴ����� Ȯ��
        if (CheckInput(Id.text, Pw.text))
        {
            //���� �� �� ��ȯ
            if (PlayerPrefs.HasKey(Id.text) &&
                PlayerPrefs.GetString(Id.text) == Pw.text)
            {
                LoadingNextScene.LoadScene("Scene");
            }
            //���� �� ���� ���� ǥ��
            else
            {
                LoginError.text = "���̵� �Ǵ� �н����尡 ��ġ���� �ʽ��ϴ�.";
            }
        }
    }

    bool CheckInput(string ID, string PW)
    {
        if(ID == "" || PW == "")
        {
            LoginError.text = "���̵� �Ǵ� �н����带 �Է����ּ���.";
            return false;
        }
        else
        {
            return true;
        }
    }
}
