﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (C) 2008-2014 ShareX Developers

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

#region License Information (Greenshot)

/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2013  Thomas Braun, Jens Klingen, Robin Krom
 *
 * For more information see: http://getgreenshot.org/
 * The Greenshot project is hosted on Sourceforge: http://sourceforge.net/projects/greenshot/
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

#endregion License Information (Greenshot)

using Greenshot;
using Greenshot.Drawing;
using Greenshot.IniFile;
using Greenshot.Plugin;
using GreenshotPlugin.Core;
using GreenshotPlugin.UnmanagedHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace HelpersLib
{
    public static class ImageHelpers
    {
        public static Image ResizeImage(Image img, Size size)
        {
            return ResizeImage(img, size.Width, size.Height);
        }

        

        public static Bitmap AddReflection(Image img, int percentage, int maxAlpha, int minAlpha)
        {
            percentage = percentage.Between(1, 100);
            maxAlpha = maxAlpha.Between(0, 255);
            minAlpha = minAlpha.Between(0, 255);

            Bitmap reflection;

            using (Bitmap bitmapRotate = (Bitmap)img.Clone())
            {
                bitmapRotate.RotateFlip(RotateFlipType.RotateNoneFlipY);
                reflection = bitmapRotate.Clone(new Rectangle(0, 0, bitmapRotate.Width, (int)(bitmapRotate.Height * ((float)percentage / 100))), PixelFormat.Format32bppArgb);
            }

            using (UnsafeBitmap unsafeBitmap = new UnsafeBitmap(reflection, true))
            {
                int alphaAdd = maxAlpha - minAlpha;
                float reflectionHeight = reflection.Height - 1;

                for (int y = 0; y < reflection.Height; ++y)
                {
                    for (int x = 0; x < reflection.Width; ++x)
                    {
                        ColorBgra color = unsafeBitmap.GetPixel(x, y);
                        byte alpha = (byte)(maxAlpha - (alphaAdd * (y / reflectionHeight)));

                        if (color.Alpha > alpha)
                        {
                            color.Alpha = alpha;
                            unsafeBitmap.SetPixel(x, y, color);
                        }
                    }
                }
            }

            return reflection;
        }

        public static Image DrawBorder(Image img, Color borderColor, int borderSize, BorderType borderType)
        {
            using (Pen borderPen = new Pen(borderColor, borderSize) { Alignment = PenAlignment.Inset })
            {
                return DrawBorder(img, borderPen, borderType);
            }
        }

        public static Image DrawBorder(Image img, Color fromBorderColor, Color toBorderColor, LinearGradientMode gradientType, int borderSize, BorderType borderType)
        {
            int width = img.Width;
            int height = img.Height;

            if (borderType == BorderType.Outside)
            {
                width += borderSize * 2;
                height += borderSize * 2;
            }

            using (LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, width, height), fromBorderColor, toBorderColor, gradientType))
            using (Pen borderPen = new Pen(brush, borderSize) { Alignment = PenAlignment.Inset })
            {
                return DrawBorder(img, borderPen, borderType);
            }
        }

        public static Image DrawBorder(Image img, Pen borderPen, BorderType borderType)
        {
            Bitmap bmp;

            if (borderType == BorderType.Inside)
            {
                bmp = (Bitmap)img;

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawRectangleProper(borderPen, 0, 0, img.Width, img.Height);
                }
            }
            else
            {
                int borderSize = (int)borderPen.Width;
                bmp = img.CreateEmptyBitmap(borderSize * 2, borderSize * 2, PixelFormat.Format32bppArgb);

                using (Graphics g = Graphics.FromImage(bmp))
                using (img)
                {
                    g.DrawRectangleProper(borderPen, 0, 0, bmp.Width, bmp.Height);
                    g.SetHighQuality();
                    g.DrawImage(img, borderSize, borderSize, img.Width, img.Height);
                }
            }

            return bmp;
        }

        public static Bitmap FillBackground(Image img, Color color)
        {
            using (Brush brush = new SolidBrush(color))
            {
                return FillBackground(img, brush);
            }
        }

        public static Bitmap FillBackground(Image img, Color fromColor, Color toColor, LinearGradientMode gradientType)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, img.Width, img.Height), fromColor, toColor, gradientType))
            {
                return FillBackground(img, brush);
            }
        }

        public static Bitmap FillBackground(Image img, Brush brush)
        {
            Bitmap result = img.CreateEmptyBitmap(PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(result))
            using (img)
            {
                g.FillRectangle(brush, 0, 0, result.Width, result.Height);
                g.SetHighQuality();
                g.DrawImage(img, 0, 0, result.Width, result.Height);
            }

            return result;
        }

        public static Image DrawCheckers(Image img)
        {
            return DrawCheckers(img, 8, Color.LightGray, Color.White);
        }

        public static Image DrawCheckers(Image img, int size, Color color1, Color color2)
        {
            Bitmap bmp = img.CreateEmptyBitmap(PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bmp))
            using (Image checker = CreateCheckers(size, color1, color2))
            using (Brush checkerBrush = new TextureBrush(checker, WrapMode.Tile))
            using (img)
            {
                g.FillRectangle(checkerBrush, new Rectangle(0, 0, bmp.Width, bmp.Height));
                g.SetHighQuality();
                g.DrawImage(img, 0, 0, img.Width, img.Height);
            }

            return bmp;
        }

        public static Bitmap RotateImage(Image inputImage, float angleDegrees, bool upsize, bool clip)
        {
            // Test for zero rotation and return a clone of the input image
            if (angleDegrees == 0f)
                return (Bitmap)inputImage.Clone();

            // Set up old and new image dimensions, assuming upsizing not wanted and clipping OK
            int oldWidth = inputImage.Width;
            int oldHeight = inputImage.Height;
            int newWidth = oldWidth;
            int newHeight = oldHeight;
            float scaleFactor = 1f;

            // If upsizing wanted or clipping not OK calculate the size of the resulting bitmap
            if (upsize || !clip)
            {
                double angleRadians = angleDegrees * Math.PI / 180d;

                double cos = Math.Abs(Math.Cos(angleRadians));
                double sin = Math.Abs(Math.Sin(angleRadians));
                newWidth = (int)Math.Round(oldWidth * cos + oldHeight * sin);
                newHeight = (int)Math.Round(oldWidth * sin + oldHeight * cos);
            }

            // If upsizing not wanted and clipping not OK need a scaling factor
            if (!upsize && !clip)
            {
                scaleFactor = Math.Min((float)oldWidth / newWidth, (float)oldHeight / newHeight);
                newWidth = oldWidth;
                newHeight = oldHeight;
            }

            // Create the new bitmap object.
            Bitmap newBitmap = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);
            newBitmap.SetResolution(inputImage.HorizontalResolution, inputImage.VerticalResolution);

            // Create the Graphics object that does the work
            using (Graphics graphicsObject = Graphics.FromImage(newBitmap))
            {
                graphicsObject.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsObject.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphicsObject.SmoothingMode = SmoothingMode.HighQuality;

                // Set up the built-in transformation matrix to do the rotation and maybe scaling
                graphicsObject.TranslateTransform(newWidth / 2f, newHeight / 2f);

                if (scaleFactor != 1f)
                    graphicsObject.ScaleTransform(scaleFactor, scaleFactor);

                graphicsObject.RotateTransform(angleDegrees);
                graphicsObject.TranslateTransform(-oldWidth / 2f, -oldHeight / 2f);

                // Draw the result
                graphicsObject.DrawImage(inputImage, 0, 0, inputImage.Width, inputImage.Height);
            }

            return newBitmap;
        }

        public static Image AnnotateImage(Image img)
        {
            return AnnotateImage(img, false, null, null, null);
        }

        public static Image AnnotateImage(Image img, bool allowSave, string configPath, Action<Image> clipboardCopyRequested, Action<Image> imageUploadRequested)
        {
            if (!IniConfig.isInitialized)
            {
                IniConfig.AllowSave = allowSave;
                IniConfig.Init(configPath);
            }

            using (Image cloneImage = (Image)img.Clone())
            using (ICapture capture = new Capture { Image = cloneImage })
            using (Surface surface = new Surface(capture))
            using (ImageEditorForm editor = new ImageEditorForm(surface, true))
            {
                editor.ClipboardCopyRequested += clipboardCopyRequested;
                editor.ImageUploadRequested += imageUploadRequested;

                if (editor.ShowDialog() == DialogResult.OK)
                {
                    using (img)
                    {
                        return editor.GetImageForExport();
                    }
                }
            }

            return img;
        }

        public static Bitmap AddShadow(Image sourceImage, float opacity, int size)
        {
            return AddShadow(sourceImage, opacity, size, 1, Color.Black, new Point(0, 0));
        }

        public static Bitmap AddShadow(Image sourceImage, float opacity, int size, float darkness, Color color, Point offset)
        {
            Image shadowImage = null;

            try
            {
                shadowImage = sourceImage.CreateEmptyBitmap(size * 2, size * 2, PixelFormat.Format32bppArgb);

                ColorMatrix maskMatrix = new ColorMatrix();
                maskMatrix.Matrix00 = 0;
                maskMatrix.Matrix11 = 0;
                maskMatrix.Matrix22 = 0;
                maskMatrix.Matrix33 = opacity;
                maskMatrix.Matrix40 = ((float)color.R).Remap(0, 255, 0, 1);
                maskMatrix.Matrix41 = ((float)color.G).Remap(0, 255, 0, 1);
                maskMatrix.Matrix42 = ((float)color.B).Remap(0, 255, 0, 1);

                Rectangle shadowRectangle = new Rectangle(size, size, sourceImage.Width, sourceImage.Height);
                maskMatrix.Apply(sourceImage, shadowImage, shadowRectangle);

                if (size > 0)
                {
                    Blur((Bitmap)shadowImage, size);
                }

                if (darkness > 1)
                {
                    ColorMatrix alphaMatrix = new ColorMatrix();
                    alphaMatrix.Matrix33 = darkness;

                    Image shadowImage2 = alphaMatrix.Apply(shadowImage);
                    shadowImage.Dispose();
                    shadowImage = shadowImage2;
                }

                Bitmap result = shadowImage.CreateEmptyBitmap(Math.Abs(offset.X), Math.Abs(offset.Y));

                using (Graphics g = Graphics.FromImage(result))
                {
                    g.SetHighQuality();
                    g.DrawImage(shadowImage, Math.Max(0, offset.X), Math.Max(0, offset.Y), shadowImage.Width, shadowImage.Height);
                    g.DrawImage(sourceImage, Math.Max(size, -offset.X + size), Math.Max(size, -offset.Y + size), sourceImage.Width, sourceImage.Height);
                }

                return result;
            }
            finally
            {
                if (sourceImage != null) sourceImage.Dispose();
                if (shadowImage != null) shadowImage.Dispose();
            }
        }

        ß

        public static Image CreateTornEdge(Image sourceImage, int toothHeight, int horizontalToothRange, int verticalToothRange, AnchorStyles sides)
        {
            Image result = sourceImage.CreateEmptyBitmap(PixelFormat.Format32bppArgb);

            using (GraphicsPath path = new GraphicsPath())
            {
                Random random = new Random();
                int horizontalRegions = sourceImage.Width / horizontalToothRange;
                int verticalRegions = sourceImage.Height / verticalToothRange;

                Point previousEndingPoint = new Point(horizontalToothRange, random.Next(1, toothHeight));
                Point newEndingPoint;

                if (sides.HasFlag(AnchorStyles.Top))
                {
                    for (int i = 0; i < horizontalRegions; i++)
                    {
                        int x = previousEndingPoint.X + horizontalToothRange;
                        int y = random.Next(1, toothHeight);
                        newEndingPoint = new Point(x, y);
                        path.AddLine(previousEndingPoint, newEndingPoint);
                        previousEndingPoint = newEndingPoint;
                    }
                }
                else
                {
                    previousEndingPoint = new Point(0, 0);
                    newEndingPoint = new Point(sourceImage.Width, 0);
                    path.AddLine(previousEndingPoint, newEndingPoint);
                    previousEndingPoint = newEndingPoint;
                }

                if (sides.HasFlag(AnchorStyles.Right))
                {
                    for (int i = 0; i < verticalRegions; i++)
                    {
                        int x = sourceImage.Width - random.Next(1, toothHeight);
                        int y = previousEndingPoint.Y + verticalToothRange;
                        newEndingPoint = new Point(x, y);
                        path.AddLine(previousEndingPoint, newEndingPoint);
                        previousEndingPoint = newEndingPoint;
                    }
                }
                else
                {
                    previousEndingPoint = new Point(sourceImage.Width, 0);
                    newEndingPoint = new Point(sourceImage.Width, sourceImage.Height);
                    path.AddLine(previousEndingPoint, newEndingPoint);
                    previousEndingPoint = newEndingPoint;
                }

                if (sides.HasFlag(AnchorStyles.Bottom))
                {
                    for (int i = 0; i < horizontalRegions; i++)
                    {
                        int x = previousEndingPoint.X - horizontalToothRange;
                        int y = sourceImage.Height - random.Next(1, toothHeight);
                        newEndingPoint = new Point(x, y);
                        path.AddLine(previousEndingPoint, newEndingPoint);
                        previousEndingPoint = newEndingPoint;
                    }
                }
                else
                {
                    previousEndingPoint = new Point(sourceImage.Width, sourceImage.Height);
                    newEndingPoint = new Point(0, sourceImage.Height);
                    path.AddLine(previousEndingPoint, newEndingPoint);
                    previousEndingPoint = newEndingPoint;
                }

                if (sides.HasFlag(AnchorStyles.Left))
                {
                    for (int i = 0; i < verticalRegions; i++)
                    {
                        int x = random.Next(1, toothHeight);
                        int y = previousEndingPoint.Y - verticalToothRange;
                        newEndingPoint = new Point(x, y);
                        path.AddLine(previousEndingPoint, newEndingPoint);
                        previousEndingPoint = newEndingPoint;
                    }
                }
                else
                {
                    previousEndingPoint = new Point(0, sourceImage.Height);
                    newEndingPoint = new Point(0, 0);
                    path.AddLine(previousEndingPoint, newEndingPoint);
                    previousEndingPoint = newEndingPoint;
                }

                path.CloseFigure();

                using (Graphics graphics = Graphics.FromImage(result))
                {
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    // Draw the created figure with the original image by using a TextureBrush so we have anti-aliasing
                    using (Brush brush = new TextureBrush(sourceImage))
                    {
                        graphics.FillPath(brush, path);
                    }
                }
            }

            return result;
        }

        public static Bitmap Sharpen(Image image, double strength)
        {
            using (Bitmap bitmap = (Bitmap)image)
            {
                if (bitmap != null)
                {
                    Bitmap sharpenImage = bitmap.Clone() as Bitmap;

                    int width = image.Width;
                    int height = image.Height;

                    // Create sharpening filter.
                    const int filterSize = 5;

                    var filter = new double[,]
                    {
                        {-1, -1, -1, -1, -1},
                        {-1,  2,  2,  2, -1},
                        {-1,  2, 16,  2, -1},
                        {-1,  2,  2,  2, -1},
                        {-1, -1, -1, -1, -1}
                    };

                    double bias = 1.0 - strength;
                    double factor = strength / 16.0;

                    const int s = filterSize / 2;

                    var result = new Color[image.Width, image.Height];

                    // Lock image bits for read/write.
                    if (sharpenImage != null)
                    {
                        BitmapData pbits = sharpenImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                        // Declare an array to hold the bytes of the bitmap.
                        int bytes = pbits.Stride * height;
                        var rgbValues = new byte[bytes];

                        // Copy the RGB values into the array.
                        Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

                        int rgb;
                        // Fill the color array with the new sharpened color values.
                        for (int x = s; x < width - s; x++)
                        {
                            for (int y = s; y < height - s; y++)
                            {
                                double red = 0.0, green = 0.0, blue = 0.0;

                                for (int filterX = 0; filterX < filterSize; filterX++)
                                {
                                    for (int filterY = 0; filterY < filterSize; filterY++)
                                    {
                                        int imageX = (x - s + filterX + width) % width;
                                        int imageY = (y - s + filterY + height) % height;

                                        rgb = imageY * pbits.Stride + 3 * imageX;

                                        red += rgbValues[rgb + 2] * filter[filterX, filterY];
                                        green += rgbValues[rgb + 1] * filter[filterX, filterY];
                                        blue += rgbValues[rgb + 0] * filter[filterX, filterY];
                                    }

                                    rgb = y * pbits.Stride + 3 * x;

                                    int r = Math.Min(Math.Max((int)(factor * red + (bias * rgbValues[rgb + 2])), 0), 255);
                                    int g = Math.Min(Math.Max((int)(factor * green + (bias * rgbValues[rgb + 1])), 0), 255);
                                    int b = Math.Min(Math.Max((int)(factor * blue + (bias * rgbValues[rgb + 0])), 0), 255);

                                    result[x, y] = Color.FromArgb(r, g, b);
                                }
                            }
                        }

                        // Update the image with the sharpened pixels.
                        for (int x = s; x < width - s; x++)
                        {
                            for (int y = s; y < height - s; y++)
                            {
                                rgb = y * pbits.Stride + 3 * x;

                                rgbValues[rgb + 2] = result[x, y].R;
                                rgbValues[rgb + 1] = result[x, y].G;
                                rgbValues[rgb + 0] = result[x, y].B;
                            }
                        }

                        // Copy the RGB values back to the bitmap.
                        Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);
                        // Release image bits.
                        sharpenImage.UnlockBits(pbits);
                    }

                    return sharpenImage;
                }
            }
            return null;
        }

        public static string OpenImageFileDialog()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image files (*.png, *.jpg, *.jpeg, *.jpe, *.jfif, *.gif, *.bmp, *.tif, *.tiff)|*.png;*.jpg;*.jpeg;*.jpe;*.jfif;*.gif;*.bmp;*.tif;*.tiff|" +
                    "PNG (*.png)|*.png|JPEG (*.jpg, *.jpeg, *.jpe, *.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif|GIF (*.gif)|*.gif|BMP (*.bmp)|*.bmp|TIFF (*.tif, *.tiff)|*.tif;*.tiff|" +
                    "All files (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }

            return null;
        }

        public static void SaveImageFileDialog(Image img)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.DefaultExt = ".png";
                sfd.Filter = "PNG (*.png)|*.png|JPEG (*.jpg, *.jpeg, *.jpe, *.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif|GIF (*.gif)|*.gif|BMP (*.bmp)|*.bmp|TIFF (*.tif, *.tiff)|*.tif;*.tiff";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = sfd.FileName;
                    string ext = Helpers.GetProperExtension(filePath);

                    if (!string.IsNullOrEmpty(ext))
                    {
                        ImageFormat imageFormat;

                        switch (ext)
                        {
                            default:
                            case "png":
                                imageFormat = ImageFormat.Png;
                                break;
                            case "jpg":
                            case "jpeg":
                            case "jpe":
                            case "jfif":
                                imageFormat = ImageFormat.Jpeg;
                                break;
                            case "gif":
                                imageFormat = ImageFormat.Gif;
                                break;
                            case "bmp":
                                imageFormat = ImageFormat.Bmp;
                                break;
                            case "tif":
                            case "tiff":
                                imageFormat = ImageFormat.Tiff;
                                break;
                        }

                        img.Save(filePath, imageFormat);
                    }
                }
            }
        }

        // http://stackoverflow.com/questions/788335/why-does-image-fromfile-keep-a-file-handle-open-sometimes
        public static Image LoadImage(string filePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath) && Helpers.IsImageFile(filePath) && File.Exists(filePath))
                {
                    return Image.FromStream(new MemoryStream(File.ReadAllBytes(filePath)));
                }
            }
            catch (Exception e)
            {
                DebugHelper.WriteException(e);
            }

            return null;
        }
    }
}