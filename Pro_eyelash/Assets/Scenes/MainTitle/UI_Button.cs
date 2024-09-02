using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Button : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textpro;
    [SerializeField] TextMeshProUGUI textpro_1;
    [SerializeField] float fSpace;
    float fLimit = 10;

    // Start is called before the first frame update
    void Start()
    {
        SetButton(false);
        textpro = GetComponentInChildren<TextMeshProUGUI>();
        textpro_1 = GetComponentInChildren<TextMeshProUGUI>();
        fSpace = 0;
    }

    // Update is called once per frame
    void Update()
    {
        EffectText();
    }

    public void SetButton(bool b)
    {
        if (b == true)
        {
            textpro.color = new Color32(255, 255, 255, 255);
            textpro.characterSpacing = 30;
            fSpace = 0.1f;
        }
        else
        {
            textpro_1.text = textpro.text;
            textpro.color = new Color32(22, 22, 22, 255);
            textpro.characterSpacing = 0;
            fSpace = 0;
        }
    }

    public void EffectText()
    {
        if (fSpace > 0)
            fSpace += (0.4f * Time.deltaTime);

        if (textpro.characterSpacing > fLimit)
        {
            textpro.characterSpacing -= fSpace;
        }
        else
        {
            textpro.characterSpacing = fLimit;
            fSpace = 0;
        }
    }

    IEnumerator Draw_TextEffect()
    {

        yield return new WaitForSeconds(1f);

    }
}
