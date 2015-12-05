using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting
{
    public class BarParams : IFormatParams
    {
        public Color BarColor;
        public bool GradientFill;
        public double ProportionValue;

        public BarParams(Color barColor, bool gradientFill)
        {
            BarColor = barColor;
            GradientFill = gradientFill;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        void IFormatParams.Persist(XmlWriter writer)
        {
            writer.WriteElementString("BarColor", BarColor.ToArgb().ToString());
            writer.WriteElementString("GradientFill", CommonHelper.BoolToString(GradientFill));
        }
    }
}
