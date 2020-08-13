using HonyakuLens.Desktop.Models.Services.OCR;
using HonyakuLens.Desktop.Models.Services.Translate;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HonyakuLens.Desktop.Models
{
    class Translation : INotifyPropertyChanged
    {
        private readonly IOcrService _ocrService = new GoogleCloudVisionV1OCRService();
        private readonly ITranslateService _translateService = new GoogleCloudTranslateV3TranslateService();
        private string _originalText;
        private string _translatedText;

        public string OriginalText
        {
            get => _originalText;
            set => Set(ref _originalText, value);
        }

        public string TranslatedText
        {
            get => _translatedText;
            set => Set(ref _translatedText, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task OcrAsync(Image image)
        {
            if ((image == null) || image.Size.IsEmpty)
            {
                return;
            }

            OriginalText = await _ocrService.OcrAsync(image);
        }

        public async Task TranslateAsync() {
            if (String.IsNullOrEmpty(OriginalText) || String.IsNullOrWhiteSpace(OriginalText))
            {
                return;
            }

            TranslatedText = await _translateService.TranslateAsync(OriginalText);
        }

        protected void Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            oldValue = newValue;

            OnPropertyChanged(propertyName);
        }
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
