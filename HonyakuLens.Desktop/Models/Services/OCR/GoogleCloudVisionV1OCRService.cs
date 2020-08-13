using Google.Cloud.Vision.V1;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using GoogleCloudVisionImage = Google.Cloud.Vision.V1.Image;
using Image = System.Drawing.Image;

namespace HonyakuLens.Desktop.Models.Services.OCR
{
    class GoogleCloudVisionV1OCRService : IOcrService
    {
        public async Task<string> OcrAsync(Image image)
        {
            using var stream = new MemoryStream();

            image.Save(stream, ImageFormat.Bmp);

            var client = ImageAnnotatorClient.Create();

            try
            {
                var response = await client.DetectDocumentTextAsync(
                    GoogleCloudVisionImage.FromBytes(stream.ToArray()));

                foreach (var page in response.Pages)
                {
                    foreach (var block in page.Blocks)
                    {
                        foreach (var paragraph in block.Paragraphs)
                        {
                            Console.WriteLine(string.Join("\n", paragraph.Words));
                        }
                    }
                }

                return response.Text;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }

            return null;
        }
    }
}
