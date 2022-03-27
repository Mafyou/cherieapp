using Android.Media;
using Android.Util;

namespace CherieAppUploadZik.Helpers;

public sealed class RecorderHelper
{
    #region Singleton
    private RecorderHelper()
    {

    }
    private static readonly Lazy<RecorderHelper> lazy = 
                        new Lazy<RecorderHelper>(() => new RecorderHelper());
    public static RecorderHelper Instance
    {
        get => lazy.Value;
    }
    #endregion

    private MediaRecorder _recorder;
    public bool IsRecorded { get; private set; } = false;

    const string TAG = "RecordSound";
    public void OnRecord(bool start)
    {
        if (start)
        {
            StartRecording();
        }
        else
        {
            StopRecording();
        }
    }

    void StartRecording()
    {
        _recorder = new MediaRecorder();
        _recorder.SetAudioSource(AudioSource.Mic);
        _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
        FilesManager.Instance.GenerateNewRecord();
        _recorder.SetOutputFile(FilesManager.Instance.FileNameRecorded);
        _recorder.SetAudioEncoder(AudioEncoder.AmrNb);

        try
        {
            _recorder.Prepare();
        }
        catch (IOException ioe)
        {
            Log.Error(TAG, ioe.ToString());
        }

        _recorder.Start();
    }

    void StopRecording()
    {
        if (_recorder == null)
        {
            return;
        }
        _recorder.Stop();
        _recorder.Release();
        _recorder = null;

        IsRecorded = true;
    }
}