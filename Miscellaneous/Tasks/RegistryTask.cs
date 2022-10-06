using System.Diagnostics;
using Microsoft.Win32;

namespace Miscellaneous.Tasks {
    #pragma warning disable CA1416 // Validate platform compatibility
    internal class RegistryTask : TaskBase {
        private const string URI_SCHEME = "yturi";

        protected override void ExecuteTask() {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && args[1].StartsWith($"{URI_SCHEME}:")) {
                Process process = new();
                process.StartInfo.FileName = $"https://www.youtube.com/watch?v={args[1].Replace($"{URI_SCHEME}:", "")}";
                process.StartInfo.WorkingDirectory = new FileInfo(args[0]).Directory?.FullName;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process.Start();
            } else {
                if (Registry.ClassesRoot.OpenSubKey(URI_SCHEME) != null) {
                    Logger.Log($"HKEY_CLASSES_ROOT/{URI_SCHEME} registry key exists");
                } else {
                    Logger.Log($"Create HKEY_CLASSES_ROOT/{URI_SCHEME} registry key");
                    using RegistryKey key = Registry.ClassesRoot.CreateSubKey(URI_SCHEME);
                    key.SetValue(null, "URL:Open youtube video");
                    key.SetValue("URL Protocol", "");
                    using RegistryKey subKeyShell = key.CreateSubKey("shell");
                    using RegistryKey subKeyOpen = subKeyShell.CreateSubKey("open");
                    using RegistryKey subKeyCommand = subKeyOpen.CreateSubKey("command");
                    subKeyCommand.SetValue(null, $"\"{Environment.GetCommandLineArgs()[0].Replace(".dll", ".exe")}\" %1");
                }
            }
        }
    }
    #pragma warning restore CA1416 // Validate platform compatibility
}
