using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork {
    [Serializable]
    public class SoundManager : ICanPlaySound {
        [SerializeField] private List<SoundClip> SoundClips;
        [SerializeField] private AudioSource SFXAudioSources;
        [SerializeField] private AudioSource GlobalAudioSources;

        public void PlayGlobalSound(string name) {
            var soundClip = SoundClips.Find(s => s.clipName == name);
            if (soundClip != null) {
                GlobalAudioSources.clip = soundClip.clip;
                GlobalAudioSources.loop = soundClip.isLoop;
                GlobalAudioSources.volume = soundClip.volume;

                GlobalAudioSources.Play();
            }
        }

        public void StopGlobalSound() {
            GlobalAudioSources.Stop();
        }

        public void PlaySFX(string name, float volumeFactor = 0) {
            var soundClip = SoundClips.Find(s => s.clipName == name);
            if (soundClip != null) {
                SFXAudioSources.PlayOneShot(soundClip.clip, soundClip.volume * volumeFactor);
            }
        }
    }


    public interface ICanPlaySound {
        public void PlayGlobalSound(string clipName);

        public void StopGlobalSound();

        public void PlaySFX(string clipName, float volumeFactor = 1);
    }
}