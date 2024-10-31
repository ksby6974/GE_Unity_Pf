using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        set { }
        get { return instance; }
    }

    protected virtual private void Awake()
    {
        if (instance == null)
        {
            instance = (T)FindObjectOfType(typeof(T));
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // DontDestroyOnLoad 메모리에 해당 오브젝트가 계속 남아있음
        DontDestroyOnLoad(gameObject);
    }
}