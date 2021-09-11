using System;
using System.Collections.Generic;
using UnityEditor;

namespace Chronity.Editor
{
    public class EditorTimer : TimerBase
    {
        /// <summary>
        /// Register a new timer that should fire an event after a certain amount of time
        /// has elapsed.
        ///
        /// Registered timers are destroyed when the scene changes.
        /// </summary>
        /// <param name="duration">The time to wait before the timer should fire, in seconds.</param>
        /// <param name="onComplete">An action to fire when the timer completes.</param>
        /// <param name="onUpdate">An action that should fire each time the timer is updated. Takes the amount
        /// of time passed in seconds since the start of the timer's current loop.</param>
        /// <param name="loop">Whether the timer should repeat after executing.</param>
        /// <returns>A timer object that allows you to examine stats and stop/resume progress.</returns>
        public static EditorTimer Register(float duration, Action onComplete, Action<float> onUpdate = null, bool loop = false)
        {
            EditorTimer result = new EditorTimer(duration, onComplete, onUpdate, loop);
            _timers.Add(result);
            return result;
        }

        public static void PauseAllTimers()
        {
            for (int i = 0; i < _timers.Count; i++)
                _timers[i].Pause();
        }

        public static void ResumeAllTimers()
        {
            for (int i = 0; i < _timers.Count; i++)
                _timers[i].Resume();
        }

        public static void CancelAllTimers()
        {
            for (int i = 0; i < _timers.Count; i++)
                _timers[i].Cancel();
        }

        private EditorTimer(float duration, Action onComplete, Action<float> onUpdate, bool looped = false)
            : base(duration, onComplete, onUpdate, looped)
        {
            EditorApplication.update += Update;
        }

        ~EditorTimer()
        {
            EditorApplication.update -= Update;
        }

        protected override void Update()
        {
            if (IsDone)
            {
                _timers.Remove(this);
            }

            base.Update();
        }

        protected override float CurrentTime => (float)EditorApplication.timeSinceStartup;

        private static List<EditorTimer> _timers = new List<EditorTimer>();
    }
}