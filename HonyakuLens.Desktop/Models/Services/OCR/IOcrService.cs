using System.Drawing;
using System.Threading.Tasks;

namespace HonyakuLens.Desktop.Models.Services.OCR
{
    interface IOcrService
    {
        Task<string> OcrAsync(Image image);
    }
}
