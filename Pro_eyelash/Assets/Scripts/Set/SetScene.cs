using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetScene : MonoBehaviour
{
    private enum SceneType
    {
        MainMenu,
        Map,
        Battle,
    }

    [SerializeField] UIManager UIManager  => UIManager.Instance;
    [SerializeField] TitleManager TitleManager => TitleManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMainMenuScene()
    {
        StartCoroutine(ChangeScene(SceneType.MainMenu));
    }

    public void ExitGame()
    {
        //GameManager.OnExitApp();

        //게임 종료
        Application.Quit();
    }

    private IEnumerator ChangeScene(SceneType type)
    {
       // UIManager.SetCanvas(UIManager.Instance.InventoryCanvas, false, true);
        yield return StartCoroutine(UIManager.Instance.FadeInOut(true));

        switch (type)
        {
            case SceneType.MainMenu:
                //UIManager.ChangeScene(GameManager.SceneData.mainMenuSceneIndex);
                TitleManager.InitGameplayData();
                TitleManager.SetInitalHand();
                break;

            case SceneType.Map:
                //UIManager.ChangeScene(GameManager.SceneData.mapSceneIndex);

                break;
            case SceneType.Battle:
                //UIManager.ChangeScene(GameManager.SceneData.combatSceneIndex);

                break;
            default:
                break;

                //throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
