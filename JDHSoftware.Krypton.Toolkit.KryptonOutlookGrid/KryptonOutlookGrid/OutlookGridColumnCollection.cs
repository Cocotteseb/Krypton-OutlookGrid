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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid
{
    /// <summary>
    /// List of the current columns of the OutlookGrid
    /// </summary>
    public class OutlookGridColumnCollection : List<OutlookGridColumn>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OutlookGridColumnCollection()
            : base()
        {
        }

        /// <summary>
        /// Gets the OutlookGridColumn in the list by its name
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <returns>OutlookGridColumn</returns>
        public OutlookGridColumn this[string columnName]
        {
            get
            {
                return this.Find(c => c.DataGridViewColumn.Name.Equals(columnName));
            }
        }

        /// <summary>
        /// Gets the number of columns grouped
        /// </summary>
        /// <returns>the number of columns grouped.</returns>
        public int CountGrouped()
        {
            return this.Count(c => c.IsGrouped == true);
        }

        /// <summary>
        /// Gets the list of grouped columns
        /// </summary>
        /// <returns>The list of grouped columns.</returns>
        public List<OutlookGridColumn> GroupedColumns()
        {
            return this.FindAll(c => c.IsGrouped);
        }

        /// <summary>
        /// Gets the column from its real index (from the underlying DataGridViewColumn)
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The OutlookGridColumn.</returns>
        public OutlookGridColumn FindFromColumnIndex(int index)
        {
            return this.FirstOrDefault(c => c.DataGridViewColumn.Index == index);
        }

        /// <summary>
        /// Gets the index (from DataGridViewColumn) of the sorted column that is not grouped if it exists.
        /// </summary>
        /// <returns>-1 if any, the index of the underlying DataGridViewColumn otherwise</returns>
        public int FindSortedColumnNotgrouped()
        {
            var res = this.Find(c => !c.IsGrouped && (c.SortDirection != SortOrder.None));
            if (res == null)
            { 
                return -1;
            }
            else
            {
                return res.DataGridViewColumn.Index;
            }
        }
    }
}
