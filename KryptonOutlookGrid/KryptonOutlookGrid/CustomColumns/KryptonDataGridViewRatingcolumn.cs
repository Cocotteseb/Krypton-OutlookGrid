//http://blogs.msdn.com/b/markrideout/archive/2006/01/18/media-player-like-rating-datagridview-column.aspx
//MS-PL License

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.CustomsColumns
{
    /// <summary>
    /// Class for a rating column
    /// </summary>
    public class KryptonDataGridViewRatingColumn : DataGridViewImageColumn
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public KryptonDataGridViewRatingColumn()
        {
            this.CellTemplate = new RatingCell();
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.ValueType = typeof(int);
        }
    }

    /// <summary>
    /// Class for a rating celle
    /// </summary>
    public class RatingCell : DataGridViewImageCell
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RatingCell()
        {
             //Value type is an integer. 
             //Formatted value type is an image since we derive from the ImageCell 
            this.ValueType = typeof(int);
        }

        /// <summary>
        /// Overrides GetFormattedValue
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rowIndex"></param>
        /// <param name="cellStyle"></param>
        /// <param name="valueTypeConverter"></param>
        /// <param name="formattedValueTypeConverter"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            //Convert integer to star images 
            return starImages[(int)value];
        }

        /// <summary>
        /// Overrides DefaultNewRowValue
        /// </summary>
        public override object DefaultNewRowValue
        {
            //default new row to 3 stars 
            get { return 3; }
        }

        /// <summary>
        /// Overrides Paint
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipBounds"></param>
        /// <param name="cellBounds"></param>
        /// <param name="rowIndex"></param>
        /// <param name="elementState"></param>
        /// <param name="value"></param>
        /// <param name="formattedValue"></param>
        /// <param name="errorText"></param>
        /// <param name="cellStyle"></param>
        /// <param name="advancedBorderStyle"></param>
        /// <param name="paintParts"></param>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            Image cellImage = (Image)formattedValue;

            int starNumber = GetStarFromMouse(cellBounds, this.DataGridView.PointToClient(Control.MousePosition));

            if (starNumber != -1)
                cellImage = starHotImages[starNumber];

            //supress painting of selection 
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, cellImage, errorText, cellStyle, advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.SelectionBackground));
        }

        /// <summary>
        /// Update cell's value when the user clicks on a star 
        /// </summary>
        /// <param name="e">A DataGridViewCellEventArgs that contains the event data.</param>
        protected override void OnContentClick(DataGridViewCellEventArgs e)
        {
            base.OnContentClick(e);
            int starNumber = GetStarFromMouse(this.DataGridView.GetCellDisplayRectangle(this.DataGridView.CurrentCellAddress.X, this.DataGridView.CurrentCellAddress.Y, false), this.DataGridView.PointToClient(Control.MousePosition));

            if (starNumber != -1)
                this.Value = starNumber;
        }

        #region Invalidate cells when mouse moves or leaves the cell

        /// <summary>
        /// Overrides OnMouseLeave
        /// </summary>
        /// <param name="rowIndex">the row that contains the cell.</param>
        protected override void OnMouseLeave(int rowIndex)
        {
            base.OnMouseLeave(rowIndex);
            this.DataGridView.InvalidateCell(this);
        }

        /// <summary>
        /// Overrides OnMouseMove
        /// </summary>
      /// <param name="e">A DataGridViewCellMouseEventArgs that contains the event data.</param>
        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.DataGridView.InvalidateCell(this);
        }
        #endregion

        #region Private Implementation

        static Image[] starImages;
        static Image[] starHotImages;
        const int IMAGEWIDTH = 58;

        private int GetStarFromMouse(Rectangle cellBounds, Point mouseLocation)
        {
            if (cellBounds.Contains(mouseLocation))
            {
                int mouseXRelativeToCell = (mouseLocation.X - cellBounds.X);
                int imageXArea = (cellBounds.Width / 2) - (IMAGEWIDTH / 2);
                if (((mouseXRelativeToCell + 4) < imageXArea) || (mouseXRelativeToCell >= (imageXArea + IMAGEWIDTH)))
                    return -1;
                else
                {
                    int oo = (int)Math.Round((((float)(mouseXRelativeToCell - imageXArea + 5) / (float)IMAGEWIDTH) * 5f), MidpointRounding.AwayFromZero);
                    if (oo > 5 || oo < 0) System.Diagnostics.Debugger.Break();
                    return oo;
                }
            }
            else
                return -1;
        }

        //setup star images 
        #region Load star images

        static RatingCell()
        {
            starImages = new Image[6];
            starHotImages = new Image[6];
            // load normal stars 
            for (int i = 0; i <= 5; i++)
                starImages[i] = (Image)JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Properties.Resources.ResourceManager.GetObject("star" + i.ToString());

            // load hot normal stars 
            for (int i = 0; i <= 5; i++)
                starHotImages[i] = (Image)JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Properties.Resources.ResourceManager.GetObject("starhot" + i.ToString());
        }
        #endregion

        #endregion

    }
} 
