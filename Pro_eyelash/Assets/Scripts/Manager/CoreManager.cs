using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// ���� ���� ����
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

    }
}
