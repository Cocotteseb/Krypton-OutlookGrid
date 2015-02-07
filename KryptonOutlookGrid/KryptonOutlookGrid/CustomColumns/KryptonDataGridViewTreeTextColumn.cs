using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.CustomsColumns
{
    public class KryptonDataGridViewTreeTextColumn : KryptonDataGridViewTextBoxColumn
    {

        public KryptonDataGridViewTreeTextColumn()
        {
            this.CellTemplate = new KryptonDataGridViewTreeTextCell();
        }
    }

    /// <summary>
    /// Class for a TextAndImage cell
    /// </summary>
    public class KryptonDataGridViewTreeTextCell : KryptonDataGridViewTextBoxCell
    {
        private const int INDENT_WIDTH = 20;
        private const int INDENT_MARGIN = 5;
        private Padding defaultPadding;
        // private int glyphWidth;

        /// <summary>
        /// Constructor
        /// </summary>
        public KryptonDataGridViewTreeTextCell()
            : base()
        {
            this.defaultPadding = this.Style.Padding;
        }

        /// <summary>
        /// Overrides Clone
        /// </summary>
        /// <returns>The cloned KryptonDataGridViewTextAndImageCell</returns>
        public override object Clone()
        {
            KryptonDataGridViewTreeTextCell c = base.Clone() as KryptonDataGridViewTreeTextCell;
            return c;
        }

        protected virtual int GlyphMargin
        {
            get
            {
                return ((this.Level - 1) * INDENT_WIDTH) + INDENT_MARGIN;
            }
        
        
        }

        public int Level
        {
            get
            {
                OutlookGridRow row =(OutlookGridRow)this.OwningRow;
                if (row != null)
                {
                    return row.NodeLevel+1; //during calculation 0 level must be 1 for multiplication
                }
                else
                    return -1;
            }
        }

        public void UpdateStyle()
        {
            OutlookGridRow node = OwningNode;
            //Console.WriteLine(DateTime.Now.ToString() + " " + node.ToString());
            bool hasChildNodes = node.HasChildren;
            int level = this.Level;
            int plus = 0;
            //if (hasChildNodes)
            //    plus = 15;
            this.Style.Padding = new Padding(defaultPadding.Left + (level * INDENT_WIDTH) + INDENT_MARGIN + plus,
                                                           defaultPadding.Top, defaultPadding.Right, defaultPadding.Bottom);
           
            
        }

        /// <summary>
        /// Overrides Paint
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipBounds"></param>
        /// <param name="cellBounds"></param>
        /// <param name="rowIndex"></param>
        /// <param name="cellState"></param>
        /// <param name="value"></param>
        /// <param name="formattedValue"></param>
        /// <param name="errorText"></param>
        /// <param name="cellStyle"></param>
        /// <param name="advancedBorderStyle"></param>
        /// <param name="paintParts"></param>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            OutlookGridRow node = OwningNode;
           // Console.WriteLine(DateTime.Now.ToString() + " " + node.ToString());
            //bool hasChildNodes = node.HasChildren;
            //int level = this.Level ;
            //int plus = 0;
            //if (hasChildNodes)
            //    plus = 15;
            //Padding currentPadding = this.InheritedStyle.Padding;
           // this.Style.Padding = new Padding(defaultPadding.Left + (level * INDENT_WIDTH) + INDENT_MARGIN,
             //                                             defaultPadding.Top, defaultPadding.Right, defaultPadding.Bottom);

                    //this.Style.Padding = new Padding(currentPadding.Left + (level * INDENT_WIDTH) + _imageWidth + INDENT_MARGIN,
            //                                   currentPadding.Top, currentPadding.Right, currentPadding.Bottom);

            //if (this.Value != null && ((TextAndImage)this.Value).Image != null)
            //{
            //    Padding inheritedPadding = this.InheritedStyle.Padding;
            //    this.Style.Padding = new Padding(18, inheritedPadding.Top, inheritedPadding.Right, inheritedPadding.Bottom);
            //    // Draw the image clipped to the cell.
            //    System.Drawing.Drawing2D.GraphicsContainer container = graphics.BeginContainer();
            //    graphics.SetClip(cellBounds);
            //    graphics.DrawImageUnscaled(((TextAndImage)this.Value).Image, new Point(cellBounds.Location.X + 2, cellBounds.Location.Y + ((cellBounds.Height - 16) / 2) - 1));
            //    graphics.EndContainer(container);
            //}

            //if (node == null) return;

            //Image image = node.Image;

            //if (this._imageHeight == 0 && image != null) this.UpdateStyle();

            // paint the cell normally
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            // TODO: Indent width needs to take image size into account
            Rectangle glyphRect = new Rectangle(cellBounds.X + this.GlyphMargin, cellBounds.Y, INDENT_WIDTH, cellBounds.Height - 1);


            ////TODO: This painting code needs to be rehashed to be cleaner
            //int level = this.Level;

            ////TODO: Rehash this to take different Imagelayouts into account. This will speed up drawing
            ////		for images of the same size (ImageLayout.None)
            //if (image != null)
            //{
            //    Point pp;
            //    if (_imageHeight > cellBounds.Height)
            //        pp = new Point(glyphRect.X + this.glyphWidth, cellBounds.Y + _imageHeightOffset);
            //    else
            //        pp = new Point(glyphRect.X + this.glyphWidth, (cellBounds.Height / 2 - _imageHeight / 2) + cellBounds.Y);

            //    // Graphics container to push/pop changes. This enables us to set clipping when painting
            //    // the cell's image -- keeps it from bleeding outsize of cells.
            //    System.Drawing.Drawing2D.GraphicsContainer gc = graphics.BeginContainer();
            //    {
            //        graphics.SetClip(cellBounds);
            //        graphics.DrawImageUnscaled(image, pp);
            //    }
            //    graphics.EndContainer(gc);
            //}

            // Paint tree lines			
            if (((KryptonOutlookGrid)node.DataGridView).ShowLines)
            {
                using (Pen linePen = new Pen(SystemBrushes.ControlDark, 1.0f))
                {
                    linePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    bool isLastSibling = node.IsLastSibling;
                    bool isFirstSibling = node.IsFirstSibling;

                    if (node.NodeLevel == 0)
                    {
                        // the Root nodes display their lines differently
                        if (isFirstSibling && isLastSibling)
                        {
                            // only node, both first and last. Just draw horizontal line
                            graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2, glyphRect.Right, cellBounds.Top + cellBounds.Height / 2);
                        }
                        else if (isLastSibling)
                        {
                            // last sibling doesn't draw the line extended below. Paint horizontal then vertical
                            graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2, glyphRect.Right, cellBounds.Top + cellBounds.Height / 2);
                            graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2);
                        }
                        else if (isFirstSibling)
                        {
                            // first sibling doesn't draw the line extended above. Paint horizontal then vertical
                            graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2, glyphRect.Right, cellBounds.Top + cellBounds.Height / 2);
                            graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2, glyphRect.X + 4, cellBounds.Bottom);
                        }
                        else
                        {
                            // normal drawing draws extended from top to bottom. Paint horizontal then vertical
                            graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2, glyphRect.Right, cellBounds.Top + cellBounds.Height / 2);
                            graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top, glyphRect.X + 4, cellBounds.Bottom);
                        }
                    }
                    else
                    {
                        if (isLastSibling)
                        {
                            // last sibling doesn't draw the line extended below. Paint horizontal then vertical
                            graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2, glyphRect.Right, cellBounds.Top + cellBounds.Height / 2);
                            graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2);
                        }
                        else
                        {
                            // normal drawing draws extended from top to bottom. Paint horizontal then vertical
                            graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2, glyphRect.Right, cellBounds.Top + cellBounds.Height / 2);
                            graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top, glyphRect.X + 4, cellBounds.Bottom);
                        }

                        // paint lines of previous levels to the root
                        OutlookGridRow previousNode = node.ParentNode;
                        int horizontalStop = (glyphRect.X + 4) - INDENT_WIDTH;

                        while (previousNode != null)//.IsRoot)
                        {
                            if (previousNode.HasChildren && !previousNode.IsLastSibling)
                            {
                                // paint vertical line
                                graphics.DrawLine(linePen, horizontalStop, cellBounds.Top, horizontalStop, cellBounds.Bottom);
                            }
                            previousNode = previousNode.ParentNode;
                            horizontalStop = horizontalStop - INDENT_WIDTH;
                        }
                    }

                }
            }

            if (node.HasChildren)
            {
                // Paint node glyphs	
                if (node.Collapsed)
                {
                    if (KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2010 || KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2013)
                    {
                        graphics.DrawImage(Properties.Resources.collapseIcon2010, glyphRect.X, glyphRect.Y + (glyphRect.Height / 2) - 4, 11, 11);
                    }
                    else
                    {
                        graphics.DrawImage(Properties.Resources.expandIcon, glyphRect.X, glyphRect.Y + (glyphRect.Height / 2) - 4, 11, 11);
                    }
                }
                else
                {
                    if (KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2010 || KryptonManager.CurrentGlobalPalette.GetRenderer() == KryptonManager.RenderOffice2013)
                    {
                        graphics.DrawImage(Properties.Resources.expandIcon2010, glyphRect.X, glyphRect.Y + (glyphRect.Height / 2) - 4, 11, 11);
                    }
                    else
                    {
                        graphics.DrawImage(Properties.Resources.collapseIcon, glyphRect.X, glyphRect.Y + (glyphRect.Height / 2) - 4, 11, 11);
                    }
                }
            }
            //graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red)), glyphRect);
        }

        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseUp(e);

            OutlookGridRow node = this.OwningNode;
            if (node != null)
                ((KryptonOutlookGrid)node.DataGridView)._inExpandCollapseMouseCapture = false;
        }
        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            Rectangle dis = this.DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            Rectangle glyphRect = new Rectangle(dis.X + this.GlyphMargin, dis.Y, INDENT_WIDTH, dis.Height - 1);

            //if (e.X > this.InheritedStyle.Padding.Left)
            if ((e.X + dis.X <= glyphRect.X + 11) &&
                (e.X + dis.X >= glyphRect.X))
            {
             
                // Expand the node
                //TODO: Calculate more precise location
                OutlookGridRow node = this.OwningNode;
                if (node != null)
                {
                    ((KryptonOutlookGrid)node.DataGridView)._inExpandCollapseMouseCapture = true;

                    if (node.Collapsed)
                        node.Expand();
                    else
                        node.Collapse();
                }
            }
            else
            {
                base.OnMouseDown(e);
            }
        }



        public OutlookGridRow OwningNode
        {
            get { return base.OwningRow as OutlookGridRow; }
        }
    }
}
