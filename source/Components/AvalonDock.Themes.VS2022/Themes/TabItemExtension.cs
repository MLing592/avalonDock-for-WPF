using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace AvalonDock.Themes.Themes
{
	public static class TabItemExtension
	{
		#region fields
		/// <summary>
		/// Backing store for LoadLayoutCommand dependency property
		/// </summary>
		public static readonly DependencyProperty LeftAreaColorProperty =
			DependencyProperty.RegisterAttached("LeftAreaColor",
			typeof(Brush),
			typeof(TabItemExtension),
			new PropertyMetadata(null, TabItemExtension.OnLeftAreaColorChanged));

		#endregion fields

		#region methods
		#region Load Layout
		/// <summary>
		/// Standard get method of <seealso cref="LeftAreaColorProperty"/> dependency property.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Brush GetLeftAreaColor(DependencyObject obj)
		{
			return (Brush)obj.GetValue(LeftAreaColorProperty);
		}

		/// <summary>
		/// Standard set method of <seealso cref="LeftAreaColorProperty"/> dependency property.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="value"></param>
		public static void SetLeftAreaColor(DependencyObject obj, Brush value)
		{
			obj.SetValue(LeftAreaColorProperty, value);
		}

		/// <summary>
		/// This method is executed if a <seealso cref="LoadLayoutCommandProperty"/> dependency property
		/// is about to change its value (eg: The framewark assigns bindings).
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void OnLeftAreaColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}

		#endregion Load Layout

		#endregion methods
	}
}
