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

        //�ݶ��̴��� ũ�⸦ UI�� ũ��� �����ϰ� ����ϴ�.
        CollierSize = new Vector3(GetComponent<RectTransform>().sizeDelta.x, GetComponent<RectTransform>().sizeDelta.y, 0);
        GetComponent<BoxCollider>().size = CollierSize;
    }
    private void OnMouseEnter()
    {
        //�ؽ�Ʈ�� ũ�⸦ ����
        txt.fontSize = (int)(txtSize * 1.5f);
    }

    private void OnMouseExit()
    {
        //�ؽ�Ʈ�� ũ�⸦ ����
        txt.fontSize = txtSize;
    }


    // Update is called once per frame
    void Update()
    {

    }


}
