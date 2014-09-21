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

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid
{
    /// <summary>
    /// List of IOutlookGridGroups
    /// </summary>
    public class OutlookGridRowNodeCollection
    {
        #region "Variables"
        private OutlookGridRow _parentNode;
        private List<OutlookGridRow> subNodes;
        #endregion

        #region "Constructor"
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parentGroup">Parent group , if any</param>
        public OutlookGridRowNodeCollection(OutlookGridRow parentNode)
        {
            this._parentNode = parentNode;
            subNodes = new List<OutlookGridRow>();
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the parent group
        /// </summary>
        public OutlookGridRow ParentNode
		{
			get
			{
				return this._parentNode;
			}
			internal set
			{
				this._parentNode = value;
			}
		}

        /// <summary>
        /// Gets the list of IOutlookGridGroup.
        /// </summary>
        public List<OutlookGridRow> Nodes
        {
            get
            {
                return subNodes;
            }
        }

        /// <summary>
        /// Gets the number of groups
        /// </summary>
        public int Count
        {
            get
            {
                return subNodes.Count;
            }
        }

        #endregion

        #region "Public methods"

        /// <summary>
        /// Gets the Group object
        /// </summary>
        /// <param name="index">Index in the list of groups.</param>
        /// <returns>The IOutlookGridGroup.</returns>
        public OutlookGridRow this[int index]
        {
            get
            {
                return subNodes[index];
            }
        }

        /// <summary>
        /// Adds a new group
        /// </summary>
        /// <param name="row">The IOutlookGridGroup.</param>
        public void Add(OutlookGridRow row)
		{
            row.ParentNode = _parentNode;
            row.NodeLevel = this.ParentNode.NodeLevel + 1; //Not ++
            subNodes.Add(row);
		}

        /// <summary>
        /// Sorts the groups
        /// </summary>
        public void Sort()
        {
            subNodes.Sort();
        }

        /// <summary>
        /// Sorts the groups
        /// </summary>
        internal void Sort(OutlookGridRowComparer comparer)
        {
            subNodes.Sort(comparer);
        }

        /// <summary>
        /// Find a group by its value
        /// </summary>
        /// <param name="value">The value of the group</param>
        /// <returns>The IOutlookGridGroup.</returns>
        //public OutlookGridRow FindRow(object value)
        //{
        //    return subNodes.Find(c => c.Value.Equals(value));
        //}

        public int IndexOf(OutlookGridRow row)
        {
            return this.subNodes.IndexOf(row);
        }

        #endregion

        #region "Internals"

        internal void Clear()
        {
            _parentNode = null;
            //If a group is collapsed the rows will not appear. Then if we clear the group the rows should not remain "collapsed"
            for (int i = 0; i < subNodes.Count; i++)
            {
                subNodes[i].Collapsed = false;
            }
            subNodes.Clear();
        }

        #endregion
    }
}