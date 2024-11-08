using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuButtons : MonoBehaviour
{
    enum MenuText
    {
        Start = 0,
        Record = 1,
        Option = 2,
        Exit = 3,
    }

    [SerializeField] TextMeshProUGUI textproText;
    [SerializeField] Color32 defaultcolor;
    [SerializeField] int iIndex;

    void Awake()
    {
        SetMenuText((MenuText)iIndex);
        defaultcolor = textproText.color;
        OnPointerExit();
    }

    private void SetMenuText(MenuText index)
    {
        textproText.text = $"{index}";
    }

    public void OnPointerEnter()
    {
        textproText.color = Color.white;
        textproText.alpha = 0.8f;
    }

    public void OnPointerExit()
    {
        textproText.color = defaultcolor;
        textproText.alpha = 0.75f;
    }
}
