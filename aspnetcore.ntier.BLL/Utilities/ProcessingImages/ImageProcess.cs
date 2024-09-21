using ImageMagick;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.BLL.Utilities.ProcessingImages
{
    public class ImageProcess
    {
        public byte[] CompressImage(IFormFile imageFile)
        {

            var magicFormat = (MagickFormat)Enum.Parse(typeof(MagickFormat), Path.GetExtension(imageFile.FileName).Replace(".", ""), true);

            using (MagickImage image = new MagickImage(imageFile.OpenReadStream(), magicFormat))
            {
                long imageLength = imageFile.Length;
                image.Format = MagickFormat.WebP; // Get or Set the format of the image.
                int quality = Convert.ToInt32(Math.Ceiling(imageLength / 1000000.0 * 1.5));

                image.Quality = quality; // This is the Compression level.

                return image.ToByteArray();
            }
        }

        public List<string> ExtractColors(IFormFile imageFile)
        {
            var magicFormat = (MagickFormat)Enum.Parse(typeof(MagickFormat), Path.GetExtension(imageFile.FileName).Replace(".", ""), true);

            var colors = new List<string>();
            using (MagickImage image = new MagickImage(imageFile.OpenReadStream(), magicFormat))
            {
                var pixels = image.GetPixels().GroupBy(x => ConvertTo8BitHex(x.ToColor())).OrderByDescending(x => x.Count()).Select(x => x.Key).Take(10);
                return pixels.ToList();
            }

        }
        string ConvertTo8BitHex(IMagickColor<ushort> color)
        {
            // Extract 16-bit ARGB values
            ushort alpha16 = color.A;
            ushort red16 = color.R;
            ushort green16 = color.G;
            ushort blue16 = color.B;

            // Convert 16-bit values to 8-bit by taking the MSB
            byte alpha8 = (byte)(alpha16 >> 8);
            byte red8 = (byte)(red16 >> 8);
            byte green8 = (byte)(green16 >> 8);
            byte blue8 = (byte)(blue16 >> 8);

            // Format as 8-bit ARGB hexadecimal
            return $"#{red8:X2}{green8:X2}{blue8:X2}";
        }
    }
}
