
using UnityEngine;

namespace chataan.Scripts.Data.Scene
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "Chataan/Settings/SceneData", order = 2)]
    public class SceneData : ScriptableObject
    {
        public int mainMenuSceneIndex = 0;
        public int mapSceneIndex = 1;
        public int combatSceneIndex = 2;
    }
}
