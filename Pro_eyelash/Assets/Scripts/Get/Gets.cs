using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace chataan.Scripts.Gets
{
    public static class Gets
    {
        public static T GetRandomItem<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new IndexOutOfRangeException("List is Empty");

            var randomIndex = Random.Range(0, list.Count);
            return list[randomIndex];
        }
    }
}


    public class Gets : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
