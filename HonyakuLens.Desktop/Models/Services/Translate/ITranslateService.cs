using System.Threading.Tasks;

namespace HonyakuLens.Desktop.Models.Services.Translate
{
    interface ITranslateService
    {
        Task<string> TranslateAsync(string text);
    }
}
