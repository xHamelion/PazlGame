using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using SkiaSharp;
using Xamarin.Forms;

namespace Pazl2
{
    class Razrez
    {
        public ImageSource[] Imag(SKBitmap _croppedBitmap, int xy)
        {
            Dictionary<int, ImageSource> valuePairs = new Dictionary<int, ImageSource>();
            SKBitmap croppedBitmap = _croppedBitmap;
            int width = croppedBitmap.Width / xy;
            int height = croppedBitmap.Height / xy;

            ImageSource[] imgSources = new ImageSource[xy * xy];
            int zs = 0;
            for (int row = 0; row < xy; row++)
            {
                for (int col = 0; col < xy; col++)
                {


                    SKBitmap bitmap = new SKBitmap(width, height);
                    SKRect dest = new SKRect(0, 0, width, height);
                    SKRect source = new SKRect(col * width, row * height, (col + 1) * width, (row + 1) * height);

                    using (SKCanvas canvas = new SKCanvas(bitmap))
                    {
                        canvas.DrawBitmap(croppedBitmap, source, dest);
                    }
                    SKImage image = SKImage.FromPixels(bitmap.PeekPixels());
                    SKData encoded = image.Encode();
                    Stream stream = encoded.AsStream();
                    imgSources[zs] = ImageSource.FromStream(() => stream);
                    valuePairs.Add(zs, ImageSource.FromStream(() => stream));
                    zs++;



                }
            }




            return imgSources;

        }
    }
}
