
using System.Collections.Generic;
using UnityEngine;
using chataan.Scripts.Enums;
using chataan.Scripts.Gets;
using System;

namespace chataan.Scripts.Data.Sound
{
    [Serializable]
    [CreateAssetMenu(fileName = "SoundProfileData", menuName = "Chataan/Sound/SoundProfileData", order = 0)]
    public class SoundProfileData : ScriptableObject
    {
        [SerializeField] private AudioActionType audioType;
        [SerializeField] private List<AudioClip> randomClipList;

        public AudioActionType AudioType => audioType;

        public List<AudioClip> RandomClipList => randomClipList;

        public AudioClip GetRandomClip() => RandomClipList.Count > 0 ? RandomClipList.GetRandomItem() : null;
    }
}
