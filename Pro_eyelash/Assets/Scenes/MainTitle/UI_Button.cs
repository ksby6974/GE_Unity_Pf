using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Button : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textpro_Main;
    [SerializeField] TextMeshProUGUI textpro_1;
    [SerializeField] float fSpace;
    [SerializeField] bool bEffect_Blink;
    float fLimit = 10;

    // Start is called before the first frame update
    void Start()
    {
        SetButton(false);
        //textpro_Main = GetComponentInChildren<TextMeshProUGUI>();
        //textpro_1 = GetComponentInChildren<TextMeshProUGUI>();
        fSpace = 0;
        bEffect_Blink = false;
    }

    // Update is called once per frame
    void Update()
    {
        EffectText();

        if (bEffect_Blink)
        {
            //StartCoroutine(Draw_TextEffect_Blink());
        }
    }

    public void SetButton(bool b)
    {
        if (b == true)
        {
            textpro_1.text = null;
            textpro_Main.color = new Color32(255, 255, 255, 255);
            textpro_Main.characterSpacing = 30;
            fSpace = 0.1f;
            bEffect_Blink = true;
        }
        else
        {
            textpro_1.text = null;
            textpro_Main.color = new Color32(22, 22, 22, 255);
            textpro_Main.characterSpacing = 0;
            fSpace = 0;
            bEffect_Blink = false;
        }
    }

    public void EffectText()
    {
        if (fSpace > 0)
            fSpace += (0.4f * Time.deltaTime);

        if (textpro_Main.characterSpacing > fLimit)
        {
            textpro_Main.characterSpacing -= fSpace;
        }
        else
        {
            textpro_Main.characterSpacing = fLimit;
            fSpace = 0;
        }
    }

    public IEnumerator Draw_TextEffect_Blink()
    {
        while (bEffect_Blink)
        {
            Color c = textpro_Main.color;
            var fTime = 0.25f;

            //fade in
            var fade = 0f;
            while (fade < fTime)
            {
                yield return null;
                fade += Time.deltaTime;

                c.a = Mathf.Lerp(0f, 1f, fade / fTime);
                textpro_Main.color = c;
            }

            yield return new WaitForSecondsRealtime(0.1f);

            fade = 0f;
            while (fade < fTime)
            {
                yield return null;
                fade += Time.deltaTime;

                c.a = Mathf.Lerp(1f, 0f, fade / fTime);
                textpro_Main.color = c;
            }

            Debug.Log($"불:{bEffect_Blink} 투명도:{textpro_Main.color.a}");
        }
    }

    IEnumerator Draw_TextEffect_Blink2()
    {
        Color c = textpro_Main.color;
        var fTime = 0.25f;

        //fade in
        var fade = 0f;
        while (fade < fTime)
        {
            yield return null;
            fade += Time.deltaTime;

            c.a = Mathf.Lerp(0f, 1f, fade / fTime);
            textpro_Main.color = c;
        }

        yield return new WaitForSecondsRealtime(0.5f);

        fade = 0f;
        while (fade < fTime)
        {
            yield return null;
            fade += Time.deltaTime;

            c.a = Mathf.Lerp(1f, 0f, fade / fTime);
            textpro_Main.color = c;
        }

        Debug.Log($"{textpro_Main.color.a}");
    }

    IEnumerator Draw_TextEffect()
    {
        int iTemp = 100;

        while (iTemp > 0)
        {
            iTemp--;
            float fTemp = Random.Range(1f, 3f);

            textpro_1.fontSize = Random.Range(60, 90);
            //textpro_1.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);

            Debug.Log($"주인:{textpro_Main.text} , 반복:{iTemp} , 크기:{textpro_1.fontSize}");

            yield return new WaitForSecondsRealtime(fTemp);
        }
    }
}
