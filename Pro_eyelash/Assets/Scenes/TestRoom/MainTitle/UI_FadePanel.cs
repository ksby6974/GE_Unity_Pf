using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadePanel : MonoBehaviour
{
    [SerializeField] bool bOn;
    [SerializeField] GameObject obj;
    [SerializeField] byte Alpha;

    void Awake()
    {
        Alpha = 255;
        obj = this.gameObject;
        bOn = true;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Draw(bOn);
    }

    public void Draw(bool b)
    {
        byte Temp = 1;

        if (b == true)
        {
            if (Alpha < 255)
            {
                Alpha += Temp;
            }
            else
            {
                Alpha = 255;
            }
        }
        else
        {
            if (Alpha > 0)
            {
                Alpha -= Temp;
            }
            else
            {
                Alpha = 0;
            }
        }

        obj.GetComponent<Image>().color = new Color32(0,0,0,Alpha);
        //obj.SetActive(false);
    }

    public void Draw_PanelOn()
    {
        Alpha = 0;
        bOn = true;
    }

    public void Draw_PanelOff()
    {
        Alpha = 255;
        bOn = false;
    }
}
