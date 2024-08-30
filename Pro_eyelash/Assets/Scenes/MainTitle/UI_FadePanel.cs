using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FadePanel : MonoBehaviour
{
    [SerializeField] bool bOn;
    [SerializeField] GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        obj = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Draw_PanelOff()
    {
        bOn = false;
        obj.SetActive(false);
    }
}
