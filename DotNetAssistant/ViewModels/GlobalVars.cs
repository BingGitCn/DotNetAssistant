using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAssistant.ViewModels
{
    public static class GlobalVars
    {
        public static PubSubEvent<string> SetCodesEventArgs = new PubSubEvent<string>();
        public static PubSubEvent StartGetCodesEventArgs = new PubSubEvent();
        public static PubSubEvent<string> GetCodesEventArgs = new PubSubEvent<string>();

        public static T ReadJson<T>(string jsonFileName)
        {
            FileInfo fileInfo = new FileInfo(jsonFileName);
            if (fileInfo.Exists)
            {
                using (FileStream stream = new FileStream(jsonFileName, FileMode.Open, FileAccess.Read))
                {
                    using StreamReader streamReader = new StreamReader(stream);
                    string value = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(value);
                }
            }

            throw new FileNotFoundException();
        }

        public static void WriteJson(object objectToSerialize, string jsonFileName)
        {
            using FileStream fileStream = new FileStream(jsonFileName, FileMode.OpenOrCreate, FileAccess.Write);
            fileStream.SetLength(0L);
            using StreamWriter streamWriter = new StreamWriter(fileStream);
            string value = JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented);
            streamWriter.Write(value);
        }

        //弹出窗口确认
        public static void ShowMessage(string msg)
        {
            HandyControl.Controls.MessageBox.Show(msg, "消息提示");
        }

        public static bool ShowConfirm(string msg)
        {
            return HandyControl.Controls.MessageBox.Ask(msg, "确认操作") == System.Windows.MessageBoxResult.OK;
        }

        public static void RunExe(string exe, string arguments = "")
        {
            ProcessStartInfo psi = new ProcessStartInfo(exe, arguments);
            psi.UseShellExecute = true;
            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生错误：" + ex.Message);
            }
        }
    }

    public class ToolCode
    {
        public string Name { set; get; }
        public bool IsCanDelete { set; get; } = true;

        [JsonIgnore]
        public Icon ToolIcon { set; get; }

        public string Code { set; get; }
        public string IDGuid { set; get; } = Guid.NewGuid().ToString();

        public ObservableCollection<TagData> Tags { set; get; } = new ObservableCollection<TagData>();
    }

    public partial class TagData : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private bool isSelected;
    }
}