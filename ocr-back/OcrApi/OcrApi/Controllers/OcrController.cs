using Microsoft.AspNetCore.Mvc;
using OcrProcess;
using Tesseract;

namespace OcrApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OcrController : Controller
    {

        // POST: OcrController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName(nameof(ProcessImage))]
        public async Task<ActionResult<string>> ProcessImage(IFormFile image)
        {
            try
            {

                OcrProcessor ocr = new OcrProcessor();
                string response = string.Empty;
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);

                    using (var img = Pix.LoadFromMemory(memoryStream.ToArray()))
                    {
                        response = ocr.Process(img);
                    }
                    return Ok(response);
                }
            }
            catch
            {
                return View();
            }
        }

    }
}
