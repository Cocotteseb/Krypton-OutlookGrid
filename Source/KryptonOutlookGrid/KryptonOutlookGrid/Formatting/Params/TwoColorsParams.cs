//--------------------------------------------------------------------------------
// Copyright (C) 2013-2015 JDH Software - <support@jdhsoftware.com>
//
// This program is provided to you under the terms of the Microsoft Public
// License (Ms-PL) as published at https://kryptonoutlookgrid.codeplex.com/license
//
// Visit http://www.jdhsoftware.com and follow @jdhsoftware on Twitter
//
//--------------------------------------------------------------------------------

using System.Drawing;
using System.Xml;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting
{
    /// <summary>
    /// Two scale color class parameters
    /// </summary>
    /// <seealso cref="JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting.IFormatParams" />
    public class TwoColorsParams : IFormatParams
    {
        /// <summary>
        /// Minimum color
        /// </summary>
        public Color MinimumColor;
        /// <summary>
        /// Maximum color
        /// </summary>
        public Color MaximumColor;
        /// <summary>
        /// Color associated to the value between min and max color
        /// </summary>
        public Color ValueColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoColorsParams"/> class.
        /// </summary>
        /// <param name="minColor">The minimum color.</param>
        /// <param name="maxColor">The maximum color.</param>
        public TwoColorsParams(Color minColor, Color maxColor)
        {
            MinimumColor = minColor;
            MaximumColor = maxColor;
        }

        /// <summary>
        /// Crée un objet qui est une copie de l'instance actuelle.
        /// </summary>
        /// <returns>
        /// Nouvel objet qui est une copie de cette instance.
        /// </returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Persists the parameters.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        public void Persist(XmlWriter writer)
        {
            writer.WriteElementString("MinimumColor", MinimumColor.ToArgb().ToString());
            writer.WriteElementString("MaximumColor", MaximumColor.ToArgb().ToString());
        }
    }
}
