using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] int iTileIndex;
    [SerializeField] int iType;
    [SerializeField] float fX;
    [SerializeField] float fY;
    public Canvas canvas;
    [SerializeField] TextMeshProUGUI textPro;

    // Start is called before the first frame update
    void Start()
    {
        iTileIndex = -1;
        textPro.text = $"{iTileIndex}";
        canvas = GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition_Text();
    }

    public void SetInit(int index, float x, float y)
    {
        iTileIndex = index;
        SetPosition(x,y);
    }

    public void SetPosition(float x, float y)
    {
        this.fX = x;
        this.fY = y;
    }

    public void SetPosition_Text()
    {
        //textPro.transform = this.transform;
    }
}
