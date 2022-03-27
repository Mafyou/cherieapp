using CherieAppUploadZik.Helpers;
using CherieAppUploadZik.Models;

namespace CherieAppUploadZik;

public partial class MainPage : ContentPage
{
    bool _startRecording = false;
    bool _startPlaying = false;
    public MainPage()
    {
        InitializeComponent();
        Device.InvokeOnMainThreadAsync(async () =>
        {
            await CheckPermissions();
            if (btnPlay.IsEnabled) CheckSoundPlayable();
        });
    }

    private async Task CheckPermissions()
    {
        var status = await Permissions.RequestAsync<Permissions.Microphone>();
        if (status == PermissionStatus.Unknown ||
            status == PermissionStatus.Denied)
        {
            btnRecord.Dispatcher.Dispatch(() =>
            {
                btnRecord.IsEnabled = false;
                btnRecord.Text = "No access to mic";
            });
        }
        status = await Permissions.RequestAsync<Permissions.Media>();
        if (status == PermissionStatus.Unknown ||
            status == PermissionStatus.Denied)
        {
            btnRecord.Dispatcher.Dispatch(() =>
            {
                btnPlay.IsEnabled = false;
                btnPlay.Text = "No access to media";
            });
        }
    }

    void OnRecordButtonClick(object sender, EventArgs e)
    {
        _startRecording = !_startRecording;

        RecorderHelper.Instance.OnRecord(_startRecording);
        btnRecord.Text = _startRecording ? "Stop" : "Record";
        CheckSoundPlayable();
    }

    private void CheckSoundPlayable()
    {
        if (!_startRecording &&
            RecorderHelper.Instance.IsRecorded)
        {
            btnPlay.Dispatcher.Dispatch(() =>
            {
                btnPlay.Text = "New Sound";
                btnPlay.IsEnabled = true;
            });
            btnSend.Dispatcher.Dispatch(() =>
            {
                btnSend.Text = "Envoyer";
                btnSend.IsEnabled = true;
            });
        }
    }

    void OnPlayButtonClick(object sender, EventArgs e)
    {
        _startPlaying = !_startPlaying;

        PlayerHelper.Instance.OnPlay(_startPlaying);
        btnPlay.Text = _startPlaying ? "Stop" : "Start";
    }

    private async void OnSendButtonClick(object sender, EventArgs e)
    {
        var soundName = await Application.Current.MainPage.DisplayPromptAsync("Nom ?", string.Empty, "Ok", "Cancel", "Titre du son");
        var id = FilesManager.Instance.FileName.Replace(".mp3", string.Empty);

        var result = await FilesManager.Instance.PushSound(
            new MySound
            {
                Id = Guid.Parse(id),
                Name = soundName,
                MyAudio = File.ReadAllBytes(FilesManager.Instance.FileNameRecorded)
            });
        await Application.Current.MainPage.DisplayAlert("Son envoyé ?", result ? "Oui" : "Non", "ok");
        MessagingCenter.Instance.Send(this, "SentSound");
    }
}
