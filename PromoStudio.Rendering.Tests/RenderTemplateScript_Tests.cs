using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Models;
using System.Collections.Generic;
using System.IO;

namespace PromoStudio.Rendering.Tests
{
    [TestClass]
    public class RenderTemplateScript_Tests
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    // Arrange
        //    var renderer = new AeProcess();

        //    var result = AsyncContext.Run(() =>
        //    {
        //        //Act
        //        return renderer.ExecuteProcess("LogoReplaceAndRender");
        //    });

        //    var x = result == null;
        //}

        [TestMethod]
        public void RenderTemplateScript_GenerateScript_AsPreview()
        {
            string scriptPath = null;
            try
            {
                // Arrange
                var templateScript = new CustomerTemplateScript()
                {
                    Template = new TemplateScript()
                    {
                        ProjectFilePath = @"C:\Temp\Ensue Si CS6.aep",
                        RenderCompStartTime = 0,
                        RenderCompEndTime = 5.74,
                        RenderCompName = "Full HD1920X1080",
                        RenderPreviewCompName = "NTSC D1 Widescreen 864X486"
                    },
                    Items = new List<CustomerTemplateScriptItem>()
                    {
                        new CustomerTemplateScriptItem() {
                            ScriptItem = new TemplateScriptItem() {
                                Category = TemplateScriptItemCategory.Logo,
                                Type = TemplateScriptItemType.Image,
                                Name = "Pond5 Logo"
                            },
                            Resource = new CustomerResource() {
                                Category = TemplateScriptItemCategory.Logo,
                                Value = @"C:\Temp\MyLogo.jpg"
                            }
                        },                        
                        new CustomerTemplateScriptItem() {
                            ScriptItem = new TemplateScriptItem() {
                                Category = TemplateScriptItemCategory.Portfolio,
                                Type = TemplateScriptItemType.Image,
                                Name = "Portfolio Image"
                            },
                            Resource = new CustomerResource() {
                                Category = TemplateScriptItemCategory.Portfolio,
                                Value = @"C:\Temp\Portfolio1.jpg"
                            }
                        }
                    }
                };

                // Act
                var rts = new RenderTemplateScript(templateScript, @"C:\Temp\Output\preview.mov", true);
                scriptPath = rts.GenerateScript();

                // Assert
                Assert.IsNotNull(scriptPath);
                Assert.IsTrue(File.Exists(scriptPath));
                var scriptContents = File.ReadAllText(scriptPath);
                Assert.IsTrue(scriptContents.StartsWith(
                    "var project = \"/C/Temp/Ensue Si CS6.aep\",\r\n" +
                    "    swapItems = [{ type: \"Footage\", comp: \"Pond5 Logo\", file: \"/C/Temp/MyLogo.jpg\" }," +
                        "{ type: \"Footage\", comp: \"Portfolio Image\", file: \"/C/Temp/Portfolio1.jpg\" }],\r\n" +
                    "    outputPath = \"/C/Temp/Output/preview.mov\",\r\n" +
                    "    renderComp = \"NTSC D1 Widescreen 864X486\",\r\n" +
                    "    renderStart = 0,\r\n" +
                    "    renderDuration = 5.74,\r\n" +
                    "    renderItemTemplate = \"NTSC-H264\",\r\n" +
                    "    renderItem, swapItem, item, layer, i, j, k;"));
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
        public void RenderTemplateScript_GenerateScript_AsFinal()
        {
            string scriptPath = null;
            try
            {
                // Arrange
                var templateScript = new CustomerTemplateScript()
                {
                    Template = new TemplateScript()
                    {
                        ProjectFilePath = @"C:\Temp\Ensue Si CS6.aep",
                        RenderCompStartTime = 0,
                        RenderCompEndTime = 5.74,
                        RenderCompName = "Full HD1920X1080",
                        RenderPreviewCompName = "NTSC D1 Widescreen 864X486"
                    },
                    Items = new List<CustomerTemplateScriptItem>()
                    {
                        new CustomerTemplateScriptItem() {
                            ScriptItem = new TemplateScriptItem() {
                                Category = TemplateScriptItemCategory.Logo,
                                Type = TemplateScriptItemType.Image,
                                Name = "Pond5 Logo"
                            },
                            Resource = new CustomerResource() {
                                Category = TemplateScriptItemCategory.Logo,
                                Value = @"C:\Temp\MyLogo.jpg"
                            }
                        },                        
                        new CustomerTemplateScriptItem() {
                            ScriptItem = new TemplateScriptItem() {
                                Category = TemplateScriptItemCategory.Portfolio,
                                Type = TemplateScriptItemType.Image,
                                Name = "Portfolio Image"
                            },
                            Resource = new CustomerResource() {
                                Category = TemplateScriptItemCategory.Portfolio,
                                Value = @"C:\Temp\Portfolio1.jpg"
                            }
                        }
                    }
                };

                // Act
                var rts = new RenderTemplateScript(templateScript, @"C:\Temp\Output\final.mov", false);
                scriptPath = rts.GenerateScript();

                // Assert
                Assert.IsNotNull(scriptPath);
                Assert.IsTrue(File.Exists(scriptPath));
                var scriptContents = File.ReadAllText(scriptPath);
                Assert.IsTrue(scriptContents.StartsWith(
                    "var project = \"/C/Temp/Ensue Si CS6.aep\",\r\n" +
                    "    swapItems = [{ type: \"Footage\", comp: \"Pond5 Logo\", file: \"/C/Temp/MyLogo.jpg\" }," +
                        "{ type: \"Footage\", comp: \"Portfolio Image\", file: \"/C/Temp/Portfolio1.jpg\" }],\r\n" +
                    "    outputPath = \"/C/Temp/Output/final.mov\",\r\n" +
                    "    renderComp = \"Full HD1920X1080\",\r\n" +
                    "    renderStart = 0,\r\n" +
                    "    renderDuration = 5.74,\r\n" +
                    "    renderItemTemplate = \"FullHD-H264\",\r\n" +
                    "    renderItem, swapItem, item, layer, i, j, k;"));
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
