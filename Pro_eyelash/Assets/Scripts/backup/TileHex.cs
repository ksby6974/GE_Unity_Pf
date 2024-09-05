using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileHex : MonoBehaviour
{
    [SerializeField] int iTileIndex;
    [SerializeField] int iType;
    [SerializeField] float fX;
    [SerializeField] float fY;
    public Canvas canvas;
    [SerializeField] TextMeshProUGUI textPro;

    // Start is called before the first frame update
    void Awake()
    {
        iTileIndex = -1;
        canvas = GetComponentInChildren<Canvas>();
        textPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Set_Text();
    }

    public void SetInit(int index, float x, float y)
    {
        iTileIndex = index;
        Debug.Log(iTileIndex);
        SetPosition(x,y);
    }

    public void SetPosition(float x, float y)
    {
        this.fX = x;
        this.fY = y;
    }

    public void Set_Text()
    {
        textPro.text = $"{iTileIndex}";
        textPro.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, 0);
    }
}
