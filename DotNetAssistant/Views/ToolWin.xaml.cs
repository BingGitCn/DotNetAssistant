using System.Globalization;
using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DotNetAssistant.Views
{
    /// <summary>
    /// Interaction logic for ToolWin
    /// </summary>
    public partial class ToolWin : UserControl
    {
        public ToolWin()
        {
            InitializeComponent();
        }
    }

    public class IntToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && (int)value == 0)
            {
                // 柔和红绿灰配色方案
                return new SolidColorBrush(Color.FromArgb(255, 255, 111, 111)); // 柔和红色
            }
            else if (value is int && (int)value == 1)
            {
                return new SolidColorBrush(Color.FromArgb(255, 111, 204, 111)); // 柔和绿色
            }
            else
            {
                return new SolidColorBrush(Color.FromArgb(255, 189, 189, 189)); // 柔和灰色
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IntToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && (int)value == 0)
            {
                return "连接失败";
            }
            else if (value is int && (int)value == 1)
            {
                return "连接成功";
            }
            else
            {
                return "正在连接";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}