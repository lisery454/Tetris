using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FrameWork {
    [Serializable]
    public class SoundManager : ICanPlaySound {
        private List<SoundClip> SoundClips = new List<SoundClip>();
        [SerializeField] private AudioSource audioSource;

        public void LoadSoundClips(string label, Action<AsyncOperationHandle<IList<SoundClip>>> OnCompleted) {
            Addressables.LoadResourceLocationsAsync(label).Completed += handle => {
                Addressables.LoadAssetsAsync<SoundClip>(handle.Result, clip => { SoundClips.Add(clip); }).Completed +=
                    OnCompleted;
            };
        }

        public void ReleaseSoundClips() {
            foreach (var soundClip in SoundClips) {
                Addressables.Release(soundClip);
            }
            
            SoundClips.Clear();
        }

        public void PlayGlobalSound(string name) {
            var soundClip = SoundClips.Find(s => s.clipName == name);
            if (soundClip != null) {
                audioSource.clip = soundClip.clip;
                audioSource.loop = soundClip.isLoop;
                audioSource.volume = soundClip.volume;

                audioSource.Play();
            }
        }

        public void StopGlobalSound() {
            audioSource.Stop();
        }

        public void PlaySFX(string name, float volumeFactor = 0) {
            var soundClip = SoundClips.Find(s => s.clipName == name);
            if (soundClip != null) {
                audioSource.PlayOneShot(soundClip.clip, soundClip.volume * volumeFactor);
            }
        }
    }


    public interface ICanPlaySound {
        public void PlayGlobalSound(string clipName);

        public void StopGlobalSound();

        public void PlaySFX(string clipName, float volumeFactor = 1);
    }
}