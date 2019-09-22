namespace AzureAIBoundingBoxCropExample
{
    /**
      Your configuration json file should look like this
          
        {
	    "predictionKeyHeader" : "Prediction-Key",
	    "predictionKey" : "your azure prediction key",
	    "predictionFile" : "C:\\path\\to\\your\\testimage.jpg",
	    "predictionEndpoint": "https://Your_azure_endpoint/for/custom/vision",
	    "ocrKeyHeader" : "Ocp-Apim-Subscription-Key",
	    "ocrKey" : "your ocr key",
	    "ocrEndpoint" : "https://your_ocr_endpoint/vision/v2.0/ocr"
        }           
        

     **/
    public class Configuration
    {
        public string predictionKeyHeader {get ; set;}
        public string predictionKey {get ; set;}
        public string predictionFile {get ; set;}
        public string predictionEndpoint {get ; set;}
        
        public string ocrKeyHeader {get ; set;}
        public string ocrKey {get ; set;}
        public string ocrEndpoint {get ; set;}
    }
}
