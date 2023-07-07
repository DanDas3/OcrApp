using NUnit.Framework;
using OcrProcess;
using Tesseract;

namespace OcrProcessTest
{
    [TestFixture]
    public class OctTest
    {
        private Pix _img;
        private OcrProcessor _ocr;
        [SetUp]
        public void SetUp()
        {
            _img = Pix.LoadFromFile("test.png");
            _ocr = new OcrProcessor();
        }

        [Test]
        public void Process_ReceiveNullArgument_ThrowsArgumentNullException()
        {
            Assert.That(() => _ocr.Process(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Process_ReceivePixImage_ReturnStringWithTextImage()
        {
            string expected = "Testando detecção de caracteres em uma imagem.\nTestando com quebra de linha.";
            var result = _ocr.Process(_img);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}