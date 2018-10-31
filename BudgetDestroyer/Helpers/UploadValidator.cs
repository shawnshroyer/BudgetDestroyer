using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace BudgetDestroyer.Helpers
{
    public class UploadValidator
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null) { return false; }

            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024) { return false; }

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                        ImageFormat.Bmp.Equals(img.RawFormat) ||
                        ImageFormat.Png.Equals(img.RawFormat) ||
                        ImageFormat.Gif.Equals(img.RawFormat) ||
                        ImageFormat.Tiff.Equals(img.RawFormat);

                }
            }
            catch
            {
                return false;
            }

        }

        public static bool IsAcceptableUploadType(HttpPostedFileBase file)
        {
            if (file == null) { return false; }

            if (file.ContentLength > 20 * 1024 * 1024 || file.ContentLength < 1024) { return false; }

            try
            {
                
                var ext = Path.GetExtension(file.FileName);
                switch (ext.ToLower())
                {
                    case ".doc":
                    case ".docx":
                    case ".xls":
                    case ".xlsx":
                    case ".ppt":
                    case ".pptx":
                    case ".txt":
                    case ".vsd":
                    case ".vsdx":
                    case ".pdf":
                        return true;
                    case ".jpg":
                    case ".jpeg":
                    case ".bmp":
                    case ".png":
                    case ".gif":
                    case ".tiff":
                        using (var img = Image.FromStream(file.InputStream))
                        {
                            if (ImageFormat.Jpeg.Equals(img.RawFormat) ||
                                ImageFormat.Bmp.Equals(img.RawFormat) ||
                                ImageFormat.Png.Equals(img.RawFormat) ||
                                ImageFormat.Gif.Equals(img.RawFormat) ||
                                ImageFormat.Tiff.Equals(img.RawFormat)) { return true; }
                        }
                        return false;
                    default:
                        return false;
                }

            }
            catch
            {
                return false;
            }

        }
    }
}