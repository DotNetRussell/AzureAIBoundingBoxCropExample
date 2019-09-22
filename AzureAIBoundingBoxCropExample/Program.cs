using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace AzureAIBoundingBoxCropExample
{
    class Program
    {
        public static Configuration AppConfiguration { get; set; }

        static void Main(string[] args)
        {     /**
                Point this file path at your config file
                it should look like this
          
                         {
	                        "predictionKeyHeader" : "Prediction-Key",
	                        "predictionKey" : "your azure prediction key",
	                        "predictionFile" : "C:\\path\\to\\your\\testimage",
	                        "predictionEndpoint": "https://Your_azure_endpoint/for/custom/vision",
	                        "ocrKeyHeader" : "Ocp-Apim-Subscription-Key",
	                        "ocrKey" : "your ocr key",
	                        "ocrEndpoint" : "https://your_ocr_endpoint/vision/v2.0/ocr"
                         }           
            
             **/
            string configPath = "";
            AppConfiguration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configPath));
            
            Start();
        }

        private static async void Start()
        {
            //First we need to get an image and turn it into a byte array
            byte[] predictionFileByteArray = File.ReadAllBytes(AppConfiguration.predictionFile);

            //Once we have the image, we can send it to the prediction endpoint on azure
            string responseString = Helpers.UploadImage(AppConfiguration.predictionKeyHeader, AppConfiguration.predictionKey, predictionFileByteArray, AppConfiguration.predictionEndpoint);

            //The azure endpoint returns a json payload which I deserialize into a response object 
            VisionResponse responseObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<VisionResponse>(responseString);

            //The response object has an N amount of predictions in it.
            //First I order by the highest probability then I select the first one which should 
            //be the highest probability of a license plate
            Prediction targetBox = responseObjects.Predictions.OrderByDescending(ro => ro.Probability).FirstOrDefault();

            //Next, I once again load our image but this time into an image object
            //(this can probably be optimized so we only read it once #yolo)
            Image predictionImage = Image.FromFile(AppConfiguration.predictionFile);

            //The azure endpoint returns a bounding box for each prediction
            //The bounding box gives 4 values Left, Top, Width, Height
            //Left = The width of your image * bounding box left = y axis intersect of your bounding box
            //Top = The height of your image * bounding box top = x axis intersect of your bounding box
            //Width = The width of your image * bounding box width = the total width of the bounding box
            //Height = The height of your image * bounding box  height = the total heigh of your bounding box
            int orginX = Convert.ToInt32(targetBox.BoundingBox.Left * predictionImage.Width);
            int orginY = Convert.ToInt32(targetBox.BoundingBox.Top * predictionImage.Height);
            int width = Convert.ToInt32(targetBox.BoundingBox.Width * predictionImage.Width);
            int height = Convert.ToInt32(targetBox.BoundingBox.Height * predictionImage.Height);

            //A new rectangle is created to define the cropped area
            Rectangle rekt = new Rectangle(orginX, orginY, width, height);
            
            //Finally we crop the image to our rectangle 
            Image croppedImage = Helpers.CropPicture(predictionImage, rekt);

            //Azure OCR requires an image of at least 50x50 so this resizes us up to at least those dimensions 
            if (croppedImage.Width < 50 || croppedImage.Height < 50)
            {
                if (croppedImage.Width < 50 && croppedImage.Height > 50)
                    croppedImage = Helpers.FormatImage(croppedImage, 50, croppedImage.Height);
                else if (croppedImage.Width > 50 && croppedImage.Height < 50)
                    croppedImage = Helpers.FormatImage(croppedImage, croppedImage.Width, 50);
                else
                    croppedImage = Helpers.FormatImage(croppedImage, 50, 50);
            }
            
            byte[] croppedAndResizedImageByteArray = Helpers.ImageToByteArray(croppedImage);
            
            string ocrStringResult = Helpers.UploadImage(AppConfiguration.ocrKeyHeader, AppConfiguration.ocrKey, croppedAndResizedImageByteArray, AppConfiguration.ocrEndpoint);

        }      
    }
}