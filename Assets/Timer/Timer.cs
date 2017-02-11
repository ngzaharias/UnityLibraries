
sealed public class Timer
{
    public delegate void Delegate();
    public event Delegate OnStarted;
    public event Delegate OnStopped;
    public event Delegate OnRestarted;
    public event Delegate OnUpdated;
    public event Delegate OnLooped;
    public event Delegate OnCompleted;

    // GetTimeRemaining
    // GetDuration
    // GetElapsed
    // GetLoopCount

    public Timer(float Duration) : this(Duration, 0) { }

    public Timer(float Duration, int Loops)
    {
        duration = Duration;
        remaining = Duration;
        loopsNum = 0;
        loopsMax = Loops;
        isRunning = false;
    }

    public void Start()
    {
        isRunning = true;

        OnStarted.Invoke();
    }

    public void Stop()
    {
        isRunning = false;

        OnStopped.Invoke();
    }

    public void Restart()
    {
        remaining = duration;
        loopsNum = 0;

        OnRestarted.Invoke();
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
            OnUpdated.Invoke();
        }
        // -1 indicates an infinitly looping timer
        else if (loopsMax == -1 || loopsNum < loopsMax)
        {
            Internal_Looped();
        }
        else
        {
            Internal_Completed();
        }
    }

    private void Internal_Looped()
    {
        remaining += duration;
        loopsNum++;

        OnLooped.Invoke();
    }

    private void Internal_Completed()
    {
        isRunning = false;

        OnCompleted.Invoke();
    }

    private float duration;
    private float remaining;
    private int loopsNum;
    private int loopsMax;

    private bool isRunning;
}
