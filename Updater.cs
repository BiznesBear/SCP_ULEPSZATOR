using System.Net;
using System.Reflection;

namespace SCP_ULEPSZATOR;

public static class Updater
{
    public const string ULEPSZATOR_LATEST_DLL_URL = "https://github.com/BiznesBear/SCP_ULEPSZATOR/releases/latest/download/SCP_ULEPSZATOR.dll";
    public const string ULEPSZATOR_LATEST_VERSION_URL = $"https://github.com/BiznesBear/SCP_ULEPSZATOR/blob/main/{VERSION_FILE_NAME}";
    public const string VERSION_FILE_NAME = "version.txt";

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

    public static void DownloadLatest()
    {
        using (WebClient wc = new WebClient())
        {
            wc.DownloadFileAsync(
                new Uri(ULEPSZATOR_LATEST_DLL_URL),
                ".\\UlepszatorTmp.dll"
            );
        }
    }
}
