using System;
using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;
using SkiaSharp;

public class ImageData
{
    [ImageType(320, 320)]
    public SKBitmap Image { get; set; }
}

public class ImagePrediction
{
    [VectorType(1, 100, 4)]
    public float[] DetectionBoxes { get; set; }
    [VectorType(1, 100)]
    public float[] DetectionScores { get; set; }
    [VectorType(1, 100)]
    public float[] DetectionClasses { get; set; }
    [VectorType(1)]
    public float[] NumDetections { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        var mlContext = new MLContext();

        var pipeline = mlContext.Transforms
            .LoadImages(outputColumnName: "input", imageFolder: "", inputColumnName: nameof(ImageData.Image))
            .Append(mlContext.Transforms.ResizeImages(outputColumnName: "input", imageWidth: 320, imageHeight: 320))
            .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input"))
            .Append(mlContext.Transforms.ApplyOnnxModel(
                modelFile: "model.onnx",
                outputColumnNames: new[] { "detection_boxes", "detection_scores", "detection_classes", "num_detections" },
                inputColumnNames: new[] { "input" }));

        var model = pipeline.Fit(mlContext.Data.LoadFromEnumerable(new List<ImageData>()));

        mlContext.Model.Save(model, null, "face_detection_model.zip");

        Console.WriteLine("Model saved to face_detection_model.zip");
    }
}
