using System;
using System.Reactive.Linq;
using System.Windows.Input;
using GameOfLife.Models;
using ReactiveUI;

namespace GameOfLife.ViewModels;

public class ControlPanelViewModel : ViewModelBase
{
    private readonly ITimeService _timeService;
    private bool _playing;

    public ControlPanelViewModel(ITimeService timeService)
    {
        _timeService = timeService;
        _timeService.PlaybackChanged += (playing) => Playing = playing;
        _playing = _timeService.IsPlaying;


        TogglePlayCommand = ReactiveCommand.Create(() =>
        {
            if (timeService.IsPlaying)
            {
                timeService.Stop();
            }
            else
            {
                timeService.Start();
            }
        });

        var canStep = this.WhenAnyValue(x => x.Playing)
            .StartWith(Playing)
            .Select(x => !x);
        StepCommand = ReactiveCommand.Create(() => { timeService.Tick(); }, canStep);
    }

    public ICommand TogglePlayCommand { get; }
    public ICommand StepCommand { get; }

    public bool Playing
    {
        get => _playing;
        set => this.RaiseAndSetIfChanged(ref _playing, value);
    }
}