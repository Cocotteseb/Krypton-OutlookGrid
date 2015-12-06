namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting
{
    /// <summary>
    /// CustomFormatRule Form
    /// </summary>
    /// <seealso cref="ComponentFactory.Krypton.Toolkit.KryptonForm" />
    partial class CustomFormatRule
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomFormatRule));
            this.KBtnOK = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.KBtnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.KLblPreview = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.KColorBtnMin = new ComponentFactory.Krypton.Toolkit.KryptonColorButton();
            this.KColorBtnMedium = new ComponentFactory.Krypton.Toolkit.KryptonColorButton();
            this.KColorBtnMax = new ComponentFactory.Krypton.Toolkit.KryptonColorButton();
            this.KComboBoxStyle = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.KLblFormat = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.KLblFill = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.KComboBoxFillMode = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.kryptonGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KComboBoxStyle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KComboBoxFillMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).BeginInit();
            this.kryptonGroup1.Panel.SuspendLayout();
            this.kryptonGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // KBtnOK
            // 
            resources.ApplyResources(this.KBtnOK, "KBtnOK");
            this.KBtnOK.Name = "KBtnOK";
            this.KBtnOK.Values.Image = ((System.Drawing.Image)(resources.GetObject("KBtnOK.Values.Image")));
            this.KBtnOK.Values.Text = resources.GetString("KBtnOK.Values.Text");
            this.KBtnOK.Click += new System.EventHandler(this.KBtnOK_Click);
            // 
            // KBtnCancel
            // 
            resources.ApplyResources(this.KBtnCancel, "KBtnCancel");
            this.KBtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.KBtnCancel.Name = "KBtnCancel";
            this.KBtnCancel.Values.Image = ((System.Drawing.Image)(resources.GetObject("KBtnCancel.Values.Image")));
            this.KBtnCancel.Values.Text = resources.GetString("KBtnCancel.Values.Text");
            this.KBtnCancel.Click += new System.EventHandler(this.KBtnCancel_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // KLblPreview
            // 
            this.KLblPreview.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.BoldControl;
            resources.ApplyResources(this.KLblPreview, "KLblPreview");
            this.KLblPreview.Name = "KLblPreview";
            this.KLblPreview.Values.Text = resources.GetString("KLblPreview.Values.Text");
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.KColorBtnMin);
            this.flowLayoutPanel1.Controls.Add(this.KColorBtnMedium);
            this.flowLayoutPanel1.Controls.Add(this.KColorBtnMax);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // KColorBtnMin
            // 
            resources.ApplyResources(this.KColorBtnMin, "KColorBtnMin");
            this.KColorBtnMin.Name = "KColorBtnMin";
            this.KColorBtnMin.SelectedColor = System.Drawing.Color.White;
            this.KColorBtnMin.SelectedRect = new System.Drawing.Rectangle(0, 0, 16, 16);
            this.KColorBtnMin.Splitter = false;
            this.KColorBtnMin.Values.Image = ((System.Drawing.Image)(resources.GetObject("KColorBtnMin.Values.Image")));
            this.KColorBtnMin.Values.Text = resources.GetString("KColorBtnMin.Values.Text");
            this.KColorBtnMin.VisibleNoColor = false;
            this.KColorBtnMin.SelectedColorChanged += new System.EventHandler<ComponentFactory.Krypton.Toolkit.ColorEventArgs>(this.KColorBtnMin_SelectedColorChanged);
            // 
            // KColorBtnMedium
            // 
            resources.ApplyResources(this.KColorBtnMedium, "KColorBtnMedium");
            this.KColorBtnMedium.Name = "KColorBtnMedium";
            this.KColorBtnMedium.SelectedColor = System.Drawing.Color.White;
            this.KColorBtnMedium.SelectedRect = new System.Drawing.Rectangle(0, 0, 16, 16);
            this.KColorBtnMedium.Splitter = false;
            this.KColorBtnMedium.Values.Image = ((System.Drawing.Image)(resources.GetObject("KColorBtnMedium.Values.Image")));
            this.KColorBtnMedium.Values.Text = resources.GetString("KColorBtnMedium.Values.Text");
            this.KColorBtnMedium.VisibleNoColor = false;
            this.KColorBtnMedium.SelectedColorChanged += new System.EventHandler<ComponentFactory.Krypton.Toolkit.ColorEventArgs>(this.KColorBtnMedium_SelectedColorChanged);
            // 
            // KColorBtnMax
            // 
            resources.ApplyResources(this.KColorBtnMax, "KColorBtnMax");
            this.KColorBtnMax.Name = "KColorBtnMax";
            this.KColorBtnMax.SelectedColor = System.Drawing.Color.White;
            this.KColorBtnMax.SelectedRect = new System.Drawing.Rectangle(0, 0, 16, 16);
            this.KColorBtnMax.Splitter = false;
            this.KColorBtnMax.Values.Image = ((System.Drawing.Image)(resources.GetObject("KColorBtnMax.Values.Image")));
            this.KColorBtnMax.Values.Text = resources.GetString("KColorBtnMax.Values.Text");
            this.KColorBtnMax.VisibleNoColor = false;
            this.KColorBtnMax.SelectedColorChanged += new System.EventHandler<ComponentFactory.Krypton.Toolkit.ColorEventArgs>(this.KColorBtnMax_SelectedColorChanged);
            // 
            // KComboBoxStyle
            // 
            this.KComboBoxStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KComboBoxStyle.DropDownWidth = 200;
            resources.ApplyResources(this.KComboBoxStyle, "KComboBoxStyle");
            this.KComboBoxStyle.Name = "KComboBoxStyle";
            this.KComboBoxStyle.SelectedIndexChanged += new System.EventHandler(this.KComboBoxStyle_SelectedIndexChanged);
            // 
            // KLblFormat
            // 
            this.KLblFormat.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.BoldControl;
            resources.ApplyResources(this.KLblFormat, "KLblFormat");
            this.KLblFormat.Name = "KLblFormat";
            this.KLblFormat.Values.Text = resources.GetString("KLblFormat.Values.Text");
            // 
            // KLblFill
            // 
            resources.ApplyResources(this.KLblFill, "KLblFill");
            this.KLblFill.Name = "KLblFill";
            this.KLblFill.Values.Text = resources.GetString("KLblFill.Values.Text");
            // 
            // KComboBoxFillMode
            // 
            this.KComboBoxFillMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KComboBoxFillMode.DropDownWidth = 200;
            this.KComboBoxFillMode.Items.AddRange(new object[] {
            resources.GetString("KComboBoxFillMode.Items"),
            resources.GetString("KComboBoxFillMode.Items1")});
            resources.ApplyResources(this.KComboBoxFillMode, "KComboBoxFillMode");
            this.KComboBoxFillMode.Name = "KComboBoxFillMode";
            this.KComboBoxFillMode.SelectedIndexChanged += new System.EventHandler(this.KComboBoxFillMode_SelectedIndexChanged);
            // 
            // kryptonGroup1
            // 
            resources.ApplyResources(this.kryptonGroup1, "kryptonGroup1");
            this.kryptonGroup1.Name = "kryptonGroup1";
            // 
            // kryptonGroup1.Panel
            // 
            this.kryptonGroup1.Panel.Controls.Add(this.KComboBoxFillMode);
            this.kryptonGroup1.Panel.Controls.Add(this.KLblFormat);
            this.kryptonGroup1.Panel.Controls.Add(this.KLblFill);
            this.kryptonGroup1.Panel.Controls.Add(this.KComboBoxStyle);
            this.kryptonGroup1.Panel.Controls.Add(this.KLblPreview);
            this.kryptonGroup1.Panel.Controls.Add(this.flowLayoutPanel1);
            this.kryptonGroup1.Panel.Controls.Add(this.pictureBox1);
            this.kryptonGroup1.Panel.Controls.Add(this.KBtnCancel);
            this.kryptonGroup1.Panel.Controls.Add(this.KBtnOK);
            // 
            // CustomFormatRule
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.KBtnCancel;
            this.Controls.Add(this.kryptonGroup1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomFormatRule";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.CustomFormatStyle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KComboBoxStyle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KComboBoxFillMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).EndInit();
            this.kryptonGroup1.Panel.ResumeLayout(false);
            this.kryptonGroup1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).EndInit();
            this.kryptonGroup1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton KBtnOK;
        private ComponentFactory.Krypton.Toolkit.KryptonButton KBtnCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel KLblPreview;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonColorButton KColorBtnMin;
        private ComponentFactory.Krypton.Toolkit.KryptonColorButton KColorBtnMedium;
        private ComponentFactory.Krypton.Toolkit.KryptonColorButton KColorBtnMax;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox KComboBoxStyle;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel KLblFormat;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel KLblFill;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox KComboBoxFillMode;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup1;
    }
}