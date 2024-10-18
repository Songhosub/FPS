using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour
{
    Button button;
    Text txt;
    int txtSize;
    Vector3 CollierSize;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        txt = GetComponentInChildren<Text>();
        txtSize = txt.fontSize;

        //콜라이더의 크기를 UI의 크기와 돌일하게 만듭니다.
        CollierSize = new Vector3(GetComponent<RectTransform>().sizeDelta.x, GetComponent<RectTransform>().sizeDelta.y, 0);
        GetComponent<BoxCollider>().size = CollierSize;
    }
    private void OnMouseEnter()
    {
        //텍스트의 크기를 증가
        txt.fontSize = (int)(txtSize * 1.5f);
    }

    private void OnMouseExit()
    {
        //텍스트의 크기를 복구
        txt.fontSize = txtSize;
    }


    // Update is called once per frame
    void Update()
    {

    }


}
