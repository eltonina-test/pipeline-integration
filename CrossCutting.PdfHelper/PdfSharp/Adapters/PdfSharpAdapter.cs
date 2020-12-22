using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using CrossCutting.PdfHelper.HtmlRenderer.Adapters;
using CrossCutting.PdfHelper.HtmlRenderer.Adapters.Entities;
using CrossCutting.PdfHelper.HtmlRenderer.PdfSharp.Utilities;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Utils;

namespace CrossCutting.PdfHelper.HtmlRenderer.PdfSharp.Adapters
{
    /// <summary>
    /// Adapter for PdfSharp library platform.
    /// </summary>
    public sealed class PdfSharpAdapter : RAdapter
    {
        #region Fields and Consts

        /// <summary>
        /// Singleton instance of global adapter.
        /// </summary>
        private static readonly PdfSharpAdapter _instance = new PdfSharpAdapter();

        #endregion


        /// <summary>
        /// Init color resolve.
        /// </summary>
        private PdfSharpAdapter()
        {
            AddFontFamilyMapping("monospace", "Courier New");
            AddFontFamilyMapping("Helvetica", "Arial");

            SetFonts();
        }

        public void SetFonts()
        {
            var families = new InstalledFontCollection();

            foreach (var family in families.Families)
            {
             //    Console.WriteLine(family.Name);
                var xFontFamily = new XFontFamily(family.Name);
                var fontFamilyAdapter = new FontFamilyAdapter(xFontFamily);

                AddFontFamily(fontFamilyAdapter);
            }
        }

        /// <summary>
        /// Singleton instance of global adapter.
        /// </summary>
        public static PdfSharpAdapter Instance => _instance;

        protected override RColor GetColorInt(string colorName)
        {
            try
            {
                var color = Color.FromKnownColor((KnownColor)System.Enum.Parse(typeof(KnownColor), colorName, true));
                return Utils.Convert(color);
            }
            catch
            {
                return RColor.Empty;
            }
        }

        protected override RPen CreatePen(RColor color)
        {
            return new PenAdapter(new XPen(Utils.Convert(color)));
        }

        protected override RBrush CreateSolidBrush(RColor color)
        {
            XBrush solidBrush;
            if (color == RColor.White)
                solidBrush = XBrushes.White;
            else if (color == RColor.Black)
                solidBrush = XBrushes.Black;
            else if (color.A < 1)
                solidBrush = XBrushes.Transparent;
            else
                solidBrush = new XSolidBrush(Utils.Convert(color));

            return new BrushAdapter(solidBrush);
        }

        protected override RBrush CreateLinearGradientBrush(RRect rect, RColor color1, RColor color2, double angle)
        {
            XLinearGradientMode mode;
            if (angle < 45)
                mode = XLinearGradientMode.ForwardDiagonal;
            else if (angle < 90)
                mode = XLinearGradientMode.Vertical;
            else if (angle < 135)
                mode = XLinearGradientMode.BackwardDiagonal;
            else
                mode = XLinearGradientMode.Horizontal;
            return new BrushAdapter(new XLinearGradientBrush(Utils.Convert(rect), Utils.Convert(color1), Utils.Convert(color2), mode));
        }

        protected override RImage ConvertImageInt(object image)
        {
            return image != null ? new ImageAdapter((XImage)image) : null;
        }

        protected override RImage ImageFromStreamInt(Stream memoryStream)
        {
            return new ImageAdapter(XImage.FromStream(() => memoryStream));
        }

        protected override RFont CreateFontInt(string family, double size, RFontStyle style)
        {
            var fontStyle = (XFontStyle)((int)style);
            XFont xFont = null;
            try
            {
                Console.WriteLine($"{family} - {size} - {fontStyle}");
                xFont = new XFont(family, size, fontStyle, new XPdfFontOptions(PdfFontEncoding.Unicode));
            }
            catch (FileNotFoundException)
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) throw;

                Console.WriteLine("I'm Linux and this is a BUG");
                throw;
                //    var sSupportedFonts = resolveLinuxFontFiles();
                // FontResolver.SetupFontsFiles(sSupportedFonts); 

                // xFont = new XFont("Lato-Regular", size, fontStyle, new XPdfFontOptions(PdfFontEncoding.Unicode));
            }

            return new FontAdapter(xFont);
        }


        private static string[] resolveLinuxFontFiles()
        {
            List<string> stringList = new List<string>();
            Regex regex = new Regex("<dir>(?<dir>.*)</dir>", RegexOptions.Compiled);
            Regex ttfRegex = new Regex("\\.ttf", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        
            using (StreamReader streamReader = new StreamReader((Stream)File.OpenRead("/etc/fonts/fonts.conf")))
            {
                Console.WriteLine("Entering the fonts.conf file");
                string input;
                while ((input = streamReader.ReadLine()) != null)
                {
                    Match match = regex.Match(input);
                    if (match.Success)
                        Console.WriteLine("Match found in the fonts.conf file");
                    {
                        string path = match.Groups["dir"].Value.Replace("~", Environment.GetEnvironmentVariable("HOME"));
                        if (Directory.Exists(path))
                        {
                            Console.WriteLine("path found inside the fonts.conf file group dir");

                            foreach (string enumerateDirectory in Directory.EnumerateDirectories(path))
                            {
                                Console.WriteLine(
                                    $"{enumerateDirectory} directory inside path --> the fonts.conf file group dir");

                                var pathFont = Path.Combine(path, enumerateDirectory);

                                foreach (string strDir in Directory.EnumerateDirectories(pathFont))
                                {
                                    Console.WriteLine($"{strDir} directory inside pathFont");
                                   
                                    foreach (string str in Directory.EnumerateFiles(strDir)
                                        .Where<string>((Func<string, bool>) (x => ttfRegex.IsMatch(x))))
                                    {
                                        Console.WriteLine(
                                            $"{str} added in stringList - directory inside path --> the fonts.conf file group dir");

                                        stringList.Add(str);
                                    }

                                }
                            }
                        }
                    }
                }
            }

            var strresult = string.Join(", ", stringList.ToArray()); 
            Console.WriteLine($"{strresult}:  resulted into stringList ");

            return stringList.ToArray();
        }


        protected override RFont CreateFontInt(RFontFamily family, double size, RFontStyle style)
        {
            var fontStyle = (XFontStyle)((int)style);
            var xFont = new XFont(((FontFamilyAdapter)family).FontFamily.Name, size, fontStyle, new XPdfFontOptions(PdfFontEncoding.Unicode));
            return new FontAdapter(xFont);
        }
    }
}