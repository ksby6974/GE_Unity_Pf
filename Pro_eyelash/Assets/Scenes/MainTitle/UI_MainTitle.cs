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
        iCursor = 0;
        panel = GetComponent<GameObject>();
        button = GetComponentsInChildren<Button>();
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
            if (fWaiting > 0)
            {
                fWaiting -= 1f * Time.deltaTime;
            }
            else
            {
                panel.GetComponentInChildren<UI_FadePanel>().Draw_PanelOff();
                bCanSelect = true;
                button[iCursor].GetComponentInChildren<UI_Button>().SetButton(true);
            }
        }
    }

    void MoveCursor()
    {
        if (bCanSelect == false)
            return;

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
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
            if (iCursor <= 0)
            {
                iCursor = iLimit;
            }
            else
            {
                iCursor -= 1;
            }
        }

        Show(iCursor);
    }

    void Show(int input)
    {
        for (int i = 0; i < button.Length; i++)
        {
            if (input == i)
            {
               // button[input].GetComponent<UI_Button>().SetButton(true);
            }
            else
            {
               // button[input].GetComponent<UI_Button>().SetButton(false);
            }
        }
    }
}
