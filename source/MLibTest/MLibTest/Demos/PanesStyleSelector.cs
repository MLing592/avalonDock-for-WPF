﻿namespace MLibTest.Demos
{
	using System.Windows.Controls;
	using System.Windows;
	using MLibTest.Demos.ViewModels.AD;

	class PanesStyleSelector : StyleSelector
	{
		public Style ToolStyle
		{
			get;
			set;
		}

		public Style FileStyle
		{
			get;
			set;
		}

		public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
		{
			if (item is ToolViewModel)
				return ToolStyle;

			if (item is DocumentViewModel)
				return FileStyle;

			return base.SelectStyle(item, container);
		}
	}
}
