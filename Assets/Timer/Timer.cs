
namespace Utility
{
	sealed public class Timer
	{
		public delegate void Delegate();
		public event Delegate OnStarted;
		public event Delegate OnStopped;
		public event Delegate OnRestarted;
		public event Delegate OnUpdated;
		public event Delegate OnLooped;
		public event Delegate OnCompleted;

		public Timer(float Duration) : this(Duration, 0) { }

		public Timer(float Duration, int Loops)
		{
			duration = Duration;
			remaining = Duration;
			loopsCount = 0;
			loopsTotal = Loops;
			isRunning = false;
		}

		public void Start()
		{
			if (isRunning == true)
				return;

			isRunning = true;

			if (OnStarted != null) OnStarted();
		}

		public void Stop()
		{
			if (isRunning == false)
				return;

			isRunning = false;

			if (OnStopped != null) OnStopped();
		}

		public void Restart()
		{
			remaining = duration;
			loopsCount = 0;
			isRunning = true;

			if (OnRestarted != null) OnRestarted();
		}

		public void Scrub(float time)
		{
			remaining = duration - time;
		}

		public void Update(float delta)
		{
			if (isRunning == false)
				return;

			if (remaining <= 0.0f)
				return;

			remaining -= delta;
			Internal_Updated();
		}

		private void Internal_Updated()
		{
			// OnUpdated should only be called when
			// the timer isn't about to finish
			if (remaining > 0.0f)
			{
				if (OnUpdated != null) OnUpdated();
			}
			// -1 indicates an infinitly looping timer
			else if (loopsTotal == -1 || loopsCount < loopsTotal)
			{
				Internal_Looped();
				Internal_Updated();
			}
			else
			{
				Internal_Completed();
			}
		}

		private void Internal_Looped()
		{
			remaining += duration;
			loopsCount++;

			if (OnLooped != null) OnLooped();
		}

		private void Internal_Completed()
		{
			remaining = 0.0f;
			isRunning = false;

			if (OnCompleted != null) OnCompleted();
		}

		public float Duration { get { return duration; } }
		public float Elapsed { get { return duration - remaining; } }
		public float Remaining { get { return remaining; } }
		public int LoopsCount { get { return loopsCount; } }
		public int LoopsTotal { get { return loopsTotal; } }
		public bool IsRunning { get { return isRunning; } }

		private float duration;
		private float remaining;
		private int loopsCount;
		private int loopsTotal;
		private bool isRunning;
	}
}