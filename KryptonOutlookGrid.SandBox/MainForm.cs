using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid;
using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.CustomsColumns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace KryptonOutlookGrid.SandBox
{
    public partial class MainForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        static Random rand = new Random();

        DateTime GetRandomDate(DateTime dtStart, DateTime dtEnd)
        {
            int cdayRange = (dtEnd - dtStart).Days;

            return dtStart.AddDays(rand.NextDouble() * cdayRange);
        }

        private void LoadData()
        {
            //Setup Rows
            OutlookGridRow row = new OutlookGridRow();
            List<OutlookGridRow> l = new List<OutlookGridRow>();
            OutlookGrid1.SuspendLayout();
            OutlookGrid1.ClearInternalRows();


            Random random = new Random();
            //.Next permet de retourner un nombre aléatoire contenu dans la plage spécifiée entre parenthèses.
            XmlDocument doc = new XmlDocument();
            doc.Load("invoices.xml");
            IFormatProvider culture = new CultureInfo("en-US", true);
            foreach (XmlNode customer in doc.SelectNodes("//invoice"))
            {
                try
                {
                    row = new OutlookGridRow();
                    row.CreateCells(OutlookGrid1, new object[] {
                    customer["CustomerID"].InnerText,
                    customer["CustomerName"].InnerText,
                    customer["Address"].InnerText,
                    customer["City"].InnerText,
                    new TextAndImage(customer["Country"].InnerText,GetFlag(customer["Country"].InnerText)),
                    DateTime.Parse(customer["OrderDate"].InnerText,culture),
                    customer["ProductName"].InnerText,  
                    double.Parse(customer["Price"].InnerText, CultureInfo.InvariantCulture), //We put a float the formatting in design does the rest
                    (double)random.Next(101) /100
                });

                    OutlookGridRow row2 = new OutlookGridRow();
                    row2.CreateCells(OutlookGrid1, new object[] {"1","test","11 test avenue","TestCity",  new TextAndImage("test",null),
                    DateTime.Now,
                    "Por",
                    double.Parse("11.5", CultureInfo.InvariantCulture),
                     (double)random.Next(101) /100
                });
                    ((KryptonDataGridViewTreeTextCell)row2.Cells[1]).UpdateStyle();
                    row.Nodes.Add(row2);

                    //TODO improve that
                    ((KryptonDataGridViewTreeTextCell)row.Cells[1]).UpdateStyle();
                    l.Add(row);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gasp...Something went wrong ! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            OutlookGrid1.ResumeLayout();
            OutlookGrid1.AssignRows(l);
            OutlookGrid1.ForceRefreshGroupBox();
            OutlookGrid1.Fill();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            OutlookGrid1.GroupBox = KryptonOutlookGridGroupBox1;
            OutlookGrid1.RegisterGroupBoxEvents();

            DataGridViewSetup setup = new DataGridViewSetup();
            setup.SetupDataGridView(this.OutlookGrid1, true);

            LoadData();
        }

        private Image GetFlag(string country)
        {
            switch (country)
            {
                case "France":
                    return Properties.Resources.flag_france;
                case "Germany":
                    return Properties.Resources.flag_germany;
                default:
                    return null;
            }
        }

        private void OutlookGrid1_Resize(object sender, EventArgs e)
        {
            int PreferredTotalWidth = 0;
            //Calculate the total preferred width
            foreach (DataGridViewColumn c in OutlookGrid1.Columns)
            {
                PreferredTotalWidth += Math.Min(c.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true), 250);
            }

            if (OutlookGrid1.Width > PreferredTotalWidth)
            {
                OutlookGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                OutlookGrid1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
            else
            {
                OutlookGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                foreach (DataGridViewColumn c in OutlookGrid1.Columns)
                {
                    c.Width = Math.Min(c.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true), 250);
                }
            }
        }

        private void OutlookGrid1_GroupImageClick(object sender, OutlookGridGroupImageEventArgs e)
        {
            MessageBox.Show("Group Image clicked for group row : " + e.Row.Group.Text);
        }

        private void buttonSpecHeaderGroup1_Click(object sender, EventArgs e)
        {
            DataGridViewSetup setup = new DataGridViewSetup();
            setup.SetupDataGridView(this.OutlookGrid1, true);
            LoadData();

        }

        private void buttonSpecHeaderGroup2_Click(object sender, EventArgs e)
        {
            OutlookGrid1.PersistConfiguration(Application.StartupPath + "grid.xml");
        }

        private void buttonSpecHeaderGroup3_Click(object sender, EventArgs e)
        {
            OutlookGrid1.ExpandNodeAll();
        }
    }
}

