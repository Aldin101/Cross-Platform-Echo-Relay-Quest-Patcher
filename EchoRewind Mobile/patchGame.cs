using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Maui;
using System.Threading.Tasks;
using BsDiff;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Diagnostics;

namespace EchoRelayInstaller
{
    public partial class patchGame : ContentPage
    {

        Label header;

        public class GameConfig
        {
            public string apiservice_host { get; set; }
            public string configservice_host { get; set; }
            public string loginservice_host { get; set; }
            public string matchingservice_host { get; set; }
            public string serverdb_host { get; set; }
            public string transactionservice_host { get; set; }
            public string publisher_lock { get; set; }
        };

        public String apkGlobal;
        public Servers[] serversGlobal;
        public int selectedServerGlobal;

        public patchGame(String apk, Servers[] servers, int selectedServer)
        {
            apkGlobal = apk;
            serversGlobal = servers;
            selectedServerGlobal = selectedServer;

            Title = "Echo Relay Quest Patcher";
            
            var patchGameMenu = new StackLayout();

            header = new Label
            {
                Text = "Patching APK...",
                FontSize = 24,
                HorizontalOptions = LayoutOptions.Center
            };
            patchGameMenu.Children.Add(header);

            Content = patchGameMenu;


            patchingSystems(servers, selectedServer, apk);
        }

        public bool patching = false;
        string downloadsPath;

        public async Task patchingSystems(Servers[] servers, int selectedServer, String apk)
        {
#if ANDROID
            downloadsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
#endif

#if WINDOWS
            downloadsPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/downloads/";
#endif
            GameConfig config = new GameConfig();
            config.apiservice_host = $"http://{servers[selectedServer].IP}:{servers[selectedServer].port}/api";
            config.configservice_host = $"ws://{servers[selectedServer].IP}:{servers[selectedServer].port}/config";
            config.loginservice_host = $"ws://{servers[selectedServer].IP}:{servers[selectedServer].port}/login?auth={GlobalVariables.passwordBox.Text}&displayname={GlobalVariables.usernameBox.Text}";
            config.matchingservice_host = $"ws://{servers[selectedServer].IP}:{servers[selectedServer].port}/matching";
            config.serverdb_host = $"ws://{servers[selectedServer].IP}:{servers[selectedServer].port}/serverdb";
            config.transactionservice_host = $"ws://{servers[selectedServer].IP}:{servers[selectedServer].port}/transaction";
            config.publisher_lock = "rad15_live";

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(config);

            var filePath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), "config.json");
            await File.WriteAllTextAsync(filePath, json);
            string[] apkPath = new string[] { apk };
            Thread thread = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                StartPatching(apkPath);
            });
            patching = true;
            thread.Start();
            
            while (thread.IsAlive)
            {
                await Task.Delay(1000);
            }
            patching = false;

#if ANDROID
            header.Text = "APK Ready, it can be found in your downloads folder\nPlease sign the APK with apk-signer and load onto headset with bugjaeger. Both can be found in the Google Play Store.";
#endif
#if WINDOWS
            header.Text = "APK Ready, it can be found in your downloads folder\nYou can load it onto your headset with SideQuest";
#endif
        }

        protected override bool OnBackButtonPressed()
        {
            if (patching)
            {
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }

        private class Hashes
        {
            public const string APK = "c14c0f68adb62a4c5deaef46d046f872"; // Hash of 
        }

        string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public async Task ExitLog(string errorString, bool error = true)
        {
            if (error)
            {
                var crashLog = new StringBuilder();
                crashLog.AppendLine("EchoRelayInstaller has crashed!\n");
                crashLog.AppendLine($"Error: {errorString}\n");
                crashLog.AppendLine($"Time: {DateTime.Now}\n");
                crashLog.AppendLine($"Version: {Assembly.GetExecutingAssembly().GetName().Version}\n");
                var crashLogPath = Path.Combine(downloadsPath, $"EchoRelayInstallerCrash{DateTime.Now}.txt");
                await File.WriteAllTextAsync(crashLogPath, crashLog.ToString());
                Environment.Exit(0);
            }
        }


        static bool CheckJson(JObject jsonObject)
        {
            // Make sure the services exist
            if (!jsonObject.ContainsKey("configservice_host"))
                return false;
            if (!jsonObject.ContainsKey("loginservice_host"))
                return false;
            if (!jsonObject.ContainsKey("matchingservice_host"))
                return false;
            if (!jsonObject.ContainsKey("publisher_lock"))
                return false;

            // Make sure they are all strings
            if (jsonObject.GetValue("configservice_host")!.Type != JTokenType.String)
                return false;
            if (jsonObject.GetValue("loginservice_host")!.Type != JTokenType.String)
                return false;
            if (jsonObject.GetValue("matchingservice_host")!.Type != JTokenType.String)
                return false;
            if (jsonObject.GetValue("publisher_lock")!.Type != JTokenType.String)
                return false;

            // Make sure the hosts are valid URLs
            if (!Uri.IsWellFormedUriString(jsonObject.Value<string>("configservice_host"), UriKind.Absolute))
                return false;
            if (!Uri.IsWellFormedUriString(jsonObject.Value<string>("loginservice_host"), UriKind.Absolute))
                return false;
            if (!Uri.IsWellFormedUriString(jsonObject.Value<string>("matchingservice_host"), UriKind.Absolute))
                return false;

            return true;
        }

        void CheckPrerequisites(string originalApkPath, string configPath)
        {
            if (!File.Exists(originalApkPath))
                ExitLog("Invalid EchoVR APK: Please drag and drop EchoVR APK onto exe");

            if (CalculateMD5(originalApkPath) != Hashes.APK)
                ExitLog("Invalid EchoVR APK (Hash mismatch) : please download the correct APK via\nOculusDB: https://oculusdb.rui2015.me/id/2215004568539258\nVersion: 4987566" + CalculateMD5(originalApkPath));

            if (!File.Exists(configPath))
                ExitLog("Invalid Config: Config not found, please confirm config is in the same directory as the executable");
#if WINDOWS
            if (!Environment.GetEnvironmentVariable("PATH")!.ToLower().Contains("java"))
                ExitLog("Java not found: Please confirm you have JDK Development Kit installed");
#endif
            string ConfigString;
            try
            {
                ConfigString = File.ReadAllText(configPath);
            }
            catch (Exception)
            {
                ExitLog("Invalid Config: Config stream unreachable, please confirm no other programs are modifying config.json");
                return; // Just to make the compiler happy
            }

            JObject ConfigJson;
            try
            {
                ConfigJson = JObject.Parse(ConfigString);
            }
            catch (Exception)
            {
                ExitLog("Invalid Config: Json could not be parsed, please confirm config formatting is correct");
                return;
            }

            if (!CheckJson(ConfigJson))
                ExitLog("Invalid Config: Service endpoints incorrect, please confrim all endpoitns are correct");

            using var libpnsovr_patchStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EchoRelayInstaller.Resources.Raw.libpnsovr_patch.bin");
            if (libpnsovr_patchStream == null)
                ExitLog("libpnsovr_patch missing!");

            using var uberJarStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EchoRelayInstaller.Resources.Raw.uber.jar");
            if (uberJarStream == null)
               ExitLog("uber.jar missing!");
        }

        public async void StartPatching(string[] args)
        {
            Console.WriteLine("Parsing arguments...");
            if (args.Length == 0)
                ExitLog("Invalid EchoVR APK: Please drag and drop EchoVR APK onto exe");

            Console.WriteLine("Generating paths...");
            var originalApkPath = args[0];
            var baseDir = Path.GetDirectoryName(args[0]);
            var newApkPath = Path.Join(downloadsPath, $"r15_goldmaster_store_patched.apk");
            var configPath = Path.Join(Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "config.json");

            Console.WriteLine("Checking prerequisites...");
            CheckPrerequisites(originalApkPath, configPath);

            Console.WriteLine("Creating extraction directory...");
            var extractedApkDir = Path.Join(Path.GetTempPath(), "EchoQuestUnzip");
            if (Directory.Exists(extractedApkDir))
                Directory.Delete(extractedApkDir, true);
            Directory.CreateDirectory(extractedApkDir);

            Console.WriteLine("Extracting files...");
            using (var archive = ZipFile.OpenRead(originalApkPath))
            {
                foreach (var entry in archive.Entries)
                {
                    var destinationPath = Path.GetFullPath(Path.Combine(extractedApkDir, entry.FullName));
                    if (!destinationPath.StartsWith(extractedApkDir, StringComparison.Ordinal))
                        throw new InvalidOperationException("Trying to create a file outside of the extraction directory.");
                    if (entry.Name != "")
                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
                    entry.ExtractToFile(destinationPath, true);
                }
            }
            var extractedLocalPath = Path.Join(extractedApkDir, "assets", "_local");
            var extractedPnsRadOvrPath = Path.Join(extractedApkDir, @"lib/arm64-v8a/libpnsovr.so");

            Console.WriteLine("Copying config.json...");
            Directory.CreateDirectory(extractedLocalPath); // No need to check for existence, as the hash will capture that
            File.Copy(configPath, Path.Join(extractedLocalPath, "config.json"));


            Console.WriteLine("Patching pnsradovr.so...");
            using var oldPnsOvrFile = File.OpenRead(extractedPnsRadOvrPath);
            using var newPnsOvrFile = File.Create(extractedPnsRadOvrPath + "_patched");
            BinaryPatch.Apply(oldPnsOvrFile, () => Assembly.GetExecutingAssembly().GetManifestResourceStream("EchoRelayInstaller.Resources.Raw.libpnsovr_patch.bin"), newPnsOvrFile);
            oldPnsOvrFile.Close();
            newPnsOvrFile.Close();

            Console.WriteLine("Swapping pnsradovr.so...");
            File.Delete(extractedPnsRadOvrPath);
            File.Move(extractedPnsRadOvrPath + "_patched", extractedPnsRadOvrPath);

            Console.WriteLine("Creating miscellaneous directory...");
            string miscDir = Path.Join(Path.GetTempPath(), "EchoQuest");
            if (Directory.Exists(miscDir))
                Directory.Delete(miscDir, true);
            Directory.CreateDirectory(miscDir);

            string unsignedApkPath;

#if WINDOWS

            Console.WriteLine("Extracting uber.jar...");
            var uberJarPath = Path.Join(miscDir, "uber.jar");
            var uberJarStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EchoRelayInstaller.Resources.Raw.uber.jar");

            Console.WriteLine("Writing uber.jar...");
            var uberJarFile = File.Create(uberJarPath);
            uberJarStream.CopyTo(uberJarFile);
            uberJarStream.Close();
            uberJarFile.Close();

            Console.WriteLine("Creating unsigned apk...");
            unsignedApkPath = Path.Join(miscDir, "unsigned.apk");
            ZipFile.CreateFromDirectory(extractedApkDir, unsignedApkPath);

            Console.WriteLine("Signing unsigned apk...");
            Process process = new();
            process.StartInfo.FileName = "java";
            process.StartInfo.Arguments = $"-jar \"{uberJarPath}\" -a \"{unsignedApkPath}\" --out \"{miscDir}\" --allowResign";
            process.Start();
            process.WaitForExit();

            Console.WriteLine("Moving signed apk...");
            if (File.Exists(newApkPath))
                File.Delete(newApkPath);
            File.Move(Path.Join(miscDir, "unsigned-aligned-debugSigned.apk"), newApkPath);

#endif

#if ANDROID
            Console.WriteLine("Creating apk...");
            unsignedApkPath = Path.Join(miscDir, "unsigned.apk");
            ZipFile.CreateFromDirectory(extractedApkDir, unsignedApkPath);

            Console.WriteLine("Moving apk...");
            if (File.Exists(newApkPath))
                newApkPath = Path.Join(Path.GetDirectoryName(newApkPath)!, $"r15_goldmaster_store_patched_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.apk");
            File.Move(unsignedApkPath, newApkPath);
#endif
            Console.WriteLine("Cleaning up temporary files...");
            Directory.Delete(extractedApkDir, true);
            Directory.Delete(miscDir, true);
            ExitLog("Finished creating patched apk! (r15_goldmaster_store_patched.apk)", false);
        }
    }
}
