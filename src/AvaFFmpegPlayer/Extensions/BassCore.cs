using Avalonia.Logging;

namespace AvaFFmpegPlayer.Extensions;

public  static class BassCore
{
    internal static bool IsInitialize { get; private set; } = false;

    private static ParametrizedLogger? _parametrizedLogger;

    public static void Initialize()
    {
        IsInitialize = false;
        _parametrizedLogger = Logger.TryGet(LogEventLevel.Error, $"{nameof(BassCore)}");
        InitDll();
        IsInitialize = ManagedBass.Bass.Init();
    }

    private static bool InitDll()
    {
        bool canInit = true;
        try
        {
            string sourceFileName = string.Empty, dllPath = string.Empty;

            if (OperatingSystem.IsWindows())
            {
                dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bass.dll");
                if (!File.Exists(dllPath))
                {
                    var platform = $"win-{ArchitectureString}";
                    if (platform.Equals("win-arm", StringComparison.CurrentCultureIgnoreCase))
                    {
                        canInit = false;
                        _parametrizedLogger?.Log(null, "Bass cannot run in win-arm platform.Stop init.");
                    }
                    else
                        sourceFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libBass", platform,
                            "bass.dll");
                }
            }
            else if (OperatingSystem.IsLinux())
            {
                dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libbass.so");
                if (!File.Exists(dllPath))
                {
                    var platform = $"linux-{ArchitectureString}";
                    sourceFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libBass", platform,
                        "libbass.so");
                }
            }
            else if (OperatingSystem.IsMacOS())
            {
                dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libbass.dylib");
                if (!File.Exists(dllPath))
                    sourceFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libBass", "osx",
                        "libbass.dylib");
            }

            if (!string.IsNullOrWhiteSpace(sourceFileName) && canInit)
            {
                if (File.Exists(sourceFileName))
                    File.Copy(sourceFileName, dllPath, true);
                else
                    canInit = false;
            }
        }
        catch (Exception ex)
        {
            canInit = false;
            _parametrizedLogger?.Log(null, ex.Message);
        }

        return canInit;
    }


    private static string ArchitectureString
    {
        get
        {
            string architectureString = RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X86 => "x86",
                Architecture.X64 => "x64",
                Architecture.Arm => "arm",
                Architecture.Arm64 => "arm64",
                _ => "unknow"
            };

            return architectureString;
        }
    }
}