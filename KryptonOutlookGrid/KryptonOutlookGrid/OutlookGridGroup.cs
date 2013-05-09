// Copyright 2006 Herre Kuijpers - <herre@xs4all.nl>
//
// This source file(s) may be redistributed, altered and customized
// by any means PROVIDING the authors name and all copyright
// notices remain intact.
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUR OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//-----------------------------------------------------------------------

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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid;
using JDHSoftware.Krypton.Toolkit.Utils;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid
{
    /// <summary>
    /// Each arrange/grouping class must implement the IOutlookGridGroup interface
    /// the Group object will determine for each object in the grid, whether it
    /// falls in or outside its group.
    /// It uses the IComparable.CompareTo function to determine if the item is in the group.
    /// This class group the elements by default (string, int, ...)
    /// </summary>
    public class OutlookgGridDefaultGroup : IOutlookGridGroup
    {
        #region "Variables"
        /// <summary>
        /// The Value of the group
        /// </summary>
        protected object val;
        /// <summary>
        /// The displayed text
        /// </summary>
        protected string text;
        /// <summary>
        /// Boolean if the group is collapsed or not
        /// </summary>
        protected bool collapsed;
        /// <summary>
        /// The associated DataGridView column.
        /// </summary>
        protected OutlookGridColumn column;
        /// <summary>
        /// The number of items in this group.
        /// </summary>
        protected int itemCount;
        /// <summary>
        /// The height (in pixels).
        /// </summary>
        protected int height;
        /// <summary>
        /// The list of rows associated to the group.
        /// </summary>
        protected List<OutlookGridRow> rows;
        /// <summary>
        /// The parent group if any.
        /// </summary>
        protected IOutlookGridGroup parentGroup;
        /// <summary>
        /// The level (nested) of grouping
        /// </summary>
        protected int level;
        /// <summary>
        /// The children groups
        /// </summary>
        protected OutlookGridGroupCollection children;
        #endregion

        #region "Constructor"
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parentGroup">The parent group if any.</param>
        public OutlookgGridDefaultGroup(IOutlookGridGroup parentGroup)
        {
            val = null;

            this.column = null;
            height = 34; // default height
            rows = new List<OutlookGridRow>();
            children = new OutlookGridGroupCollection(parentGroup);
        }
        #endregion

        #region IOutlookGridGroup Members

        /// <summary>
        /// Gets or sets the list of rows associated to the group.
        /// </summary>
        public List<OutlookGridRow> Rows
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
            }
        }

        /// <summary>
        /// Gets or sets the parent group.
        /// </summary>
        /// <value>The parent group.</value>
        public IOutlookGridGroup ParentGroup
        {
            get
            {
                return parentGroup;
            }
            set
            {
                parentGroup = value;
            }
        }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public OutlookGridGroupCollection Children
        {
            get
            {
                return children;
            }
            set
            {
                children = value;
            }
        }

        /// <summary>
        /// Gets or sets the displayed text
        /// </summary>
        public virtual string Text
        {
            get
            {
                if (column == null)
                    return string.Format("Unbound group: {0} ({1})", Value.ToString(), itemCount == 1 ? "1 item" : itemCount.ToString() + " items");
                else
                    return string.Format("{0}: {1} ({2})", column.DataGridViewColumn.HeaderText, Value.ToString(), itemCount == 1 ? "1 item" : itemCount.ToString() + " items");
            }
            set
            {
                text = value;
            }
        }

        /// <summary>
        /// Gets or sets the Value of the group
        /// </summary>
        public virtual object Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
            }
        }

        /// <summary>
        /// Boolean if the group is collapsed or not
        /// </summary>
        public virtual bool Collapsed
        {
            get
            {
                return collapsed;
            }
            set
            {
                collapsed = value;
            }
        }

        /// <summary>
        /// Gets or sets the associated DataGridView column.
        /// </summary>
        public virtual OutlookGridColumn Column
        {
            get
            {
                return column;
            }
            set
            {
                column = value;
            }
        }

        /// <summary>
        /// Gets or set the number of items in this group.
        /// </summary>
        public virtual int ItemCount
        {
            get
            {
                return itemCount;
            }
            set
            {
                itemCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the height (in pixels).
        /// </summary>
        public virtual int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Overrides the Clone() function
        /// </summary>
        /// <returns>OutlookgGridDefaultGroup</returns>
        public virtual object Clone()
        {
            OutlookgGridDefaultGroup gr = new OutlookgGridDefaultGroup(this.parentGroup);
            gr.column = this.column;
            gr.val = this.val;
            gr.collapsed = this.collapsed;
            gr.text = this.text;
            gr.height = this.height;
            return gr;
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// this is a basic string comparison operation. 
        /// all items are grouped and categorised based on their string-appearance.
        /// </summary>
        /// <param name="obj">the value in the related column of the item to compare to</param>
        /// <returns></returns>
        public virtual int CompareTo(object obj)
        {
            int orderModifier = (Column.SortDirection == SortOrder.Ascending ? 1 : -1);
            return string.Compare(val.ToString(), obj.ToString()) * orderModifier;
        }
        #endregion
    }

    /// <summary>
    /// This group simple example of an implementation which groups the items into Alphabetic categories
    /// based only on the first letter of each item
    /// 
    /// For this we need to override the Value property (used for comparison)
    /// and the CompareTo function.
    /// Also the Clone method must be overriden, so this Group object can create clones of itself.
    /// Cloning of the group is used by the OutlookGrid
    /// </summary>
    public class OutlookGridAlphabeticGroup : OutlookgGridDefaultGroup
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parentGroup">The parentGroup if any.</param>
        public OutlookGridAlphabeticGroup(IOutlookGridGroup parentGroup)
            : base(parentGroup)
        {
        }

        /// <summary>
        /// Gets or sets the displayed text.
        /// </summary>
        public override string Text
        {
            get
            {
                return string.Format("{0}: {1} ({2})", LangManager.Instance.GetString("AlphabeticGroupText"), column.DataGridViewColumn.HeaderText, Value.ToString(), itemCount == 1 ? LangManager.Instance.GetString("OneItem") : itemCount.ToString() + LangManager.Instance.GetString("XXXItems"));
            }
            set
            {
                text = value;
            }
        }

        /// <summary>
        /// Gets or sets the Alphabetic value
        /// </summary>
        public override object Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value.ToString().Substring(0, 1).ToUpper();
            }
        }

        #region ICloneable Members

        /// <summary>
        /// Overrides the Clone() function
        /// </summary>
        /// <returns>OutlookGridAlphabeticGroup</returns>
        public override object Clone()
        {
            OutlookGridAlphabeticGroup gr = new OutlookGridAlphabeticGroup(this.parentGroup);
            gr.column = this.column;
            gr.val = this.val;
            gr.collapsed = this.collapsed;
            gr.text = this.text;
            gr.height = this.height;
            return gr;
        }

        #endregion

        #region IComparable Members
        /// <summary>
        /// overide the CompareTo, so only the first character is compared, instead of the whole string
        /// this will result in classifying each item into a letter of the Alphabet.
        /// for instance, this is usefull when grouping names, they will be categorized under the letters A, B, C etc..
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int CompareTo(object obj)
        {
            int orderModifier = (Column.SortDirection == SortOrder.Ascending ? 1 : -1);

            if (obj is OutlookGridAlphabeticGroup)
            {
               return string.Compare(val.ToString(), ((OutlookGridAlphabeticGroup)obj).Value.ToString()) * orderModifier;
            }
            else
            {
                return 0;
            }
        }
        #endregion IComparable Members
    }

    /// 
    /// this group simple example of an implementation which groups the items into day categories
    /// based on, today, yesterday, last week etc
    /// 
    /// for this we need to override the Value property (used for comparison)
    /// and the CompareTo function.
    /// Also the Clone method must be overriden, so this Group object can create clones of itself.
    /// Cloning of the group is used by the OutlookGrid
    /// 
    public class OutlookGridDateTimeGroup : OutlookgGridDefaultGroup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parentGroup">The parentGroup if any.</param>
        public OutlookGridDateTimeGroup(IOutlookGridGroup parentGroup)
            : base(parentGroup)
        {
        }

        /// <summary>
        /// Gets or sets the displayed text.
        /// </summary>
        public override string Text
        {
            get
            {
                return string.Format("{0}: {1} ({2})", LangManager.Instance.GetString("DateGroupText"), column.DataGridViewColumn.HeaderText, Value.ToString(), itemCount == 1 ? LangManager.Instance.GetString("OneItem") : itemCount.ToString() + LangManager.Instance.GetString("XXXItems"));
            }
            set
            {
                text = value;
            }
        }

        private DateTime valDateTime;

        /// <summary>
        /// Gets or sets the Date value
        /// </summary>
        public override object Value
        {
            get
            {
                return val;
            }
            set
            {
                //If no date Time let the valDateTime to the min value !
                if (value != null && value != DBNull.Value)
                    valDateTime = DateTime.Parse(value.ToString());
                else
                    valDateTime = DateTime.MinValue;

                val = OutlookGridGroupHelpers.GetDayText(valDateTime);
            }
        }

        #region ICloneable Members

        /// <summary>
        /// Overrides the Clone() function
        /// </summary>
        /// <returns>OutlookGridDateTimeGroup</returns>
        public override object Clone()
        {
            OutlookGridDateTimeGroup gr = new OutlookGridDateTimeGroup(this.parentGroup);
            gr.column = this.column;
            gr.val = this.val;
            gr.valDateTime = this.valDateTime;
            gr.collapsed = this.collapsed;
            gr.text = this.text;
            gr.height = this.height;
            return gr;
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Overrides CompareTo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int CompareTo(object obj)
        {
            int orderModifier = (Column.SortDirection == SortOrder.Ascending ? 1 : -1);
            DateTime val;
            if (obj is DateTime)
            {
                val = DateTime.Parse(obj.ToString());
            }
            else if (obj is OutlookGridDateTimeGroup)
            {
                val = ((OutlookGridDateTimeGroup)obj).valDateTime;
            }
            else
            {
                val = new DateTime();
            }

            if (OutlookGridGroupHelpers.GetDateCode(valDateTime.Date) == OutlookGridGroupHelpers.GetDateCode(val.Date))
            {
                return 0;
            }
            else
            {
                return DateTime.Compare(valDateTime.Date, val.Date) * orderModifier ;
            }
        }
        #endregion IComparable Members
    }

    /// 
    /// this group simple example of an implementation which groups the items into years
    /// 
    /// for this we need to override the Value property (used for comparison)
    /// and the CompareTo function.
    /// Also the Clone method must be overriden, so this Group object can create clones of itself.
    /// Cloning of the group is used by the OutlookGrid
    /// 
    public class OutlookGridYearGroup : OutlookgGridDefaultGroup
    {
        public OutlookGridYearGroup(IOutlookGridGroup parentGroup)
            : base(parentGroup)
        {
        }

        public override string Text
        {
            get
            {
                return string.Format("{0}: {1} ({2})", LangManager.Instance.GetString("YearGroupText"), column.DataGridViewColumn.HeaderText, Value.ToString(), itemCount == 1 ? LangManager.Instance.GetString("OneItem") : itemCount.ToString() + LangManager.Instance.GetString("XXXItems"));
            }
            set
            {
                text = value;
            }
        }

        private DateTime valDateTime;

        public override object Value
        {
            get
            {
                return val;
            }
            set
            {
                //If no date Time let the valDateTime to the min value !
                if (value != null && value != DBNull.Value)
                {
                    valDateTime = DateTime.Parse(value.ToString());
                    val = valDateTime.Year;
                }
                else
                {
                    valDateTime = DateTime.MinValue;
                    val = LangManager.Instance.GetString("NODATE"); 
                }
            }
        }

        #region ICloneable Members

        /// <summary>
        /// Overrides the Clone() function
        /// </summary>
        /// <returns>OutlookGridYearGroup</returns>
        public override object Clone()
        {
            OutlookGridYearGroup gr = new OutlookGridYearGroup(this.parentGroup);
            gr.column = this.column;
            gr.val = this.val;
            gr.valDateTime = this.valDateTime;
            gr.collapsed = this.collapsed;
            gr.text = this.text;
            gr.height = this.height;
            return gr;
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Overrides compareTo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int CompareTo(object obj)
        {
            int orderModifier = (Column.SortDirection == SortOrder.Ascending ? 1 : -1);
            DateTime val;
            //obj is a proper datetime or it can be group itself, so select the proper conversion
            if (obj is DateTime)
            {
                val = DateTime.Parse(obj.ToString());
            }
            else if (obj is OutlookGridYearGroup)
            {
                val = ((OutlookGridYearGroup)obj).valDateTime;
            }
            else
            {
                val = new DateTime();
            }

            if (valDateTime.Year == val.Year)
            {
                return 0;
            }
            else
            {
                return valDateTime.Year.CompareTo(val.Year) * orderModifier;
            }
        }
        #endregion IComparable Members
    }
}