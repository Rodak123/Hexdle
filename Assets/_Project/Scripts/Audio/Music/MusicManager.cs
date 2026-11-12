using UnityEngine;

namespace Rodak.Hexdle
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private MusicSO music;
        [SerializeField] private AudioSource musicAudioSource;

        private void Awake()
        {
            musicAudioSource.loop = true;
            musicAudioSource.clip = music.SongClip;
            musicAudioSource.Play();
        }

    }
}
