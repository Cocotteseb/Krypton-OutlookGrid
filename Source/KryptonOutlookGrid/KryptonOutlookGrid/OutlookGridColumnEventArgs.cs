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

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid
{
    /// <summary>
    /// Class for events of the column in the groupbox.
    /// </summary>
    public class OutlookGridColumnEventArgs : EventArgs
    {
        private OutlookGridColumn column;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="col">The OutlookGridColumn.</param>
        public OutlookGridColumnEventArgs(OutlookGridColumn col)
        {
            this.column = col;
        }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        public OutlookGridColumn Column
        {
            get
            {
                return this.column;
            }
            set
            {
                this.column = value;
            }
        }
    }
}
