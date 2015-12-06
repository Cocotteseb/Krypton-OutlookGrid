//--------------------------------------------------------------------------------
// Copyright (C) 2013-2015 JDH Software - <support@jdhsoftware.com>
//
// This program is provided to you under the terms of the Microsoft Public
// License (Ms-PL) as published at https://kryptonoutlookgrid.codeplex.com/license
//
// Visit http://www.jdhsoftware.com and follow @jdhsoftware on Twitter
//
//--------------------------------------------------------------------------------

using System;
using System.Drawing;
using static ColorHelper;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting
{
    /// <summary>
    /// Color Formatting class : all the magic !
    /// </summary>
    public static class ColorFormatting
    {
        /// <summary>
        /// Returns the percentage value for a Bar formatting.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static double ConvertBar(double value, double min, double max) {
            double percent;
            if (min == max)
            {
                percent = 1.0;
            }
            else
            {
                //Min can be different from 0 
                percent = (value - min) / (max - min);
            }
            return percent;
        }

        /// <summary>
        /// Returns the color for a 2scale color formatting.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="par">The 2color parameters.</param>
        /// <returns></returns>
        public static Color ConvertTwoRange(double value, double min, double max, TwoColorsParams par)
        {
            HSVColor A = ColorToHSV(par.MinimumColor);
            HSVColor B = ColorToHSV(par.MaximumColor);

            //Ratio
            double percent;
            if (min == max)
            {
                percent = 1.0;
            }
            else
            {
                //Min can be different from 0 
                percent = (value - min) / (max - min);
            }
            return Color.FromArgb((int)Math.Round(par.MinimumColor.A + (par.MaximumColor.A - par.MinimumColor.A) * percent),  (int)Math.Round(par.MinimumColor.R + (par.MaximumColor.R - par.MinimumColor.R) * percent),   (int)Math.Round(par.MinimumColor.G + (par.MaximumColor.G - par.MinimumColor.G) * percent),   (int)Math.Round(par.MinimumColor.B + (par.MaximumColor.B - par.MinimumColor.B) * percent));
        }

        /// <summary>
        /// Returns the color for a 3scale color formatting.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="par">The 3color parameters.</param>
        /// <returns></returns>
        public static Color ConvertThreeRange(double value, double min, double max, ThreeColorsParams par)
        {
            HSVColor A = ColorToHSV(par.MinimumColor);
            HSVColor B = ColorToHSV(par.MaximumColor);
            HSVColor C = ColorToHSV(par.MediumColor);

            //Ratio
            double percent;
            if (min == max)
            {
                percent = 1.0;
            }
            else
            {
                //Min can be different from 0 
                percent = (value - min) / (max - min);
            }

            if (percent == 0.5)
            {
                return par.MediumColor;
            }
            else if (percent <= 0.5)
            {
                return Color.FromArgb((int)Math.Round(par.MinimumColor.A + (par.MediumColor.A - par.MinimumColor.A) * percent), (int)Math.Round(par.MinimumColor.R + (par.MediumColor.R - par.MinimumColor.R) * percent), (int)Math.Round(par.MinimumColor.G + (par.MediumColor.G - par.MinimumColor.G) * percent), (int)Math.Round(par.MinimumColor.B + (par.MediumColor.B - par.MinimumColor.B) * percent));
            }
            else
            {
                return Color.FromArgb((int)Math.Round(par.MediumColor.A + (par.MaximumColor.A - par.MediumColor.A) * percent), (int)Math.Round(par.MediumColor.R + (par.MaximumColor.R - par.MediumColor.R) * percent), (int)Math.Round(par.MediumColor.G + (par.MaximumColor.G - par.MediumColor.G) * percent), (int)Math.Round(par.MediumColor.B + (par.MaximumColor.B - par.MediumColor.B) * percent));
            }
        }

        private static HSVColor ColorToHSV(Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            double hue = color.GetHue();
            double saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            double value = max / 255d;

            return new HSVColor((float)hue, (float)saturation, (float)value);
        }

        private static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
        /// <summary>
        /// Interpolate colors 0.0 - 1.0        
        /// </summary>        
        private static Color Interpolate(double percent, params Color[] colors)
        {
            int left = (int)Math.Floor(percent * (colors.Length - 1));
            int right = (int)Math.Ceiling(percent * (colors.Length - 1));
            Color colorLeft = colors[left];
            Color colorRight = colors[right];

            double step = 1.0 / (colors.Length - 1);
            double percentRight = (percent - (left * step)) / step;
            double percentLeft = 1.0 - percentRight;
            return Color.FromArgb((byte)(colorLeft.A * percentLeft + colorRight.A * percentRight), (byte)(colorLeft.R * percentLeft + colorRight.R * percentRight), (byte)(colorLeft.G * percentLeft + colorRight.G * percentRight), (byte)(colorLeft.B * percentLeft + colorRight.B * percentRight));
        }
    }
}

