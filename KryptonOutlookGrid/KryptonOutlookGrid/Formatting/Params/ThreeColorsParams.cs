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
using System.Xml;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting
{
    /// <summary>
    /// Three scale color class parameters
    /// </summary>
    /// <seealso cref="JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting.IFormatParams" />
    public class ThreeColorsParams : IFormatParams
    {
        /// <summary>
        /// The minimum color
        /// </summary>
        public Color MinimumColor;
        /// <summary>
        /// The medium color
        /// </summary>
        public Color MediumColor;
        /// <summary>
        /// The maximum color
        /// </summary>
        public Color MaximumColor;
        /// <summary>
        /// The color associated to the value
        /// </summary>
        public Color ValueColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreeColorsParams"/> class.
        /// </summary>
        /// <param name="minColor">The minimum color.</param>
        /// <param name="mediumColor">Color of the medium.</param>
        /// <param name="maxColor">The maximum color.</param>
        public ThreeColorsParams(Color minColor, Color mediumColor, Color maxColor)
        {
            MinimumColor = minColor;
            MediumColor = mediumColor;
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
        void IFormatParams.Persist(XmlWriter writer)
        {
            writer.WriteElementString("MinimumColor", MinimumColor.ToArgb().ToString());
            writer.WriteElementString("MediumColor", MediumColor.ToArgb().ToString());
            writer.WriteElementString("MaximumColor", MaximumColor.ToArgb().ToString());
        }
    }
}
