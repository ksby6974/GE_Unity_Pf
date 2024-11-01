using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 가장 먼저 실행
[DefaultExecutionOrder(-10)]
public class CenterManager : MonoBehaviour
{
    public CenterManager()
    {
    }

    public static CenterManager Instance
    {
        get;
        private set;
    }

    //[Header("Data")]
    // [SerializeField] private SceneData sceneData;

    protected UIManager UIManager => UIManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
