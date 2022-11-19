namespace GameOfLife.Models;

public delegate void PlaybackChangedEvent(bool isPlaying);

/// <summary>
/// Service which manages time in the application. 
/// </summary>
public interface ITimeService
{
    /// <summary>
    /// Whether the service is currently playing or stopped.
    /// </summary>
    /// <returns></returns>
    bool IsPlaying { get; }

    public event PlaybackChangedEvent? PlaybackChanged;

    /// <summary>
    /// Start the time flow.
    /// </summary>
    void Start();

    /// <summary>
    /// Stop the time flow. 
    /// </summary>
    void Stop();

    /// <summary>
    /// Manually fire a tick. 
    /// </summary>
    void Tick();
}