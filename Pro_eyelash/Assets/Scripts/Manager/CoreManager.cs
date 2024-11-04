using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// 가장 먼저 실행
[DefaultExecutionOrder(-10)]

//public class CoreManager : Singleton<CoreManager>

public class CoreManager : MonoBehaviour
{
    public CoreManager()
    {
    }

    public static CoreManager Instance
    {
        get;
        private set;
    }

    //[Header("Data")]
    // [SerializeField] private SceneData sceneData;

    protected UIManager UIManager => UIManager.Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //CardActionProcessor.Initialize();
            //EnemyActionProcessor.Initialize();
            //InitGameplayData();
            //SetInitalHand();
        }
    }

    // 게임 데이터 전부 초기화
    public void InitAllData()
    {
        InitHand();
    }

    // 카드

    // 핸드 초기화
    public void InitHand()
    {

    }

    // 프로그램 닫기
    public void ExitGame()
    {

    }
}
