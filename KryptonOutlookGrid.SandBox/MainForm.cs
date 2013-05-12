using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid;
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
    public partial class MainForm : Form
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

            OutlookGridRow row = new OutlookGridRow();
            List<OutlookGridRow> l = new List<OutlookGridRow>();

            OutlookGrid1.SuspendLayout();

            OutlookGrid1.ClearInternalRows();

            //Setup Rows

            Random random = new Random();
            int rndnbr = 0;
            string tt = null;

            //.Next permet de retourner un nombre aléatoire contenu dans la plage spécifiée entre parenthèses.
            XmlDocument doc = new XmlDocument();
            doc.Load("invoices.xml");
            IFormatProvider  culture   = new CultureInfo("en-US", true);
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
                    customer["Country"].InnerText,
                    DateTime.Parse(customer["OrderDate"].InnerText,culture),
                    customer["ProductName"].InnerText,  
                    customer["Price"].InnerText
                });
                    l.Add(row);
                }
                catch (Exception xe) { }
            }
            //    for (int i = 0; i <= 10000; i++)
            //    {
            //        rndnbr = random.Next(0, 10000);
            ////        tt = random.Next(Strings.Asc("A"), Strings.Asc("Z") + 1);

            //        row = new OutlookGridRow();
            //       row.CreateCells(OutlookGrid1, new object[] {
            //    rndnbr.ToString,
            //    tt,
            //    RandomDate(DateTime.Today.AddDays(-600))
            //});
            //        l.Add(row);
            //    }
            OutlookGrid1.ResumeLayout();

            OutlookGrid1.AssignRows(l);
            OutlookGrid1.ForceRefreshGroupBox();
            OutlookGrid1.Fill();
        }

        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void OutlookGrid1_Resize(object sender, EventArgs e)
        {
            //int PreferredTotalWidth = 0;
            //foreach (DataGridViewColumn c in OutlookGrid1.Columns)
            //{
            //    PreferredTotalWidth += Math.Min(c.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true), 250);
            //}

            //if (OutlookGrid1.Width > PreferredTotalWidth)
            //{
            //    OutlookGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //}
            //else
            //{
            //    OutlookGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //    foreach (DataGridViewColumn c in OutlookGrid1.Columns)
            //    {
            //        c.Width = Math.Min(c.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true), 250);
            //        //TODO imposr un max
            //    }
            //}
        }
    }
}

