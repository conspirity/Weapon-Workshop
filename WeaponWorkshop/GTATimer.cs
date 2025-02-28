// Original author of this namespace: unknown user from GTA Forums
// source: https://gtaforums.com/topic/924484-c-using-timers-and-intervals/
// Modified by ConcatSpirity

using GTA;

namespace GTATimers
{
    public delegate void TimerElapsedEvent();

    public class GTATimer
    {
        private int _counter;
        private int _lastGameTime;

        public event TimerElapsedEvent OnTimerElapsed;
        public string Name;
        public int Interval;
        public bool Running;

        public GTATimer(string name, int interval)
        {
            Name = name;
            Interval = interval;
        }

        public void Start()
        {
            _lastGameTime = Game.GameTime;
            _counter = Interval;
            Running = true;
        }

        public void Resume()
        {
            Running = true;
        }

        public void Stop()
        {
            Running = false;
        }

        public void Reset()
        {
            _counter = Interval;
            Running = true;
        }

        public void Update()
        {
            int currentTime = Game.GameTime;
            int elapsedTime = currentTime - _lastGameTime;
            _lastGameTime = currentTime;
            DecrementTimer(elapsedTime);
        }

        public void Update(int elapsedTime)
        {
            DecrementTimer(elapsedTime);
        }

        private void DecrementTimer(int amount)
        {
            _counter -= amount;

            if (_counter <= 0)
            {
                OnTimerElapsed?.Invoke();
                _counter += Interval;
            }
        }
    }
}
