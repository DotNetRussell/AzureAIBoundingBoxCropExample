using System.Collections.Generic;

namespace AzureAIBoundingBoxCropExample
{
    public class VisionResponse
    {
        public IEnumerable<Prediction> Predictions { get; set; }
    }
}
