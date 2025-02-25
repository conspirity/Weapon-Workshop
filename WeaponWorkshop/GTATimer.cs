using GTA;

namespace GTATimers
{
    public delegate void TimerElapsedEvent(string name);

    public class GTATimer
    {
        public event TimerElapsedEvent onTimerElapsed;

        public string Name;
        private int Interval;
        private int Counter;
        private int LastGameTime;

        public bool Running;

        public GTATimer(string name, int interval)
        {
            Name = name;
            Interval = interval;
        }

        public void Start()
        {
            LastGameTime = Game.GameTime;
            Counter = Interval;
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
            Counter = Interval;
            Running = true;
        }

        public void Update()
        {
            int currentTime = Game.GameTime;
            int elapsedTime = currentTime - LastGameTime;
            LastGameTime = currentTime;
            DecrementTimer(elapsedTime);
        }

        public void Update(int elapsedTime)
        {
            DecrementTimer(elapsedTime);
        }

        private void DecrementTimer(int amount)
        {
            Counter -= amount;

            if (Counter <= 0)
            {
                onTimerElapsed?.Invoke(Name);
                Counter += Interval;
            }
        }
    }
}
