using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainTitle : MonoBehaviour
{
    int iLimit = 3;
    [SerializeField] Button [] button;
    [SerializeField] int iCursor;
    [SerializeField] float fWaiting;
    [SerializeField] bool bCanSelect;
    [SerializeField] GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        bCanSelect = false;
        fWaiting = 1f;
        iCursor = -1;
        //panel = GetComponent<GameObject>();
        button = GetComponentsInChildren<Button>();

        Debug.Log($"{panel}");
    }

    // Update is called once per frame
    void Update()
    {
        DrawFade();
        MoveCursor();
    }

    void DrawFade()
    {
        if (bCanSelect == false)
        {
            if (fWaiting <= 0)
            {
                panel.GetComponent<UI_FadePanel>().Draw_PanelOff();
                bCanSelect = true;
                iCursor = 0;
                button[iCursor].GetComponentInChildren<UI_Button>().SetButton(true);
            }
            else
            {
                fWaiting -= 1f * Time.deltaTime;
            }
        }
    }

    void MoveCursor()
    {
        //
        if (bCanSelect == false)
            return;

        // 이동 확인
        bool bTemp = false;

        // 목록 이동
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            bTemp = true;

            if (iCursor >= iLimit)
            {
                iCursor = 0;
            }
            else
            {
                iCursor += 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            bTemp = true;

            if (iCursor <= 0)
            {
                iCursor = iLimit;
            }
            else
            {
                iCursor -= 1;
            }
        }

        // 출력
        if (bTemp == true)
            Show(iCursor);
    }

    void Show(int input)
    {
        for (int i = 0; i < button.Length; i++)
        {
            if (input == i)
            {
                button[i].GetComponent<UI_Button>().SetButton(true);
            }
            else
            {
               button[i].GetComponent<UI_Button>().SetButton(false);
            }
        }
    }
}
