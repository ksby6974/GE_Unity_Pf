using chataan.Scripts.Data.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace chataan.Scripts.Utils
{
    [RequireComponent(typeof(Button))]
    public class ButtonSoundPlayer : MonoBehaviour
    {
        [SerializeField] private SoundProfileData soundProfileData;

        private Button button;
        private SoundProfileData SoundProfileData => soundProfileData;
        private SoundManager SoundManager => SoundManager.Instance;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(PlayButton);
        }

        public void PlayButton() => SoundManager.PlayOneShotButton(SoundProfileData.GetRandomClip());
    }
}