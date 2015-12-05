using ComponentFactory.Krypton.Toolkit;
using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.CustomColumns
{
    public class KryptonDataGridViewFormattingColumn : KryptonDataGridViewTextBoxColumn
    {
        private bool _contrastTextColor; 


        public KryptonDataGridViewFormattingColumn()
            : base()
        {
            this.CellTemplate = new FormattingCell();
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.ValueType = typeof(FormattingCell);
            ContrastTextColor = false;
        }

        public bool ContrastTextColor
        {
            get
            {
                return _contrastTextColor;
            }

            set
            {
                _contrastTextColor = value;
            }
        }
    }

    /// <summary>
    /// Class for a rating celle
    /// </summary>
    public class FormattingCell : KryptonDataGridViewTextBoxCell
    {

        public EnumConditionalFormatType FormatType { get; set; }
        public IFormatParams FormatParams { get; set; }


        private Color ContrastColor(Color color)
        {
            int d = 0;
            //  Counting the perceptive luminance - human eye favors green color... 
            double a = (1
                        - (((0.299 * color.R)
                        + ((0.587 * color.G) + (0.114 * color.B)))
                        / 255));
            if ((a < 0.5))
            {
                d = 0;
            }
            else
            {
                //  bright colors - black font
                d = 255;
            }

            //  dark colors - white font
            return Color.FromArgb(d, d, d);
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
        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, System.Windows.Forms.DataGridViewElementStates cellState, object value, object formattedValue, string errorText, System.Windows.Forms.DataGridViewCellStyle cellStyle, System.Windows.Forms.DataGridViewAdvancedBorderStyle advancedBorderStyle,
        System.Windows.Forms.DataGridViewPaintParts paintParts)
        {
            if (FormatParams != null)  // null can happen when cell set to Formatting but no condition has been set !
            {
                switch (FormatType)
                {
                    case EnumConditionalFormatType.Bar:
                        int barWidth;
                        BarParams par = (BarParams)FormatParams;
                        barWidth = (int)((cellBounds.Width - 10) * par.ProportionValue);
                        Style.BackColor = this.DataGridView.DefaultCellStyle.BackColor;
                        Style.ForeColor = this.DataGridView.DefaultCellStyle.ForeColor;

                        if (barWidth > 0) //(double)value > 0 &&
                        {
                            Rectangle r = new Rectangle(cellBounds.X + 3, cellBounds.Y + 3, barWidth, cellBounds.Height - 8);
                            if (par.GradientFill)
                            {
                                using (LinearGradientBrush linearBrush = new LinearGradientBrush(r, par.BarColor, Color.White, LinearGradientMode.Horizontal)) //Color.FromArgb(255, 247, 251, 242)
                                {
                                    graphics.FillRectangle(linearBrush, r);
                                }
                            }
                            else
                            {
                                using (SolidBrush solidBrush = new SolidBrush(par.BarColor)) //Color.FromArgb(255, 247, 251, 242)
                                {
                                    graphics.FillRectangle(solidBrush, r);
                                }
                            }

                            using (Pen pen = new Pen(par.BarColor)) //Color.FromArgb(255, 140, 197, 66)))
                            {
                                graphics.DrawRectangle(pen, r);
                            }
                        }

                        break;
                    case EnumConditionalFormatType.TwoColorsRange:
                        TwoColorsParams TWCpar = (TwoColorsParams)FormatParams;
                        Style.BackColor = TWCpar.ValueColor;
                      //  if (ContrastTextColor)
                            Style.ForeColor = ContrastColor(TWCpar.ValueColor);
                        break;
                    case EnumConditionalFormatType.ThreeColorsRange:
                        ThreeColorsParams THCpar = (ThreeColorsParams)FormatParams;
                        Style.BackColor = THCpar.ValueColor;
                        Style.ForeColor = ContrastColor(THCpar.ValueColor);
                        break;
                    default:
                        Style.BackColor = this.DataGridView.DefaultCellStyle.BackColor;
                        Style.ForeColor = this.DataGridView.DefaultCellStyle.ForeColor;
                        break;
                }
            }
            else
            {
                Style.BackColor = this.DataGridView.DefaultCellStyle.BackColor;
                Style.ForeColor = this.DataGridView.DefaultCellStyle.ForeColor;
            }

            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle,
            DataGridViewPaintParts.None | DataGridViewPaintParts.ContentForeground);
        }
    }
}
