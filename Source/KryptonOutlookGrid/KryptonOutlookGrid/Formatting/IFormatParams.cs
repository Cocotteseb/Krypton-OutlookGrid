//--------------------------------------------------------------------------------
// Copyright (C) 2013-2021 JDH Software - <support@jdhsoftware.com>
//
// This program is provided to you under the terms of the Microsoft Public
// License (Ms-PL) as published at https://github.com/Cocotteseb/Krypton-OutlookGrid/blob/master/LICENSE.md
//
// Visit https://www.jdhsoftware.com and follow @jdhsoftware on Twitter
//
//--------------------------------------------------------------------------------

using System;
using System.Xml;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting
{
    /// <summary>
    /// Parameter class for Conditionnal Formatting
    /// </summary>
    /// <seealso cref="System.ICloneable" />
    public interface IFormatParams : ICloneable
    {
        /// <summary>
        /// Persists the parameters.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        void Persist(XmlWriter writer);
    }
}
