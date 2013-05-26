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

        private void MainForm_Load(object sender, EventArgs e)
        {
            OutlookGrid1.GroupBox = KryptonOutlookGridGroupBox1;
            OutlookGrid1.RegisterGroupBoxEvents();

            //Setup Columns
            OutlookGrid1.AddInternalColumn(ColumnCustomerID, new OutlookgGridDefaultGroup(null), SortOrder.None, false);
            OutlookGrid1.AddInternalColumn(ColumnCustomerName, new OutlookGridAlphabeticGroup(null), SortOrder.None, false);
            OutlookGrid1.AddInternalColumn(ColumnAddress, new OutlookgGridDefaultGroup(null), SortOrder.None, false);
            OutlookGrid1.AddInternalColumn(ColumnCity, new OutlookgGridDefaultGroup(null), SortOrder.None, false);
            OutlookGrid1.AddInternalColumn(ColumnCountry, new OutlookgGridDefaultGroup(null), SortOrder.None, false);
            OutlookGrid1.AddInternalColumn(ColumnOrderDate, new OutlookGridDateTimeGroup(null), SortOrder.None, false);
            OutlookGrid1.AddInternalColumn(ColumnProduct, new OutlookgGridDefaultGroup(null), SortOrder.None, false);
            OutlookGrid1.AddInternalColumn(ColumnPrice, new OutlookgGridDefaultGroup(null), SortOrder.None, false);
            OutlookGrid1.AddInternalColumn(SatisfactionColumn, new OutlookgGridDefaultGroup(null), SortOrder.None, false);

            //Setup Rows
            OutlookGridRow row = new OutlookGridRow();
            List<OutlookGridRow> l = new List<OutlookGridRow>();
            OutlookGrid1.SuspendLayout();
            OutlookGrid1.ClearInternalRows();
     

            Random random = new Random();
            int rndnbr = 0;
            string tt = null;

            //.Next permet de retourner un nombre aléatoire contenu dans la plage spécifiée entre parenthèses.
            XmlDocument doc = new XmlDocument();
            doc.Load("invoices.xml");
            IFormatProvider  culture  = new CultureInfo("en-US", true);
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
                    l.Add(row);
                }
                catch (Exception ex) {
                    MessageBox.Show("Gasp...Something went wrong ! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            OutlookGrid1.ResumeLayout();
            OutlookGrid1.AssignRows(l);
            OutlookGrid1.ForceRefreshGroupBox();
            OutlookGrid1.Fill();
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
    }
}

