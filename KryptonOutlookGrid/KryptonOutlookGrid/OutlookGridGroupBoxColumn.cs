//--------------------------------------------------------------------------------
// Copyright (C) 2013 JDH Software - <support@jdhsoftware.com>
//
// This program is provided to you under the terms of the Microsoft Public
// License (Ms-PL) as published at https://kryptonoutlookgrid.codeplex.com/license
//
// Visit http://www.jdhsoftware.com and follow @jdhsoftware on Twitter
//
//--------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid
{
    /// <summary>
    /// Column for the OutlookGrid GroupBox
    /// </summary>
    public class OutlookGridGroupBoxColumn : IEquatable<OutlookGridGroupBoxColumn>
    {
        #region "Constructor"
        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public OutlookGridGroupBoxColumn()
        //{}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="columnText">The display text of the column.</param>
        /// <param name="sort">The column sort order.</param>
        public OutlookGridGroupBoxColumn(string columnName, string columnText, SortOrder sort)
        {
            Text = columnText;
            ColumnName = columnName;
            SortOrder = sort;
        }
        #endregion

        #region "Properties"

        public Rectangle Rect { get; set; }
        public string Text { get; set;}
        public bool Pressed { get; set; }
        public SortOrder SortOrder { get; set; }
        public string ColumnName { get; set; }
        public bool IsMoving { get; set; }
        public bool IsHovered { get; set; }

        #endregion

        #region "Implements"
        /// <summary>
        /// Defines Equals method on the columnName
        /// </summary>
        /// <param name="other">The OutlookGridGroupBoxColumn to compare with.</param>
        /// <returns>True or False.</returns>
        public bool Equals(OutlookGridGroupBoxColumn other)
        {
            return this.ColumnName.Equals(other.ColumnName);
        }
        #endregion
    }
}
