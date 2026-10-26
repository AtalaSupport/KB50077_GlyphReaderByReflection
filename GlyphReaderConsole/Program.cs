using Atalasoft.Imaging;
using Atalasoft.Ocr;
using Atalasoft.Ocr.GlyphReader;
using System.Reflection;

namespace GlyphReaderConsole
{
    internal class Program
    {
        //static GlyphReaderLoader loader = new GlyphReaderLoader();

        static void Main(string[] args)
        {
            Console.WriteLine("SimpleAtalaConsole Starting...");
            string imgPath = GetWorkingDir();
            string inFile = "images\\eric.tif";
            Console.WriteLine("  inFile: " + inFile);
            string outFile = imgPath + "out.txt";
            Console.WriteLine("  outFile: " + outFile);

            Console.WriteLine("BEGIN Processing");



            string pageText = null;

            Console.WriteLine("LOADED!");

            //GlyphReaderEngine engine = new GlyphReaderEngine();
            OcrEngine engine = CreateGlyphreaderEngine();

            engine.Initialize();
            OcrPreprocessingOptions opts = engine.AvailablePreprocessingOptions;
            Console.WriteLine("AUTO ROTTATE: " + opts.AutoRotate.ToString());


            AtalaImage img = new AtalaImage(inFile);
            Console.WriteLine("  Hi, I have an AtalaImage...");
            Console.WriteLine("    Size: " + img.Size.ToString());
            Console.WriteLine("    PixelFormat: " + img.PixelFormat.ToString());


            OcrPage result = engine.Recognize(img);
            pageText = result.GetTextString(false);
            img.Dispose();
            Console.WriteLine("page text\n\n" + pageText);


            Console.WriteLine("END Processing");

            Console.WriteLine("SimpleAtalaConsole finished... press RETURN to exit");
            Console.ReadLine();
        }

        private static OcrEngine CreateGlyphreaderEngine()
        {
            try
            {
                string ocrPath = System.IO.Directory.GetCurrentDirectory() + "\\OcrResources\\";
                GlyphReaderLoader loader = new GlyphReaderLoader(ocrPath);

                string dllPath = ocrPath + "/Atalasoft.dotImage.Ocr.Glyphreader.dll";
                Assembly asm = Assembly.LoadFile(dllPath);
                Type obj = asm.GetType("Atalasoft.Ocr.GlyphReader.GlyphReaderEngine");
                ConstructorInfo ci = obj.GetConstructor(new Type[0]);
                OcrEngine engine = ci.Invoke(null) as OcrEngine;

                return engine;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convenience method to get the root directory of the project - really only useful for debugging
        /// </summary>
        /// <returns></returns>
        private static string GetWorkingDir()
        {
            string cwd = System.IO.Directory.GetCurrentDirectory();
            //Console.WriteLine("cwd is '{0}'", cwd);

            if (cwd.EndsWith("\\bin\\Debug\\net8.0-windows"))
            {
                cwd = cwd.Replace("\\bin\\Debug\\net8.0-windows", "\\..\\..\\");
                //cwd = cwd.Replace("\\bin\\Debug", "\\");
                //Console.WriteLine("updated cwd is '{0}'", cwd);
            }
            return cwd;
        }
    }
}
