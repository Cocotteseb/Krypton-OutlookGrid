using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting
{
    public interface IFormatParams : ICloneable
    {
        void Persist(XmlWriter writer);
    }
}
