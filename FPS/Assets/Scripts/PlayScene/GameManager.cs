using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    static public GameManager GM;

    public enum GameState
    {
        Play,
        Stop
    };

    public GameState gameState;

    //시네마신
    public GameObject StartCinemachine;
    public static bool Cinema = true;
    

    //UI
    public Image HitEffect;
    public TMP_Text HPTxt;
    public Slider HPSlider;
    public Image HPSliderImage;
    public TMP_Text MagTxt;
    public TMP_Text BombCount;
    public Image LoadingImage;
    public Image RifleImage;
    public Image BombImage;
    public Image SniperImage;
    public Image ClearPop;
    public Image DiePop;
    public Image MenuPop;

    //상태 확인
    public List<GameObject> Monster = new List<GameObject>();

    private void Awake()
    {
        GM = this;
        if(!Cinema)
        {
            //시네머신이 일어나지 않을 때 UFO 오브젝트가 다른 곳에 위치하는 오류를 방지
            GameObject.Find("Obstacle/UFO").GetComponent<UFOMove>().Move();
        }
        //시네머신이 1회만 작동하도록 활성화 설정
        StartCinemachine.SetActive(Cinema);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Play;

        HitEffect.gameObject.SetActive(false);

        LoadingImage.gameObject.SetActive(false);
        RifleImage.gameObject.SetActive(true);
        BombImage.gameObject.SetActive(false);
        SniperImage.gameObject.SetActive(false);

        ClearPop.gameObject.SetActive(false);
        DiePop.gameObject.SetActive(false);
        MenuPop.gameObject.SetActive(false);

        Cinema = false;

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu();
        }

        if(Monster.Count <= 0)
        {
            GameClaer();
        }
    }

    public void HPUI(int hp, int maxHp)
    {
        HPSlider.value = (float)hp / (float)maxHp;
        HPTxt.text = "" + hp;
        if(hp < (maxHp / 3))
        {
            HPTxt.color = Color.red;
            HPSliderImage.color = Color.red;
            HitEffect.gameObject.SetActive(true);
        }
    }

    public void MagText(int mag)
    {
        if(mag == -10)
        {
            MagTxt.text = "-";
        }
        else
        {
            MagTxt.text = "" + mag;
        }
    }

    public void CountText(int count)
    {
        BombCount.text = "" + count;
    }
    public void ModeImage(string mode, float time)
    {
        StartCoroutine(ModeImageCC(mode, time));
    }
    public void Hit()
    {
        StartCoroutine(HitCC());
    }

    IEnumerator ModeImageCC(string mode, float time)
    {
        LoadingImage.gameObject.SetActive(true);
        RifleImage.gameObject.SetActive(false);
        BombImage.gameObject.SetActive(false);
        SniperImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(time);
        LoadingImage.gameObject.SetActive(false);
        if (mode == "Rifle")
        {
            RifleImage.gameObject.SetActive(true);
        }
        if (mode == "Bomb")
        {
            BombImage.gameObject.SetActive(true);
        }
        if (mode == "Sniper")
        {
            SniperImage.gameObject.SetActive(true);
        }
    }

    IEnumerator HitCC()
    {
        HitEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        HitEffect.gameObject.SetActive(false);
    }

    public void ReturnToMain()
    {
        LoadingNextScene.LoadScene("LogInScene");
    }

    public void Restart()
    {
        gameState = GameState.Play;
        Cursor.visible = false;
        Cinema = false;
        LoadingNextScene.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Return()
    {
        gameState = GameState.Play;
        Cursor.visible = false;
        MenuPop.gameObject.SetActive(false);
    }

    void Menu()
    {
        gameState = GameState.Stop;
        Cursor.visible = true;
        MenuPop.gameObject.SetActive(true);
    }

    void GameClaer()
    {
        gameState = GameState.Stop;
        Cursor.visible = true;
        ClearPop.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        gameState = GameState.Stop;
        Cursor.visible = true;
        DiePop.gameObject.SetActive(true);
    }
}
