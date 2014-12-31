using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace TouchGame
{
    class StringFormatImg
    {
        private System.Drawing.Bitmap GetTmpBitMap;
        private System.Drawing.Font GetFont;
        private System.Drawing.Graphics GetGraphics;
        private System.Drawing.SizeF GetSizeF;
        private System.Drawing.Bitmap GetBmp;
        public StringFormatImg(string Text, string Fonts, float FontsSize, Color FontsColor)
        {
            GetTmpBitMap = new System.Drawing.Bitmap(1, 1);
            GetFont = new System.Drawing.Font(Fonts, FontsSize);
            GetGraphics = System.Drawing.Graphics.FromImage(GetTmpBitMap);
            GetSizeF = GetGraphics.MeasureString(Text, GetFont);
            GetBmp = new System.Drawing.Bitmap((int)GetSizeF.Width, (int)GetSizeF.Height);
            GetGraphics = System.Drawing.Graphics.FromImage(GetBmp);
            GetGraphics.DrawString(Text, GetFont, new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(FontsColor.A, FontsColor.R, FontsColor.G, FontsColor.B)), new System.Drawing.PointF());
            GetGraphics.Dispose();
        }

        public Stream Out2D()
        {
            MemoryStream GetStream = new MemoryStream();
            GetBmp.Save(GetStream, System.Drawing.Imaging.ImageFormat.Png);
            GetStream.Seek(0, SeekOrigin.Begin);
            Stream outstream = GetStream;
            return outstream;
        }

    }
}
