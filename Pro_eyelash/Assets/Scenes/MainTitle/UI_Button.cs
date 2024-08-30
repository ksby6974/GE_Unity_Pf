using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Button : MonoBehaviour
{
    [SerializeField] bool bOn;
    [SerializeField] TextMeshProUGUI textpro;
    [SerializeField] float fSpace;

    // Start is called before the first frame update
    void Start()
    {
        SetButton(false);
        textpro = GetComponentInChildren<TextMeshProUGUI>();
        fSpace = 0;

        Debug.Log($"{textpro.text} : {textpro.color}");
    }

    // Update is called once per frame
    void Update()
    {
        EffectText();
    }

    public void SetButton(bool b)
    {
       // Debug.Log($"{textpro.text} : SetButton");

        if (b == true)
        {
            textpro.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            textpro.color = new Color32(22, 22, 22, 255);
            textpro.characterSpacing = 20;
        }

        bOn = b;
    }

    public void EffectText()
    {
        if (bOn == true)
        {
            if (fSpace > 0)
            {
                fSpace -= 0.05f * Time.deltaTime;
                //Debug.Log($"{textpro.text} : {fSpace}");
            }
            textpro.characterSpacing = fSpace;
        }
    }
}
