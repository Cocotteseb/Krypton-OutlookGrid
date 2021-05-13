//--------------------------------------------------------------------------------
// Copyright (C) 2013-2021 JDH Software - <support@jdhsoftware.com>
//
// This program is provided to you under the terms of the Microsoft Public
// License (Ms-PL) as published at https://github.com/Cocotteseb/Krypton-OutlookGrid/blob/master/LICENSE.md
//
// Visit https://www.jdhsoftware.com and follow @jdhsoftware on Twitter
//
//--------------------------------------------------------------------------------

using ComponentFactory.Krypton.Toolkit;
using System.Drawing;
using System.Xml;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting
{
    /// <summary>
    /// Parameters for Bar formatting
    /// </summary>
    /// <seealso cref="JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting.IFormatParams" />
    public class BarParams : IFormatParams
    {
        /// <summary>
        /// The bar color
        /// </summary>
        public Color BarColor;
        /// <summary>
        /// The gradient fill
        /// </summary>
        public bool GradientFill;
        /// <summary>
        /// The proportion value
        /// </summary>
        public double ProportionValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="BarParams"/> class.
        /// </summary>
        /// <param name="barColor">Color of the bar.</param>
        /// <param name="gradientFill">if set to <c>true</c> [gradient fill].</param>
        public BarParams(Color barColor, bool gradientFill)
        {
            BarColor = barColor;
            GradientFill = gradientFill;
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
            writer.WriteElementString("BarColor", BarColor.ToArgb().ToString());
            writer.WriteElementString("GradientFill", CommonHelper.BoolToString(GradientFill));
        }
    }
}
