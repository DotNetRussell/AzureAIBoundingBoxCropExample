
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Drawing.Imaging;

namespace AzureAIBoundingBoxCropExample
{
    public static class Helpers
    {
        /// <summary>
        /// Resizes your image
        /// </summary>
        /// <param name="img">Image to resize</param>
        /// <param name="outputWidth">Desired output width</param>
        /// <param name="outputHeight">Designed output height</param>
        /// <returns>Returns the resized image</returns>
        public static Image FormatImage(Image img, int outputWidth, int outputHeight)
        {
            Bitmap outputImage = null;
            Graphics graphics = null;
            try
            {
                outputImage = new Bitmap(outputWidth, outputHeight, PixelFormat.Format16bppRgb555);
                graphics = Graphics.FromImage(outputImage);
                graphics.DrawImage(img, new Rectangle(0, 0, outputWidth, outputHeight),
                new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);

                return outputImage;
            }
            catch (Exception ex)
            {
                return img;
            }
        }

        /// <summary>
        /// Converts an image into a byte array for transmission 
        /// </summary>
        /// <param name="imageIn">Image to convert</param>
        /// <returns>A byte array of the input image</returns>
        public static byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Sends an image to the inputed azure endpoint
        /// </summary>
        /// <param name="keyheader">Azure api key header</param>
        /// <param name="key">Azure api key</param>
        /// <param name="data">Image you are uploading</param>
        /// <param name="endpoint">Azure endpoint</param>
        /// <returns></returns>
        public static string UploadImage(string keyheader, string key, byte[] data, string endpoint)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);

            request.Method = "POST";
            request.Headers.Add(keyheader, key);
            request.ContentType = "application/octet-stream";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }

        /// <summary>
        /// Crops the input image into the rectangles dimensions 
        /// </summary>
        /// <param name="source">Input image</param>
        /// <param name="cropArea">Area to crop input image to</param>
        /// <returns>Coppred image</returns>
        public static Image CropPicture(Image source, Rectangle cropArea)
        {
            Bitmap retImg;
            using (var bitmap = new Bitmap(source))
            {
                retImg = bitmap.Clone(cropArea, source.PixelFormat);
            }
            return (Image)retImg;
        }
    }
}
