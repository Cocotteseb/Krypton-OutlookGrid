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
using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid;
using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.CustomsColumns;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid
{
    /// <summary>
    /// OutlookGridRow - subclasses the DataGridView's DataGridViewRow class
    /// In order to support grouping with the same look and feel as Outlook, the behaviour
    /// of the DataGridViewRow is overridden by the OutlookGridRow.
    /// The OutlookGridRow has 2 main additional properties: the Group it belongs to and
    /// a the IsRowGroup flag that indicates whether the OutlookGridRow object behaves like
    /// a regular row (with data) or should behave like a Group row.
    /// </summary>
    public class OutlookGridRow : DataGridViewRow
    {
        #region "Variables"

        private bool isGroupRow;
        private IOutlookGridGroup group;
        private bool _collapsed; //For TreeNode
        private OutlookGridRowNodeCollection nodeCollection; //For TreeNode
        private int _nodeLevel; //For TreeNode
        private OutlookGridRow _parentNode; //for TreeNode
        #endregion

        #region "Properties"

        /// <summary>
        /// Gets or sets the group to the row belongs
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IOutlookGridGroup Group
        {
            get { return group; }
            set { group = value; }
        }

        /// <summary>
        /// Gets or sets if a row is a Group row
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsGroupRow
        {
            get { return isGroupRow; }
            set { isGroupRow = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Collapsed
        {
            get { return _collapsed; }
            set { _collapsed = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OutlookGridRowNodeCollection Nodes
        {
            get { return nodeCollection; }
            set { nodeCollection = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsFirstSibling
        {
            get
            {
                return (this.NodeIndex == 0);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsLastSibling
        {
            get
            {
                OutlookGridRow parent = this._parentNode;
                if (parent != null && parent.HasChildren)
                {
                    return (this.NodeIndex == parent.Nodes.Count - 1);
                }
                else
                    return true;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasChildren
        {
            get
            {
                return nodeCollection.Count > 0;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int NodeLevel
        {
            get { return _nodeLevel; }
            set { _nodeLevel = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OutlookGridRow ParentNode
        {
            get { return _parentNode; }
            set { _parentNode = value; }
        }

        public int NodeIndex
        {
            get
            {
                if (_parentNode != null)
                    return _parentNode.Nodes.IndexOf(this);
                else
                    return 0;
            }
        }
        #endregion

        #region "Constructors"

        /// <summary>
        /// Default Constructor
        /// </summary>
        public OutlookGridRow()
            : this(null, false)
        {
            //nodeCollection = new OutlookGridRowNodeCollection(this);
            //NodeLevel = 0;
            //Collapsed = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="group">The group the row is associated to.</param>
        public OutlookGridRow(IOutlookGridGroup group)
            : this(group, false)
        {
            //nodeCollection = new OutlookGridRowNodeCollection(this);
            //NodeLevel = 0;
            //Collapsed = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="group">The group the row is associated to.</param>
        /// <param name="isGroupRow">Determines if it a group row.</param>
        public OutlookGridRow(IOutlookGridGroup group, bool isGroupRow)
            : base()
        {
            this.group = group;
            this.isGroupRow = isGroupRow;
            nodeCollection = new OutlookGridRowNodeCollection(this);
            NodeLevel = 0;
            Collapsed = true;
        }

        #endregion

        #region "Overrides"

        /// <summary>
        /// Overrides the GetState method
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public override DataGridViewElementStates GetState(int rowIndex)
        {
            //yes its readable at least it was ;)
            if ((IsGroupRow && IsAParentCollapsed(group, 0)) || (!IsGroupRow && group != null && (group.Collapsed || IsAParentCollapsed(group, 0))) || (!IsGroupRow && IsAParentNodeCollapsed(this, 0)))
            {
                return base.GetState(rowIndex) & DataGridViewElementStates.Selected;
            }
            //For the TreeGridView project if the selection mode is FullRow subnodes that where collapsed disappear when parent collapse/expands
            //because for an unknown reason the state becomes None instead of at least visible.
            if (base.GetState(rowIndex) == DataGridViewElementStates.None)
                return DataGridViewElementStates.Visible;
            else
                return base.GetState(rowIndex);
        }

        /// <summary>
        /// the main difference with a Group row and a regular row is the way it is painted on the control.
        /// the Paint method is therefore overridden and specifies how the Group row is painted.
        /// Note: this method is not implemented optimally. It is merely used for demonstration purposes
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipBounds"></param>
        /// <param name="rowBounds"></param>
        /// <param name="rowIndex"></param>
        /// <param name="rowState"></param>
        /// <param name="isFirstDisplayedRow"></param>
        /// <param name="isLastVisibleRow"></param>
        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, bool isFirstDisplayedRow, bool isLastVisibleRow)
        {
            if (this.isGroupRow)
            {
                KryptonOutlookGrid grid = (KryptonOutlookGrid)this.DataGridView;
                int rowHeadersWidth = grid.RowHeadersVisible ? grid.RowHeadersWidth : 0;
                int groupLevelIndentation = group.Level * StaticValues._groupLevelMultiplier;

                int gridwidth = grid.Columns.GetColumnsWidth(DataGridViewElementStates.Visible);
                Rectangle myRowBounds = rowBounds;
                myRowBounds.Width = gridwidth;

                IPaletteBack paletteBack = grid.StateNormal.DataCell.Back;
                IPaletteBorder paletteBorder = grid.StateNormal.DataCell.Border;

                PaletteState state = PaletteState.Normal;
                if (grid.PreviousSelectedGroupRow == rowIndex && (KryptonManager.CurrentGlobalPalette.GetRenderer() != KryptonManager.RenderOffice2013))
                    state = PaletteState.CheckedNormal;

                using (RenderContext renderContext = new RenderContext(grid, graphics, myRowBounds, grid.Renderer))
                {
                    using (GraphicsPath path = grid.Renderer.RenderStandardBorder.GetBackPath(renderContext, myRowBounds, paletteBorder, VisualOrientation.Top, PaletteState.Normal))
                    {
                        //Back
                        IDisposable unused = grid.Renderer.RenderStandardBack.DrawBack(renderContext,
                            myRowBounds,
                            path,
                            paletteBack,
                            VisualOrientation.Top,
                            state,
                            null);

                        // We never save the memento for reuse later
                        if (unused != null)
                        {
                            unused.Dispose();
                            unused = null;
                        }
                    }
                }

                // Draw the botton : solid line for 2007 palettes or dot line for 2010 palettes, full background for 2013
                if (KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2010)
                {
                    using (Pen focusPen = new Pen(Color.Gray))
                    {
                        focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        graphics.DrawLine(focusPen, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset, rowBounds.Bottom - 1, gridwidth + 1, rowBounds.Bottom - 1);
                    }
                }
                else if (KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2013)
                {
                    using (SolidBrush br = new SolidBrush(Color.FromArgb(225, 225, 225)))
                    {
                        graphics.FillRectangle(br, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset, rowBounds.Bottom - StaticValues._2013GroupRowHeight, gridwidth + 1, StaticValues._2013GroupRowHeight - 1);
                    }
                }
                else
                {
                    using (SolidBrush br = new SolidBrush(paletteBorder.GetBorderColor1(state)))
                    {
                        graphics.FillRectangle(br, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset, rowBounds.Bottom - 2, gridwidth + 1, 2);
                    }
                }

                //Draw right vertical bar 
                if (grid.CellBorderStyle == DataGridViewCellBorderStyle.SingleVertical || grid.CellBorderStyle == DataGridViewCellBorderStyle.Single)
                {
                    using (SolidBrush br = new SolidBrush(paletteBorder.GetBorderColor1(state)))
                    {
                        graphics.FillRectangle(br, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + gridwidth, rowBounds.Top, 1, rowBounds.Height);
                    }
                }

                //Set the icon and lines according to the renderer
                if (group.Collapsed)
                {
                    if (KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2010 || KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2013)
                    {
                        graphics.DrawImage(Properties.Resources.collapseIcon2010, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 4 + groupLevelIndentation, rowBounds.Bottom - 18, 11, 11);
                    }
                    else
                    {
                        graphics.DrawImage(Properties.Resources.expandIcon, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 4 + groupLevelIndentation, rowBounds.Bottom - 18, 11, 11);
                    }
                }
                else
                {
                    if (KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2010 || KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2013)
                    {
                        graphics.DrawImage(Properties.Resources.expandIcon2010, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 4 + groupLevelIndentation, rowBounds.Bottom - 18, 11, 11);
                    }
                    else
                    {
                        graphics.DrawImage(Properties.Resources.collapseIcon, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 4 + groupLevelIndentation, rowBounds.Bottom - 18, 11, 11);
                    }
                }

                //Draw image group
                int imageoffset = 0;
                if (this.group.GroupImage != null)
                {
                    if (KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2010 || KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2013)
                    {
                        graphics.DrawImage(this.group.GroupImage, rowHeadersWidth - grid.HorizontalScrollingOffset + StaticValues._ImageOffsetwidth + groupLevelIndentation, rowBounds.Bottom - StaticValues._2013OffsetHeight, StaticValues._groupImageSide, StaticValues._groupImageSide);
                        imageoffset = StaticValues._ImageOffsetwidth;
                    }
                    else
                    {
                        graphics.DrawImage(this.group.GroupImage, rowHeadersWidth - grid.HorizontalScrollingOffset + StaticValues._ImageOffsetwidth + groupLevelIndentation, rowBounds.Bottom - StaticValues._defaultOffsetHeight, StaticValues._groupImageSide, StaticValues._groupImageSide);
                        imageoffset = StaticValues._ImageOffsetwidth;
                    }
                }

                //Draw text, using the current grid font
                int offsetText = rowHeadersWidth - grid.HorizontalScrollingOffset + 18 + imageoffset + groupLevelIndentation;
                if (KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2013)
                {
                    TextRenderer.DrawText(graphics, group.Text, grid.GridPalette.GetContentShortTextFont(PaletteContentStyle.LabelBoldControl, state), new Rectangle(offsetText, rowBounds.Bottom - StaticValues._2013OffsetHeight, rowBounds.Width - offsetText, rowBounds.Height), grid.GridPalette.GetContentShortTextColor1(PaletteContentStyle.LabelNormalControl, state),
                                 TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.PreserveGraphicsClipping);
                }
                else
                {
                    TextRenderer.DrawText(graphics, group.Text, grid.GridPalette.GetContentShortTextFont(PaletteContentStyle.LabelBoldControl, state), new Rectangle(offsetText, rowBounds.Bottom - StaticValues._defaultOffsetHeight, rowBounds.Width - offsetText, rowBounds.Height), grid.GridPalette.GetContentShortTextColor1(PaletteContentStyle.LabelNormalControl, state),
                                   TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.PreserveGraphicsClipping);
                }

                ////Debug Hits
                ////ExpandCollaspe icon
                //graphics.DrawRectangle(new Pen(Color.Red), new Rectangle(rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 4 + group.Level * 15, rowBounds.Bottom - 18, 11, 11));
                ////Image
                //if (KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2013)
                //    graphics.DrawRectangle(new Pen(Color.Blue), new Rectangle(rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + StaticValues._ImageOffsetwidth + groupLevelIndentation, rowBounds.Bottom - StaticValues._2013OffsetHeight, StaticValues._groupImageSide, StaticValues._groupImageSide));
                //else
                //    graphics.DrawRectangle(new Pen(Color.Blue), new Rectangle(rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + StaticValues._ImageOffsetwidth + groupLevelIndentation, rowBounds.Bottom - StaticValues._defaultOffsetHeight, StaticValues._groupImageSide, StaticValues._groupImageSide));
            }
            else
            {
                base.Paint(graphics, clipBounds, rowBounds, rowIndex, rowState, isFirstDisplayedRow, isLastVisibleRow);
            }
        }

        /// <summary>
        /// Overrides the PaintCells : not executing if a group row.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipBounds"></param>
        /// <param name="rowBounds"></param>
        /// <param name="rowIndex"></param>
        /// <param name="rowState"></param>
        /// <param name="isFirstDisplayedRow"></param>
        /// <param name="isLastVisibleRow"></param>
        /// <param name="paintParts"></param>
        protected override void PaintCells(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, bool isFirstDisplayedRow, bool isLastVisibleRow, DataGridViewPaintParts paintParts)
        {
            if (!this.isGroupRow)
                base.PaintCells(graphics, clipBounds, rowBounds, rowIndex, rowState, isFirstDisplayedRow, isLastVisibleRow, paintParts);
        }

        public override string ToString()
        {
            string res = "";
            try
            {
                res += "OutlookGridRow ";
                foreach (DataGridViewCell c in this.Cells)
                {
                    if (c.Value != null)
                        res += c.Value.ToString() ?? string.Empty;
                }
            }
            catch { }
            return res;
        }

        #endregion

        #region "Public methods"

        /// <summary>
        /// Gets if the row has one parent that is collapsed
        /// </summary>
        /// <param name="gr">The group to look at.</param>
        /// <param name="i">Fill 0 to first this method (used for recursive).</param>
        /// <returns>True or false.</returns>
        public bool IsAParentCollapsed(IOutlookGridGroup gr, int i)
        {
            i++;
            if (gr.ParentGroup != null)
            {
                //if it is not the original group but it is one parent and if it is collapsed just stop here
                //no need to look further to the parents (one of the parents can be expanded...)
                //if (i > 1 && gr.Collapsed)
                if (gr.ParentGroup.Collapsed)
                    return true;
                else
                    return IsAParentCollapsed(gr.ParentGroup, i);
            }
            else
            {
                //if 1 that means there is no parent
                if (i == 1)
                    return false;
                else
                    return gr.Collapsed;
            }
        }


        public bool IsAParentNodeCollapsed(OutlookGridRow row, int i)
        {
            i++;
            //Console.WriteLine(row.ToString());
            if (row.ParentNode != null)
            {
                //if it is not the original group but it is one parent and if it is collapsed just stop here
                //no need to look further to the parents (one of the parents can be expanded...)
                if (row.ParentNode.Collapsed)
                    return true;
                else
                    return IsAParentNodeCollapsed(row.ParentNode, i);
            }
            else //no parent
            {
                if (i == 1) //if 1 that means there is no parent
                    return false;
                else //return the final parent collapsed state
                    return row.Collapsed;
            }
        }

        /// <summary>
        /// Expand the group the row belongs to.
        /// </summary>
        public void ExpandGroup()
        {
            SetGroupCollapse(false);
        }

        /// <summary>
        /// Collaspe the group the row belongs to.
        /// </summary>
        public void CollapseGroup()
        {
            SetGroupCollapse(true);
        }

        internal void SetGroupCollapse(bool collapsed)
        {
            if (this.IsGroupRow)
            {
                this.Group.Collapsed = collapsed;

                //this is a workaround to make the grid re-calculate it's contents and backgroun bounds
                // so the background is updated correctly.
                // this will also invalidate the control, so it will redraw itself
                this.Visible = false;
                this.Visible = true;

                //When collapsing the first row still seeing it.
                if (this.Index < this.DataGridView.FirstDisplayedScrollingRowIndex)
                    this.DataGridView.FirstDisplayedScrollingRowIndex = this.Index;
            }
        }

        internal void SetNodeCollapse(bool collapsed)
        {
            if (this.HasChildren)
            {
                this.Collapsed = collapsed;

                //this is a workaround to make the grid re-calculate it's contents and backgroun bounds
                // so the background is updated correctly.
                // this will also invalidate the control, so it will redraw itself
                this.Visible = false;
                this.Visible = true;

                //When collapsing the first row still seeing it.
                if (this.Index < this.DataGridView.FirstDisplayedScrollingRowIndex)
                    this.DataGridView.FirstDisplayedScrollingRowIndex = this.Index;
            }
        }



        #endregion

        #region "Private methods"

        /// <summary>
        /// this function checks if the user hit the expand (+) or collapse (-) icon.
        /// if it was hit it will return true
        /// </summary>
        /// <param name="e">mouse click event arguments</param>
        /// <returns>returns true if the icon was hit, false otherwise</returns>
        internal bool IsIconHit(DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0) return false;
            if (!this.isGroupRow) return false;

            KryptonOutlookGrid grid = (KryptonOutlookGrid)this.DataGridView;
            Rectangle rowBounds = grid.GetRowDisplayRectangle(this.Index, false);

            int rowHeadersWidth = grid.RowHeadersVisible ? grid.RowHeadersWidth : 0;
            int l = e.X + grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Left;
            if (this.isGroupRow &&
                (l >= rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 4 + group.Level * StaticValues._groupLevelMultiplier) &&
                (l <= rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 4 + group.Level * StaticValues._groupLevelMultiplier + 11) &&
                (e.Y >= rowBounds.Height - 18) &&
                (e.Y <= rowBounds.Height - 7))
                return true;

            return false;
        }

        //internal bool IsNodeIconHit(DataGridViewCellMouseEventArgs e)
        //{
        //    if (e.ColumnIndex < 0) return false;
        //    if (!this.HasChildren) return false;

        //    DataGridViewCell cell = this.Cells[e.ColumnIndex];
        //    if (cell.GetType() is KryptonDataGridViewTreeTextCell) {
        //    cell.
        //    }
        //    KryptonOutlookGrid grid = (KryptonOutlookGrid)this.DataGridView;


        //    Rectangle glyphRect = new Rectangle(rect.X + this.GlyphMargin, rect.ContentBounds.Y, INDENT_WIDTH, this.ContentBounds.Height - 1);

        //    if ((e.X <= glyphRect.X + 11) &&
        //        (e.X >= glyphRect.X) &&
        //       (e.Y >= glyphRect.Y + (glyphRect.Height / 2) - 4) &&
        //     (e.Y <= glyphRect.Y + (glyphRect.Height / 2) - 4 + 11))


        //    if (this.isGroupRow &&
        //        (l >= rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 4 + group.Level * StaticValues._groupLevelMultiplier) &&
        //        (l <= rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 4 + group.Level * StaticValues._groupLevelMultiplier + 11) &&
        //        (e.Y >= rowBounds.Height - 18) &&
        //        (e.Y <= rowBounds.Height - 7))
        //        return true;

        //    return false;
        //}

        internal bool IsGroupImageHit(DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0) return false;
            if (this.isGroupRow || this.group.GroupImage == null) return false;


            KryptonOutlookGrid grid = (KryptonOutlookGrid)this.DataGridView;
            Rectangle rowBounds = grid.GetRowDisplayRectangle(this.Index, false);

            int rowHeadersWidth = grid.RowHeadersVisible ? grid.RowHeadersWidth : 0;
            int l = e.X + grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Left;
            int offsetHeight;
            if (KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2013)
                offsetHeight = StaticValues._2013OffsetHeight;
            else
                offsetHeight = StaticValues._defaultOffsetHeight;
            if (this.isGroupRow &&
                (l >= rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 18 + group.Level * StaticValues._groupLevelMultiplier) &&
                (l <= rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 18 + group.Level * StaticValues._groupLevelMultiplier + 16) &&
                (e.Y >= rowBounds.Height - offsetHeight) &&
                (e.Y <= rowBounds.Height - 6))
                return true;

            return false;
        }

        #endregion

        public void Collapse()
        {
            ((KryptonOutlookGrid)this.DataGridView).CollapseNode(this);
            //if (!this.Collapsed)
            //{
            //    SetNodeCollapse(true);
            //}
        }


        public void Expand()
        {
            ((KryptonOutlookGrid)this.DataGridView).ExpandNode(this);
            //if (this.Collapsed)
            //{
            //    SetNodeCollapse(false);
            //}
        }
    }
}
