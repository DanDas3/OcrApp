using Tesseract;

namespace OcrProcess
{
    public class OcrProcessor
    {
        public OcrProcessor()
        {

        }

        public string Process(Pix img)
        {
            string result = string.Empty;
            try
            {
                if (img == null)
                {
                    throw new ArgumentNullException(nameof(img));
                }

                using (var ocrEngine = new TesseractEngine("tessdata", "por", EngineMode.Default))
                {
                    using (var page = ocrEngine.Process(img))
                    {
                        result = page.GetText().Trim();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                string type = ex.GetType().Name;
                throw;
            }
        }
    }
}