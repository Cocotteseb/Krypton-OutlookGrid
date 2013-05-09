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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using ComponentFactory.Krypton.Toolkit;
using System.ComponentModel;
using JDHSoftware.Krypton.Toolkit.Utils;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid
{
    /// <summary>
    /// GroupBox for the Krypton OutlookGrid
    /// </summary>
    public partial class KryptonOutlookGridGroupBox : UserControl
    {
        private List<OutlookGridGroupBoxColumn> columnsList;

        //Krypton
        private IPalette _palette;
        private PaletteRedirect _paletteRedirect;
        private PaletteBackInheritRedirect _paletteBack;
        private PaletteBorderInheritRedirect _paletteBorder;
        private PaletteContentInheritRedirect _paletteContent;
        private PaletteDataGridViewRedirect _paletteDataGridView;
        private PaletteDataGridViewAll _paletteDataGridViewAll;
        private IDisposable _mementoBack;
        private PaletteBorder _border;

        //Mouse
        private Point _mouse;
        private bool isDragging;
        private int indexselected;

        //Context menu
        private KryptonContextMenu KCtxMenu;
        private KryptonContextMenuItems _menuItems;
        private KryptonContextMenuItem _menuSortAscending;
        private KryptonContextMenuItem _menuSortDescending;
        private KryptonContextMenuItem _menuUnGroup;
        private KryptonContextMenuSeparator _menuSeparator1;
        private KryptonContextMenuItem _menuFullExpand;
        private KryptonContextMenuItem _menuFullCollapse;
        private KryptonContextMenuSeparator _menuSeparator2;
        private KryptonContextMenuItem _menuClearGrouping;
        private KryptonContextMenuItem _menuHideGroupBox;

        /// <summary>
        /// Column Sort Changed Event
        /// </summary>
        public event EventHandler<OutlookGridColumnEventArgs> ColumnSortChanged;
        /// <summary>
        /// Column Group Added Event
        /// </summary>
        public event EventHandler<OutlookGridColumnEventArgs> ColumnGroupAdded;
        /// <summary>
        /// Column Group removed Event
        /// </summary>
        public event EventHandler<OutlookGridColumnEventArgs> ColumnGroupRemoved;
        /// <summary>
        /// Clear grouping event
        /// </summary>
        public event EventHandler ClearGrouping;
        /// <summary>
        /// Full Expand event
        /// </summary>
        public event EventHandler FullExpand;
        /// <summary>
        /// Full Collapse event
        /// </summary>
        public event EventHandler FullCollapse;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public KryptonOutlookGridGroupBox()
        {
            // To remove flicker we use double buffering for drawing
            SetStyle(
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            InitializeComponent();

            columnsList = new List<OutlookGridGroupBoxColumn>();

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
            // Store the inherit instances
            _paletteBack = new PaletteBackInheritRedirect(_paletteRedirect);
            _paletteBorder = new PaletteBorderInheritRedirect(_paletteRedirect);
            _paletteContent = new PaletteContentInheritRedirect(_paletteRedirect);
            _paletteDataGridView = new PaletteDataGridViewRedirect(_paletteRedirect, null);
            _paletteDataGridViewAll = new PaletteDataGridViewAll(_paletteDataGridView, null);

            // Create storage that maps onto the inherit instances
            _border = new PaletteBorder(_paletteBorder, null);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets access to the common textbox appearance entries that other states can override.
        /// </summary>
        [Category("Visuals")]
        [Description("Overrides borders settings.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteBorder Border
        {
            get { return _border; }
        }

        #endregion

        #region Overrides

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Overrides the paint event
        /// </summary>
        /// <param name="e">PaintEventArgs</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            int num2 = 5;
            if (_palette != null)
            {
                // (3) Get the renderer associated with this palette
                IRenderer renderer = _palette.GetRenderer();

                // (4) Create the rendering context that is passed into all renderer calls
                using (RenderContext renderContext = new RenderContext(this, e.Graphics, e.ClipRectangle, renderer))
                {
                    _paletteBack.Style = PaletteBackStyle.PanelClient;
                    _paletteBorder.Style = PaletteBorderStyle.HeaderPrimary;
                    using (GraphicsPath path = renderer.RenderStandardBorder.GetBackPath(renderContext, e.ClipRectangle, _paletteBorder, VisualOrientation.Top, PaletteState.Normal))
                    {
                        _mementoBack = renderer.RenderStandardBack.DrawBack(renderContext,
                            ClientRectangle,
                            path,
                            _paletteBack,
                            VisualOrientation.Top,
                            PaletteState.Normal,
                            _mementoBack);
                    }
                    renderer.RenderStandardBorder.DrawBorder(renderContext, ClientRectangle, _border, VisualOrientation.Top, PaletteState.Normal);

                    //If no grouped columns, draw to the indicating text
                    if (columnsList.Count == 0)
                    {
                        TextRenderer.DrawText(e.Graphics, LangManager.Instance.GetString("DRAGCOLUMNTOGROUP"), _palette.GetContentShortTextFont(PaletteContentStyle.LabelNormalPanel, PaletteState.Normal), e.ClipRectangle, _palette.GetContentShortTextColor1(PaletteContentStyle.LabelNormalPanel, PaletteState.Normal),
                            TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.PreserveGraphicsClipping);
                    }

                    PaletteState state;
                    _paletteBack.Style = PaletteBackStyle.GridHeaderColumnList;
                    _paletteBorder.Style = PaletteBorderStyle.GridHeaderColumnList;
                    // PaintGroupBox(e.Graphics, e.ClipRectangle, this.Font, "Drag a column here to group", columnsList.Count > 0);

                    //Draw the column boxes
                    foreach (OutlookGridGroupBoxColumn current in this.columnsList)
                    {
                        Rectangle rectangle = default(Rectangle);
                        rectangle.Width = 100;
                        rectangle.X = num2;
                        rectangle.Y = (e.ClipRectangle.Height - 25) / 2;
                        rectangle.Height = 25;
                        num2 += 105;
                        current.Rect = rectangle;

                        if (current.IsHovered)
                            state = PaletteState.Tracking;
                        else if (current.Pressed)
                            state = PaletteState.Pressed;
                        else
                            state = PaletteState.Normal;
                        // Do we need to draw the background?
                        if (_paletteBack.GetBackDraw(PaletteState.Normal) == InheritBool.True)
                        {
                            //Back
                            using (GraphicsPath path = renderer.RenderStandardBorder.GetBackPath(renderContext, rectangle, _paletteBorder, VisualOrientation.Top, PaletteState.Normal))
                            {
                                _mementoBack = renderer.RenderStandardBack.DrawBack(renderContext,
                                    rectangle,
                                    path,
                                    _paletteBack,
                                    VisualOrientation.Top,
                                    state,
                                    _mementoBack);
                            }

                            //Border
                            renderer.RenderStandardBorder.DrawBorder(renderContext, rectangle, _paletteBorder, VisualOrientation.Top, state);

                            //Text
                            TextRenderer.DrawText(e.Graphics, current.Text, _palette.GetContentShortTextFont(PaletteContentStyle.GridHeaderColumnList, state), rectangle, _palette.GetContentShortTextColor1(PaletteContentStyle.GridHeaderColumnList, state),
                                TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.PreserveGraphicsClipping);

                            //Sort Glyph
                            renderer.RenderGlyph.DrawGridSortGlyph(renderContext, current.SortOrder, rectangle, _paletteDataGridViewAll.HeaderColumn.Content, state, false);
                        }

                        //Draw the column box while it is moving
                        if (current.IsMoving)
                        {
                            Rectangle rectangle1 = new Rectangle(_mouse.X, _mouse.Y, current.Rect.Width, current.Rect.Height);
                            //this.Renderer.PaintMovingColumn(graphics, this.currentDragColumn, rectangle1);
                            using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(70, Color.Gray)))
                            {
                                e.Graphics.FillRectangle(solidBrush, rectangle1);
                            }

                            TextRenderer.DrawText(e.Graphics, current.Text, _palette.GetContentShortTextFont(PaletteContentStyle.GridHeaderColumnList, PaletteState.Disabled), rectangle1, _palette.GetContentShortTextColor1(PaletteContentStyle.GridHeaderColumnList, PaletteState.Disabled),
                                TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.PreserveGraphicsClipping);
                        }
                    }
                }
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// Overrides the MouseDown event.
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            foreach (OutlookGridGroupBoxColumn c in this.columnsList)
            {
                if (c.Rect != null)
                {
                    if (c.Rect.Contains(e.X, e.Y) && (e.Button == System.Windows.Forms.MouseButtons.Left))
                    {
                        c.Pressed = true;
                    }
                }
            }
            this.Invalidate();
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Overrides the MouseUp event.
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            List<OutlookGridGroupBoxColumn> l = new List<OutlookGridGroupBoxColumn>();

            foreach (OutlookGridGroupBoxColumn c in this.columnsList)
            {
                if (c.Rect != null)
                {
                    if (c.IsMoving && !this.Bounds.Contains(e.Location))
                    {
                        l.Add(c);
                    }

                    //Stop moving and pressing
                    c.Pressed = false;
                    c.IsMoving = false;
                }
            }

            //no more dragging
            isDragging = false;

            //Ungroup columns dragged outside the box
            if (l.Count > 0)
            {
                foreach (OutlookGridGroupBoxColumn c in l)
                {
                    //Warn the Grid
                    OnColumnGroupRemoved(new OutlookGridColumnEventArgs(new OutlookGridColumn(c.ColumnName, null, null, SortOrder.None, false)));

                    columnsList.Remove(c);
                }
            }
            this.Invalidate();
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Overrides the MouseClick event.
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                indexselected = -1;
                for (int i = 0; i < this.columnsList.Count; i++)
                {
                    if (columnsList[i].Rect != null && columnsList[i].Rect.Contains(e.X, e.Y))
                    {
                        indexselected = i;
                    }
                }
                ShowColumnBoxContextMenu();
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                foreach (OutlookGridGroupBoxColumn c in this.columnsList)
                {
                    if (c.Rect != null && c.Rect.Contains(e.X, e.Y))
                    {
                        c.SortOrder = c.SortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
                        //Warn the Grid
                        OnColumnSortChanged(new OutlookGridColumnEventArgs(new OutlookGridColumn(c.ColumnName, null, null, c.SortOrder, false)));
                    }
                }
            }

            base.OnMouseClick(e);
        }

        /// <summary>
        /// Overrides the MouseMove event.
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            _mouse = e.Location;
            foreach (OutlookGridGroupBoxColumn c in this.columnsList)
            {
                if (c.Rect != null)
                {
                    //Update hovering
                    c.IsHovered = c.Rect.Contains(e.X, e.Y);

                    //declare dragging
                    if (c.Rect.Contains(e.X, e.Y) && (e.Button == System.Windows.Forms.MouseButtons.Left) && !isDragging)
                    {
                        isDragging = true;
                        c.IsMoving = true;
                        //Console.WriteLine(_mouse.ToString());
                    }
                }
            }
            Invalidate();
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles OnPalettePaint Event
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A PaletteLayoutEventArgs that contains the event data.</param>
        private void OnPalettePaint(object sender, PaletteLayoutEventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Handles OnGlobalPaletteChanged event
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        private void OnGlobalPaletteChanged(object sender, EventArgs e)
        {
            // (5) Unhook events from old palette
            if (_palette != null)
                _palette.PalettePaint -= new EventHandler<PaletteLayoutEventArgs>(OnPalettePaint);

            // (6) Cache the new IPalette that is the global palette
            _palette = KryptonManager.CurrentGlobalPalette;
            _paletteRedirect.Target = _palette; //!!!!!!

            // (7) Hook into events for the new palette
            if (_palette != null)
                _palette.PalettePaint += new EventHandler<PaletteLayoutEventArgs>(OnPalettePaint);

            // (8) Change of palette means we should repaint to show any changes
            Invalidate();
        }

        /// <summary>
        /// Raises the ColumnSortChanged event.
        /// </summary>
        /// <param name="e">A OutlookGridColumnEventArgs that contains the event data.</param>
        protected virtual void OnColumnSortChanged(OutlookGridColumnEventArgs e)
        {
            if (ColumnSortChanged != null)
                ColumnSortChanged(this, e);
        }

        /// <summary>
        /// Raises the ColumnGroupAdded event.
        /// </summary>
        /// <param name="e">A OutlookGridColumnEventArgs that contains the event data.</param>
        protected virtual void OnColumnGroupAdded(OutlookGridColumnEventArgs e)
        {
            if (ColumnGroupAdded != null)
                ColumnGroupAdded(this, e);
        }

        /// <summary>
        /// Raises the ColumnGroupRemoved event.
        /// </summary>
        /// <param name="e">A OutlookGridColumnEventArgs that contains the event data.</param>
        protected virtual void OnColumnGroupRemoved(OutlookGridColumnEventArgs e)
        {
            if (ColumnGroupRemoved != null)
                ColumnGroupRemoved(this, e);
        }

        /// <summary>
        /// Raises the ClearGrouping event.
        /// </summary>
        /// <param name="e">A EventArgs that contains the event data.</param>
        protected virtual void OnClearGrouping(EventArgs e)
        {
            if (ClearGrouping != null)
                ClearGrouping(this, e);
        }

        /// <summary>
        /// Raises the FullExpand event.
        /// </summary>
        /// <param name="e">A EventArgs that contains the event data.</param>
        protected virtual void OnFullExpand(EventArgs e)
        {
            if (FullExpand != null)
                FullExpand(this, e);
        }

        /// <summary>
        /// Raises the FullCollapse event.
        /// </summary>
        /// <param name="e">A EventArgs that contains the event data.</param>
        protected virtual void OnFullCollapse(EventArgs e)
        {
            if (FullCollapse != null)
                FullCollapse(this, e);
        }

        /// <summary>
        /// Handles the HideGroupBox event
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        private void OnHideGroupBox(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Handles the ClearGrouping event
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        private void OnClearGrouping(object sender, EventArgs e)
        {
            OnClearGrouping(new EventArgs());
            columnsList.Clear();
            this.Invalidate();
        }

        /// <summary>
        /// Handles the FullCollapse event
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        private void OnFullCollapse(object sender, EventArgs e)
        {
            OnFullCollapse(new EventArgs());
        }

        /// <summary>
        /// Handles the FullExpand event
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        private void OnFullExpand(object sender, EventArgs e)
        {
            OnFullExpand(new EventArgs());
        }

        /// <summary>
        /// Handles the SortAscending event
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        private void OnSortAscending(object sender, EventArgs e)
        {
            OutlookGridGroupBoxColumn col = columnsList[indexselected];
            col.SortOrder = SortOrder.Ascending;
            OnColumnSortChanged(new OutlookGridColumnEventArgs(new OutlookGridColumn(columnsList[indexselected].ColumnName, null, null, SortOrder.Ascending, false)));
            this.Invalidate();
        }

        /// <summary>
        /// Handles the SortDescending event
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        private void OnSortDescending(object sender, EventArgs e)
        {
            OutlookGridGroupBoxColumn col = columnsList[indexselected];
            col.SortOrder = SortOrder.Descending;
            OnColumnSortChanged(new OutlookGridColumnEventArgs(new OutlookGridColumn(col.ColumnName, null, null, SortOrder.Descending, false)));
            this.Invalidate();
        }

        /// <summary>
        /// Handles the UnGroup event
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        private void OnUngroup(object sender, EventArgs e)
        {
            OutlookGridGroupBoxColumn col = columnsList[indexselected];
            OnColumnGroupRemoved(new OutlookGridColumnEventArgs(new OutlookGridColumn(col.ColumnName, null, null, SortOrder.None, false)));
            columnsList.Remove(col);
            this.Invalidate();
        }

        /// <summary>
        /// Handles the DragDrop event. Add a new grouping column following a drag drop from the grid
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A DragEventArgs that contains the event data.</param>
        private void KryptonOutlookGridGroupBox_DragDrop(object sender, DragEventArgs e)
        {
            string columnToMove = e.Data.GetData(typeof(string)) as string;
            string columnName;
            string columnText;
            SortOrder sortOrder;
            string[] res = columnToMove.Split('|');
            columnName = res[0];
            columnText = res[1];
            sortOrder = SortOrder.Ascending;
            OutlookGridGroupBoxColumn colToAdd = new OutlookGridGroupBoxColumn(columnName, columnText, sortOrder);
            if (!String.IsNullOrEmpty(columnToMove) && !columnsList.Contains(colToAdd))
            {
                columnsList.Add(colToAdd);

                try
                {
                    //Warns the grid of a new grouping
                    OnColumnGroupAdded(new OutlookGridColumnEventArgs(new OutlookGridColumn(columnName, null, null, SortOrder.None, true)));
                    this.Invalidate();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Failed to group.\n\n Error:" + exc.Message,
                                      "Grid GroupBox",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Hnadles the DragEnter event.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">A DragEventArgs that contains the event data.</param>
        private void KryptonOutlookGridGroupBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Show the context menu for column box
        /// </summary>
        private void ShowColumnBoxContextMenu()
        {
            if (_menuItems == null)
            {
                // Create individual items
                _menuSortAscending = new KryptonContextMenuItem(LangManager.Instance.GetString("SORTASCENDING"), Properties.Resources.sort_ascending, new EventHandler(OnSortAscending));
                _menuSortDescending = new KryptonContextMenuItem(LangManager.Instance.GetString("SORTDESCENDING"), Properties.Resources.sort_descending, new EventHandler(OnSortDescending));
                _menuUnGroup = new KryptonContextMenuItem(LangManager.Instance.GetString("UNGROUP"), Properties.Resources.element_delete, new EventHandler(OnUngroup));
                _menuSeparator1 = new KryptonContextMenuSeparator();
                _menuFullExpand = new KryptonContextMenuItem(LangManager.Instance.GetString("FULLEXPAND"), Properties.Resources.navigate_plus, new EventHandler(OnFullExpand));
                _menuFullCollapse = new KryptonContextMenuItem(LangManager.Instance.GetString("FULLCOLLAPSE"), Properties.Resources.navigate_minus, new EventHandler(OnFullCollapse));
                _menuSeparator2 = new KryptonContextMenuSeparator();
                _menuClearGrouping = new KryptonContextMenuItem(LangManager.Instance.GetString("CLEARGROUPING"), Properties.Resources.element_selection_delete, new EventHandler(OnClearGrouping));
                _menuHideGroupBox = new KryptonContextMenuItem(LangManager.Instance.GetString("HIDEGROUPBOX"), null, new EventHandler(OnHideGroupBox));

                // Add items inside an items collection (apart from separator1 which is only added if required)
                _menuItems = new KryptonContextMenuItems(new KryptonContextMenuItemBase[] { _menuSortAscending,
                                                                                            _menuSortDescending,
                                                                                            _menuUnGroup,
                                                                                            _menuSeparator1,
                                                                                            _menuFullExpand,
                                                                                            _menuFullCollapse,
                                                                                            _menuSeparator2,
                                                                                            _menuClearGrouping,
                                                                                            _menuHideGroupBox
                                                                                          });
            }

            // Ensure we have a krypton context menu if not already present
            if (this.KCtxMenu == null)
                KCtxMenu = new KryptonContextMenu();


            // Update the individual menu options
            OutlookGridGroupBoxColumn col = null;
            if (indexselected > -1)
                col = columnsList[indexselected];

            _menuSortAscending.Visible = col != null;
            _menuSortDescending.Visible = col != null;
            _menuSortAscending.Checked = col != null && col.SortOrder == SortOrder.Ascending;
            _menuSortDescending.Checked = col != null && col.SortOrder == SortOrder.Descending;
            _menuUnGroup.Visible = col != null;
            _menuFullExpand.Enabled = columnsList.Count > 0;
            _menuFullCollapse.Enabled = columnsList.Count > 0;
            _menuClearGrouping.Enabled = columnsList.Count > 0;


            _menuSeparator1.Visible = (_menuSortAscending.Visible || _menuSortDescending.Visible || _menuUnGroup.Visible);

            if (!KCtxMenu.Items.Contains(_menuItems))
                KCtxMenu.Items.Add(_menuItems);

            // Show the menu!
            KCtxMenu.Show(this);
        }
        
        /// <summary>
        /// DO NOT USE THIS FUNCTION YOURSELF, USE the corresponding function in OutlookGrid
        /// Update the grouping columns.
        /// </summary>
        /// <param name="list">The list of OutlookGridColumn</param>
        public void UpdateGroupingColumns(List<OutlookGridColumn> list)
        {
            columnsList.Clear();
            OutlookGridGroupBoxColumn colToAdd;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsGrouped)
                {
                    colToAdd = new OutlookGridGroupBoxColumn(list[i].DataGridViewColumn.Name, list[i].DataGridViewColumn.HeaderText, list[i].SortDirection);
                    columnsList.Add(colToAdd);
                }
            }
            this.Invalidate();
        }


        /// <summary>
        /// Checks if the column exists in the GroupBox control
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <returns>True if exists, otherwise false.</returns>
        public bool Contains(string columnName)
        {
            for (int i = 0; i < columnsList.Count; i++)
            {
                if (columnsList[i].ColumnName == columnName)
                    return true;
            }
            return false;
        }

        #endregion
    }
}

