using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting
{
    public class ThreeColorsParams : IFormatParams
    {
        public Color MinimumColor;
        public Color MediumColor;
        public Color MaximumColor;
        public Color ValueColor;

        public ThreeColorsParams(Color minColor, Color mediumColor, Color maxColor)
        {
            MinimumColor = minColor;
            MediumColor = mediumColor;
            MaximumColor = maxColor;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Persist(XmlWriter writer)
        {
            writer.WriteElementString("MinimumColor", MinimumColor.ToArgb().ToString());
            writer.WriteElementString("MediumColor", MediumColor.ToArgb().ToString());
            writer.WriteElementString("MaximumColor", MaximumColor.ToArgb().ToString());
        }
    }
}
