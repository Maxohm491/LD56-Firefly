using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Firefly.Audio
{
    public class RhythmController : MonoBehaviour
    {
        [SerializeField] private float _bpm;
        [SerializeField] private AudioSource _source;
        [SerializeField] private List<Interval> _intervals;

        private void Update()
        {
            float time = _source.timeSamples / _source.clip.frequency;
            foreach (Interval interval in _intervals)
            {
                float sampledTime = time / interval.GetBeatLength(_bpm);
                interval.CheckForNewInterval(sampledTime);
            }
        }

        public void AddBeatListener(float steps, UnityAction listener)
        {
            var interval = _intervals.Find(i => i.Steps == steps);
            if (interval == null)
            {
                interval = new Interval(steps);
                _intervals.Add(interval);
            }

            interval.Handler.AddListener(listener);
        }

        public void RemoveBeatListener(float steps, UnityAction listener)
        {
            foreach (var interval in _intervals)
            {
                if (interval.Steps == steps)
                {
                    interval.Handler.RemoveListener(listener);
                }
            }
        }
    }

    [System.Serializable]
    public class Interval
    {
        [SerializeField] private float _steps;
        [SerializeField] private UnityEvent _listener;
        private int _lastInterval;

        public float Steps => _steps;
        public UnityEvent Handler => _listener;

        public Interval(float steps)
        {
            _steps = steps;
        }

        public float GetBeatLength(float bpm)
        {
            return 60f / (bpm * _steps);
        }

        public void CheckForNewInterval(float interval)
        {
            if (Mathf.FloorToInt(interval) != _lastInterval)
            {
                _lastInterval = Mathf.FloorToInt(interval);
                _listener?.Invoke();
            }
        }
    }
}