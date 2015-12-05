using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting
{
    public class TwoColorsParams : IFormatParams
    {
        public Color MinimumColor;
        public Color MaximumColor;
        public Color ValueColor;

        public TwoColorsParams(Color minColor, Color maxColor)
        {
            MinimumColor = minColor;
            MaximumColor = maxColor;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Persist(XmlWriter writer)
        {
            writer.WriteElementString("MinimumColor", MinimumColor.ToArgb().ToString());
            writer.WriteElementString("MaximumColor", MaximumColor.ToArgb().ToString());
        }
    }
}
