using Microsoft.AspNetCore.Mvc;
using OcrProcess;
using System.IO.Compression;
using System.Text;
using Tesseract;
using static System.Net.Mime.MediaTypeNames;

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

        [HttpPost("ProcessImage")]
        public async Task<IActionResult> ProcessImageZip(List<IFormFile> images)
        {
            try
            {
                if (images == null || images.Count == 0)
                {
                    return BadRequest("Nenhuma imagem foi enviada.");
                }

                var ocr = new OcrProcessor();
                var response = new StringBuilder();

                using (var memoryStream = new MemoryStream())
                {
                    using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        // Utilizando Parallel.ForEach para processar várias imagens em paralelo
                        Parallel.ForEach(images, (image, parallelLoopState, i) =>
                        {
                            using (var imgMemoryStream = new MemoryStream())
                            {
                                image.CopyTo(imgMemoryStream);

                                using (var img = Pix.LoadFromMemory(imgMemoryStream.ToArray()))
                                {
                                    string text = ocr.Process(img);

                                    // Cria um arquivo de texto para cada imagem no arquivo ZIP
                                    var entry = zipArchive.CreateEntry($"Text_{image.FileName}.txt");

                                    using (var entryStream = entry.Open())
                                    using (var writer = new StreamWriter(entryStream, Encoding.UTF8))
                                    {
                                        writer.Write(text);
                                    }

                                    lock (response)
                                    {
                                        response.AppendLine($"Texto da imagem {i + 1} adicionado ao arquivo ZIP.");
                                    }
                                }
                            }
                        });
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    return File(memoryStream.ToArray(), "application/zip", "Texts.zip");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

    }
}
