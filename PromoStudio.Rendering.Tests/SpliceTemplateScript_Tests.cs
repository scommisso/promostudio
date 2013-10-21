using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Models;
using System.Collections.Generic;
using System.IO;

namespace PromoStudio.Rendering.Tests
{
    [TestClass]
    public class SpliceTemplateScript_Tests
    {
        [TestMethod]
        public void SpliceTemplateScript_GenerateScript_AsPreview()
        {
            string scriptPath = null;
            try
            {
                // Arrange
                var spliceScript = new CustomerVideo()
                {
                    Items = new List<CustomerVideoItem>() {
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.StockVideo,
                            StockVideo = new StockVideo() { FilePath = @"C:\Temp\InFile1.mov" },
                            SortOrder = 1
                        },
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.StockVideo,
                            StockVideo = new StockVideo() { FilePath = @"C:\Temp\InFile2.mov" },
                            SortOrder = 3
                        },
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.CustomerTemplateScript,
                            CustomerScript = new CustomerTemplateScript() { PreviewFilePath = @"C:\Temp\Output\PreviewTemplate1.mov" },
                            SortOrder = 2
                        },
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.CustomerTemplateScript,
                            CustomerScript = new CustomerTemplateScript() { PreviewFilePath = @"C:\Temp\Output\PreviewTemplate2.mov" },
                            SortOrder = 4
                        },
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.StockAudio,
                            StockAudio = new StockAudio() { FilePath = @"C:\Temp\InAudio1.mp3"}
                        },
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.StockAudio,
                            StockAudio = new StockAudio() { FilePath = @"C:\Temp\InAudio2.mp3"}
                        },
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.CustomerVideoVoiceOver,
                            VoiceOver = new CustomerVideoVoiceOver() { FilePath = @"C:\Temp\Uploads\VoiceActor1.mp3"}
                        }
                    },
                    RenderStatus = CustomerVideoRenderStatus.Pending
                };

                // Act
                var rts = new SpliceTemplateScript(spliceScript, @"C:\Temp\Output\preview.mov", true);
                scriptPath = rts.GenerateScript();

                // Assert
                Assert.IsNotNull(scriptPath);
                Assert.IsTrue(File.Exists(scriptPath));
                var scriptContents = File.ReadAllText(scriptPath);
                Assert.IsTrue(scriptContents.StartsWith(
                    "var project = \"/C/Temp/AE TEST CS6.aep\",\r\n" +
                    "    video = [{ file: \"/C/Temp/InFile1.mov\", includeAudio: true }," +
                        "{ file: \"/C/Temp/Output/PreviewTemplate1.mov\", includeAudio: true }," +
                        "{ file: \"/C/Temp/InFile2.mov\", includeAudio: true }," +
                        "{ file: \"/C/Temp/Output/PreviewTemplate2.mov\", includeAudio: true }],\r\n" +
                    "    audio = [{ file: \"/C/Temp/InAudio1.mp3\", gainAdjust: 0 }," +
                        "{ file: \"/C/Temp/InAudio2.mp3\", gainAdjust: 0 }," +
                        "{ file: \"/C/Temp/Uploads/VoiceActor1.mp3\", gainAdjust: 0 }],\r\n" +
                    "    renderComp = \"NTSC 24FPS\",\r\n" +
                    "    outputPath = \"/C/Temp/Output/preview.mov\",\r\n" +
                    "    renderItemTemplate = \"NTSC-H264\","));
            }
            finally
            {
                if (!string.IsNullOrEmpty(scriptPath) && File.Exists(scriptPath))
                {
                    try
                    {
                        File.Delete(scriptPath);
                    }
                    catch { }
                }
            }
        }

        [TestMethod]
        public void SpliceTemplateScript_GenerateScript_AsFinal()
        {
            string scriptPath = null;
            try
            {
                // Arrange
                var spliceScript = new CustomerVideo()
                {
                    Items = new List<CustomerVideoItem>() {
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.StockVideo,
                            StockVideo = new StockVideo() { FilePath = @"C:\Temp\InFile1.mov" },
                            SortOrder = 1
                        },
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.StockVideo,
                            StockVideo = new StockVideo() { FilePath = @"C:\Temp\InFile2.mov" },
                            SortOrder = 3
                        },
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.CustomerTemplateScript,
                            CustomerScript = new CustomerTemplateScript() { CompletedFilePath = @"C:\Temp\Output\PreviewTemplate1.mov" },
                            SortOrder = 2
                        },
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.CustomerTemplateScript,
                            CustomerScript = new CustomerTemplateScript() { CompletedFilePath = @"C:\Temp\Output\PreviewTemplate2.mov" },
                            SortOrder = 4
                        },
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.StockAudio,
                            StockAudio = new StockAudio() { FilePath = @"C:\Temp\InAudio1.mp3"}
                        },
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.StockAudio,
                            StockAudio = new StockAudio() { FilePath = @"C:\Temp\InAudio2.mp3"}
                        },
                        new CustomerVideoItem() {
                            Type = CustomerVideoItemType.CustomerVideoVoiceOver,
                            VoiceOver = new CustomerVideoVoiceOver() { FilePath = @"C:\Temp\Uploads\VoiceActor1.mp3"}
                        }
                    },
                    RenderStatus = CustomerVideoRenderStatus.Pending
                };

                // Act
                var rts = new SpliceTemplateScript(spliceScript, @"C:\Temp\Output\final.mov", false);
                scriptPath = rts.GenerateScript();

                // Assert
                Assert.IsNotNull(scriptPath);
                Assert.IsTrue(File.Exists(scriptPath));
                var scriptContents = File.ReadAllText(scriptPath);
                Assert.IsTrue(scriptContents.StartsWith(
                    "var project = \"/C/Temp/AE TEST CS6.aep\",\r\n" +
                    "    video = [{ file: \"/C/Temp/InFile1.mov\", includeAudio: true }," +
                        "{ file: \"/C/Temp/Output/PreviewTemplate1.mov\", includeAudio: true }," +
                        "{ file: \"/C/Temp/InFile2.mov\", includeAudio: true }," +
                        "{ file: \"/C/Temp/Output/PreviewTemplate2.mov\", includeAudio: true }],\r\n" +
                    "    audio = [{ file: \"/C/Temp/InAudio1.mp3\", gainAdjust: 0 }," +
                        "{ file: \"/C/Temp/InAudio2.mp3\", gainAdjust: 0 }," +
                        "{ file: \"/C/Temp/Uploads/VoiceActor1.mp3\", gainAdjust: 0 }],\r\n" +
                    "    renderComp = \"HDTV 24FPS\",\r\n" +
                    "    outputPath = \"/C/Temp/Output/final.mov\",\r\n" +
                    "    renderItemTemplate = \"FullHD-H264\","));
            }
            finally
            {
                if (!string.IsNullOrEmpty(scriptPath) && File.Exists(scriptPath))
                {
                    try
                    {
                        File.Delete(scriptPath);
                    }
                    catch { }
                }
            }
        }
    }
}
