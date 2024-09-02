using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadePanel : MonoBehaviour
{
    [SerializeField] bool bOn;
    [SerializeField] GameObject obj;
    float fLimit;
    byte Alpha;

    void Awake()
    {
        fLimit = 255;
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
        if (b == true)
        {

        }
        else
        {

        }

        obj.GetComponent<Image>().color = new Color32(0,0,0,Alpha);
    }

    public void Draw_PanelOn()
    {
        bOn = true;
        obj.SetActive(true);
    }

    public void Draw_PanelOff()
    {
        bOn = false;
        obj.SetActive(false);
    }
}
