using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using NLog.Fluent;
using ReactiveUI;
using Splat;

namespace GameOfLife.Models;

public class TimeService : ITimeService, IEnableLogger
{
    private readonly TimeSpan _resolution;
    private readonly List<Action> _actions;

    private IDisposable? _task;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tickResolution">Delay between two "ticks". Actions registered to the service will be invoked at every tick.</param>
    public TimeService(TimeSpan tickResolution)
    {
        _actions = new List<Action>();
        _resolution = tickResolution;
    }

    public event PlaybackChangedEvent? PlaybackChanged;

    public void Start()
    {
        if (_task != null)
        {
            this.Log().Info("Starting the time service, but it was already started.");
            return;
        }

        this.Log().Debug("Starting the time service.");
        _task = RxApp.TaskpoolScheduler.SchedulePeriodic(_resolution, Tick);
        PlaybackChanged?.Invoke(true);
    }

    public void Stop()
    {
        if (_task == null)
        {
            this.Log().Info("Stopping the time service, but it was not started.");
            return;
        }

        this.Log().Debug("Stopping the time service.");
        _task.Dispose();
        _task = null;
        PlaybackChanged?.Invoke(false);
    }

    public bool IsPlaying
    {
        get => _task != null;
    }

    public void Tick()
    {
        _actions.ForEach(a => a());
    }

    /// <summary>
    /// Example of operator overloading. 
    /// </summary>
    /// <param name="service">lhs</param>
    /// <param name="x">rhs</param>
    /// <returns>The lhs</returns>
    public static TimeService operator +(TimeService service, Action x)
    {
        service._actions.Add(x);
        return service;
    }
}