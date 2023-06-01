﻿using System;
using UnityEngine;

namespace ChittaExorcist.Common.Utilities
{
    public class Timer
    {
        public event Action OnTimeDone;

        private float _startTime;
        private readonly float _duration;
        private float _targetTime;

        private bool _isActive;

        public Timer(float duration)
        {
            _duration = duration;
        }

        public void StartTimer()
        {
            _startTime = Time.time;
            _targetTime = _startTime + _duration;
            _isActive = true;
        }

        public void StartTimer(float newDuration)
        {
            _startTime = Time.time;
            _targetTime = _startTime + newDuration;
            _isActive = true;
        }

        public void StopTimer()
        {
            _isActive = false;
        }

        public void Tick()
        {
            if (!_isActive) return;

            if (Time.time >= _targetTime)
            {
                OnTimeDone?.Invoke();
                StopTimer();
            }
        }
    }
}