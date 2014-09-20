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
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using ComponentFactory.Krypton.Toolkit;
using System.Diagnostics;
using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.CustomsColumns;
using System.Globalization;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid
{
    internal class OutlookGridRowComparer2 : IComparer<OutlookGridRow>
    {
        List<Tuple<int, SortOrder, IComparer>> sortColumnIndexAndOrder;

        public OutlookGridRowComparer2(List<Tuple<int, SortOrder, IComparer>> sortList)
        {
            this.sortColumnIndexAndOrder = sortList;
        }

        #region IComparer Members

        public int Compare(OutlookGridRow x, OutlookGridRow y)
        {
            int compareResult = 0;
            int orderModifier;
            try
            {
                for (int i = 0; i < sortColumnIndexAndOrder.Count; i++)
                {
                    if (compareResult == 0)
                    {
                        orderModifier = (sortColumnIndexAndOrder[i].Item2 == SortOrder.Ascending ? 1 : -1);

                        object o1 = x.Cells[sortColumnIndexAndOrder[i].Item1].Value;
                        object o2 = y.Cells[sortColumnIndexAndOrder[i].Item1].Value;
                        if (sortColumnIndexAndOrder[i].Item3 != null)
                        {
                           return sortColumnIndexAndOrder[i].Item3.Compare(o1, o2) * orderModifier;
                        }
                        else
                        {
                            if ((o1 == null || o1 == DBNull.Value) && (o2 != null && o2 != DBNull.Value))
                            {
                                compareResult = 1;
                            }
                            else if ((o1 != null && o1 != DBNull.Value) && (o2 == null || o2 == DBNull.Value))
                            {
                                compareResult = -1;
                            }
                            else
                            {
                                if (o1 is string)
                                {
                                    compareResult = string.Compare(o1.ToString(), o2.ToString()) * orderModifier;
                                }
                                else if (o1 is DateTime)
                                {
                                    compareResult = ((DateTime)o1).CompareTo((DateTime)o2) * orderModifier;
                                }
                                else if (o1 is int)
                                {
                                    compareResult = ((int)o1).CompareTo((int)o2) * orderModifier;
                                }
                                else if (o1 is bool)
                                {
                                    bool b1 = (bool)o1;
                                    bool b2 = (bool)o2;
                                    compareResult = (b1 == b2 ? 0 : b1 == true ? 1 : -1) * orderModifier;
                                }
                                else if (o1 is float)
                                {
                                    float n1 = (float)o1;
                                    float n2 = (float)o2;
                                    compareResult = (n1 > n2 ? 1 : n1 < n2 ? -1 : 0) * orderModifier;
                                }
                                else if (o1 is double)
                                {
                                    double n1 = (double)o1;
                                    double n2 = (double)o2;
                                    compareResult = (n1 > n2 ? 1 : n1 < n2 ? -1 : 0) * orderModifier;
                                }
                                else if (o1 is decimal)
                                {
                                    decimal d1 = (decimal)o1;
                                    decimal d2 = (decimal)o2;
                                    compareResult = (d1 > d2 ? 1 : d1 < d2 ? -1 : 0) * orderModifier;
                                }
                                else if (o1 is long)
                                {
                                    long n1 = (long)o1;
                                    long n2 = (long)o2;
                                    compareResult = (n1 > n2 ? 1 : n1 < n2 ? -1 : 0) * orderModifier;
                                }
                                else if (o1 is TextAndImage)
                                {
                                    compareResult = string.Compare(((TextAndImage)o1).ToString(), ((TextAndImage)o2).ToString()) * orderModifier;
                                }
                            }
                        }
                    }
                }
                return compareResult;
            }
            catch (Exception ex)
            {
                throw new Exception("OutlookGridRowComparer: " + this.ToString(), ex);
            }
        }
        #endregion
    }
}
