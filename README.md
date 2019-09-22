# Azure AI Bounding Box Image Crop with OCR Example
Azure Cognitive Services Vision to get a bounding box | Crop | OCR Example

Just a simple Azure Custom Vision Example. 

The result of the call to the azure endpoint will be an array of predictions. Each prediction will have a confidence score and a bounding box. 

This project takes that bounding box and crops the original image to it, then sends it off for OCR processing. 