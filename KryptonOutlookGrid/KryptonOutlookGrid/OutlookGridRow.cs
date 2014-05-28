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
        private int Indent;

        #endregion

        #region "Properties"
        /// <summary>
        /// Gets or sets the indent factor of the row (when it is a Group row).
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PIndent
        {
            get
            {
                return this.Indent;
            }
            set
            {
                this.Indent = value;
            }
        }

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
        #endregion

        #region "Constructors"

        /// <summary>
        /// Default Constructor
        /// </summary>
        public OutlookGridRow()
            : this(null, false)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="group">The group the row is associated to.</param>
        public OutlookGridRow(IOutlookGridGroup group)
            : this(group, false)
        {
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
            //yes its readable ;)
            if ((IsGroupRow && IsAParentCollapsed(group, 0)) || (!IsGroupRow && group != null && (group.Collapsed || IsAParentCollapsed(group, 0))))
            {
                return base.GetState(rowIndex) & DataGridViewElementStates.Selected;
            }
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
                if (i > 1 && gr.Collapsed)
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

        internal bool IsGroupImageHit(DataGridViewCellMouseEventArgs e)
        {
            if (this.group.GroupImage == null) return false;
            if (e.ColumnIndex < 0) return false;

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
    }
}
