using CherieAppUploadZik.Models;
using CherieAppUploadZik.Services;
using Newtonsoft.Json;
using System.Text;

namespace CherieAppUploadZik.Helpers;

public class FilesManager
{
    #region Singleton
    private FilesManager()
    {
        GenerateNewRecord();
    }
    private static readonly Lazy<FilesManager> lazy =
                        new Lazy<FilesManager>(() => new FilesManager());
    public static FilesManager Instance
    {
        get => lazy.Value;
    }
    #endregion

    public void GenerateNewRecord()
    {
        string rootFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
        string musicFolder = Android.OS.Environment.DirectoryMusic;
        FileName = string.Format("{0}.mp3", Guid.NewGuid());
        _fileNameRecorded = Path.Combine(rootFolder, musicFolder, FileName);
        File.Create(_fileNameRecorded);
    }
    private string _fileNameRecorded;
    public string FileName { get; set; }
    public string FileNameRecorded
    {
        get
        {
            
            return _fileNameRecorded;
        }
        private set => _fileNameRecorded = value;
    }

    /// <summary>
    /// Push a new sound to the db
    /// </summary>
    /// <param name="mySound">The sound to send.</param>
    /// <returns>True if it's correct otherwise, false.</returns>
    public async Task<bool> PushSound(MySound mySound)
    {
        try
        {
            var seri = JsonConvert.SerializeObject(mySound);
            var json = await APIService.Instance.Client.PostAsync("/create",
                new StringContent(seri, Encoding.UTF8, "application/json")
                );
            if (json.IsSuccessStatusCode) return true;
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public async Task<bool> DeleteSound(MySound mySound)
    {
        try
        {
            var seri = JsonConvert.SerializeObject(mySound);
            var json = await APIService.Instance.Client.PostAsync("/delete",
                new StringContent(seri, Encoding.UTF8, "application/json")
                );
            if (json.IsSuccessStatusCode) return true;
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}