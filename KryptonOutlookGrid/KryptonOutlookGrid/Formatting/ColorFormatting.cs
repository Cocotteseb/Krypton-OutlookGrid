using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using static ColorHelper;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting
{
    public static class ColorFormatting
    {

        //    function transition(value, maximum, start_point, end_point):
        //return start_point + (end_point - start_point)*value/maximum

        //        public transition(double value, int max, )


        //Red to white (near what I want to achieve)
        //public static Color ConvertTwoRange(double value, TwoColorsParams par)
        //{
        //    if (value > 100) { value = 100; }
        //    return ColorHelper.FromHSV(ColorHelper.ToHSV(par.MinimumColor.R, par.MinimumColor.G, par.MinimumColor.B).Hue , (float)value / 100, 100);
        //    //return ColorHelper.FromHSV((float)value, (float)1, (float)1);
        //}

        //Flash green to flash red
        //public static Color ConvertTwoRange(double value, double min, double max, TwoColorsParams par)
        //{
        //    double range = (max - min) / 2;
        //    value -= max - range;
        //    double factor = 255 / range;
        //    double red = value < 0 ? value * factor : 255;
        //    double green = value > 0 ? (range - value) * factor : 255;

        //    // Create and return brush
        //    Color color = Color.FromArgb(255,(byte)red, (byte)green, 0);

        //    return color;
        //}

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
            float H;
            if (A.Hue == B.Hue)
            {
                H = A.Hue;
            }
            else
            {
                //OK for blue  ------> red 
                //float TRange =Math.Abs (A.Hue-B.Hue);
                //if (A.Hue > B.Hue)
                //{
                //    TRange = 360 - A.Hue + B.Hue;
                //}
                //else
                //{
                //    TRange = B.Hue - A.Hue;
                //}
                //float tmp = (float)percent * TRange / 100;
                //H = A.Hue + tmp;


                //red -----------> blue
                bool inverse = false;
                float TRange = Math.Abs(A.Hue - B.Hue);
                if (TRange > 180) // 360/ 2 
                {
                    TRange = 360 - TRange;
                    inverse = true;
                }
                float tmp = (float)percent * TRange / 100;


                if (inverse)
                {
                    H = A.Hue - tmp;
                    if (H < 0)
                    {
                        H = 360 + H;
                    }
                }
                else
                {
                    H = A.Hue + tmp;
                    if (H > 360)
                    {
                        H = H - 360;
                    }
                }

            }

            float S;
            if (A.Saturation == B.Saturation)
            {
                S = A.Saturation;
            }
            else
            {
                double SRange = (double)Math.Abs(A.Saturation - B.Saturation);
                S = (float)(percent * SRange / 100); // (float)(A.Saturation + percent * SRange / 100) % 100;
            }
            S = 1; // force

            return ColorFromHSV(H, S, 1);
            // if (value > 100) { value = 100; }
            // return ColorHelper.FromHSV(ColorHelper.ToHSV(par.MinimumColor.R, par.MinimumColor.G, par.MinimumColor.B).Hue, (float)value / 100, 100);
            //return ColorHelper.FromHSV((float)value, (float)1, (float)1);
        }
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

        public static HSVColor ColorToHSV(Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            double hue = color.GetHue();
            double saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            double value = max / 255d;

            return new HSVColor((float)hue, (float)saturation, (float)value);
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
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
        public static Color Interpolate(double percent, params Color[] colors)
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

        //public static Color Convert(double value)
        //{

        // Get the value limits from parameter
        //try
        //{
        //    string[] limits = (parameter as string).Split(new char[] { '|' });
        //    min = double.Parse(limits[0], CultureInfo.InvariantCulture);
        //    max = double.Parse(limits[1], CultureInfo.InvariantCulture);
        //}
        //catch (Exception)
        //{
        //    throw new ArgumentException("Parameter not valid. Enter in format: 'MinDouble|MaxDouble'");
        //}

        //if (max <= min)
        //{
        //    throw new ArgumentException("Parameter not valid. MaxDouble has to be greater then MinDouble.");
        //}

        //if (value >= min && value <= max)
        //{
        //    // Calculate color channels
        //    double range = (max - min) / 2;
        //    value -= max - range;
        //    double factor = 255 / range;
        //    double red = value < 0 ? value * factor : 255;
        //    double green = value > 0 ? (range - value) * factor : 255;

        //    // Create and return brush
        //    Color color = Color.FromArgb(255, (byte)red, (byte)green, 0);

        //    return color;
        //}

        //switch (format.FormatType)
        //{
        //    case EnumConditionalFormatType.Bar:
        //        break;
        //    case EnumConditionalFormatType.TwoColorRange:
        //        //TODO handle color
        //        if (value > 100) { value = 100; }
        //        return ColorHelper.FromHSV(3, (float)value / 100, 100);
        //        break;
        //    case EnumConditionalFormatType.ThreeColorRange:
        //        break;
        //}
        //return new Color();
        //}


        //private const int RGB_MAX = 255; // Reduce this for a darker range
        //private const int RGB_MIN = 100; // Increase this for a lighter range

        //private static Color getColorFromPercentage(int percentage)
        //{
        //    // Work out the percentage of red and green to use (i.e. a percentage
        //    // of the range from RGB_MIN to RGB_MAX)
        //    var redPercent = Math.Min(200 - (percentage * 2), 100) / 100f;
        //    var greenPercent = Math.Min(percentage * 2, 100) / 100f;

        //    // Now convert those percentages to actual RGB values in the range
        //    // RGB_MIN - RGB_MAX
        //    var red = RGB_MIN + ((RGB_MAX - RGB_MIN) * redPercent);
        //    var green = RGB_MIN + ((RGB_MAX - RGB_MIN) * greenPercent);

        //    return Color.FromArgb((int)red, (int)green, RGB_MIN);
        //}

    }
}

