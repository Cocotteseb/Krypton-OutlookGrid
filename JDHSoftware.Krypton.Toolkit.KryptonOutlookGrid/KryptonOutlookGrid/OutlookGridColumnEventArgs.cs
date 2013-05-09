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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.Diagnostics;

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
