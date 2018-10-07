using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using System;
using System.Threading;

namespace Snapshot.App
{
    [Service]
    public class BackgroundService : Service
    {
        private static readonly string Tag = "X:" + typeof(BackgroundService).Name;
        private const int TimerWait = 10000;
        private Timer _timer;
        private DateTime _startTime;
        private bool _isStarted;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Debug(Tag, $"OnStartCommand called at {_startTime}, flags={flags}, startid={startId}");
            if (_isStarted)
            {
                TimeSpan runtime = DateTime.UtcNow.Subtract(_startTime);
                Log.Debug(Tag, $"This service was already started, it's been running for {runtime:c}.");
            }
            else
            {
                _startTime = DateTime.UtcNow;
                Log.Debug(Tag, $"Starting the service, at {_startTime}.");
                _timer = new Timer(HandleTimerCallback, _startTime, 0, TimerWait);
                _isStarted = true;
            }
            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            // This is a started service, not a bound service, so we just return null.
            return null;
        }
        
        public override void OnDestroy()
        {
            _timer.Dispose();
            _timer = null;
            _isStarted = false;

            TimeSpan runtime = DateTime.UtcNow.Subtract(_startTime);
            Log.Debug(Tag, $"Simple Service destroyed at {DateTime.UtcNow} after running for {runtime:c}.");
            base.OnDestroy();
        }

        private void HandleTimerCallback(object state)
        {
            TimeSpan runTime = DateTime.UtcNow.Subtract(_startTime);
            Log.Debug(Tag, $"This service has been running for {runTime:c} (since ${state}).");
        }
    }
}

