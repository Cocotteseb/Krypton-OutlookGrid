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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid
{
    /// <summary>
    /// Column for the OutlookGrid
    /// </summary>
    public class OutlookGridColumn :IEquatable <OutlookGridColumn>
    {
        private IOutlookGridGroup groupingType;
        private DataGridViewColumn column;
        private SortOrder sortDirection;
        private bool isGrouped;
        private int groupOrder;
        private string name;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="col">The DataGridViewColumn.</param>
        /// <param name="group">The group chosen for the column.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="grouped">true if grouped, false otherwise.</param>
        public OutlookGridColumn(DataGridViewColumn col, IOutlookGridGroup group, SortOrder sort, bool grouped)
        {
            this.column = col;
            this.groupingType = group;
            sortDirection = sort;
            isGrouped = grouped;
            this.name = col.Name; 
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnName">The name.</param>
        /// <param name="col">The DataGridViewColumn.</param>
        /// <param name="group">The group chosen for the column.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="grouped">true if grouped, false otherwise.</param>
        public OutlookGridColumn(string columnName, DataGridViewColumn col, IOutlookGridGroup group, SortOrder sort, bool grouped)
        {
            this.name = columnName;
            this.column = col;
            this.groupingType = group;
            sortDirection = sort;
            isGrouped = grouped;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the column name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets if the column is grouped
        /// </summary>
        public bool IsGrouped
        {
            get { return isGrouped; }
            set { isGrouped = value; }
        }

        /// <summary>
        /// Gets or sets the sort direction
        /// </summary>
        public SortOrder SortDirection
        {
            get { return sortDirection; }
            set { sortDirection = value; }
        }

        /// <summary>
        /// Gets or sets the associated DataGridViewColumn
        /// </summary>
        public DataGridViewColumn DataGridViewColumn
        {
            get { return column; }
            set { column = value; }
        }

        /// <summary>
        /// Gets or sets the group
        /// </summary>
        public IOutlookGridGroup GroupingType
        {
            get { return groupingType; }
            set { groupingType = value; }
        }

        /// <summary>
        /// Gets or sets the group order
        /// </summary>
        public int GroupOrder
        {
            get { return groupOrder; }
            set { groupOrder = value; }
        }
        #endregion

        #region Implements

        /// <summary>
        /// Defines Equals methode (interface IEquatable)
        /// </summary>
        /// <param name="other">The OutlookGridColumn to compare with</param>
        /// <returns></returns>
        public bool Equals(OutlookGridColumn other)
        {
            return column.Name.Equals(other.DataGridViewColumn.Name);
        }

        #endregion
    }
}
