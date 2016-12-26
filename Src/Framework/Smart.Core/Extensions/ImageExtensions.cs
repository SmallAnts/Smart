using System.Drawing;
using System.Drawing.Drawing2D;
using Smart.Core.Utilites;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System;
using System.IO;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 图片操作扩展类
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, ZoomMode mode)
        {
            using (Image originalImage = Bitmap.FromFile(originalImagePath),
               thumbnailImage = originalImage.GetThumbnailImage(width, height, mode))
            {
                // 以jpg格式保存缩略图
                thumbnailImage.Save(thumbnailPath, ImageFormat.Jpeg);
            }
        }

        /// <summary>
        /// 获取缩略图片
        /// </summary>
        /// <param name="originalImage">原始图片</param>
        /// <param name="destRect">缩略图的位置大小</param>
        /// <param name="srcRect">原始图片要生成缩略图部分的位置大小</param>
        /// <returns></returns>
        public static Image GetThumbnailImage(this Image originalImage, Rectangle destRect, Rectangle srcRect = default(Rectangle))
        {
            if (srcRect.IsEmpty) srcRect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);

            //新建空白缩略图片
            var bitmap = new Bitmap(destRect.Width, destRect.Height);
            //新建一个画板
            using (var graphic = Graphics.FromImage(bitmap))
            {
                // 设置高质量插值法
                graphic.InterpolationMode = InterpolationMode.High;
                // 设置高质量,低速度呈现平滑程度
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                // 清空画布并以透明背景色填充
                graphic.Clear(Color.Transparent);
                // 在指定位置并且按指定大小绘制原图片的指定部分
                graphic.DrawImage(originalImage, destRect, srcRect, GraphicsUnit.Pixel);
            }
            return bitmap;
        }

        /// <summary>
        /// 获取缩略图片
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode">缩略图生成模式</param>
        /// <returns></returns>
        public static Image GetThumbnailImage(this Image originalImage, int width, int height, ZoomMode mode = ZoomMode.FixedAuto)
        {
            #region 计算缩略图缩放信息
            Rectangle srcRect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);
            Rectangle destRect = new Rectangle(0, 0, width, height);
            if (mode == ZoomMode.FixedAuto)
            {
                if (destRect.Width > srcRect.Width && destRect.Height < srcRect.Height)
                {
                    mode = ZoomMode.FixedHeight;
                }
                else
                {
                    mode = ZoomMode.FixedWidth;
                }
            }
            switch (mode)
            {
                case ZoomMode.Tensile:  // 指定高宽缩放（可能变形）                
                    break;
                case ZoomMode.FixedWidth:   // 指定宽，高按比例                    
                    destRect.Height = originalImage.Height * width / originalImage.Width;
                    break;
                case ZoomMode.FixedHeight:  //指定高，宽按比例
                    destRect.Width = originalImage.Width * height / originalImage.Height;
                    break;
                case ZoomMode.Cut:  // 指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)destRect.Width / (double)destRect.Height)
                    {
                        srcRect.Width = originalImage.Height * destRect.Width / destRect.Height;
                        srcRect.Height = originalImage.Height;
                        srcRect.X = (originalImage.Width - srcRect.Width) / 2;
                        srcRect.Y = 0;
                    }
                    else
                    {
                        srcRect.Width = originalImage.Width;
                        srcRect.Height = originalImage.Width * height / destRect.Width;
                        srcRect.X = 0;
                        srcRect.Y = (originalImage.Height - srcRect.Height) / 2;
                    }
                    break;
            }
            #endregion
            return GetThumbnailImage(originalImage, destRect, srcRect);
        }

        /// <summary>
        /// 将base64字符串转换为IMAGE
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static Image Base64ToImage(this string base64String)
        {
            var ext = Regex.Match(base64String, "(?<=data:image/)[^;]+").Value?.ToLower();
            byte[] arr = Convert.FromBase64String(Regex.Replace(base64String, "^.*base64,", ""));
            using (var ms = new MemoryStream(arr))
            {
                using (var bmp = new Bitmap(ms))
                {
                    return bmp;
                }
            }
        }

        /// <summary>
        /// 将IMAGE转换为base64字符串
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToBase64String(this Image image, ImageFormat format = null)
        {
            if (format == null)
            {
                format = image.RawFormat;
            }
            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);
                var array = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(array, 0, (int)ms.Length);
                var baser64String = Convert.ToBase64String(array);
                return baser64String;
            }
        }
    }
}
