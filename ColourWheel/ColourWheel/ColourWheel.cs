using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VoxelGame;

namespace ColourWheel
{
    class ColourWheel
    {

        public int Size = 200;
        public Texture2D WheelTexture;

        public ColourWheel(GraphicsDevice gd, int size)
        {
            Size = size;

            WheelTexture = new Texture2D(gd, Size,Size,false,SurfaceFormat.Color);
            Color[] colors = new Color[Size*Size];

            float cx = Size / 2f;
            float cy = Size / 2f;
            float radius = cx;
            float radiusSquared = radius * radius;
            for(float x = 0;x<Size;x+=1f)
                for (float y = 0; y < Size; y += 1f)
                {
                    float xx = x - cx;
                    float yy = y - cy;
                    float distanceSquared = xx*xx + yy*yy;
                    float a = (float)Math.Atan2(xx, yy) + MathHelper.Pi;
                    
                    if (distanceSquared <= radiusSquared)
                    {
                        float l = 1.0f - ((1.0f/radiusSquared)*distanceSquared);

                        var rgb = RGBFromHsl(a, 1f, l);
                        colors[((int)x*Size) + (int)y] = rgb;
                    }
                }
           
            WheelTexture.SetData(colors);
        }

    
        private Color RGBFromHsl(float hue, float saturation, float lightness)
        {
            hue = MathHelper.ToDegrees(hue);

            if (hue < 0.0f || hue > 360f) throw new ArgumentOutOfRangeException("hue");
            if (saturation < 0.0f || saturation > 1.0f) throw new ArgumentOutOfRangeException("saturation");
            if (lightness < 0.0f || lightness > 1.0f) throw new ArgumentOutOfRangeException("lightness");

            if (saturation == 0.0f)
            {
                var b1 = (byte)(lightness * 255);
                return new Color(b1, b1, b1, 255);
            }

            var t2 = 0.0f;

            if (lightness < 0.5f)
                t2 = lightness * (1.0f + saturation);
            else if (lightness >= 0.5f)
                t2 = lightness + saturation - lightness * saturation;

            var t1 = 2.0f * lightness - t2;

            var h = hue / 360f;

            var tr = h + 1.0f / 3.0f;
            var tg = h;
            var tb = h - 1.0f / 3.0f;

            tr = tr < 0.0f ? tr + 1.0f : tr > 1.0f ? tr - 1.0f : tr;
            tg = tg < 0.0f ? tg + 1.0f : tg > 1.0f ? tg - 1.0f : tg;
            tb = tb < 0.0f ? tb + 1.0f : tb > 1.0f ? tb - 1.0f : tb;

            double r;
            if (6.0f * tr < 1.0f)
                r = t1 + (t2 - t1) * 6.0f * tr;
            else if (2.0f * tr < 1.0f)
                r = t2;
            else if (3.0f * tr < 2.0f)
                r = t1 + (t2 - t1) * ((2.0f / 3.0f) - tr) * 6.0f;
            else
                r = t1;

            double g;
            if (6.0f * tg < 1.0f)
                g = t1 + (t2 - t1) * 6.0f * tg;
            else if (2.0f * tg < 1.0f)
                g = t2;
            else if (3.0f * tg < 2.0f)
                g = t1 + (t2 - t1) * ((2.0f / 3.0f) - tg) * 6.0f;
            else
                g = t1;

            double b;
            if (6.0f * tb < 1.0f)
                b = t1 + (t2 - t1) * 6.0f * tb;
            else if (2.0f * tb < 1.0f)
                b = t2;
            else if (3.0f * tb < 2.0f)
                b = t1 + (t2 - t1) * ((2.0f / 3.0f) - tb) * 6.0f;
            else
                b = t1;

            return new Color((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), 255);
        }

    }
}
