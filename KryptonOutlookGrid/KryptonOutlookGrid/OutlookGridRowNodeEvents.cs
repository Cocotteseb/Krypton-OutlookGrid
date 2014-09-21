using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid
{
     public class OutlookGridRowNodeEventBase : EventArgs
	{
		private OutlookGridRow _row;

        public OutlookGridRowNodeEventBase(OutlookGridRow node)
		{
			this._row = node;
		}

        public OutlookGridRow Node
		{
			get { return _row; }
		}
	}
	public class CollapsingEventArgs : System.ComponentModel.CancelEventArgs
	{
        private OutlookGridRow _node;

		private CollapsingEventArgs() { }
        public CollapsingEventArgs(OutlookGridRow node)
			: base()
		{
			this._node = node;
		}
        public OutlookGridRow Node
		{
			get { return _node; }
		}

	}
    public class CollapsedEventArgs : OutlookGridRowNodeEventBase
	{
		public CollapsedEventArgs(OutlookGridRow node)
			: base(node)
		{
		}
	}

	public class ExpandingEventArgs:System.ComponentModel.CancelEventArgs
	{
		private OutlookGridRow _node;

		private ExpandingEventArgs() { }
		public ExpandingEventArgs(OutlookGridRow node):base()
		{
			this._node = node;
		}
		public OutlookGridRow Node
		{
			get { return _node; }
		}

	}
    public class ExpandedEventArgs : OutlookGridRowNodeEventBase
	{
		public ExpandedEventArgs(OutlookGridRow node):base(node)
		{
		}
	}

}
