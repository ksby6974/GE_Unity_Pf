using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textproText;

    void Awake()
    {
        textproText = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
