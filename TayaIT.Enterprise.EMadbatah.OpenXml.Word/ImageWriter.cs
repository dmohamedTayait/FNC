using System;
using System.IO;
using System.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using WP = DocumentFormat.OpenXml.Wordprocessing;
using wp = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using a = DocumentFormat.OpenXml.Drawing;
using pic = DocumentFormat.OpenXml.Drawing.Pictures;
using DocumentFormat.OpenXml;
namespace TayaIT.Enterprise.EMadbatah.OpenXml.Word
{



    public static class ImageWriter
    {
        public static void CreateImageDocument(string docxFilePath, string resfolderpath, string[] files)
        {


            using (WordprocessingWorker doc = new WordprocessingWorker(docxFilePath, WordprocessingWorker.GetDocParts(resfolderpath), DocFileOperation.CreateNew))
            {
                int i = 1000;
                foreach (string file in files)
                {
                    AddImage(doc.DocMainPart.Document.Body, doc.DocMainPart, file, "rId" + i);
                    i++;
                }
                doc.Save();
            }

        }

       


        public static string GraphicDataUri = "http://schemas.openxmlformats.org/drawingml/2006/picture";
        public static void AddImage(this WP.Body body, MainDocumentPart mainpart, string filename, string SigId)
        {

            byte[] imageFileBytes;
            Bitmap image=null;

            // Open a stream on the image file and read it's contents.
            using (FileStream fsImageFile = System.IO.File.OpenRead(filename))
            {
                imageFileBytes = new byte[fsImageFile.Length];
                fsImageFile.Read(imageFileBytes, 0, imageFileBytes.Length);
                image = new Bitmap(fsImageFile);
            }
            long imageWidthEMU = (long)((image.Width / image.HorizontalResolution) * 714400L) ;//* 914400L);
            long imageHeightEMU = (long)((image.Height / image.VerticalResolution) * 714400L);

            // add this is not already there
            try
            {
                if (mainpart.GetPartById(SigId) == null)
                {
                    var imagePart = mainpart.AddNewPart<ImagePart>("image/jpeg", SigId);

                    using (BinaryWriter writer = new BinaryWriter(imagePart.GetStream()))
                    {
                        writer.Write(imageFileBytes);
                        writer.Flush();
                    }
                }
            }
            catch // thrown if not found
            {
                var imagePart = mainpart.AddNewPart<ImagePart>("image/jpeg", SigId);

                using (BinaryWriter writer = new BinaryWriter(imagePart.GetStream()))
                {
                    writer.Write(imageFileBytes);
                    writer.Flush();
                }
            
            }
            WP.Paragraph para = 
            new WP.Paragraph(
              new WP.Run(
                new WP.Drawing(
                  new wp.Inline(

                    new wp.Extent()
                    {
                        Cx = imageWidthEMU,
                        Cy = imageHeightEMU
                    },

                    new wp.EffectExtent()
                    {
                        LeftEdge = 19050L,
                        TopEdge = 0L,
                        RightEdge = 9525L,
                        BottomEdge = 0L
                    },

                    new wp.DocProperties()
                    {
                        Id = (UInt32Value)1U,
                        Name = "Inline Text Wrapping Picture",
                        Description = GraphicDataUri
                    },

                    new wp.NonVisualGraphicFrameDrawingProperties(
                      new a.GraphicFrameLocks() { NoChangeAspect = true }),

                    new a.Graphic(
                      new a.GraphicData(
                        new pic.Picture(

                          new pic.NonVisualPictureProperties(
                            new pic.NonVisualDrawingProperties()
                            {
                                Id = (UInt32Value)0U,
                                Name = filename
                            },
                            new pic.NonVisualPictureDrawingProperties()),

                          new pic.BlipFill(
                            new a.Blip() { Embed = SigId },
                            new a.Stretch(
                              new a.FillRectangle())),

                          new pic.ShapeProperties(
                            new a.Transform2D(
                              new a.Offset() { X = 0L, Y = 0L },
                              new a.Extents()
                              {
                                  Cx = imageWidthEMU,
                                  Cy = imageHeightEMU
                              }),

                            new a.PresetGeometry(
                              new a.AdjustValueList()
                            ) { Preset = a.ShapeTypeValues.Rectangle }))
                      ) { Uri = GraphicDataUri })
                  )
                  {
                      DistanceFromTop = (UInt32Value)0U,
                      DistanceFromBottom = (UInt32Value)0U,
                      DistanceFromLeft = (UInt32Value)0U,
                      DistanceFromRight = (UInt32Value)0U
                  }))
                  
            );

            body.Append(para);
        }


    }
}