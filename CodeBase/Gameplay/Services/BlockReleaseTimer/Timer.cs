using UnityEngine;

namespace CodeBase.Gameplay.Services.BlockReleaseTimer
{
    public struct Timer
    {
        public float CurrentTime { get; private set; }
        public float Duration { get; private set; }
        public bool IsElapsed => CurrentTime >= Duration;

        public void ChangeDuration(float duration) => 
            Duration = duration;

        public void Reset() => 
            CurrentTime = 0;

        public void Update()
        {
            if (IsElapsed)
                return;
            
            CurrentTime += Time.deltaTime;
        }
    }
}