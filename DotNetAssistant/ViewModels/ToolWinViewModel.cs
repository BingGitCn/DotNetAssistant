using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNetAssistant.Views;
using Prism.Mvvm;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DotNetAssistant.ViewModels
{
    public partial class ToolWinViewModel : ObservableObject
    {
        public ToolWinViewModel()
        {
            isTopCrop = GlobalVars.systemConfig.IsTopCrop;
            if (IsTopCrop)
                TopWinCrop();
            else
                UnTopWinCrop();
        }

        [ObservableProperty]
        private bool isTopCrop = false;

        private DispatcherTimer timer = new DispatcherTimer();

        [RelayCommand]
        private void TopWinCrop()
        {
            GlobalVars.systemConfig.IsTopCrop = true;
            GlobalVars.WriteJson(GlobalVars.systemConfig, AppDomain.CurrentDomain.BaseDirectory + "SystemConfig.json");

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            if (dataObject.GetDataPresent(DataFormats.Bitmap))
            {
                var image = Clipboard.GetImage();

                if (!AreBitmapsEqual(image, lastImage))
                {
                    // 显示并置顶新图像窗口
                    ShowImageWindow(image);
                    lastImage = image;
                }
            }
        }

        private BitmapSource lastImage = null;

        private void ShowImageWindow(BitmapSource image)
        {
            int width = image.PixelWidth;
            int height = image.PixelHeight;

            DotNetAssistant.Views.CropImageWindow imageWindow = new DotNetAssistant.Views.CropImageWindow(image);
            imageWindow.SetWH(width, height);
            imageWindow.Topmost = true; // 将新窗口置顶显示
            imageWindow.Owner = Application.Current.MainWindow;
            imageWindow.Show();
        }

        private bool AreBitmapsEqual(BitmapSource bitmap1, BitmapSource bitmap2)
        {
            if (bitmap1 == null && bitmap2 == null)
                return true;
            if (bitmap1 == null || bitmap2 == null)
                return false;
            if (bitmap1.PixelWidth != bitmap2.PixelWidth || bitmap1.PixelHeight != bitmap2.PixelHeight)
                return false;

            // 对比图像像素
            int stride = (bitmap1.PixelWidth * bitmap1.Format.BitsPerPixel + 7) / 8;
            byte[] pixels1 = new byte[bitmap1.PixelHeight * stride];
            byte[] pixels2 = new byte[bitmap2.PixelHeight * stride];
            bitmap1.CopyPixels(pixels1, stride, 0);
            bitmap2.CopyPixels(pixels2, stride, 0);
            for (int i = 0; i < pixels1.Length; i++)
            {
                if (pixels1[i] != pixels2[i])
                    return false;
            }

            return true;
        }

        [RelayCommand]
        private void UnTopWinCrop()
        {
            GlobalVars.systemConfig.IsTopCrop = false;
            GlobalVars.WriteJson(GlobalVars.systemConfig, AppDomain.CurrentDomain.BaseDirectory + "SystemConfig.json");

            timer.Stop();
        }
    }
}