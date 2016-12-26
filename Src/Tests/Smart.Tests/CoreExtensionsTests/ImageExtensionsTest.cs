using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using Smart.Core.Extensions;

namespace Smart.Tests.CoreExtensionsTests
{
    [TestClass]
    public class ImageExtensionsTest
    {
        [TestMethod]
        public void ImageToBase64String()
        {
            var img = Image.FromFile(@"E:\GitHub\SmallAnts\Smart\Src\Sample\Smart.Sample.Web\Assets\images\headphoto.jpg");
            var base64Str = img.ToBase64String();
        }

        [TestMethod]
        public void Base64StringToImage()
        {
        }
    }
}
