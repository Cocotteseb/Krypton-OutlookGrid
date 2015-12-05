using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting
{
    public class ConditionalFormatting
    {
        public string ColumnName { get; set; }
        public EnumConditionalFormatType FormatType { get; set; }
        public IFormatParams FormatParams {get; set; }
        public double minValue { get; set; }
        public double maxValue { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ConditionalFormatting() { }

        /// <summary>
        /// Constructor. (Only use for Context menu !)
        /// </summary>
        /// <param name="formatType">The conditional formatting type.</param>
        /// <param name="formatParams">The conditional formatting parameters.</param>
        public ConditionalFormatting(EnumConditionalFormatType formatType, IFormatParams formatParams)
        {
            FormatType = formatType;
            FormatParams = formatParams;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="columnName">Name of the column to apply the formatting.</param>
        /// <param name="formatType">The conditional formatting type.</param>
        /// <param name="formatParams">The conditional formatting parameters.</param>
        public ConditionalFormatting(string columnName, EnumConditionalFormatType formatType, IFormatParams formatParams)
        {
            ColumnName = columnName;
            FormatType = formatType;
            FormatParams = formatParams;
        }

        internal void Persist(XmlWriter writer)
        {
            writer.WriteStartElement("Condition");
            writer.WriteElementString("ColumnName", ColumnName);
            writer.WriteElementString("FormatType", FormatType.ToString());
            //tofo
            writer.WriteStartElement("FormatParams");
            FormatParams.Persist(writer);
            writer.WriteEndElement(); //FormatParams
            //No need to persist min/max Value.
            writer.WriteEndElement(); //Condition
        }



    }
}
