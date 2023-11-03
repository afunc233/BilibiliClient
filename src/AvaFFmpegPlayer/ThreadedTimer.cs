namespace AvaFFmpegPlayer;

internal class ThreadedTimer
{
    private readonly Thread Worker;
    private readonly CancellationTokenSource Cts = new();

    private bool IsDisposed;
    public event EventHandler Elapsed;

    public ThreadedTimer(int intervalMillis = 1, int resolution = 1)
    {
        Worker = new Thread(WorkerLoop) { IsBackground = true };
        Interval = TimeSpan.FromMilliseconds(intervalMillis);
        // var (Minimum, Maximum) = NativeMethods.GetTimerPeriod();
        // Resolution = resolution.Clamp(Minimum.Clamp(1, Minimum), Maximum.Clamp(1, Maximum));
    }

    public bool IsRunning { get; private set; }

    public TimeSpan Interval { get; }

    public int Resolution { get; }

    public void Start()
    {
        if (IsRunning)
            return;

        IsRunning = true;
        Worker.Start();
    }

    private void WorkerLoop()
    {
        const double Bias = 0.0005;
        var token = Cts.Token;
        var cycleClock = new MultimediaStopwatch();
        var resolutionMillis = (uint)Resolution;

        try
        {
            cycleClock.Restart();
            if (OperatingSystem.IsWindows())
            {
                
            }

            while (!token.IsCancellationRequested)
            {
                Elapsed?.Invoke(this, EventArgs.Empty);
                while (!token.IsCancellationRequested)
                {
                    if (cycleClock.ElapsedSeconds >= Interval.TotalSeconds - Bias)
                        break;
                    Thread.Sleep(Resolution);
                }

                cycleClock.Restart();
            }
        }
        finally
        {
            if (OperatingSystem.IsWindows())
            {
                
            }
            IsRunning = false;
        }
    }

    public void Dispose()
    {
        if (IsDisposed)
            return;

        IsDisposed = true;
        Cts.Cancel();

        if (IsRunning && Environment.CurrentManagedThreadId != Worker.ManagedThreadId)
            Worker.Join();

        Cts.Dispose();
    }
}