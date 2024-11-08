using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using chataan.Scripts.Data.Scene;
using chataan.Scripts.Data.Play;
using chataan.Scripts.Data.Encounter;

// ���� ���� ����
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

    // ���� ������ ���� �ʱ�ȭ
    public void InitAllData()
    {
        InitHand();
    }

    // ī��

    // �ڵ� �ʱ�ȭ
    public void InitHand()
    {

    }

    // ���α׷� �ݱ�
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
