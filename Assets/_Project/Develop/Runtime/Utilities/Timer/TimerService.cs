using System;
using System.Collections;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Utilities.Timer
{
    public class TimerService : IDisposable
    {
        private readonly float _cooldown;
        private readonly ReactiveEvent _cooldownEnded;
        private readonly ReactiveVariable<float> _currentTime;

        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private Coroutine _cooldownProcess;

        public TimerService(float cooldown, ICoroutinesPerformer coroutinesPerformer)
        {
            _cooldown = cooldown;
            _coroutinesPerformer = coroutinesPerformer;

            _cooldownEnded = new ReactiveEvent();
            _currentTime = new ReactiveVariable<float>();
        }

        public bool IsOver => _currentTime.Value <= 0;

        public IReadOnlyEvent CooldownEnded => _cooldownEnded;
        public IReadOnlyVariable<float> CurrentTime => _currentTime;

        public void Dispose()
        {
            Stop();
        }

        public void Restart()
        {
            Stop();
            _cooldownProcess = _coroutinesPerformer.StartPerform(CooldownProcess());
        }

        public void Stop()
        {
            if(_cooldownProcess != null)
                _coroutinesPerformer.StopPerform(_cooldownProcess);
        }

        private IEnumerator CooldownProcess()
        {
            _currentTime.Value = _cooldown;

            while (IsOver == false)
            {
                _currentTime.Value -= Time.deltaTime;
                yield return null;
            }

            _cooldownEnded.Invoke();
        }
    }
}
