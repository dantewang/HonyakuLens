using HonyakuLens.Desktop.ViewModels;
using HonyakuLens.Desktop.ViewModels.Messaging;
using System.Drawing;
using System.Windows;

namespace HonyakuLens.Desktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowModel Model
        {
            get { return (MainWindowModel)Resources["ViewModel"]; }
        }

        private OutputWindow _outputWindow;

        public MainWindow()
        {
            InitializeComponent();

            MaxHeight = 1080;
            MaxWidth = 1920;

            Model.Initialize(GetCaptureRegion, OpenOutputWindow);
        }

        private void OpenOutputWindow()
        {
            if (_outputWindow == null)
            {
                _outputWindow = new OutputWindow();

                _outputWindow.Closed += delegate
                {
                    _outputWindow = null;
                };
            }

            _outputWindow.Show();
        }

        private Rectangle GetCaptureRegion()
        {
            var borderThickness = Border_WindowBorder.BorderThickness;

            int x = (int)(Left + borderThickness.Left);
            int y = (int)(Top + borderThickness.Top + Grid_TitleBar.Height);
            int width = (int)(Width - borderThickness.Left - borderThickness.Right);
            int height = (int)(Height - Grid_TitleBar.Height - borderThickness.Top - borderThickness.Bottom);

            return new Rectangle(x, y, width, height);
        }

    }
}
