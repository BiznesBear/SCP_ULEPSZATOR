using MelonLoader;
using System.Net;
using System.Reflection;

namespace SCP_ULEPSZATOR;

public static class Updater
{
    public const string ULEPSZATOR_LATEST_DLL_URL = "https://github.com/BiznesBear/SCP_ULEPSZATOR/releases/latest/download/SCP_ULEPSZATOR.dll";
    public const string ULEPSZATOR_LATEST_VERSION_URL = $"https://raw.githubusercontent.com/BiznesBear/SCP_ULEPSZATOR/refs/heads/main/{VERSION_FILE_NAME}";
    public const string VERSION_FILE_NAME = "version.txt";
    public const string UPDATE_TMP_FILE_NAME = "SCP_ULEPSZATOR.dll";
    

    public static string Version => Reader.ReadToEnd();

    private static StreamReader Reader => new(Assembly.GetExecutingAssembly().GetManifestResourceStream($"{nameof(SCP_ULEPSZATOR)}.{VERSION_FILE_NAME}"));


    public static string CheckLatestVersion()
    {
        try {
            using (WebClient wc = new WebClient())
            {
                Stream myStream = wc.OpenRead(ULEPSZATOR_LATEST_VERSION_URL);
                StreamReader sr = new StreamReader(myStream);
                return sr.ReadToEnd();
            }
        }
        catch
        {
            Core.Instance.LogAll("Cannot access latest version. Please check https://github.com/BiznesBear/SCP_ULEPSZATOR for more information");
        }

        return "Cannot access latest version. Please check https://github.com/BiznesBear/SCP_ULEPSZATOR for more information";
    }


    public static bool CanUpdate(out string result)
    {
        if(Version == CheckLatestVersion())
        {
            result = "Ulepszator is up to date";
            return false;
        }

        result = "Ulepszator is outdated";
        return true;
    }


    

    public static bool DownloadLatest()
    {
        try
        {
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFileAsync(
                    new Uri(ULEPSZATOR_LATEST_DLL_URL),
                    $"{Path.GetDirectoryName(Core.Instance.MelonAssembly.Location)}\\{UPDATE_TMP_FILE_NAME}"
                );
            }
            return true;
        }
        catch
        {
            Core.Instance.LogAll("Failed to download latest version of Ulepszator");
            return false;
        }
        
    }
}
