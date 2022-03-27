using CherieAppUploadZik.Helpers;
using CherieAppUploadZik.Models;
using CherieAppUploadZik.Services;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CherieAppUploadZik.ViewModels;

internal class MySoundsViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    /// <summary>
    /// Fires PropertyChangedEventHandler, for bindables
    /// </summary>
    protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public MySoundsViewModel()
    {
        Task.Run(async () =>
        {
            await loadingSounds();
        });
        MessagingCenter.Instance.Subscribe<MainPage>(this, "SentSound", async (e) => await loadingSounds());
    }
    private ObservableCollection<MySound> _mySounds;
    public ObservableCollection<MySound> MySounds
    {
        get => _mySounds;
        set
        {
            if (_mySounds != value)
            {
                _mySounds = value;
                OnPropertyChanged();
            }
        }
    }
    private async Task loadingSounds()
    {
        var list = await APIService.Instance.Client.GetStringAsync("/list");
        MySounds = new ObservableCollection<MySound>(
            JsonConvert.DeserializeObject<List<MySound>>(list));
    }
    private bool _isPlaying { get; set; }
    public ICommand PlayCommand
    {
        get
        {
            return new Command((e) =>
            {
                var item = e as MySound;
                _isPlaying = !_isPlaying;
                MusicController.DoThePlaying(item);
            });
        }
    }
    public ICommand DeleteCommand
    {
        get
        {
            return new Command((e) =>
            {
                Task.Run(async () =>
                {
                    await FilesManager.Instance.DeleteSound(e as MySound);
                    await loadingSounds();
                    RefreshCommand.Execute(null);
                });
            });
        }
    }
    private bool _isRefreshing = false;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            _isRefreshing = value;
            OnPropertyChanged();
        }
    }
    public ICommand RefreshCommand
    {
        get
        {
            return new Command(async e =>
            {
                IsRefreshing = true;
                await loadingSounds();
                IsRefreshing = false;
            });
        }
    }
}