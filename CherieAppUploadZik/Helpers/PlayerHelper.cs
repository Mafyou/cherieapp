namespace CherieAppUploadZik.Helpers;

public class PlayerHelper
{
    #region Singleton
    private PlayerHelper()
    {

    }
    private static readonly Lazy<PlayerHelper> lazy =
                        new Lazy<PlayerHelper>(() => new PlayerHelper());
    public static PlayerHelper Instance
    {
        get => lazy.Value;
    }
    #endregion

    private MediaPlayer _player;
    const string TAG = "PlaySound";
    public void OnPlay(bool start)
    {
        if (start)
        {
            if (File.Exists(FilesManager.Instance.FileNameRecorded))
            {
                StartPlaying();
            }
        }
        else
        {
            StopPlaying();
        }
    }
    void StartPlaying()
    {
        _player = new MediaPlayer();
        try
        {
            _player.SetDataSource(FilesManager.Instance.FileNameRecorded);
            _player.Prepare();
            _player.Start();
        }
        catch (IOException)
        {
            Log.Error(TAG, "There was an error trying to start the MediaPlayer!");
        }
    }

    void StopPlaying()
    {
        if (_player == null)
        {
            return;
        }
        _player.Release();
        _player = null;
    }
}