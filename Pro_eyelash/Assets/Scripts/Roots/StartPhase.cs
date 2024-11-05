using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-11)]
public class StartPhase : MonoBehaviour
{
    void Awake()
    {
        try
        {
            //
            if (!CoreManager.Instance)
            {
                SceneManager.LoadScene("Phase", LoadSceneMode.Additive);
            }

            // ÇØ´ç °´Ã¼ ÆÄ±«
            Destroy(gameObject);
        }
        catch (Exception exception)
        {
            Debug.LogError($"¸Õ°¡ Àß¸øµÆÀ½¤» : {exception}");
            throw;
        }
    }
}
