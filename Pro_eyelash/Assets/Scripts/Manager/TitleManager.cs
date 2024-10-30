using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI [] textPros;

    // Start is called before the first frame update
    void Start()
    {
        SetMenuText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetMenuText()
    {
        textPros[0].text = "START";
        textPros[1].text = "LIBRARY";
        textPros[2].text = "OPTION";
        textPros[3].text = "EXIT";
    }
}
