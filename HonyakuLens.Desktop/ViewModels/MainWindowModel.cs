using HonyakuLens.Desktop.ViewModels.Messaging;
using System;
using System.Drawing;
using System.Windows.Input;

namespace HonyakuLens.Desktop.ViewModels
{
    class MainWindowModel
    {
        public MainProcessCommand Command { get; } = new MainProcessCommand();

        public void Initialize(GetCaptureRegionFunc getCaptureRegionFunc, OpenWindowFunc openWindowFunc)
        {
            Command.GetCaptureRegion = getCaptureRegionFunc;
            Command.OpenWindow = openWindowFunc;
        }

        internal class MainProcessCommand : ICommand
        {
            public GetCaptureRegionFunc GetCaptureRegion;
            public OpenWindowFunc OpenWindow;

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                var rectangle = GetCaptureRegion();

                var bitmap = new Bitmap(
                    rectangle.Width, rectangle.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                var graphics = Graphics.FromImage(bitmap);

                graphics.CopyFromScreen(rectangle.Left, rectangle.Top, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);

                bitmap.Save(".\\capture.bmp");

                OpenWindow();

                MessageBus.Default.Send(Destinations.SCREENSHOT, new Message() { Payload = bitmap });
            }
        }
    }

    public delegate Rectangle GetCaptureRegionFunc();
    public delegate void OpenWindowFunc();

}
