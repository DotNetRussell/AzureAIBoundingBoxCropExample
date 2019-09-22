# Azure AI Bounding Box Image Crop with OCR Example

Just a simple Azure Custom Vision Example. The model should be trained for Object Detection.

The result of the call to the azure endpoint will be an array of predictions. Each prediction will have a confidence score and a bounding box. 

This project takes that bounding box and crops the original image to it, then sends it off for OCR processing. 
