using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StraviaTecMovil.Helpers
{
    public class ActivityTimer
    {
        private Timer _timer;
        private bool _isRunning;
        private bool _onActivity;
        private TimerCallback _callback;
        
        private DateTime _startTime;
        private DateTime _endTime;
        private DateTime _currentPausedTime;
        private TimeSpan _dueTime;
        private TimeSpan _period;
        private TimeSpan _pausedTime;

        public ActivityTimer(TimerCallback tickCallback, TimeSpan dueTime, TimeSpan period)
        {
            _dueTime = dueTime;
            _period = period;
            _pausedTime = TimeSpan.FromSeconds(0);
            _callback = tickCallback;

            _timer = new Timer(tickCallback);
        }

        public void StartWithDueTime()
        {
            Start(true);
        }

        public void Start(bool dueTime = false)
        {
            if (_isRunning)
                return;

            _startTime = DateTime.Now;
            _timer.Change(dueTime ? _dueTime : TimeSpan.FromSeconds(0), _period);
            _isRunning = true;
            _onActivity = true;
        }

        public void Pause()
        {
            if (!_isRunning)
                return;

            _currentPausedTime = DateTime.Now;
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _isRunning = false;
        }

        public void Resume(bool dueTime = false)
        {
            if (_isRunning)
                return;

            _pausedTime += DateTime.Now.Subtract(_currentPausedTime);
            _timer.Change(dueTime ? _dueTime : TimeSpan.FromSeconds(0), _period);
            _isRunning = true;
        }

        public void ResumeWithDueTime()
        {
            Resume(true);
        }

        public void Toggle()
        {
            if (!_onActivity)
                return;

            if (_isRunning)
            {
                Pause();
            } else
            {
                Resume();
            }
        }

        public void Stop()
        {
            _endTime = DateTime.Now;
            _timer.Dispose();
            _isRunning = false;
            _onActivity = false;
            _pausedTime = TimeSpan.FromSeconds(0);
            _timer = new Timer(_callback);
        }

        public TimeSpan GetTotalActivityTime()
        {
            if (_onActivity)
                Stop();

            return (_endTime - _startTime) - _pausedTime;
        }

        public TimeSpan GetPausedTime()
        {
            if (_onActivity)
                Stop();

            return _pausedTime;
        }

        public TimeSpan GetElapsedTime()
        {
            return (DateTime.Now - _startTime) - _pausedTime;
        }

        public bool IsRunning()
        {
            return _isRunning;
        }

        public bool OnActivity()
        {
            return _onActivity;
        }
    }
}
