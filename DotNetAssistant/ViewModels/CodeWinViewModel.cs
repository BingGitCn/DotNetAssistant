using CommunityToolkit.Mvvm.ComponentModel;

namespace DotNetAssistant.ViewModels
{
    public partial class CodeWinViewModel : ObservableObject
    {
        public CodeWinViewModel()
        {
            GlobalVars.SetCodesEventArgs.Subscribe(codes => Codes = codes); ;
            GlobalVars.StartGetCodesEventArgs.Subscribe(GetCodes);
        }

        [ObservableProperty]
        private string _codes = "Edit by Bing";

        private void GetCodes()
        {
            GlobalVars.GetCodesEventArgs.Publish(Codes);
        }
    }
}