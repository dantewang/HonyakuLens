using System.Drawing;
using HonyakuLens.Desktop.Models;
using HonyakuLens.Desktop.ViewModels.Messaging;
using System.ComponentModel;
using System;

namespace HonyakuLens.Desktop.ViewModels
{
    class OutputWindowModel
    {
        public Translation TranslationModel { get; }  = new Translation();

        public OutputWindowModel()
        {
            TranslationModel.PropertyChanged += TranslationModel_PropertyChanged;

            MessageBus.Default.Register(Destinations.SCREENSHOT, Screenshot_Received);
        }

        internal void OnClosed(object sender, EventArgs e)
        {
            MessageBus.Default.Unregister(Destinations.SCREENSHOT, Screenshot_Received);
        }

        private async void TranslationModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Equals(e.PropertyName, "OriginalText"))
            {
                await TranslationModel.TranslateAsync();
            }
        }

        private async void Screenshot_Received(Message message)
        {
            object payload = message.Payload;

            if (payload is Image)
            {
                await TranslationModel.OcrAsync(payload as Image);
            }
        }
    }
}
