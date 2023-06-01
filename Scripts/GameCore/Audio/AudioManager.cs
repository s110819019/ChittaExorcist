using System;
using System.Collections;
using System.Collections.Generic;
using ChittaExorcist.Common.Module;
using UnityEngine;

namespace ChittaExorcist.GameCore.AudioSettings
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        // public static AudioManager instance;
        
        private AudioSource _bgmSource;
        private AudioSource _playerOnceSource;
        
        private List<AudioSource> _activeAudioSources = new List<AudioSource>();
        private List<AudioSource> _inactiveAudioSources = new List<AudioSource>();
        private Dictionary<string, AudioSource> _playingAudioSources = new Dictionary<string, AudioSource>();

        private Dictionary<string, IEnumerator> _jobs = new Dictionary<string, IEnumerator>();

        private Dictionary<string, IEnumerator> _stopJobs = new Dictionary<string, IEnumerator>();

        #region w/ Init

        private void InitAudio()
        {
            _bgmSource = gameObject.AddComponent<AudioSource>();
            _bgmSource.loop = true;
            _playerOnceSource = gameObject.AddComponent<AudioSource>();
        }

        #endregion

        #region w/ Public Functions

        public void PlayAudio(AudioDataSO audioDataSo)
        {
            if (audioDataSo == null)
            {
                Debug.LogWarning("Try to play audio with null audio data");
                return;
            }
            if (audioDataSo.AudioClip == null)
            {
                Debug.LogWarning("Try to play audio with no audio clip");
                return;
            }
            
            AddJob(new AudioJob(AudioAction.START, audioDataSo));
        }

        public void StopAudio(AudioDataSO audioDataSo)
        {
            AddJob(new AudioJob(AudioAction.STOP, audioDataSo));
        }
        
        public void RestartAudio(AudioDataSO audioDataSo)
        {
            AddJob(new AudioJob(AudioAction.RESTART, audioDataSo));
        }

        public void PlayOnceAudio(AudioDataSO audioDataSo)
        {
            if (audioDataSo == null)
            {
                Debug.LogWarning("Try to play audio with null audio data");
                return;
            }
            if (audioDataSo.AudioClip == null)
            {
                Debug.LogWarning("Try to play audio with no audio clip");
                return;
            }
            AddJob(new AudioJob(AudioAction.ONCE, audioDataSo));
        }

        public void PauseAllAudio()
        {
            foreach (var item in _playingAudioSources)
                item.Value.Pause();
            _bgmSource.Pause();
        }

        public void ContinueAllAudio()
        {
            foreach (var item in _playingAudioSources)
                item.Value.UnPause();
            _bgmSource.UnPause();
        }

        public void PlayBGM(AudioDataSO data)
        {
            InitTargetAudio(data, ref _bgmSource);
            _bgmSource.loop = true;
            _bgmSource.Play();
        }
        
        #endregion

        #region w/ Audio Source

        private AudioSource GetAudioSource()
        {
            AudioSource inactiveAudioSource = null;
            if (_inactiveAudioSources.Count > 0)
            {
                inactiveAudioSource = _inactiveAudioSources[0];
                _inactiveAudioSources.RemoveAt(0);
            }
            else
            {
                inactiveAudioSource = gameObject.AddComponent<AudioSource>();
            }
            _activeAudioSources.Add(inactiveAudioSource);
            return inactiveAudioSource;
        }

        private AudioSource GetPlayingSource(string audioName)
        {
            _playingAudioSources.TryGetValue(audioName, out var targetAudio);
            return targetAudio;
        }
        
        #endregion

        #region w/ Audio Job

        private void OnceAudio(AudioJob job)
        {
            _playerOnceSource.clip = null;
            _playerOnceSource.volume = 1.0f;
            _playerOnceSource.outputAudioMixerGroup = job.AudioData.AudioMixerGroup;
            _playerOnceSource.PlayOneShot(job.AudioData.AudioClip, job.AudioData.AudioVolume);
        }
        
        private void StartAudio(AudioJob job)
        {
            var targetAudio = GetPlayingSource(job.AudioData.AudioName);
            // 已有在播放中則返回
            if (targetAudio != null) return;
            targetAudio = GetAudioSource();
            InitTargetAudio(job, ref targetAudio);
            targetAudio.Play();

            _playingAudioSources[job.AudioData.AudioName] = targetAudio;
            
            // 非 loop 則在播放結束時執行一次 StopAudio
            if (!job.AudioData.IsLoop)
            {
                if (_stopJobs.TryGetValue(job.AudioData.AudioName, out IEnumerator jobRunner))
                {
                    StopCoroutine(jobRunner);
                    _stopJobs.Remove(job.AudioData.AudioName);
                }
                
                jobRunner = WaitAudioFinish(job);
                _stopJobs.Add(job.AudioData.AudioName, jobRunner);
                StartCoroutine(jobRunner);
            }
        }
        
        private void RestartAudio(AudioJob job)
        {
            var targetAudio = GetPlayingSource(job.AudioData.AudioName);
            if (targetAudio == null) return;
            InitTargetAudio(job, ref targetAudio);
            targetAudio.Stop();
            targetAudio.Play();

            _playingAudioSources[job.AudioData.AudioName] = targetAudio;
            
            if (!job.AudioData.IsLoop)
            {
                if (_stopJobs.TryGetValue(job.AudioData.AudioName, out IEnumerator jobRunner))
                {
                    StopCoroutine(jobRunner);
                    _stopJobs.Remove(job.AudioData.AudioName);
                }
                
                jobRunner = WaitAudioFinish(job);
                _stopJobs.Add(job.AudioData.AudioName, jobRunner);
                StartCoroutine(jobRunner);
            }
        }
        
        private void StopAudio(AudioJob job)
        {
            var targetAudio = GetPlayingSource(job.AudioData.AudioName);
            if (targetAudio == null) return;

            if (_stopJobs.TryGetValue(job.AudioData.AudioName, out IEnumerator jobRunner))
            {
                StopCoroutine(jobRunner);
                _stopJobs.Remove(job.AudioData.AudioName);
            }

            targetAudio.Stop();
            targetAudio.clip = null;
            targetAudio.loop = false;
            _activeAudioSources.Remove(targetAudio);
            _inactiveAudioSources.Add(targetAudio);
            _playingAudioSources.Remove(job.AudioData.AudioName);
        }

        private IEnumerator WaitAudioFinish(AudioJob job)
        {
            float length = job.AudioData.AudioClip.length;
            yield return new WaitForSeconds(length);
            // 為了執行一次 StopAudio
            StopAudio(job);
            _stopJobs.Remove(job.AudioData.AudioName);
        }

        private void InitTargetAudio(AudioJob job, ref AudioSource targetAudio)
        {
            targetAudio.loop = job.AudioData.IsLoop;
            targetAudio.clip = job.AudioData.AudioClip;
            targetAudio.volume = job.AudioData.AudioVolume;
            targetAudio.outputAudioMixerGroup = job.AudioData.AudioMixerGroup;
        }

        private void InitTargetAudio(AudioDataSO data, ref AudioSource targetAudio)
        {
            targetAudio.loop = data.IsLoop;
            targetAudio.clip = data.AudioClip;
            targetAudio.volume = data.AudioVolume;
            targetAudio.outputAudioMixerGroup = data.AudioMixerGroup;
        }
        
        private IEnumerator RunAudioJob(AudioJob job)
        {
            yield return new WaitForSeconds(job.AudioData.Delay);

            switch (job.AudioAction)
            {
                case AudioAction.START:
                    StartAudio(job);
                    break;
                case AudioAction.STOP:
                    StopAudio(job);
                    break;
                case AudioAction.RESTART:
                    RestartAudio(job);
                    break;
                case AudioAction.ONCE:
                    OnceAudio(job);
                    break;
                case AudioAction.PAUSE:
                    Debug.Log("Audio Pause Not Done");
                    break;
            }

            _jobs.Remove(job.AudioData.AudioName);
        }
        
        private void AddJob(AudioJob job)
        {
            if (_jobs.ContainsKey(job.AudioData.AudioName)) {
                RemoveJob(job);
            }
            
            // start job
            IEnumerator jobRunner = RunAudioJob(job);
            _jobs.Add(job.AudioData.AudioName, jobRunner);
            StartCoroutine(jobRunner);
        }

        private void RemoveJob(AudioJob job)
        {
            if (!_jobs.ContainsKey(job.AudioData.AudioName))
            {
                Debug.LogWarning("Trying to stop a job [" + job.AudioData.AudioName + "] that is not running.");
                return;
            }

            IEnumerator runningJob = (IEnumerator) _jobs[job.AudioData.AudioName];
            StopCoroutine(runningJob);
            _jobs.Remove(job.AudioData.AudioName);
        }

        #endregion

        #region w/ Class and Enum

        private enum AudioAction
        {
            START,
            STOP,
            RESTART,
            ONCE,
            PAUSE,
        }
        
        private class AudioJob
        {
            public readonly AudioAction AudioAction;
            public readonly AudioDataSO AudioData;
            
            public AudioJob(AudioAction action, AudioDataSO data)
            {
                AudioAction = action;
                AudioData = data;
            }
        }

        #endregion

        #region w/ Unity Functions

        protected override void Awake()
        {
            base.Awake();
            // instance = this;
            InitAudio();
        }

        // private void Update()
        // {
        //     // Debug.Log("Active Source: " + _activeAudioSources.Count);
        //     // Debug.Log("Playing Source: " + _playingAudioSources.Count);
        //     // Debug.Log("Stop Jobs: " + _stopJobs.Count);
        //     // Debug.Log("Job : " + _jobs.Count);
        // }

        #endregion
    }
}