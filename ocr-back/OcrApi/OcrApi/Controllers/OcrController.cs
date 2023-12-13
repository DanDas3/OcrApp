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
        public async Task<ActionResult<string>> ProcessImage(List<IFormFile> images)
        {
            try
            {
                if(images == null || images.Count == 0)
                {
                    return BadRequest("Nenhuma imagem foi enviada.");
                }
                int i = 0;
                OcrProcessor ocr = new OcrProcessor();
                string response = string.Empty;
                foreach (var image in images)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);

                        using (var img = Pix.LoadFromMemory(memoryStream.ToArray()))
                        {
                            response += $"Pagina: {++i}\n\n{(ocr.Process(img))}\n\n";
                        }
                    }
                }
                return Ok(response);
            }
            catch
            {
                return View();
            }
        }

    }
}
