//--------------------------------------------------------------------------------
// Copyright (C) 2013-2015 JDH Software - <support@jdhsoftware.com>
//
// This program is provided to you under the terms of the Microsoft Public
// License (Ms-PL) as published at https://kryptonoutlookgrid.codeplex.com/license
//
// Visit http://www.jdhsoftware.com and follow @jdhsoftware on Twitter
//
//--------------------------------------------------------------------------------

using ComponentFactory.Krypton.Toolkit;
using JDHSoftware.Krypton.Toolkit.Utils.Lang;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting
{
    public partial class CustomFormatRule : KryptonForm
    {
        /// <summary>
        /// The colors
        /// </summary>
        public Color colMin, colMedium, colMax;
        /// <summary>
        /// The Conditional Formatting type
        /// </summary>
        public EnumConditionalFormatType mode;
        /// <summary>
        /// Gradient boolean
        /// </summary>
        public bool gradient;
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFormatRule"/> class.
        /// </summary>
        /// <param name="initialmode">The  Conditional Formatting type.</param>
        public CustomFormatRule(EnumConditionalFormatType initialmode)
        {
            InitializeComponent();
            KComboBoxFillMode.SelectedIndex = 0;
            KComboBoxStyle.SelectedIndex = -1;
            mode = initialmode;
            colMin = Color.FromArgb(84, 179, 112);
            colMedium = Color.FromArgb(252, 229, 130);
            colMax = Color.FromArgb(243, 120, 97);
        }

        private void CustomFormatStyle_Load(object sender, EventArgs e)
        {
            KColorBtnMin.SelectedColor = colMin;
            KColorBtnMedium.SelectedColor = colMedium;
            KColorBtnMax.SelectedColor = colMax;

            int selected = -1;
            string[] names = Enum.GetNames(typeof(EnumConditionalFormatType));
            for (int i = 0; i < names.Length; i++)
            {
                if (mode.ToString().Equals(names[i]))
                    selected = i;
                KComboBoxStyle.Items.Add(new KryptonListItem(LangManager.Instance.GetString(names[i])) { Tag = names[i] });
            }
            KComboBoxStyle.SelectedIndex = selected;
        }

        private void KBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void KBtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            switch (mode)
            {
                case EnumConditionalFormatType.Bar:
                    if (gradient)
                    {
                        using (LinearGradientBrush br = new LinearGradientBrush(e.ClipRectangle, colMin, Color.White, LinearGradientMode.Horizontal))
                        {
                            e.Graphics.FillRectangle(br, e.ClipRectangle);
                        }
                    }
                    else
                    {
                        using (SolidBrush br = new SolidBrush(colMin))
                        {
                            e.Graphics.FillRectangle(br, e.ClipRectangle);
                        }
                    }
                    using (Pen pen = new Pen(colMin)) //Color.FromArgb(255, 140, 197, 66)))
                    {
                       Rectangle rect  =  e.ClipRectangle;
                        rect.Inflate(-1, -1);
                        e.Graphics.DrawRectangle(pen, rect);
                    }
                    break;
                case EnumConditionalFormatType.TwoColorsRange:
                    // Draw the background gradient.
                    using (LinearGradientBrush br = new LinearGradientBrush(e.ClipRectangle, colMin, colMax, LinearGradientMode.Horizontal))
                    {
                        e.Graphics.FillRectangle(br, e.ClipRectangle);
                    }
                    break;
                case EnumConditionalFormatType.ThreeColorsRange:
                    // Draw the background gradient.              
                    using (LinearGradientBrush br = new LinearGradientBrush(e.ClipRectangle, colMin, colMax, LinearGradientMode.Horizontal))
                    {
                        ColorBlend blend = new ColorBlend();
                        blend.Colors = new Color[] { colMin, colMedium, colMax };
                        blend.Positions = new float[] { 0f, 0.5f, 1.0f };
                        br.InterpolationColors = blend;
                        e.Graphics.FillRectangle(br, e.ClipRectangle);
                    }
                    break;
            }
        }

        private void KColorBtnMin_SelectedColorChanged(object sender, ColorEventArgs e)
        {
            colMin = KColorBtnMin.SelectedColor;
            pictureBox1.Invalidate();
        }

        private void KColorBtnMedium_SelectedColorChanged(object sender, ColorEventArgs e)
        {
            colMedium = KColorBtnMedium.SelectedColor;
            pictureBox1.Invalidate();
        }

        private void KColorBtnMax_SelectedColorChanged(object sender, ColorEventArgs e)
        {
            colMax = KColorBtnMax.SelectedColor;
            pictureBox1.Invalidate();
        }

        private void KComboBoxFillMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            gradient = KComboBoxFillMode.SelectedIndex == 1;
            UpdateUI();
        }

        private void KComboBoxStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            mode = (EnumConditionalFormatType)Enum.Parse(typeof(EnumConditionalFormatType), ((KryptonListItem)KComboBoxStyle.Items[KComboBoxStyle.SelectedIndex]).Tag.ToString());
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (mode)
            {
                case EnumConditionalFormatType.Bar:
                    KLblFill.Visible = true;
                    KComboBoxFillMode.Visible = true;
                    KColorBtnMin.Visible = true;
                    KColorBtnMedium.Visible = false;
                    KColorBtnMax.Visible = false;
                    break;
                case EnumConditionalFormatType.TwoColorsRange:
                    KLblFill.Visible = false;
                    KComboBoxFillMode.Visible = false;
                    KColorBtnMin.Visible = true;
                    KColorBtnMedium.Visible = false;
                    KColorBtnMax.Visible = true;
                    break;
                case EnumConditionalFormatType.ThreeColorsRange:
                    KLblFill.Visible = false;
                    KComboBoxFillMode.Visible = false;
                    KColorBtnMin.Visible = true;
                    KColorBtnMedium.Visible = true;
                    KColorBtnMax.Visible = true;
                    break;
            }
            pictureBox1.Invalidate();
        }
    }
}
