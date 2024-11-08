using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using chataan.Scripts.Data.Scene;
using chataan.Scripts.Data.Play;
using chataan.Scripts.Data.Encounter;

// 가장 먼저 실행
[DefaultExecutionOrder(-10)]
public class CoreManager : Singleton<CoreManager>
{
    [Header("Data")]
    [SerializeField] private PlayData playData;
    [SerializeField] private EncounterData encounterData;
    [SerializeField] private SceneData sceneData;

    public SceneData SceneData => sceneData;
    public EncounterData EncounterData => encounterData;
    public PlayData PlayData => playData;
   // public PersistentGameplayData PersistentGameplayData { get; private set; }
    protected UIManager UIManager => UIManager.Instance;

    private new void Awake()
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
        Debug.Log("Game End");
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
              Application.Quit();
        #endif
    }
}
