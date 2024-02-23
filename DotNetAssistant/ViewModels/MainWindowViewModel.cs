using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using ICSharpCode.AvalonEdit.Rendering;
using Microsoft.Win32;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace DotNetAssistant.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = "DotNet Assistant  V" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [ObservableProperty]
        private int _selectTabIndex = 0;

        [ObservableProperty]
        private string searchCode = "";

        [ObservableProperty]
        private ObservableCollection<ToolCode> sharpList = new ObservableCollection<ToolCode>();

        private ObservableCollection<ToolCode> totalSharpList = new ObservableCollection<ToolCode>();

        [ObservableProperty]
        private int selectSharpIndex = -1;

        private readonly IRegionManager regionManager;

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            GlobalVars.GetCodesEventArgs.Subscribe(GetCodes);
            init();
        }

        private async void init()
        {
            totalToolList.Clear();

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Tools.json"))
            {
                totalToolList = GlobalVars.ReadJson<ObservableCollection<ToolCode>>(AppDomain.CurrentDomain.BaseDirectory + "Tools.json");
            }
            else
            {
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "cmd", Code = "cmd" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "PowerShell", Code = "PowerShell" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "记事本", Code = "notepad" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "计算器", Code = "calc" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "屏幕键盘", Code = "osk" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "控制面板", Code = "control" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "系统设置", Code = "ms-settings:" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "系统信息", Code = "msinfo32" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "程序和功能", Code = "appwiz.cpl" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "设备管理器", Code = "devmgmt.msc" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "计算机管理", Code = "compmgmt.msc" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "磁盘管理", Code = "diskmgmt.msc" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "网络连接", Code = "ncpa.cpl" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "防火墙", Code = "firewall.cpl" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "防火墙规则", Code = "wf.msc" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "注册表", Code = "regedit" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "组策略", Code = "gpedit.msc" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "远程控制", Code = "mstsc" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "开机启动", Code = "shell:startup" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "服务", Code = "services.msc" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "电源选项", Code = "powercfg.cpl" });
                totalToolList.Add(new ToolCode() { IsCanDelete = false, Name = "屏幕分辨率", Code = " desk.cpl" });
            }

            for (int i = 0; i < totalToolList.Count; i++)
            {
                try
                {
                    totalToolList[i].ToolIcon = GetFileIcon(totalToolList[i].Code);
                }
                catch { }
            }

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Codes"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Codes");

            string[] sharpJsonFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Codes", "*.json");
            totalSharpList.Clear();
            for (int i = 0; i < sharpJsonFiles.Length; i++)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        totalSharpList.Add(GlobalVars.ReadJson<ToolCode>(sharpJsonFiles[i]));
                    }
                    catch { }
                });
            }

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Tags.json"))
                TagList = GlobalVars.ReadJson<ObservableCollection<TagData>>(AppDomain.CurrentDomain.BaseDirectory + "Tags.json");

            SearchStarted(SearchCode);
        }

        private SemaphoreSlim SearchStartedSS = new SemaphoreSlim(1);

        [RelayCommand]
        private void SearchStarted(string _searchCode)
        {
            if (SearchStartedSS.Wait(0))
            {
                if (SelectTabIndex == 0)
                {
                    var result = totalToolList.Where(s => s.Name.Contains(SearchCode)).ToList();
                    ToolList.Clear();
                    foreach (var item in result)
                    {
                        ToolList.Add(item);
                    }
                }
                if (SelectTabIndex == 1)
                {
                    List<ToolCode> result = new List<ToolCode>();
                    var selectedTags = TagList.Where(s => s.IsSelected == true).ToList();
                    if (selectedTags.Count > 0)
                    {
                        result = (totalSharpList.Where(s => s.Name.Contains(SearchCode))).Where(ns => selectedTags.All(st => (ns.Tags.Select(nst => nst.Name)).Contains(st.Name))).ToList();
                    }
                    else
                        result = totalSharpList.Where(s => s.Name.Contains(SearchCode)).ToList();

                    SharpList.Clear();
                    foreach (var item in result)
                    {
                        SharpList.Add(item);
                    }
                }

                SearchStartedSS.Release();
            }
        }

        [RelayCommand]
        private void AddCode()
        {
            GlobalVars.StartGetCodesEventArgs.Publish();
        }

        private void GetCodes(string codes)
        {
            if (SelectTabIndex == 1)
            {
                if (isSave)
                {
                    isSave = false;
                    SharpList[SelectSharpIndex].Code = codes;
                    SharpList[SelectSharpIndex].Tags.Clear();
                    foreach (var item in TagList)
                        if (item.IsSelected) SharpList[SelectSharpIndex].Tags.Add(item);
                    GlobalVars.WriteJson(SharpList[SelectSharpIndex], AppDomain.CurrentDomain.BaseDirectory + "Codes\\" + SharpList[SelectSharpIndex].IDGuid + ".json");
                    foreach (var item in TagList) item.IsSelected = false;
                    SearchStarted(SearchCode);
                    return;
                }

                if (SearchCode.Replace(" ", "") != "" && codes.Replace(" ", "") != "")
                {
                    ToolCode tc = new ToolCode() { Name = SearchCode, Code = codes };
                    foreach (var item in TagList)
                        if (item.IsSelected) tc.Tags.Add(item);

                    totalSharpList.Add(tc);
                    GlobalVars.WriteJson(tc, AppDomain.CurrentDomain.BaseDirectory + "Codes\\" + tc.IDGuid + ".json");
                    SearchCode = "";
                    SearchStarted(SearchCode);
                }
                else
                {
                    GlobalVars.ShowMessage("【添加步骤】\r\n" +
                        "1. 搜索框输入标题。\r\n" +
                        "2. 代码框输入代码。\r\n" +
                        "3. 新增标签，选择标签。\r\n" +
                        "4. 点击添加按钮。");
                }
            }
        }

        [RelayCommand]
        private void DelCode()
        {
            if (GlobalVars.ShowConfirm("确认删除？"))
                try
                {
                    if (SelectTabIndex == 1)
                    {
                        File.Delete((AppDomain.CurrentDomain.BaseDirectory + "Codes\\" + SharpList[SelectSharpIndex].IDGuid + ".json"));
                        init();
                    }
                }
                catch { }
        }

        private bool isSave = false;

        [RelayCommand]
        private void SaveCode()
        {
            bool rst = GlobalVars.ShowConfirm("请注意标签选择，是否继续保存？");
            if (rst)
            {
                isSave = true;
                GlobalVars.StartGetCodesEventArgs.Publish();
            }
        }

        private bool isNeedToUpdateToolRegion = true;

        [RelayCommand]
        private async Task UpdateToolRegion()
        {
            if (SelectTabIndex == 1 && isNeedToUpdateToolRegion)
            {
                regionManager.Regions["ToolRegion"].RequestNavigate("CodeWin");
                GlobalVars.SetCodesEventArgs.Publish("");
                isNeedToUpdateToolRegion = false;
                await Task.Delay(10);
                SearchCode = "";
                SearchStarted(SearchCode);
            }
            if (SelectTabIndex == 0)
            {
                isNeedToUpdateToolRegion = true;
                regionManager.Regions["ToolRegion"].RequestNavigate("ToolWin");
                await Task.Delay(10);
                SearchCode = "";
            }
        }

        [RelayCommand]
        private void UpdateCode()
        {
            try
            {
                if (SelectTabIndex == 1)
                    GlobalVars.SetCodesEventArgs.Publish(SharpList[SelectSharpIndex].Code);
            }
            catch { }
        }

        [ObservableProperty]
        private string tagName = "";

        [ObservableProperty]
        private ObservableCollection<TagData> tagList = new ObservableCollection<TagData>();

        [RelayCommand]
        private void AddTag()
        {
            if (!string.IsNullOrEmpty(TagName))
            {
                if (TagList.Select(s => s.Name).ToList().Contains(TagName))
                    return;

                TagList.Add(new TagData() { Name = TagName });
                TagName = "";

                for (int i = 0; i < TagList.Count; i++)
                    TagList[i].IsSelected = false;
                GlobalVars.WriteJson(TagList, AppDomain.CurrentDomain.BaseDirectory + "Tags.json");
            }
            else
                for (int i = 0; i < TagList.Count; i++)
                    TagList[i].IsSelected = false;
        }

        [RelayCommand]
        private void ClearTag()
        {
            for (int i = 0; i < TagList.Count; i++)
                TagList[i].IsSelected = false;
        }

        #region Tool

        private ObservableCollection<ToolCode> totalToolList = new ObservableCollection<ToolCode>();

        [ObservableProperty]
        private ObservableCollection<ToolCode> toolList = new ObservableCollection<ToolCode>();

        [ObservableProperty]
        private int selectToolIndex = -1;

        [RelayCommand]
        private void UpdateTool()
        {
            try
            {
                GlobalVars.RunExe(ToolList[SelectToolIndex].Code, "");
            }
            catch { }
        }

        [RelayCommand]
        private void AddTool()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.DefaultExt = ".exe";
                openFileDialog.Filter = "exe文件 (*.exe)|*.exe|所有文件 (*.*)|*.*";
                bool? result = openFileDialog.ShowDialog();
                if (result == true)
                {
                    string filePath = openFileDialog.FileName;
                    string fileName = Path.GetFileNameWithoutExtension(filePath);

                    string fileVersionInfo = GetFileVersionInfo(filePath);
                    if (fileVersionInfo != "")
                        fileName = fileVersionInfo;

                    totalToolList.Insert(totalToolList.Count, new ToolCode() { Name = fileName, Code = filePath });
                    GlobalVars.WriteJson(totalToolList, AppDomain.CurrentDomain.BaseDirectory + "Tools.json");
                    init();
                }
            }
            catch { }
        }

        private string GetFileVersionInfo(string filePath)
        {
            try
            {
                // 获取文件版本信息
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filePath);
                // 检查文件版本信息是否包含产品名称
                if (!string.IsNullOrEmpty(versionInfo.ProductName))
                {
                    return versionInfo.FileDescription;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("无法获取文件版本信息: " + ex.Message);
            }

            return "";
        }

        [RelayCommand]
        private void DelTool()
        {
            try
            {
                if (ToolList[SelectToolIndex].IsCanDelete)
                {
                    if (GlobalVars.ShowConfirm("确认删除：" + ToolList[SelectToolIndex].Name + "？"))
                    {
                        totalToolList.RemoveAt(SelectToolIndex);
                        GlobalVars.WriteJson(totalToolList, AppDomain.CurrentDomain.BaseDirectory + "Tools.json");
                        init();
                    }
                }
                else GlobalVars.ShowMessage("禁止删除！");
            }
            catch { }
        }

        [RelayCommand]
        private void SortTool()
        {
            try
            {
                var sort = totalToolList.OrderBy(x => x.Name).ToList();
                totalToolList.Clear();
                foreach (var item in sort)
                    totalToolList.Add(item);

                GlobalVars.WriteJson(totalToolList, AppDomain.CurrentDomain.BaseDirectory + "Tools.json");
                init();
            }
            catch { }
        }

        [RelayCommand]
        private void ReverseTool()
        {
            try
            {
                var sort = totalToolList.Reverse().ToList();
                totalToolList.Clear();
                foreach (var item in sort)
                    totalToolList.Add(item);

                GlobalVars.WriteJson(totalToolList, AppDomain.CurrentDomain.BaseDirectory + "Tools.json");
                init();
            }
            catch { }
        }

        [RelayCommand]
        private void RemoveTool()
        {
            try
            {
                var sort = totalToolList.Where(x => (!x.IsCanDelete || (x.IsCanDelete && File.Exists(x.Code)))).ToList();
                totalToolList.Clear();
                foreach (var item in sort)
                    totalToolList.Add(item);

                GlobalVars.WriteJson(totalToolList, AppDomain.CurrentDomain.BaseDirectory + "Tools.json");
                init();
            }
            catch { }
        }

        private Icon GetFileIcon(string filePath)
        {
            Icon icon = null;
            try
            {
                // 提取与文件关联的图标
                icon = Icon.ExtractAssociatedIcon(filePath);
            }
            catch (Exception ex)
            {
            }
            return icon;
        }

        #endregion Tool
    }
}