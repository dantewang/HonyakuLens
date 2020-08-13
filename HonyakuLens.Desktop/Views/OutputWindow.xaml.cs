using HonyakuLens.Desktop.ViewModels;
using System.Windows;

namespace HonyakuLens.Desktop.Views
{
    /// <summary>
    /// OutputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OutputWindow : Window
    {
        private OutputWindowModel Model
        {
            get { return (OutputWindowModel)Resources["ViewModel"]; }
        }

        public OutputWindow()
        {
            InitializeComponent();

            ShowInTaskbar = false;

            Closed += Model.OnClosed;
        }
    }
}
