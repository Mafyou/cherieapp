using Microsoft.Extensions.Configuration;

namespace MAUICherieApp.ViewModels;

public class MainPageViewModel : ViewModelBase
{
    private bool _isPlaying { get; set; }
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
    public ICommand PauseCommand
    {
        get
        {
            return new Command((e) =>
            {
                var item = e as MySound;
                _isPlaying = !_isPlaying;
                MusicController.DoThePlaying();
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
    public MainPageViewModel()
    {
        Task.Run(async () =>
        {
            await loadingSounds();
            var d = new MyDiscussion();
            await d.ReadMyMessage();
        });
        MessagingCenter.Instance.Subscribe<string>(this, "NewSound", item =>
        {
            RefreshCommand.Execute(null);
        });
        MessagingCenter.Instance.Subscribe<App>(this, "RefreshCollectionView", item =>
        {
            RefreshCommand.Execute(null);
        });
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
}