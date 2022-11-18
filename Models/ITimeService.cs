namespace GameOfLife.Models;

/// <summary>
/// Service which manages time in the application. 
/// </summary>
public interface ITimeService
{
    /// <summary>
    /// Start the time flow.
    /// </summary>
    void Start();
    
    /// <summary>
    /// Stop the time flow. 
    /// </summary>
    void Stop();
}