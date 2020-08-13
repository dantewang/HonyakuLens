using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HonyakuLens.Desktop.Models.Services.Translate
{
    class GoogleCloudTranslateV3TranslateService : ITranslateService
    {
        private readonly string _projectID;

        public GoogleCloudTranslateV3TranslateService()
        {
            _projectID = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID");
        }

        public async Task<string> TranslateAsync(string text)
        {
            var translationServiceClient = TranslationServiceClient.Create();

            try
            {
                var response = await translationServiceClient.TranslateTextAsync(
                    new TranslateTextRequest
                    {
                        Contents = { text,},
                        TargetLanguageCode = "en",
                        ParentAsLocationName = new LocationName(_projectID, "global"),
                    });

                string translatedText = "";

                foreach (var translation in response.Translations)
                {
                    translatedText += translation.TranslatedText;
                }

                return translatedText;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }

            return null;
        }
    }
}
