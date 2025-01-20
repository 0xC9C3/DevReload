using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using DevReload.Proxy;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using log4net;

namespace DevReload;

public class DevReloadPlugin : IPlugin
{
    DevReloadPlugin()
    {
        if (fileWatcher != null)
        {
            return;
        }

        Log.Debug($"[DevReload] init - target {DevReload.Settings.Instance.TargetPath}");
        fileWatcher = CreateFileWatcher(DevReload.Settings.Instance.TargetPath);
        Store.LoadAssembly();
    }

    public static readonly ILog Log = Logger.GetLoggerInstanceForType();

    public UserControl Control => new();

    public JsonSettings Settings => DevReload.Settings.Instance;

    public string Author => "0xC9C3";

    public string Description => "Hot Reload";

    public string Name => "DevReload";

    public string Version => "stable";

    private static FileSystemWatcher fileWatcher;
    private string lastFileMD5;


    public void Deinitialize()
    {
    }

    public void Disable()
    {
    }

    public void Enable()
    {
    }

    public void Initialize()
    {

    }

    public async Task<LogicResult> Logic(Logic logic)
    {
        return LogicResult.Unprovided;
    }

    public MessageResult Message(Message message)
    {
        return MessageResult.Unprocessed;
    }

    private FileSystemWatcher CreateFileWatcher(string path)
    {
        if (fileWatcher != null)
        {
            return fileWatcher;
        }

        FileSystemWatcher watcher = new FileSystemWatcher();
        watcher.Path = Path.GetDirectoryName(path);
        watcher.NotifyFilter = NotifyFilters.LastWrite;
        watcher.Filter = Path.GetFileName(path);

        watcher.Changed += new FileSystemEventHandler(OnChanged);
        watcher.EnableRaisingEvents = true;
        return watcher;
    }

    private void OnChanged(object source, FileSystemEventArgs e)
    {
        try
        {
            string md5 = CalcFileMD5(e.FullPath);
            if (lastFileMD5 == md5)
            {
                return;
            }

            // Specify what is done when a file is changed, created, or deleted.
            Log.Debug($"Reloading {e.FullPath}");
            bool wasRunning = BotManager.IsRunning;
            if (wasRunning) {
                BotManager.Stop();
                Log.Debug($"Stop Bot for reload");
                while (BotManager.IsRunning) {Thread.Sleep(500);}
            }

            lastFileMD5 = md5;
            Store.DeinitializeAll();
            Store.LoadAssembly();
            Store.InitializeAll();

            if (wasRunning) {
                Log.Debug($"Start Bot for reload");
                BotManager.Start();
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error during reload {ex}");
        }
    }

    private static string CalcFileMD5(string path)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = WaitForLockedFile(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
            }
        }
    }

    // https://stackoverflow.com/a/3677960
    private static FileStream WaitForLockedFile(string fullPath, FileMode mode, FileAccess access, FileShare share)
    {
        for (int numTries = 0; numTries < 10; numTries++)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fullPath, mode, access, share);
                return fs;
            }
            catch (IOException)
            {
                if (fs != null)
                {
                    fs.Dispose();
                }
                Thread.Sleep(50);
            }
        }

        Log.Error($"Failed to wait for file");

        return null;
    }
}
