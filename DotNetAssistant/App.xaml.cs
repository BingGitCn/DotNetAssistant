using DotNetAssistant.Views;
using Prism.Ioc;
using System.Windows;

namespace DotNetAssistant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //指定名称，可以不填，则默认"ViewA"
            containerRegistry.RegisterForNavigation<CodeWin>("CodeWin");
            containerRegistry.RegisterForNavigation<ToolWin>("ToolWin");
        }
    }
}