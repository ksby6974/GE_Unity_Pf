using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using chataan.Scripts.Enums;
using chataan.Scripts.Gets;

namespace chataan.Scripts.Data
{
    [CreateAssetMenu(fileName = "Sound Profile", menuName = "Chataan/Containers/SoundProfile", order = 1)]
    public class SoundProfileData : MonoBehaviour
    {
        [SerializeField] private AudioActionType audioType;
        [SerializeField] private List<AudioClip> randomClipList;

        public AudioActionType AudioType => audioType;

        public List<AudioClip> RandomClipList => randomClipList;

        public AudioClip GetRandomClip() => RandomClipList.Count > 0 ? RandomClipList.GetRandomItem() : null;
    }
}
