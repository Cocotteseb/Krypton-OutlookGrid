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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using ComponentFactory.Krypton.Toolkit;
using JDHSoftware.Krypton.Toolkit.Utils.Lang;
using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.CustomsColumns;
using System.Collections;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid
{
    /// <summary>
    /// Krypton DataGridView allowing nested grouping and unlimited sorting
    /// </summary>
    public partial class KryptonOutlookGrid : KryptonDataGridView
    {
        private KryptonOutlookGridGroupBox groupBox;
        //Krypton
        private IPalette _palette;
        private PaletteRedirect _paletteRedirect;
        private PaletteBackInheritRedirect _paletteBack;
        private PaletteBorderInheritRedirect _paletteBorder;
        //private PaletteContentInheritRedirect _paletteContent;
        private IDisposable _mementoBack;

        private OutlookGridGroupCollection groupCollection;     // List of Groups (of rows)
        private List<OutlookGridRow> internalRows;              // List of Rows in order to keep them as is (without grouping,...)
        private OutlookGridColumnCollection internalColumns;    // List of columns in order to know if sorted, Grouped, types,...
        private int _previousGroupRowSelected = -1; //Useful to allow the selection of a group row or not when on mouse down 

        //Krypton ContextMenu for the columns header
        private KryptonContextMenu KCtxMenu;
        private KryptonContextMenuItems _menuItems;
        private KryptonContextMenuItem _menuSortAscending;
        private KryptonContextMenuItem _menuSortDescending;
        private KryptonContextMenuItem _menuClearSorting;
        private KryptonContextMenuSeparator _menuSeparator1;
        private KryptonContextMenuItem _menuGroupByThisColumn;
        private KryptonContextMenuItem _menuUngroupByThisColumn;
        private KryptonContextMenuItem _menuShowGroupBox;
        private KryptonContextMenuItem _menuHideGroupBox;
        private KryptonContextMenuSeparator _menuSeparator2;
        private KryptonContextMenuItem _menuBestFitColumn;
        private KryptonContextMenuItem _menuBestFitAllColumns;
        private KryptonContextMenuSeparator _menuSeparator3;
        private KryptonContextMenuItem _menuVisibleColumns;
        private KryptonContextMenuItem _menuGroupInterval;
        private KryptonContextMenuItem _menuSortBySummary;
        private KryptonContextMenuItem _menuExpand;
        private KryptonContextMenuItem _menuCollapse;
        private KryptonContextMenuSeparator _menuSeparator4;
        private int colSelected = 1;         //for menu

        //For the Drag and drop of columns
        private Rectangle DragDropRectangle;
        private int DragDropSourceIndex;
        private int DragDropTargetIndex;
        private int DragDropCurrentIndex = -1;
        private int DragDropType; //0=column, 1=row

        private bool _hideColumnOnGrouping;

        //Nodes
        private bool _showLines;
        internal bool _inExpandCollapseMouseCapture = false;

        /// <summary>
        /// Group Image Click Event
        /// </summary>
        public event EventHandler<OutlookGridGroupImageEventArgs> GroupImageClick;
        /// <summary>
        /// Node expanding event
        /// </summary>
        public event EventHandler<ExpandingEventArgs> NodeExpanding;
        /// <summary>
        /// Node Expanded event
        /// </summary>
        public event EventHandler<ExpandedEventArgs> NodeExpanded;
        /// <summary>
        /// Node Collapsing Event
        /// </summary>
        public event EventHandler<CollapsingEventArgs> NodeCollapsing;
        /// <summary>
        /// Node Collapsed event
        /// </summary>
        public event EventHandler<CollapsedEventArgs> NodeCollapsed;

        #region OutlookGrid constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public KryptonOutlookGrid()
        {
            InitializeComponent();

            // very important, this indicates that a new default row class is going to be used to fill the grid
            // in this case our custom OutlookGridRow class
            base.RowTemplate = new OutlookGridRow();
            this.groupCollection = new OutlookGridGroupCollection(null);
            internalRows = new List<OutlookGridRow>();
            internalColumns = new OutlookGridColumnCollection();

            // Cache the current global palette setting
            _palette = KryptonManager.CurrentGlobalPalette;

            // Hook into palette events
            if (_palette != null)
                _palette.PalettePaint += new EventHandler<PaletteLayoutEventArgs>(OnPalettePaint);

            // (4) We want to be notified whenever the global palette changes
            KryptonManager.GlobalPaletteChanged += new EventHandler(OnGlobalPaletteChanged);

            // Create redirection object to the base palette
            _paletteRedirect = new PaletteRedirect(_palette);

            // Create accessor objects for the back, border and content
            _paletteBack = new PaletteBackInheritRedirect(_paletteRedirect);
            _paletteBorder = new PaletteBorderInheritRedirect(_paletteRedirect);
            //_paletteContent = new PaletteContentInheritRedirect(_paletteRedirect);

            this.AllowUserToOrderColumns = false;  //we will handle it ourselves
            _hideColumnOnGrouping = false;
        }

        /// <summary>
        /// Definitvely removes flickering - may not work on some systems/can cause higher CPU usage.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }


        #endregion OutlookGrid constructor

        #region OutlookGrid Properties

        /// <summary>
        /// Gets the RowTemplate of the grid.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewRow RowTemplate
        {
            get
            {
                return base.RowTemplate;
            }
        }

        /// <summary>
        /// Gets if the grid is grouped
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsGridGrouped
        {
            get
            {
                return !(groupCollection.Count == 0);
            }
        }

        /// <summary>
        /// Gets or sets the OutlookGridGroupBox
        /// </summary>
        [Category("Behavior")]
        [Description("Associate the OutlookGridGroupBox with the grid.")]
        [DefaultValue(null)]
        public KryptonOutlookGridGroupBox GroupBox
        {
            get
            {
                return groupBox;
            }
            set
            {
                groupBox = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of rows in the grid (without grouping,... for having a copy)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OutlookGridRow> InternalRows
        {
            get { return internalRows; }
            set { internalRows = value; }
        }

        /// <summary>
        /// Gets or sets the previous selected group row
        /// </summary>
        [Browsable(false)]
        public int PreviousSelectedGroupRow
        {
            get
            {
                return _previousGroupRowSelected;
            }

            set
            {
                _previousGroupRowSelected = value;
            }
        }

        /// <summary>
        /// Gets the Krypton Palette of the OutlookGrid
        /// </summary>
        [Browsable(false)]
        public IPalette GridPalette
        { get { return _palette; } }

        /// <summary>
        /// Gets or sets the group collection.
        /// </summary>
        /// <value>OutlookGridGroupCollection.</value>
        [Browsable(false)]
        public OutlookGridGroupCollection GroupCollection
        {
            get
            {
                return this.groupCollection;
            }
            set
            {
                this.groupCollection = value;
            }
        }

        /// <summary>
        /// Gets or sets the HideColumnOnGrouping property.
        /// </summary>
        /// <value>True if the column should be hidden when it is grouped, false otherwise.</value>
        [Category("Behavior")]
        [Description("Hide the column when it is grouped.")]
        [DefaultValue(false)]
        public bool HideColumnOnGrouping
        {
            get
            {
                return _hideColumnOnGrouping;
            }
            set
            {
                this._hideColumnOnGrouping = value;
            }
        }

        [DefaultValue(true)]
        public bool ShowLines
        {
            get { return this._showLines; }
            set
            {
                if (value != this._showLines)
                {
                    this._showLines = value;
                    this.Invalidate();
                }
            }
        }

        #endregion OutlookGrid property definitions

        #region OutlookGrid Overrides

        /// <summary>
        /// Disposing ressources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_mementoBack != null)
                {
                    _mementoBack.Dispose();
                    _mementoBack = null;
                }

                // (10) Unhook from the palette events
                if (_palette != null)
                {
                    _palette.PalettePaint -= new EventHandler<PaletteLayoutEventArgs>(OnPalettePaint);
                    _palette = null;
                }

                // (11) Unhook from the static events, otherwise we cannot be garbage collected
                KryptonManager.GlobalPaletteChanged -= new EventHandler(OnGlobalPaletteChanged);

                //Unhook from specific events 
                if (groupBox != null)
                {
                    groupBox.ColumnGroupAdded -= ColumnGroupAddedEvent;
                    groupBox.ColumnSortChanged -= ColumnSortChangedEvent;
                    groupBox.ColumnGroupRemoved -= ColumnGroupRemovedEvent;
                    groupBox.ClearGrouping -= ClearGroupingEvent;
                    groupBox.FullCollapse -= FullCollapseEvent;
                    groupBox.FullExpand -= FullExpandEvent;
                    groupBox.ColumnGroupOrderChanged -= ColumnGroupIndexChangedEvent;
                    groupBox.GroupExpand -= GridGroupExpandEvent;
                    groupBox.GroupCollapse -= GridGroupCollapseEvent;
                    groupBox.GroupIntervalClick -= GroupIntervalClickEvent;
                    groupBox.SortBySummaryCount -= SortBySummaryCountEvent;
                }
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Overrides OnCellBeginEdit
        /// </summary>
        /// <param name="e">DataGridViewCellCancelEventArgs</param>
        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            OutlookGridRow row = (OutlookGridRow)base.Rows[e.RowIndex];
            if (row.IsGroupRow)
                e.Cancel = true;
            else
                base.OnCellBeginEdit(e);
        }

        /// <summary>
        /// Overrides OnCellDoubleClick (expand/collapse group rows)
        /// </summary>
        /// <param name="e">DataGridViewCellEventArgs</param>
        protected override void OnCellDoubleClick(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                OutlookGridRow row = (OutlookGridRow)base.Rows[e.RowIndex];
                if (row.IsGroupRow)
                {
                    row.Group.Collapsed = !row.Group.Collapsed;

                    //this is a workaround to make the grid re-calculate it's contents and background bounds
                    // so the background is updated correctly.
                    // this will also invalidate the control, so it will redraw itself
                    row.Visible = false;
                    row.Visible = true;
                    return;
                }
            }
            base.OnCellDoubleClick(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // used to keep extra mouse moves from selecting more rows when collapsing
            base.OnMouseUp(e);
            this._inExpandCollapseMouseCapture = false;
        }

        /// <summary>
        /// Overrides OnMouseDown
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //stores values for drag/drop operations if necessary
            if (this.AllowDrop)
            {
                if (this.HitTest(e.X, e.Y).ColumnIndex == -1 && this.HitTest(e.X, e.Y).RowIndex > -1)
                {
                    //if this is a row header cell
                    if (this.Rows[this.HitTest(e.X, e.Y).RowIndex].Selected)
                    {
                        //if this row is selected
                        DragDropType = 1;
                        Size DragSize = SystemInformation.DragSize;
                        DragDropRectangle = new Rectangle(new Point(e.X - (DragSize.Width / 2), e.Y - (DragSize.Height / 2)), DragSize);
                        DragDropSourceIndex = this.HitTest(e.X, e.Y).RowIndex;
                    }
                    else
                    {
                        DragDropRectangle = Rectangle.Empty;
                    }
                }
                else if (this.HitTest(e.X, e.Y).ColumnIndex > -1 && this.HitTest(e.X, e.Y).RowIndex == -1)
                {
                    //if this is a column header cell
                    //if (this.Columns[this.HitTest(e.X, e.Y).ColumnIndex].Selected)
                    //{
                    DragDropType = 0;
                    DragDropSourceIndex = this.HitTest(e.X, e.Y).ColumnIndex;
                    Size DragSize = SystemInformation.DragSize;
                    DragDropRectangle = new Rectangle(new Point(e.X - (DragSize.Width / 2), e.Y - (DragSize.Height / 2)), DragSize);
                    //}
                    //else
                    //{
                    //    DragDropRectangle = Rectangle.Empty;
                    //} //end if
                }
                else
                {
                    DragDropRectangle = Rectangle.Empty;
                }
            }
            else
            {
                DragDropRectangle = Rectangle.Empty;
            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Overrides OnMouseDown
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // while we are expanding and collapsing a node mouse moves are
            // supressed to keep selections from being messed up.
            if (!this._inExpandCollapseMouseCapture)
            {
                bool dragdropdone = false;
                //handles drag/drop operations
                if (this.AllowDrop)
                {
                    if ((e.Button & MouseButtons.Left) == MouseButtons.Left && Cursor.Current != Cursors.SizeWE)
                    {
                        if (DragDropRectangle != Rectangle.Empty && !DragDropRectangle.Contains(e.X, e.Y))
                        {
                            if (DragDropType == 0)
                            {
                                OutlookGridColumn col = internalColumns.FindFromColumnIndex(DragDropSourceIndex);
                                string groupInterval = "";
                                string groupType = "";
                                string groupSortBySummaryCount = "";

                                if (col.GroupingType != null)
                                {
                                    groupType = col.GroupingType.GetType().Name.ToString();
                                    if (groupType == typeof(OutlookGridDateTimeGroup).Name)
                                        groupInterval = ((OutlookGridDateTimeGroup)col.GroupingType).Interval.ToString();
                                    groupSortBySummaryCount = CommonHelper.BoolToString(col.GroupingType.SortBySummaryCount);
                                }
                                //column drag/drop
                                string info = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", col.Name.ToString(), col.DataGridViewColumn.HeaderText.ToString(), col.DataGridViewColumn.HeaderCell.SortGlyphDirection.ToString(), col.DataGridViewColumn.SortMode.ToString(), groupType, groupInterval, groupSortBySummaryCount);
                                DragDropEffects DropEffect = this.DoDragDrop(info, DragDropEffects.Move);
                                dragdropdone = true;
                            }
                            else if (DragDropType == 1)
                            {
                                //row drag/drop
                                DragDropEffects DropEffect = this.DoDragDrop(this.Rows[DragDropSourceIndex], DragDropEffects.Move);
                                dragdropdone = true;
                            }
                        }
                    }
                }
                base.OnMouseMove(e);
                if (dragdropdone)
                    this.CellOver = new Point(-2, -2);//To avoid that the column header appears in a pressed state - Modification of ToolKit
            }
        }

        /// <summary>
        /// Overrides OnDragLeave
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnDragLeave(EventArgs e)
        {
            if (DragDropCurrentIndex > -1 && DragDropType == 0)
            {
                DataGridViewColumn col = this.Columns[DragDropCurrentIndex];
                if (this.groupBox != null && groupBox.Contains(col.Name))
                {
                    DragDropCurrentIndex = -1;
                    //this.InvalidateColumn(col.Index);
                    this.Invalidate();
                }
                else
                {
                    DragDropCurrentIndex = -1;
                }
            }

            base.OnDragLeave(e);
        }

        /// <summary>
        /// Overrides OnDragOver
        /// </summary>
        /// <param name="drgevent">DragEventArgs</param>
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            //runs while the drag/drop is in progress
            if (this.AllowDrop)
            {
                drgevent.Effect = DragDropEffects.Move;
                if (DragDropType == 0)
                {
                    //column drag/drop
                    int CurCol = this.HitTest(this.PointToClient(new Point(drgevent.X, drgevent.Y)).X, this.PointToClient(new Point(drgevent.X, drgevent.Y)).Y).ColumnIndex;
                    if (DragDropCurrentIndex != CurCol)
                    {
                        DragDropCurrentIndex = CurCol;
                        this.Invalidate(); //repaint
                    }
                }
                else if (DragDropType == 1)
                {
                    //row drag/drop
                    int CurRow = this.HitTest(this.PointToClient(new Point(drgevent.X, drgevent.Y)).X, this.PointToClient(new Point(drgevent.X, drgevent.Y)).Y).RowIndex;
                    if (DragDropCurrentIndex != CurRow)
                    {
                        DragDropCurrentIndex = CurRow;
                        this.Invalidate(); //repaint
                    }
                }
            }
            base.OnDragOver(drgevent);
        }

        /// <summary>
        /// Overrides OnDragDrop
        /// </summary>
        /// <param name="drgevent">DragEventArgs</param>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            //runs after a drag/drop operation for column/row has completed
            if (this.AllowDrop)
            {
                if (drgevent.Effect == DragDropEffects.Move)
                {
                    Point ClientPoint = this.PointToClient(new Point(drgevent.X, drgevent.Y));
                    if (DragDropType == 0)
                    {
                        //if this is a column drag/drop operation
                        DragDropTargetIndex = this.HitTest(ClientPoint.X, ClientPoint.Y).ColumnIndex;
                        if (DragDropTargetIndex > -1 && DragDropCurrentIndex < this.ColumnCount - 1)
                        {
                            DragDropCurrentIndex = -1;
                            //*************************************************
                            //'SourceColumn' is null after the line of code
                            //below executes... Why? This works fine for rows!!
                            string r = drgevent.Data.GetData(typeof(string)) as string;
                            string[] res = r.Split('|');
                            DataGridViewColumn SourceColumn = this.Columns[res[0]];
                            int SourceDisplayIndex = SourceColumn.DisplayIndex;
                            DataGridViewColumn TargetColumn = this.Columns[DragDropTargetIndex];
                            int TargetDisplayIndex = TargetColumn.DisplayIndex;
                            SourceColumn.DisplayIndex = TargetDisplayIndex;
                            TargetColumn.DisplayIndex = SourceDisplayIndex;
                            //*************************************************
                            //this.Columns.RemoveAt(DragDropSourceIndex);
                            //this.Columns.Insert(DragDropTargetIndex, SourceColumn);

                            SourceColumn.Selected = false;
                            TargetColumn.Selected = false;
                            //this.Columns[DragDropTargetIndex].Selected = true;
                            this.CurrentCell = this[DragDropTargetIndex, 0];
                        } //end if
                    }
                    else if (DragDropType == 1)
                    {
                        //if this is a row drag/drop operation
                        DragDropTargetIndex = this.HitTest(ClientPoint.X, ClientPoint.Y).RowIndex;
                        if (DragDropTargetIndex > -1 && DragDropCurrentIndex < this.RowCount - 1)
                        {
                            DragDropCurrentIndex = -1;
                            DataGridViewRow SourceRow = drgevent.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                            this.Rows.RemoveAt(DragDropSourceIndex);
                            this.Rows.Insert(DragDropTargetIndex, SourceRow);
                            this.Rows[DragDropTargetIndex].Selected = true;
                            this.CurrentCell = this[0, DragDropTargetIndex];
                        }
                    }
                }
            }
            DragDropCurrentIndex = -1;
            this.Invalidate();
            base.OnDragDrop(drgevent);
        }

        /// <summary>
        /// Overrides OnCellPainting - Drawing a line for drag and drop
        /// </summary>
        /// <param name="e">DataGridViewCellPaintingEventArgs</param>
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            //draws red drag/drop target indicator lines if necessary
            if (DragDropCurrentIndex > -1)
            {
                if (DragDropType == 0)
                {
                    //column drag/drop
                    if (e.ColumnIndex == DragDropCurrentIndex)// && DragDropCurrentIndex < this.ColumnCount)
                    {
                        //if this cell is in the same column as the mouse cursor
                        using (Pen p = new Pen(Color.Red, 1))
                        {
                            e.Graphics.DrawLine(p, e.CellBounds.Left - 1, e.CellBounds.Top, e.CellBounds.Left - 1, e.CellBounds.Bottom);
                        }
                    } //end if
                }
                else if (DragDropType == 1)
                {
                    //row drag/drop
                    if (e.RowIndex == DragDropCurrentIndex && DragDropCurrentIndex < this.RowCount - 1)
                    {
                        //if this cell is in the same row as the mouse cursor

                        using (Pen p = new Pen(Color.Red, 1))
                        {
                            e.Graphics.DrawLine(p, e.CellBounds.Left, e.CellBounds.Top - 1, e.CellBounds.Right, e.CellBounds.Top - 1);
                        }
                    }
                }
            }
            base.OnCellPainting(e);
        }

        /// <summary>
        /// Overrides OnCellMouseEnter
        /// </summary>
        /// <param name="e">DataGridViewCellEventArgs</param>
        protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
        {
            base.OnCellMouseEnter(e);
        }

        /// <summary>
        /// Overrides OnCellMouseDown - Check if the user has clicked on +/- of a group row
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            //base.OnCellMouseDown(e); //needed.
            if (e.RowIndex < 0)
            {
                base.OnCellMouseDown(e); // To allow column resizing
                return;
            }
            OutlookGridRow row = (OutlookGridRow)base.Rows[e.RowIndex];
            //System.Diagnostics.Debug.WriteLine("OnCellMouseDown " + DateTime.Now.Ticks.ToString() + "IsIconHit" + row.IsIconHit(e).ToString());
            if (_previousGroupRowSelected != -1 && _previousGroupRowSelected != e.RowIndex)
                InvalidateRow(PreviousSelectedGroupRow);

            PreviousSelectedGroupRow = -1;
            if (row.IsGroupRow)
            {
                PreviousSelectedGroupRow = e.RowIndex;
                this.ClearSelection(); //unselect
                if (row.IsIconHit(e))
                {
                    row.Group.Collapsed = !row.Group.Collapsed;

                    //this is a workaround to make the grid re-calculate it's contents and backgroun bounds
                    // so the background is updated correctly.
                    // this will also invalidate the control, so it will redraw itself
                    row.Visible = false;
                    row.Visible = true;
                    //When collapsing the first row still seeing it.
                    if (row.Index < this.FirstDisplayedScrollingRowIndex)
                        this.FirstDisplayedScrollingRowIndex = row.Index;
                }
                else if (row.IsGroupImageHit(e))
                {
                    OnGroupImageClick(new OutlookGridGroupImageEventArgs(row));
                }
                else
                {
                    InvalidateRow(e.RowIndex);
                }
            }
            else
            {
                base.OnCellMouseDown(e);
            }
        }

        /// <summary>
        /// Overrides OnColumnHeaderMouseClick
        /// </summary>
        /// <param name="e">DataGridViewCellMouseEventArgs</param>
        protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
        {
            colSelected = e.ColumnIndex; //To keep a track on which column we have pressed
            //runs when the mouse is clicked over a column header cell
            if (e.ColumnIndex > -1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    ShowColumnHeaderContextMenu(e.ColumnIndex);
                }
                else if (e.Button == MouseButtons.Left)
                {
                    OutlookGridColumn col = internalColumns.FindFromColumnIndex(e.ColumnIndex);
                    if (col.DataGridViewColumn.SortMode != DataGridViewColumnSortMode.NotSortable)
                    {
                        SortOrder previousSort = col.SortDirection;
                        //Reset all sorting column only if not Ctrl or Shift or the column is grouped
                        if (Control.ModifierKeys != Keys.Shift && Control.ModifierKeys != Keys.Control && !col.IsGrouped)
                        {
                            ResetAllSortingColumns();
                        }

                        //Remove this SortIndex
                        if (Control.ModifierKeys == Keys.Control)
                        {
                            UnSortColum(col);
                        }
                        //Add the first or a new SortIndex
                        else
                        {
                            if (previousSort == SortOrder.None)
                            {
                                SortColumn(col, SortOrder.Ascending);
                            }
                            else
                            {
                                SortColumn(col, (previousSort == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending);
                            }
                        }

#if DEBUG
                        internalColumns.DebugOutput();
#endif

                        //Refresh the groupBox if the column is grouped
                        if (col.IsGrouped)
                        {
                            ForceRefreshGroupBox();
                        }

                        //Apply the changes
                        Fill();
                    }
                }
            }
            base.OnColumnHeaderMouseClick(e);
        }

        /// <summary>
        /// Overrides OnCellFormatting
        /// </summary>
        /// <param name="e">DataGridViewCellFormattingEventArgs event args.</param>
        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            //Allows to have a picture in the first column
            if (e.DesiredType.Name == "Image" && e.Value != null && e.Value.GetType().Name != e.DesiredType.Name && e.Value.GetType().Name != "Bitmap")
            {
                e.Value = null;
            }

            base.OnCellFormatting(e);
        }

        #endregion

        #region OutlookGrid Events

        /// <summary>
        /// Redraw on OnPalettePaint
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPalettePaint(object sender, PaletteLayoutEventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Update palettes on OnGlobalPaletteChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGlobalPaletteChanged(object sender, EventArgs e)
        {
            // (5) Unhook events from old palette
            if (_palette != null)
                _palette.PalettePaint -= new EventHandler<PaletteLayoutEventArgs>(OnPalettePaint);

            // (6) Cache the new IPalette that is the global palette
            _palette = KryptonManager.CurrentGlobalPalette;
            _paletteRedirect.Target = _palette; //!!!!!! important

            //Reflect changes for the grouped heights
            int h = StaticValues._defaultGroupRowHeight; // default height
            if (KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2013)
                h = StaticValues._2013GroupRowHeight; // special height for office 2013         
            
            //For each outlookgridcolumn
            for (int j = 0; j < internalColumns.Count; j++)
            {
                if (internalColumns[j].GroupingType != null)
                    internalColumns[j].GroupingType.Height = h;
            }

            // (7) Hook into events for the new palette
            if (_palette != null)
                _palette.PalettePaint += new EventHandler<PaletteLayoutEventArgs>(OnPalettePaint);

            // (8) Change of palette means we should repaint to show any changes
            Invalidate();
        }

        /// <summary>
        /// Clear sorting for the column selected by the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnColumnClearSorting(object sender, EventArgs e)
        {
            if (colSelected > -1)
            {
                OutlookGridColumn col = internalColumns.FindFromColumnIndex(colSelected);
                UnSortColum(col);
                Fill();
            }
        }

        /// <summary>
        /// Ascending sort for the column selected by the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnColumnSortAscending(object sender, EventArgs e)
        {
            if (colSelected > -1)
            {
                OutlookGridColumn col = internalColumns.FindFromColumnIndex(colSelected);
                SortColumn(col, SortOrder.Ascending);
                if (col.IsGrouped)
                {
                    ForceRefreshGroupBox();
                }
                Fill();
            }
        }

        /// <summary>
        /// Descending sort for the column selected by the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnColumnSortDescending(object sender, EventArgs e)
        {
            if (colSelected > -1)
            {
                OutlookGridColumn col = internalColumns.FindFromColumnIndex(colSelected);
                SortColumn(col, SortOrder.Descending);
                if (col.IsGrouped)
                {
                    ForceRefreshGroupBox();
                }
                Fill();
            }
        }

        /// <summary>
        /// Grouping for the column selected by the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGroupByThisColumn(object sender, EventArgs e)
        {
            if (colSelected > -1)
            {
                OutlookGridColumn col = internalColumns.FindFromColumnIndex(colSelected);
                GroupColumn(col, SortOrder.Ascending, null);
                ForceRefreshGroupBox();
                Fill();
            }
        }

        /// <summary>
        /// Ungrouping for the column selected by the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnGroupByThisColumn(object sender, EventArgs e)
        {
            if (colSelected > -1)
            {
                OutlookGridColumn col = internalColumns.FindFromColumnIndex(colSelected);
                UnGroupColumn(col.Name);
                ForceRefreshGroupBox();
                Fill();
            }
        }

        private void OnGroupCollapse(object sender, EventArgs e)
        {
            OutlookGridColumn col = internalColumns.FindFromColumnIndex(colSelected);
            Collapse(col.Name);
        }

        private void OnGroupExpand(object sender, EventArgs e)
        {
            OutlookGridColumn col = internalColumns.FindFromColumnIndex(colSelected);
            Expand(col.Name);
        }

        private void OnSortBySummary(object sender, EventArgs e)
        {

            KryptonContextMenuItem item = (KryptonContextMenuItem)sender;
            OutlookGridColumn col = internalColumns.FindFromColumnIndex(colSelected);
            col.GroupingType.SortBySummaryCount = item.Checked;
            ForceRefreshGroupBox();
            Fill();
        }

        private void OnGroupIntervalClick(object sender, EventArgs e)
        {
            KryptonContextMenuItem item = (KryptonContextMenuItem)sender;
            OutlookGridColumn col = internalColumns.FindFromColumnIndex(colSelected);
            ((OutlookGridDateTimeGroup)col.GroupingType).Interval = (OutlookGridDateTimeGroup.DateInterval)Enum.Parse(typeof(OutlookGridDateTimeGroup.DateInterval), item.Tag.ToString());
            ForceRefreshGroupBox();
            Fill();
        }

        private void OnColumnVisibleCheckedChanged(object sender, EventArgs e)
        {
            KryptonContextMenuCheckBox item = (KryptonContextMenuCheckBox)sender;
            this.Columns[(int)item.Tag].Visible = item.Checked;
        }

        /// <summary>
        /// Shows the groupbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShowGroupBox(object sender, EventArgs e)
        {
            if (groupBox != null)
                groupBox.Show();

        }

        /// <summary>
        /// Hide the groupbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHideGroupBox(object sender, EventArgs e)
        {
            if (groupBox != null)
                groupBox.Hide();
        }

        /// <summary>
        /// Resizes the selected column by the menu to best fit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBestFitColumn(object sender, EventArgs e)
        {
            if (colSelected > -1)
            {
                Cursor.Current = Cursors.WaitCursor;
                this.AutoResizeColumn(colSelected, DataGridViewAutoSizeColumnMode.AllCells);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Resizes all columns to best fit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBestFitAllColumns(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Handles the ColumnSortChangedEvent event. Update the header (glyph) and fill the grid too.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A OutlookGridColumnEventArgs that contains the event data.</param>
        private void ColumnSortChangedEvent(object sender, OutlookGridColumnEventArgs e)
        {
#if (DEBUG)
            Console.WriteLine("OutlookGrid - Receives ColumnSortChangedEvent : " + e.Column.Name + " " + e.Column.SortDirection.ToString());
#endif
            internalColumns[e.Column.Name].SortDirection = e.Column.SortDirection;
            internalColumns[e.Column.Name].DataGridViewColumn.HeaderCell.SortGlyphDirection = e.Column.SortDirection;
            Fill();
        }

        /// <summary>
        /// Handles the ColumnGroupAddedEvent event. Fill the grid too.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A OutlookGridColumnEventArgs that contains the event data.</param>
        private void ColumnGroupAddedEvent(object sender, OutlookGridColumnEventArgs e)
        {
            GroupColumn(e.Column.Name, e.Column.SortDirection, null);
            //We fill again the grid with the new Grouping info
            Fill();
#if (DEBUG)
            Console.WriteLine("OutlookGrid - Receives ColumnGroupAddedEvent : " + e.Column.Name);
#endif
        }

        /// <summary>
        /// Handles the ColumnGroupRemovedEvent event. Fill the grid too.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A OutlookGridColumnEventArgs that contains the event data.</param>
        private void ColumnGroupRemovedEvent(object sender, OutlookGridColumnEventArgs e)
        {
            UnGroupColumn(e.Column.Name);
            //We fill again the grid with the new Grouping info
            Fill();
#if (DEBUG)
            Console.WriteLine("OutlookGrid - Receives ColumnGroupRemovedEvent : " + e.Column.Name);
#endif
        }

        /// <summary>
        /// Handles the ClearGroupingEvent event. Fill the grid too.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        private void ClearGroupingEvent(object sender, EventArgs e)
        {
            ClearGroups();
#if (DEBUG)
            Console.WriteLine("OutlookGrid - Receives ClearGroupingEvent");
#endif
        }

        /// <summary>
        /// Handles the FullCollapseEvent event.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        private void FullCollapseEvent(object sender, EventArgs e)
        {
            CollapseAll();
#if (DEBUG)
            Console.WriteLine("OutlookGrid - Receives FullCollapseEvent");
#endif
        }

        /// <summary>
        /// Handles the FullExpandEvent event.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        private void FullExpandEvent(object sender, EventArgs e)
        {
            ExpandAll();
#if (DEBUG)
            Console.WriteLine("OutlookGrid - Receives FullExpandEvent");
#endif
        }

        /// <summary>
        /// Handles the GroupExpandEvent event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridGroupExpandEvent(object sender, OutlookGridColumnEventArgs e)
        {
            Expand(e.Column.Name);
#if (DEBUG)
            Console.WriteLine("OutlookGrid - Receives GridGroupExpandEvent");
#endif
        }

        private void GridGroupCollapseEvent(object sender, OutlookGridColumnEventArgs e)
        {
            Collapse(e.Column.Name);
#if (DEBUG)
            Console.WriteLine("OutlookGrid - Receives GridGroupCollapseEvent");
#endif
        }

        private void ColumnGroupIndexChangedEvent(object sender, OutlookGridColumnEventArgs e)
        {
            //TODO 25/01/2014
            internalColumns.ChangeGroupIndex(e.Column);
            Fill(); //to reflect the changes
            ForceRefreshGroupBox();
#if (DEBUG)
            Console.WriteLine("OutlookGrid - Receives ColumnGroupIndexChangedEvent");
#endif
        }

        private void GroupIntervalClickEvent(object sender, OutlookGridColumnEventArgs e)
        {
            OutlookGridColumn col = internalColumns.FindFromColumnName(e.Column.Name);
            ((OutlookGridDateTimeGroup)col.GroupingType).Interval = ((OutlookGridDateTimeGroup)e.Column.GroupingType).Interval;
            Fill();
#if (DEBUG)
            Console.WriteLine("OutlookGrid - Receives GroupIntervalClickEvent");
#endif
        }

        private void SortBySummaryCountEvent(object sender, OutlookGridColumnEventArgs e)
        {
            OutlookGridColumn col = internalColumns.FindFromColumnName(e.Column.Name);
            col.GroupingType.SortBySummaryCount = e.Column.GroupingType.SortBySummaryCount;
            Fill();
#if (DEBUG)
            Console.WriteLine("OutlookGrid - Receives SortBySummaryCountEvent");
#endif
        }

        /// <summary>
        /// Raises the GroupImageClick event.
        /// </summary>
        /// <param name="e">A OutlookGridGroupImageEventArgs that contains the event data.</param>
        protected virtual void OnGroupImageClick(OutlookGridGroupImageEventArgs e)
        {
            if (GroupImageClick != null)
                GroupImageClick(this, e);
        }

        /// <summary>
        /// Raises the NodeExpanding event.
        /// </summary>
        /// <param name="e">A ExpandingEventArgs that contains the event data.</param>
        protected virtual void OnNodeExpanding(ExpandingEventArgs e)
        {
            if (this.NodeExpanding != null)
            {
                NodeExpanding(this, e);
            }
        }

        /// <summary>
        /// Raises the NodeExpanded event.
        /// </summary>
        /// <param name="e">A ExpandedEventArgs that contains the event data.</param>
        protected virtual void OnNodeExpanded(ExpandedEventArgs e)
        {
            if (this.NodeExpanded != null)
            {
                NodeExpanded(this, e);
            }
        }

        /// <summary>
        /// Raises the NodeCollapsing event.
        /// </summary>
        /// <param name="e">A CollapsingEventArgs that contains the event data.</param>
        protected virtual void OnNodeCollapsing(CollapsingEventArgs e)
        {
            if (this.NodeCollapsing != null)
            {
                NodeCollapsing(this, e);
            }

        }

        /// <summary>
        /// Raises the NodeCollapsed event.
        /// </summary>
        /// <param name="e">A CollapsedEventArgs that contains the event data.</param>
        protected virtual void OnNodeCollapsed(CollapsedEventArgs e)
        {
            if (this.NodeCollapsed != null)
            {
                NodeCollapsed(this, e);
            }
        }

        #endregion

        #region OutlookGrid methods

        /// <summary>
        /// Add a column for internal uses of the OutlookGrid. The column must already exists in the datagridview. Do this *BEFORE* using the grid (sorting and grouping, filling,...)
        /// </summary>
        /// <param name="col">The DataGridViewColumn.</param>
        /// <param name="group">The group type for the column.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <param name="groupIndex">The column's position in grouping and at which level.</param>
        /// <param name="sortIndex">the column's position among sorted columns.</param>
        public void AddInternalColumn(DataGridViewColumn col, IOutlookGridGroup group, SortOrder sortDirection, int groupIndex, int sortIndex)
        {
            AddInternalColumn(new OutlookGridColumn(col, group, sortDirection, groupIndex, sortIndex));
            //internalColumns.Add(new OutlookGridColumn(col, group, sortDirection, groupIndex, sortIndex));
            ////Already reflect the SortOrder on the column
            //col.HeaderCell.SortGlyphDirection = sortDirection;
            //if (this._hideColumnOnGrouping && groupIndex > -1 && group.AllowHiddenWhenGrouped)
            //    col.Visible = false;
        }

        /// <summary>
        /// Add a column for internal uses of the OutlookGrid. The column must already exists in the datagridview. Do this *BEFORE* using the grid (sorting and grouping, filling,...)
        /// </summary>
        /// <param name="col">The configured OutlookGridColumn.</param>
        public void AddInternalColumn(OutlookGridColumn col)
        {
            Debug.Assert(col != null);
            if (col != null)
            {
                internalColumns.Add(col);
                //Already reflect the SortOrder on the column
                col.DataGridViewColumn.HeaderCell.SortGlyphDirection = col.SortDirection;
                if (this._hideColumnOnGrouping && col.GroupIndex > -1 && col.GroupingType.AllowHiddenWhenGrouped)
                    col.DataGridViewColumn.Visible = false;
            }
        }

        /// <summary>
        /// Add an array of OutlookGridColumns for internal use of OutlookGrid. The columns must already exist in the datagridview. Do this *BEFORE* using the grid (sorting and grouping, filling,...)
        /// </summary>
        /// <param name="cols"></param>
        public void AddRangeInternalColumns(params OutlookGridColumn[] cols)
        {
            Debug.Assert(cols != null);
            // All columns with DisplayIndex != -1 are put into the initialColumns array
            foreach (OutlookGridColumn col in cols)
            {
                AddInternalColumn(col);
            }
        }

        /// <summary>
        /// Group a column
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="sortDirection">The sort direction of the group./</param>
        /// <param name="gr">The IOutlookGridGroup object.</param>
        public void GroupColumn(string columnName, SortOrder sortDirection, IOutlookGridGroup gr)
        {
            GroupColumn(internalColumns[columnName], sortDirection, gr);
        }

        /// <summary>
        /// Group a column
        /// </summary>
        /// <param name="col">The name of the column.</param>
        /// <param name="sortDirection">The sort direction of the group./</param>
        /// <param name="gr">The IOutlookGridGroup object.</param>
        public void GroupColumn(OutlookGridColumn col, SortOrder sortDirection, IOutlookGridGroup gr)
        {
            if (!col.IsGrouped)
            {
                col.GroupIndex = ++internalColumns.MaxGroupIndex;
                if (col.SortIndex > -1)
                    internalColumns.RemoveSortIndex(col);
                col.SortDirection = sortDirection;
                col.DataGridViewColumn.HeaderCell.SortGlyphDirection = sortDirection;
                if (gr != null)
                    col.GroupingType = gr;
                if (_hideColumnOnGrouping && col.GroupingType.AllowHiddenWhenGrouped)
                    col.DataGridViewColumn.Visible = false;
            }
#if DEBUG
            internalColumns.DebugOutput();
#endif
        }

        /// <summary>
        /// Ungroup a column
        /// </summary>
        /// <param name="columnName">The OutlookGridColumn.</param>
        public void UnGroupColumn(string columnName)
        {
            UnGroupColumn(internalColumns[columnName]);
        }

        /// <summary>
        /// Ungroup a column
        /// </summary>
        /// <param name="col">The OutlookGridColumn.</param>
        public void UnGroupColumn(OutlookGridColumn col)
        {
            if (col.IsGrouped)
            {
                internalColumns.RemoveGroupIndex(col);
                col.SortDirection = SortOrder.None;
                col.DataGridViewColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                col.GroupingType.Collapsed = false;
                if (_hideColumnOnGrouping && col.GroupingType.AllowHiddenWhenGrouped)
                    col.DataGridViewColumn.Visible = true;
            }
#if DEBUG
            internalColumns.DebugOutput();
#endif
        }

        /// <summary>
        /// Sort the column. Call Fill after to make the changes
        /// </summary>
        /// <param name="col">The outlookGridColumn</param>
        /// <param name="sort">The new SortOrder.</param>
        public void SortColumn(OutlookGridColumn col, SortOrder sort)
        {
            //Change the SortIndex and MaxSortIndex only if it is not a grouped column
            if (!col.IsGrouped && col.SortIndex == -1)
                col.SortIndex = ++internalColumns.MaxSortIndex;
            //Change the order in all cases
            col.SortDirection = sort;
            col.DataGridViewColumn.HeaderCell.SortGlyphDirection = sort;
#if DEBUG
            internalColumns.DebugOutput();
#endif
        }

        /// <summary>
        /// UnSort the column. Call Fill after to make the changes
        /// </summary>
        /// <param name="col">The outlookGridColumn.</param>
        public void UnSortColum(OutlookGridColumn col)
        {
            //Remove the SortIndex and rearrange the SortIndexes only if the column is not grouped
            if (!col.IsGrouped)
            {
                internalColumns.RemoveSortIndex(col);
                col.SortDirection = System.Windows.Forms.SortOrder.None;
                col.DataGridViewColumn.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
            }
#if DEBUG
            internalColumns.DebugOutput();
#endif
        }

        /// <summary>
        /// Collapse all groups
        /// </summary>
        public void CollapseAll()
        {
            SetGroupCollapse(true);
        }

        /// <summary>
        /// Expands all groups
        /// </summary>
        public void ExpandAll()
        {
            SetGroupCollapse(false);
        }

        public void ExpandNodeAll()
        {
            foreach (OutlookGridRow r in this.Rows)
            {
                RecursiveSetNodeCollapse(r, false);
            }
            this.Rows[0].Visible = !this.Rows[0].Visible;
            this.Rows[0].Visible = !this.Rows[0].Visible;

            //When collapsing the first row still seeing it.
            if (this.Rows[0].Index < this.FirstDisplayedScrollingRowIndex)
                this.FirstDisplayedScrollingRowIndex = this.Rows[0].Index;
        }

        public void CollapseNodeAll()
        {
            foreach (OutlookGridRow r in this.Rows)
            {
                RecursiveSetNodeCollapse(r, true);
            }
            this.Rows[0].Visible = !this.Rows[0].Visible;
            this.Rows[0].Visible = !this.Rows[0].Visible;

            //When collapsing the first row still seeing it.
            if (this.Rows[0].Index < this.FirstDisplayedScrollingRowIndex)
                this.FirstDisplayedScrollingRowIndex = this.Rows[0].Index;
        }

        /// <summary>
        /// Expand all groups associated to a specific column
        /// </summary>
        /// <param name="col">The DataGridViewColumn</param>
        public void Expand(string col)
        {
            SetGroupCollapse(col, false);
        }

        /// <summary>
        /// Collapse all groups associated to a specific column
        /// </summary>
        /// <param name="col">The DataGridViewColumn</param>
        public void Collapse(string col)
        {
            SetGroupCollapse(col, true);
        }

        /// <summary>
        /// Expand a group row
        /// </summary>
        /// <param name="row">Index of the row</param>
        public void Expand(int row)
        {
            SetGroupCollapse(row, false);
        }

        /// <summary>
        /// Collapse a group row
        /// </summary>
        /// <param name="row">Index of the row</param>
        public void Collapse(int row)
        {
            SetGroupCollapse(row, true);
        }

        /// <summary>
        /// Clear all groups. Performs a fill grid too.
        /// </summary>
        public void ClearGroups()
        {
            ClearGroupsWithoutFilling();
            Fill();
        }

        /// <summary>
        /// Clear all groups. No FillGrid calls.
        /// </summary>
        public void ClearGroupsWithoutFilling()
        {
            //TODO check that
            //reset groups and collapsed statuses
            groupCollection.Clear();
            //reset groups in columns
            internalColumns.MaxGroupIndex = -1;
            for (int i = 0; i < this.internalColumns.Count; i++)
            {
                if (internalColumns[i].IsGrouped)
                    internalColumns[i].DataGridViewColumn.Visible = true;
                internalColumns[i].GroupIndex = -1;
            }
        }

        /// <summary>
        /// Gets the index of the previous group row if any.
        /// </summary>
        /// <param name="currentRow">Current row index</param>
        /// <returns>Index of the group row, -1 otherwise</returns>
        public int PreviousGroupRowIndex(int currentRow)
        {
            for (int i = currentRow - 1; i >= 0; i--)
            {
                OutlookGridRow row = (OutlookGridRow)base.Rows[i];
                if (row.IsGroupRow)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets all the subrows of a grouprow (recursive)
        /// </summary>
        /// <param name="list">The result list of OutlookGridRows</param>
        /// <param name="grouprow">The IOutlookGridGroup that contains rows to inspect.</param>
        /// <returns>A list of OutlookGridRows</returns>
        public List<OutlookGridRow> GetSubRows(ref List<OutlookGridRow> list, IOutlookGridGroup grouprow)
        {
            list.AddRange(grouprow.Rows);
            for (int i = 0; i < grouprow.Children.Count; i++)
            {
                if (grouprow.Children.Count > 0)
                    GetSubRows(ref list, grouprow.Children[i]);
            }

            return list;
        }

        /// <summary>
        /// Register for events concerning the groupbox
        /// </summary>
        public void RegisterGroupBoxEvents()
        {
            //Register for event of the associated KryptonGroupBox
            if (this.GroupBox != null)
            {
                groupBox.ColumnGroupAdded += ColumnGroupAddedEvent;
                groupBox.ColumnSortChanged += ColumnSortChangedEvent;
                groupBox.ColumnGroupRemoved += ColumnGroupRemovedEvent;
                groupBox.ClearGrouping += ClearGroupingEvent;
                groupBox.FullCollapse += FullCollapseEvent;
                groupBox.FullExpand += FullExpandEvent;
                groupBox.ColumnGroupOrderChanged += ColumnGroupIndexChangedEvent;
                groupBox.GroupCollapse += GridGroupCollapseEvent;
                groupBox.GroupExpand += GridGroupExpandEvent;
                groupBox.GroupIntervalClick += GroupIntervalClickEvent;
                groupBox.SortBySummaryCount += SortBySummaryCountEvent;
            }
        }

        /// <summary>
        /// Synchronize the OutlookGrid Group Box with the current status of the grid
        /// </summary>
        public void ForceRefreshGroupBox()
        {
            if (groupBox != null)
                groupBox.UpdateGroupingColumns(internalColumns.FindGroupedColumns());
        }

        /// <summary>
        /// Show the context menu header
        /// </summary>
        /// <param name="columnIndex">The column used by the context menu.</param>
        private void ShowColumnHeaderContextMenu(int columnIndex)
        {
            OutlookGridColumn col = internalColumns.FindFromColumnIndex(columnIndex);
            // Create menu items the first time they are needed
            if (_menuItems == null)
            {
                // Create individual items
                _menuSortAscending = new KryptonContextMenuItem(LangManager.Instance.GetString("SORTASCENDING"), Properties.Resources.sort_ascending, OnColumnSortAscending);
                _menuSortDescending = new KryptonContextMenuItem(LangManager.Instance.GetString("SORTDESCENDING"), Properties.Resources.sort_descending, new EventHandler(OnColumnSortDescending));
                _menuClearSorting = new KryptonContextMenuItem(LangManager.Instance.GetString("CLEARSORTING"), Properties.Resources.sort_up_down_delete_16, new EventHandler(OnColumnClearSorting));
                _menuSeparator1 = new KryptonContextMenuSeparator();
                _menuExpand = new KryptonContextMenuItem(LangManager.Instance.GetString("EXPAND"), Properties.Resources.element_plus_16, new EventHandler(OnGroupExpand));
                _menuCollapse = new KryptonContextMenuItem(LangManager.Instance.GetString("COLLAPSE"), Properties.Resources.element_minus_16, new EventHandler(OnGroupCollapse));
                _menuSeparator4 = new KryptonContextMenuSeparator();
                _menuGroupByThisColumn = new KryptonContextMenuItem(LangManager.Instance.GetString("GROUP"), Properties.Resources.element, new EventHandler(OnGroupByThisColumn));
                _menuUngroupByThisColumn = new KryptonContextMenuItem(LangManager.Instance.GetString("UNGROUP"), Properties.Resources.element_delete, new EventHandler(OnUnGroupByThisColumn));
                _menuShowGroupBox = new KryptonContextMenuItem(LangManager.Instance.GetString("SHOWGROUPBOX"), null, new EventHandler(OnShowGroupBox));
                _menuHideGroupBox = new KryptonContextMenuItem(LangManager.Instance.GetString("HIDEGROUPBOX"), null, new EventHandler(OnHideGroupBox));
                _menuSeparator2 = new KryptonContextMenuSeparator();
                _menuBestFitColumn = new KryptonContextMenuItem(LangManager.Instance.GetString("BESTFIT"), null, new EventHandler(OnBestFitColumn));
                _menuBestFitAllColumns = new KryptonContextMenuItem(LangManager.Instance.GetString("BESTFITALL"), Properties.Resources.fit_to_size, new EventHandler(OnBestFitAllColumns));
                _menuSeparator3 = new KryptonContextMenuSeparator();
                _menuVisibleColumns = new KryptonContextMenuItem(LangManager.Instance.GetString("COLUMNS"), Properties.Resources.table2_selection_column, null);
                _menuGroupInterval = new KryptonContextMenuItem(LangManager.Instance.GetString("GROUPINTERVAL"));
                _menuSortBySummary = new KryptonContextMenuItem(LangManager.Instance.GetString("SORTBYSUMMARYCOUNT"), null, new EventHandler(OnSortBySummary));
                _menuSortBySummary.CheckOnClick = true;

                //Group Interval
                KryptonContextMenuItems _GroupIntervalItems;
                KryptonContextMenuItem it = null;
                string[] names = Enum.GetNames(typeof(OutlookGridDateTimeGroup.DateInterval));
                KryptonContextMenuItemBase[] arrayOptions = new KryptonContextMenuItemBase[names.Length];
                for (int i = 0; i < names.Length; i++)
                {
                    it = new KryptonContextMenuItem(LangManager.Instance.GetString(names[i]));
                    it.Tag = names[i];
                    it.Click += OnGroupIntervalClick;
                    arrayOptions[i] = it;
                }
                _GroupIntervalItems = new KryptonContextMenuItems(arrayOptions);
                _menuGroupInterval.Items.Add(_GroupIntervalItems);

                //Visible Columns
                KryptonContextMenuCheckBox it2 = null;
                KryptonContextMenuItemBase[] arrayCols = new KryptonContextMenuItemBase[this.Columns.Count];
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    it2 = new KryptonContextMenuCheckBox(this.Columns[i].HeaderText);
                    it2.Checked = this.Columns[i].Visible;
                    it2.Tag = this.Columns[i].Index;
                    it2.CheckedChanged += OnColumnVisibleCheckedChanged;
                    arrayCols[i] = it2;
                }
                _menuVisibleColumns.Items.AddRange(arrayCols);

                //Add items inside an items collection (apart from separator1 which is only added if required)
                _menuItems = new KryptonContextMenuItems(new KryptonContextMenuItemBase[] { _menuSortAscending,
                                                                                            _menuSortDescending,
                                                                                            _menuSortBySummary,
                                                                                            _menuClearSorting,
                                                                                            _menuSeparator1,
                                                                                            _menuExpand,
                                                                                            _menuCollapse,
                                                                                            _menuSeparator4,
                                                                                            _menuGroupByThisColumn,
                                                                                            _menuGroupInterval,
                                                                                            _menuUngroupByThisColumn,
                                                                                            _menuShowGroupBox,
                                                                                            _menuHideGroupBox,
                                                                                            _menuSeparator2,
                                                                                            _menuBestFitColumn,
                                                                                            _menuBestFitAllColumns,
                                                                                            _menuSeparator3,
                                                                                            _menuVisibleColumns});
            }

            // Ensure we have a krypton context menu if not already present
            if (this.KCtxMenu == null)
                KCtxMenu = new KryptonContextMenu();

            // Update the individual menu options
            if (col != null)
            {
                _menuSortAscending.Visible = col.DataGridViewColumn.SortMode != DataGridViewColumnSortMode.NotSortable;
                _menuSortAscending.Checked = col.SortDirection == SortOrder.Ascending ? true : false;
                _menuSortDescending.Checked = col.SortDirection == SortOrder.Descending ? true : false;
                _menuSortDescending.Visible = col.DataGridViewColumn.SortMode != DataGridViewColumnSortMode.NotSortable;
                _menuSortBySummary.Visible = col.IsGrouped;
                _menuSortBySummary.Checked = col.GroupingType.SortBySummaryCount;
                _menuClearSorting.Enabled = col.SortDirection != SortOrder.None && !col.IsGrouped;
                _menuClearSorting.Visible = col.DataGridViewColumn.SortMode != DataGridViewColumnSortMode.NotSortable;
                _menuSeparator1.Visible = (_menuSortAscending.Visible || _menuSortDescending.Visible || _menuClearSorting.Visible);
                _menuExpand.Visible = col.IsGrouped;
                _menuCollapse.Visible = col.IsGrouped;
                _menuSeparator4.Visible = (_menuExpand.Visible || _menuCollapse.Visible);
                _menuGroupByThisColumn.Visible = !col.IsGrouped && col.DataGridViewColumn.SortMode != DataGridViewColumnSortMode.NotSortable;
                _menuGroupInterval.Visible = col.IsGrouped && col.DataGridViewColumn.SortMode != DataGridViewColumnSortMode.NotSortable && col.GroupingType.GetType() == typeof(OutlookGridDateTimeGroup);
                if (_menuGroupInterval.Visible)
                {
                    string currentInterval = Enum.GetName(typeof(OutlookGridDateTimeGroup.DateInterval), ((OutlookGridDateTimeGroup)col.GroupingType).Interval);
                    foreach (KryptonContextMenuItem item in ((KryptonContextMenuItems)_menuGroupInterval.Items[0]).Items)
                    {
                        item.Checked = item.Tag.ToString() == currentInterval;
                    }
                }
                _menuUngroupByThisColumn.Visible = col.IsGrouped && col.DataGridViewColumn.SortMode != DataGridViewColumnSortMode.NotSortable;
                _menuShowGroupBox.Visible = (groupBox != null) && !groupBox.Visible;
                _menuHideGroupBox.Visible = (groupBox != null) && groupBox.Visible;
                _menuSeparator2.Visible = (_menuGroupByThisColumn.Visible || _menuUngroupByThisColumn.Visible || _menuShowGroupBox.Visible || _menuHideGroupBox.Visible);
                _menuBestFitColumn.Visible = true;
            }
            else
            {
                _menuSortAscending.Visible = false;
                _menuSortDescending.Visible = false;
                _menuSortBySummary.Visible = false;
                _menuClearSorting.Visible = false;
                _menuSeparator1.Visible = (_menuSortAscending.Visible || _menuSortDescending.Visible || _menuClearSorting.Visible);
                _menuExpand.Visible = false;
                _menuCollapse.Visible = false;
                _menuSeparator4.Visible = (_menuExpand.Visible || _menuCollapse.Visible);
                _menuGroupByThisColumn.Visible = false;
                _menuGroupInterval.Visible = false;
                _menuUngroupByThisColumn.Visible = false;
                _menuShowGroupBox.Visible = (groupBox != null) && !groupBox.Visible;
                _menuHideGroupBox.Visible = (groupBox != null) && groupBox.Visible;
                _menuSeparator2.Visible = (_menuGroupByThisColumn.Visible || _menuUngroupByThisColumn.Visible || _menuShowGroupBox.Visible || _menuHideGroupBox.Visible);
                _menuBestFitColumn.Visible = false;
            }

            if (!KCtxMenu.Items.Contains(_menuItems))
                KCtxMenu.Items.Add(_menuItems);

            // Show the menu!
            KCtxMenu.Show(this);
        }

        /// <summary>
        /// Clear all sorting columns only (not the grouped ones)
        /// </summary>
        public void ResetAllSortingColumns()
        {
            internalColumns.MaxSortIndex = -1;
            foreach (OutlookGridColumn col in internalColumns)
            {
                if (!col.IsGrouped && col.SortDirection != SortOrder.None)
                {
                    col.DataGridViewColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                    col.SortDirection = SortOrder.None;
                    col.SortIndex = -1;
                }
            }
#if DEBUG
            internalColumns.DebugOutput();
#endif
        }

        ///// <summary>
        ///// Sort the grid
        ///// </summary>
        ///// <param name="comparer">The IComparer object.</param>
        //public void Sort(IComparer<OutlookGridRow> comparer)
        //{
        //    Fill();
        //}

        /// <summary>
        /// Clear the internal Rows
        /// </summary>
        public void ClearInternalRows()
        {
            internalRows.Clear();
        }

        /// <summary>
        /// Assign the rows to the internal list.
        /// </summary>
        /// <param name="l">List of OutlookGridRows</param>
        public void AssignRows(List<OutlookGridRow> l)
        {
            internalRows = l;
        }

        /// <summary>
        /// Assign the rows to the internal list.
        /// </summary>
        /// <param name="col">DataGridViewRowCollection</param>
        public void AssignRows(DataGridViewRowCollection col)
        {
            //dataSource.Rows = col.Cast<OutlookGridRow>().ToList();
            internalRows = col.Cast<OutlookGridRow>().ToList();
        }

        /// <summary>
        /// Collapse/Expand all group rows
        /// </summary>
        /// <param name="collapsed">True if collapsed, false if expanded</param>
        private void SetGroupCollapse(bool collapsed)
        {
            if (!IsGridGrouped || internalRows.Count == 0)
                return;

            //// loop through all rows to find the GroupRows
            //for (int i = 0; i < this.Rows.Count; i++)
            //{
            //    if (((OutlookGridRow)this.Rows[i]).IsGroupRow)
            //        ((OutlookGridRow)this.Rows[i]).Group.Collapsed = collapsed;
            //}
            RecursiveSetGroupCollapse(this.groupCollection, collapsed);

            // workaround, make the grid refresh properly
            this.Rows[0].Visible = !this.Rows[0].Visible;
            this.Rows[0].Visible = !this.Rows[0].Visible;

            //When collapsing the first row still seeing it.
            if (this.Rows[0].Index < this.FirstDisplayedScrollingRowIndex)
                this.FirstDisplayedScrollingRowIndex = this.Rows[0].Index;
        }

        private void RecursiveSetGroupCollapse(OutlookGridGroupCollection col, bool collapsed)
        {
            for (int i = 0; i < col.Count; i++)
            {
                col[i].Collapsed = collapsed;
                RecursiveSetGroupCollapse(col[i].Children, collapsed);
            }
        }

        private void SetGroupCollapse(string c, bool collapsed)
        {
            if (!IsGridGrouped || internalRows.Count == 0)
                return;

            // loop through all rows to find the GroupRows
            //for (int i = 0; i < this.Rows.Count; i++)
            //{
            //    if (((OutlookGridRow)this.Rows[i]).IsGroupRow && ((OutlookGridRow)this.Rows[i]).Group.Column.DataGridViewColumn.Name == c.Name)
            //        ((OutlookGridRow)this.Rows[i]).Group.Collapsed = collapsed;
            //}
            RecursiveSetGroupCollapse(c, this.groupCollection, collapsed);

            // workaround, make the grid refresh properly
            this.Rows[0].Visible = !this.Rows[0].Visible;
            this.Rows[0].Visible = !this.Rows[0].Visible;

            //When collapsing the first row still seeing it.
            if (this.Rows[0].Index < this.FirstDisplayedScrollingRowIndex)
                this.FirstDisplayedScrollingRowIndex = this.Rows[0].Index;
        }

        private void RecursiveSetGroupCollapse(string c, OutlookGridGroupCollection col, bool collapsed)
        {
            for (int i = 0; i < col.Count; i++)
            {
                if (col[i].Column.Name == c)
                    col[i].Collapsed = collapsed;
                RecursiveSetGroupCollapse(c, col[i].Children, collapsed);
            }
        }

        /// <summary>
        /// Collapse/Expand a group row
        /// </summary>
        /// <param name="rowindex">The index of the group row.</param>
        /// <param name="collapsed">True if collapsed, false if expanded.</param>
        private void SetGroupCollapse(int rowindex, bool collapsed)
        {
            if (!IsGridGrouped || internalRows.Count == 0 || rowindex < 0)
                return;

            OutlookGridRow row = (OutlookGridRow)base.Rows[rowindex];
            if (row.IsGroupRow)
            {
                row.Group.Collapsed = collapsed;

                //this is a workaround to make the grid re-calculate it's contents and backgroun bounds
                // so the background is updated correctly.
                // this will also invalidate the control, so it will redraw itself
                row.Visible = false;
                row.Visible = true;

                //When collapsing the first row still seeing it.
                if (row.Index < this.FirstDisplayedScrollingRowIndex)
                    this.FirstDisplayedScrollingRowIndex = row.Index;
            }
        }


        private void RecursiveSetNodeCollapse(OutlookGridRow r, bool collapsed)
        {
            if (r.HasChildren)
            {
                r.Collapsed = collapsed;
                foreach (OutlookGridRow r2 in r.Nodes.Nodes)
                {
                   RecursiveSetNodeCollapse(r2,collapsed);
                }
            }
        }

        #region Grid Fill functions


        private void NonGroupedRecursiveFillOutlookGridRows(List<OutlookGridRow> l, List<OutlookGridRow> tmp)
        {
            for (int i = 0; i < l.Count; i++)
            {
                tmp.Add(l[i]);

                //Recusive call
                if (l[i].HasChildren)
                {
                    NonGroupedRecursiveFillOutlookGridRows(l[i].Nodes.Nodes, tmp);
                }
            }
        }



        /// <summary>
        /// Fill the grid (grouping, sorting,...)
        /// </summary>
        public void Fill()
        {
            Cursor.Current = Cursors.WaitCursor;
#if (DEBUG)
            Stopwatch azer = new Stopwatch();
            azer.Start();
#endif
            List<OutlookGridRow> list;
            List<OutlookGridRow> tmp; // = new List<OutlookGridRow>();
            IOutlookGridGroup grParent = null;
            this.Rows.Clear();
            this.groupCollection.Clear();

            if (internalRows.Count == 0)
                return;
            list = internalRows;


            // this block is used of grouping is turned off
            // this will simply list all attributes of each object in the list
            if (internalColumns.CountGrouped() == 0)
            {
                //Applying sort
                try
                {
                    list.Sort(new OutlookGridRowComparer2(internalColumns.GetIndexAndSortSortedOnlyColumns()));
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to sort.\n\n Error:" + e.Message,
                                   "Grid Sorting",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                }

                //Add rows to underlying DataGridView
                //TODO : boolean to choose fill mode
                //this.Rows.AddRange(list.ToArray());
                tmp = new List<OutlookGridRow>();//list.Count);
                NonGroupedRecursiveFillOutlookGridRows(list, tmp);

                //Add all the rows to the grid
                this.Rows.AddRange(tmp.ToArray());
            }
            // this block is used when grouping is used
            // items in the list must be sorted, and then they will automatically be grouped
            else
            {
                //Group part of the job
                try
                {
                    //We get the columns that are grouped
                    List<OutlookGridColumn> groupedColumns = internalColumns.FindGroupedColumns();

                    //For each outlookgrid row in the grid
                    for (int j = 0; j < list.Count; j++)
                    {
                        //Reload the groups collection for each rows !!
                        OutlookGridGroupCollection children = this.groupCollection;

                        //For each grouped column (ordered by GroupIndex)
                        for (int i = 0; i < groupedColumns.Count; i++)
                        {
                            if (i == 0)
                                grParent = null;

                            //Gets the stored value
                            object value = list[j].Cells[groupedColumns[i].DataGridViewColumn.Index].Value;
                            object formattedValue;

                            //We get the formatting value according to the type of group (Alphabetic, DateTime,...)
                            groupedColumns[i].GroupingType.Value = value;
                            formattedValue = groupedColumns[i].GroupingType.Value;

                            //We search the corresponding group.
                            IOutlookGridGroup gr = children.FindGroup(formattedValue);
                            if (gr == null)
                            {
                                gr = (IOutlookGridGroup)groupedColumns[i].GroupingType.Clone();
                                gr.ParentGroup = grParent;
                                gr.Column = groupedColumns[i];
                                gr.Value = value;
                                gr.FormatStyle = groupedColumns[i].DataGridViewColumn.DefaultCellStyle.Format; //We can the formatting applied to the cell to the group
                                if (value is TextAndImage)
                                    gr.GroupImage = ((TextAndImage)value).Image;
                                gr.Level = i;
                                children.Add(gr);
                            }

                            //Go deeper in the groups, set current group as parent
                            grParent = gr;
                            children = gr.Children;

                            //if we have browsed all the groups we are sure to be in the righr group: add rows and update counters !
                            if (i == groupedColumns.Count - 1)
                            {
                                list[j].Group = gr;
                                gr.Rows.Add(list[j]);
                                RecursiveIncrementParentGroupCounters(gr);
                            }
                        }
                    }

                    //reset local variable for GC
                    groupedColumns = null;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to add rows.\n\n Error:" + e.Message,
                                   "Grid Filling",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                }

                //Sorting part : sort the groups between them and sort the rows inside the groups
                try
                {
                    //int index = internalColumns.FindSortedColumnNotgrouped();
                    //RecursiveSort(this.groupCollection, index, (index == -1) ? SortOrder.None : internalColumns.FindFromColumnIndex(index).SortDirection);
                    List<Tuple<int, SortOrder, IComparer>> sortList = internalColumns.GetIndexAndSortSortedOnlyColumns();
                    if (sortList.Count > 0)
                        RecursiveSort(this.groupCollection, sortList);
                    else
                        RecursiveSort(this.groupCollection, internalColumns.GetIndexAndSortGroupedColumns());
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to sort.\n\n Error:" + e.Message,
                                   "Grid Sorting",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                }

                //Reinit!
                tmp = new List<OutlookGridRow>();//list.Count);
                //Get a list of rows (grouprow and non-grouprow)
                RecursiveFillOutlookGridRows(this.groupCollection, tmp);

                //Finally add the rows to underlying datagridview after all the magic !
                this.Rows.AddRange(tmp.ToArray());
            }
            Cursor.Current = Cursors.Default;
#if (DEBUG)
            azer.Stop();
            Console.WriteLine("FillGrid : " + azer.ElapsedMilliseconds + " ms");
#endif
        }

        /// <summary>
        /// Sort recursively the OutlookGridRows within groups
        /// </summary>
        /// <param name="groupCollection">The OutlookGridGroupCollection.</param>
        /// <param name="sortList">The list of sorted columns</param>
        private void RecursiveSort(OutlookGridGroupCollection groupCollection, List<Tuple<int, SortOrder, IComparer>> sortList)
        {
            //We sort the groups
            if (groupCollection.Count > 0)
            {
                if (groupCollection[0].Column.GroupingType.SortBySummaryCount)
                    groupCollection.Sort(new OutlookGridGroupCountComparer());
                else
                    groupCollection.Sort();
            }

            //Sort the rows inside each group
            for (int i = 0; i < groupCollection.Count; i++)
            {
                //If there is no child group then we have only rows...
                if ((groupCollection[i].Children.Count == 0) && sortList.Count > 0)
                {
                    //We sort the rows according to the sorted only columns
                    groupCollection[i].Rows.Sort(new OutlookGridRowComparer2(sortList));
                }
                //else
                //{
                //    Console.WriteLine("groupCollection[i].Rows" + groupCollection[i].Rows.Count.ToString());
                //    //We sort the rows according to the group sort (useful for alphbetics for example)
                //    groupCollection[i].Rows.Sort(new OutlookGridRowComparer(groupCollection[i].Column.DataGridViewColumn.Index, internalColumns[groupCollection[i].Column.DataGridViewColumn.Name].SortDirection));
                //}

                //Recursive call for children
                RecursiveSort(groupCollection[i].Children, sortList);
            }
        }

        /// <summary>
        /// Update all the parents counters of a group
        /// </summary>
        /// <param name="l">The group whic</param>
        private void RecursiveIncrementParentGroupCounters(IOutlookGridGroup l)
        {
            //Add +1 to the counter
            l.ItemCount++;
            if (l.ParentGroup != null)
            {
                //Recursive call for parent
                RecursiveIncrementParentGroupCounters(l.ParentGroup);
            }
        }

        /// <summary>
        /// Transform a group in a list of OutlookGridRows. Recursive call
        /// </summary>
        /// <param name="l">The OutlookGridGroupCollection that contains the groups and associated rows.</param>
        /// <param name="tmp">A List of OutlookGridRow</param>
        private void RecursiveFillOutlookGridRows(OutlookGridGroupCollection l, List<OutlookGridRow> tmp)
        {
            OutlookGridRow grRow;
            IOutlookGridGroup gr;

            //for each group
            for (int i = 0; i < l.List.Count; i++)
            {
                gr = l.List[i];

                //Create the group row
                grRow = (OutlookGridRow)this.RowTemplate.Clone();
                grRow.Group = gr;
                grRow.IsGroupRow = true;
                grRow.Height = gr.Height;
                grRow.CreateCells(this, gr.Value);
                tmp.Add(grRow);

                //Recusive call
                if (gr.Children.Count > 0)
                {
                    RecursiveFillOutlookGridRows(gr.Children, tmp);
                }

                //We add the rows associated with the current group
                //TODO : boolean to choose fill mode
                //tmp.AddRange(gr.Rows);
                NonGroupedRecursiveFillOutlookGridRows(gr.Rows, tmp);
            }
        }

        #endregion Grid Fill functions

        /// <summary>
        /// Persist the configuration of the KryptonOutlookGrid
        /// </summary>
        /// <param name="path">The path where the .xml file will be saved.</param>
        public void PersistConfiguration(string path)
        {
            OutlookGridColumn col = null;
            using (XmlWriter writer = XmlWriter.Create(path, new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("OutlookGrid");
                writer.WriteAttributeString("V", "1");
                writer.WriteElementString("GroupBox", groupBox.Visible.ToString());
                writer.WriteElementString("HideColumnOnGrouping", CommonHelper.BoolToString(this.HideColumnOnGrouping));
                writer.WriteStartElement("Columns");
                for (int i = 0; i < this.internalColumns.Count; i++)
                {
                    col = this.internalColumns[i];
                    writer.WriteStartElement("Column");
                    writer.WriteElementString("Name", col.Name);
                    writer.WriteStartElement("GroupingType");
                    if (col.GroupingType != null)
                    {
                        writer.WriteElementString("Name",col.GroupingType.GetType().AssemblyQualifiedName); //.GetType().Name.ToString());
                        writer.WriteElementString("OneItemText", col.GroupingType.OneItemText);
                        writer.WriteElementString("XXXItemsText", col.GroupingType.XXXItemsText);
                        writer.WriteElementString("SortBySummaryCount", CommonHelper.BoolToString(col.GroupingType.SortBySummaryCount));
                        writer.WriteElementString("ItemsComparer", (col.GroupingType.ItemsComparer == null) ? "" : col.GroupingType.ItemsComparer.GetType().AssemblyQualifiedName);
                        if (col.GroupingType.GetType() == typeof(OutlookGridDateTimeGroup))
                        {
                            writer.WriteElementString("GroupDateInterval", ((OutlookGridDateTimeGroup)col.GroupingType).Interval.ToString());
                        }
                    }
                    writer.WriteEndElement();
                    writer.WriteElementString("SortDirection", col.SortDirection.ToString());
                    writer.WriteElementString("GroupIndex", col.GroupIndex.ToString());
                    writer.WriteElementString("SortIndex", col.SortIndex.ToString());
                    writer.WriteElementString("Visible", col.DataGridViewColumn.Visible.ToString());
                    writer.WriteElementString("Width", col.DataGridViewColumn.Width.ToString());
                    writer.WriteElementString("Index", col.DataGridViewColumn.Index.ToString());
                    writer.WriteElementString("DisplayIndex", col.DataGridViewColumn.DisplayIndex.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }
        }

        /// <summary>
        /// Clears everything in the OutlookGrid (groups, rows, columns, DataGridViewColumns). Ready for a completely new start.
        /// </summary>
        public void ClearEverything()
        {
            this.groupCollection.Clear();
            this.internalRows.Clear();
            this.internalColumns.Clear();
            this.Columns.Clear();
            //Snif everything is gone ! Ready for a new start !
        }
        #endregion OutlookGrid methods

        public bool CollapseNode(OutlookGridRow node)
        {
            if (!node.Collapsed)
            {
                CollapsingEventArgs exp = new CollapsingEventArgs(node);
                this.OnNodeCollapsing(exp);

                if (!exp.Cancel)
                {
                    node.SetNodeCollapse(true);

                    CollapsedEventArgs exped = new CollapsedEventArgs(node);
                    this.OnNodeCollapsed(exped);
                }

                return !exp.Cancel;
            }
            else
            {
                // row isn't expanded, so we didn't do anything.				
                return false;
            }
        }

        public bool ExpandNode(OutlookGridRow node)
        {
            if (node.Collapsed)
            {
                ExpandingEventArgs exp = new ExpandingEventArgs(node);
                this.OnNodeExpanding(exp);

                if (!exp.Cancel)
                {
                    node.SetNodeCollapse(false);

                    ExpandedEventArgs exped = new ExpandedEventArgs(node);
                    this.OnNodeExpanded(exped);
                }

                return !exp.Cancel;
            }
            else
            {
                // row isn't expanded, so we didn't do anything.				
                return false;
            }
        }
    }
}