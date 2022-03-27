using Android.Media;
using Android.Util;
using MAUICherieApp.Models;

namespace MAUICherieApp.Helpers;

public static class MusicController
{
    const string TAG = "RecordSound";
    public static void DoThePlaying(MySound mySound)
    {
        var _player = new MediaPlayer();
        try
        {

            string rootFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            string musicFolder = Android.OS.Environment.DirectoryMusic;
            string fileName = string.Format("{0}.mp3", Guid.NewGuid());
            string fileNameRecorded = Path.Combine(rootFolder, musicFolder, fileName);
            File.Create(fileNameRecorded);

            File.WriteAllBytes(fileNameRecorded, mySound.MyAudio);

            _player.SetDataSource(fileNameRecorded);
            _player.Prepare();
            _player.Start();
        }
        catch (IOException)
{
            Log.Error(TAG, "There was an error trying to start the MediaPlayer!");
        }

    }
    public static void DoThePlaying()
    {
        var _player = new MediaPlayer();
        try
        {
            _player.Pause();
        }
        catch (IOException)
{
            Log.Error(TAG, "There was an error trying to pause the MediaPlayer!");
        }
    }
}