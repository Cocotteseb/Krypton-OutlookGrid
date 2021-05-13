using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid
{
    public class FilterColumnHeaderCell : DataGridViewColumnHeaderCell
    {
        ComboBoxState currentState = ComboBoxState.Normal;
        Point cellLocation;
        Rectangle buttonRect;

        //public event EventHandler<ColumnFilterClickedEventArg> FilterButtonClicked;

        protected override void OnDataGridViewChanged()
        {
            try
            {
                Padding dropDownPadding = new Padding(0, 0, 20, 0);
                this.Style.Padding = Padding.Add(this.InheritedStyle.Padding, dropDownPadding);
            }
            catch { }
            base.OnDataGridViewChanged();
        }
      
            protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
        
            int width = 100; // 20 px
            buttonRect = new Rectangle(cellBounds.X + cellBounds.Width - width, cellBounds.Y, width, cellBounds.Height);
            cellLocation = cellBounds.Location;
            ComboBoxRenderer.DrawDropDownButton(graphics, buttonRect, currentState);
        }
        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (this.IsMouseOverButton(e.Location))
                currentState = ComboBoxState.Pressed;
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            if (this.IsMouseOverButton(e.Location))
            {
                currentState = ComboBoxState.Normal;
                this.OnFilterButtonClicked();
            }
            base.OnMouseUp(e);
        }
        private bool IsMouseOverButton(Point e)
        {
            Point p = new Point(e.X + cellLocation.X, e.Y + cellLocation.Y);
            if (p.X >= buttonRect.X && p.X <= buttonRect.X + buttonRect.Width &&
                p.Y >= buttonRect.Y && p.Y <= buttonRect.Y + buttonRect.Height)
            {
                return true;
            }
            return false;
        }
        protected virtual void OnFilterButtonClicked()
        {
            //    if (this.FilterButtonClicked != null)
            //    {
            //        this.FilterButtonClicked(this, new ColumnFilterClickedEventArg(this.ColumnIndex, this.buttonRect));
            //    }
        }
    }
}