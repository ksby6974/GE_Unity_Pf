using UnityEngine;
using Random = UnityEngine.Random;
using chataan.Scripts.Data.Scene;
using chataan.Scripts.Data.Encounter;
using chataan.Scripts.Data.Settings;
using chataan.Scripts.Gets;
using chataan.Scripts.Chara;
using chataan.Scripts.Data.Card;
using chataan.Scripts.Card;
using chataan.Scripts.Managers;

// ����������������������������������������������������
// ������ �ٽ� Ŭ����
// ���� ���� ����
// ����������������������������������������������������
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
    public SavePlayData SavePlayData { get; private set; }
    protected UIManager UIManager => UIManager.Instance;

    // ��������������������������������������������������
    // �̱��� ���
    // ��������������������������������������������������
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
            SetCardAction.Initialize();
            SetEnemyAction.Initialize();
            InitPlayData();
            InitHand();
        }
    }

    // ��������������������������������������������������
    // ���� ������ �ʱ�ȭ
    // ��������������������������������������������������
    public void InitPlayData()
    {
        SavePlayData = new SavePlayData(playData);
        if (UIManager)
        {
            UIManager.InfoCanvas.ResetCanvas();
        }
    }

    // ��������������������������������������������������
    // ī�� �ʱ�ȭ
    // ��������������������������������������������������
    public CardBase BuildAndGetCard(CardData targetData, Transform parent)
    {
        var clone = Instantiate(PlayData.CardPrefab, parent);
        clone.SetCard(targetData);
        return clone;
    }

    // ��������������������������������������������������
    // �ڵ� �ʱ�ȭ
    // ��������������������������������������������������
    public void InitHand()
    {
        SavePlayData.CurrentCardsList.Clear();

        if (SavePlayData.IsRandomHand)
        {
            for (var i = 0; i < PlayData.RandomCardCount; i++)
            {
                SavePlayData.CurrentCardsList.Add(PlayData.AllCardsList.GetRandomItem());
            }
        }      
        else
        {
            foreach (var cardData in PlayData.InitalDeck.CardList)
            {
                SavePlayData.CurrentCardsList.Add(cardData);
            }
        }
    }

    // ��������������������������������������������������
    // ���� ���� ����
    // ��������������������������������������������������
    public void NextEncounter()
    {
        SavePlayData.CurrentEncounterId++;
        if (SavePlayData.CurrentEncounterId >= EncounterData.EnemyEncounterList[SavePlayData.CurrentStageId].EnemyEncounterList.Count)
        {
            SavePlayData.CurrentEncounterId = Random.Range(0,EncounterData.EnemyEncounterList[SavePlayData.CurrentStageId].EnemyEncounterList.Count);
        }
    }

    // ��������������������������������������������������
    // ���α׷� �ݱ�
    // ��������������������������������������������������
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
