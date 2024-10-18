using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    Speaker[] speakers;
    [SerializeField]
    DialogData[] dialogs;
    [SerializeField]
    bool isFirst = true;
    int currentDialogIndex = -1;
    int currentSpeakerIndex = 0;
    float typingSpeed = 0.095f;
    bool isTypingEffect;

    private void Awake()
    {
        Setup();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(()=>UpdateDialog());
    }

    private void Setup()
    {
        for(int i = 0; i < speakers.Length; i++)
        {
            SetActiveObject(speakers[i], false);
            speakers[i].spriteRenderer.gameObject.SetActive(true);
        }
    }

    public bool UpdateDialog()
    {
        if (isFirst == true)
        {
            Setup();
            SetNextDialog();
            isFirst = false;
        }

        if (isTypingEffect == false)
        {
            if (dialogs.Length > currentDialogIndex + 1)
            {
                SetNextDialog();
            }

            else
            {
                for (int i = 0; i < speakers.Length; i++)
                {
                    SetActiveObject(speakers[i], false);
                    speakers[i].spriteRenderer.gameObject.SetActive(false);
                }

                return true;
            }
        }
        return false;
    }

    private void SetNextDialog()
    {
        SetActiveObject(speakers[currentSpeakerIndex], false);
        currentDialogIndex++;
        currentSpeakerIndex = dialogs[currentDialogIndex].SpeakerIndex;
        SetActiveObject(speakers[currentSpeakerIndex], true);
        StartCoroutine("OnTypingText");
    }

    private void SetActiveObject(Speaker speaker, bool visible)
    {
        speaker.imageDialog.gameObject.SetActive(visible);
        speaker.textName.gameObject.SetActive(visible);
        speaker.textDialog.gameObject.SetActive(visible);

        Color color = speaker.spriteRenderer.color;
        color.a = visible == true ? 1 : 0.2f;
        speaker.spriteRenderer.color = color;
    }

    IEnumerator OnTypingText()
    {
        int index = 0;
        isTypingEffect = true;
        while (index < dialogs[currentDialogIndex].dialog.Length)
        {
            speakers[currentSpeakerIndex].textDialog.text = dialogs[currentDialogIndex].dialog.Substring(0, index);
            index++;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(1.5f + dialogs[currentDialogIndex].dialog.Length * 0.055f);

        isTypingEffect = false;
    }

    [System.Serializable]
    public struct Speaker
    {
        public Image imageDialog;
        public Image spriteRenderer;
        public Text textName;
        public Text textDialog;
    }

    [System.Serializable]
    public struct DialogData
    {
        public int SpeakerIndex;
        [TextArea]
        public string dialog;
    }

}
