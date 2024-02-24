using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DotNetAssistant.Views
{
    /// <summary>
    /// CropImageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CropImageWindow : Window
    {
        private bool isDragging = false;
        private Point startPoint;

        public CropImageWindow(BitmapSource image)
        {
            InitializeComponent();
            img.Source = image;
            img.Stretch = Stretch.Uniform;
            // 注册窗口鼠标事件
            this.MouseDown += CropImageWindow_MouseDown;
            this.MouseMove += CropImageWindow_MouseMove;
            this.MouseUp += CropImageWindow_MouseUp;
            Loaded += CropImageWindow_Loaded;
        }

        private void CropImageWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var cursorPosition = System.Windows.Forms.Control.MousePosition;

            double mouseX = cursorPosition.X;
            double mouseY = cursorPosition.Y;

            this.Left = mouseX * scale - this.Width - 10;
            this.Top = mouseY * scale - this.Height - 10;
            this.WindowState = WindowState.Normal;
        }

        private double scale = 1;

        public void SetWH(double w, double h)
        {
            double dpiX, dpiY;
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            {
                dpiX = g.DpiX;
                dpiY = g.DpiY;
            }

            scale = 96.0 / dpiX;

            this.Width = w * scale;
            this.Height = h * scale;
        }

        private void CropImageWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // 鼠标左键按下时开始拖动
            if (e.ChangedButton == MouseButton.Left)
            {
                isDragging = true;
                startPoint = e.GetPosition(this);
            }
        }

        private void CropImageWindow_MouseMove(object sender, MouseEventArgs e)
        {
            // 拖动窗口
            if (isDragging)
            {
                Point currentPoint = e.GetPosition(this);
                this.Left += currentPoint.X - startPoint.X;
                this.Top += currentPoint.Y - startPoint.Y;
            }
        }

        private void CropImageWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // 鼠标左键释放时停止拖动
            if (e.ChangedButton == MouseButton.Left)
            {
                isDragging = false;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                this.Close();
            }
        }
    }
}