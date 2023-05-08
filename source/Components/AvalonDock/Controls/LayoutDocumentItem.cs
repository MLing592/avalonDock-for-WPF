/************************************************************************
   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at https://opensource.org/licenses/MS-PL
 ************************************************************************/

using AvalonDock.Commands;
using AvalonDock.Layout;
using AvalonDock.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace AvalonDock.Controls
{
	/// <inheritdoc />
	/// <summary>
	/// This is a wrapper for around the custom Pane content view of <see cref="LayoutElement"/>.
	/// Implements the <see cref="AvalonDock.Controls.LayoutItem" />
	///
	/// All DPs implemented here can be bound in a corresponding style to control parameters
	/// in dependency properties via binding in MVVM.
	/// </summary>
	/// <seealso cref="AvalonDock.Controls.LayoutItem" />
	public class LayoutDocumentItem : LayoutItem
	{
		#region fields

		private LayoutDocument _document;   // The content of this item
		#region Add
		private ICommand _defaultChangeTabColorCommand;
		#endregion

		#endregion fields

		#region Constructors

		/// <summary>Class constructor</summary>
		internal LayoutDocumentItem()
		{
		}

		#endregion Constructors

		#region Properties

		#region Description

		/// <summary><see cref="Description"/> dependency property.</summary>
		public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(string), typeof(LayoutDocumentItem),
					new FrameworkPropertyMetadata(null, OnDescriptionChanged));

		/// <summary>Gets/sets the description to display (in the <see cref="NavigatorWindow"/>) for the Pane item.</summary>
		[Bindable(true), Description("Gets/sets the description to display (in the NavigatorWindow) for the Pane item."), Category("Other")]
		public string Description
		{
			get => (string)GetValue(DescriptionProperty);
			set => SetValue(DescriptionProperty, value);
		}

		/// <summary>Handles changes to the <see cref="Description"/> property.</summary>
		private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((LayoutDocumentItem)d).OnDescriptionChanged(e);

		/// <summary>Provides derived classes an opportunity to handle changes to the <see cref="Description"/> property.</summary>
		protected virtual void OnDescriptionChanged(DependencyPropertyChangedEventArgs e) => _document.Description = (string)e.NewValue;

		#endregion Description

		#region ChangeTabColorCommand Add

		/// <summary><see cref="ChangeTabColorCommand"/> dependency property.</summary>
		public static readonly DependencyProperty ChangeTabColorCommandProperty = DependencyProperty.Register(nameof(ChangeTabColorCommand), typeof(ICommand), typeof(LayoutItem),
				new FrameworkPropertyMetadata(null, OnChangeTabColorCommandChanged, CoerceChangeTabColorCommandValue));

		/// <summary>Gets/sets the command to execute when user click the Pane close button.</summary>
		[Bindable(true), Description("Gets/sets the command to execute when user click the Pane command of change tab color ."), Category("Other")]
		public ICommand ChangeTabColorCommand
		{
			get => (ICommand)GetValue(ChangeTabColorCommandProperty);
			set => SetValue(ChangeTabColorCommandProperty, value);
		}

		/// <summary>Handles changes to the <see cref="ChangeTabColorCommand"/> property.</summary>
		/// <summary>属性值更改时引发的回调函数 <see cref="ChangeTabColorCommand"/> property.</summary>
		private static void OnChangeTabColorCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}

		/// <summary>Coerces the <see cref="ChangeTabColorCommand"/>  value.</summary>
		/// <summary>用来强制限制属性值的范围或取值 <see cref="ChangeTabColorCommand"/>  value.</summary>
		private static object CoerceChangeTabColorCommandValue(DependencyObject d, object value) => value;

		/// <summary>
		/// 是否可以执行
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private bool CanExecuteChangeTabColorCommand(object parameter) => _document != null;

		/// <summary>
		/// 执行命令
		/// </summary>
		/// <param name="parameter"></param>
		protected virtual void ExecuteChangeTabColorCommand(object parameter)
		{
			//获取根组件DockingManager
			DockingManager dockingManager = null;
			var parent = this.LayoutElement.Parent;
			while (parent != null)
			{
				parent = parent.Parent;
				if (parent is LayoutRoot root)
				{
					dockingManager = root.Manager;
					break;
				}
			}
			if (dockingManager == null) return;

			//转换brush
			var brush = parameter as SolidColorBrush;
			if (brush == null) return;

			//更换资源颜色
			var resourceDict = dockingManager.Resources;
			Action<IEnumerable<ResourceDictionary>,int> loopThroughDicts = null;
			loopThroughDicts = (dicts,level) =>
			{
				if (level > 3) return;
				foreach (ResourceDictionary dict in dicts)
				{
					var leftKey = dict.Keys.OfType<ComponentResourceKey>().FirstOrDefault(k => k.ResourceId.ToString() == "DocumentWellTabUnselectedRectangleBackground");
					var middleKey = dict.Keys.OfType<ComponentResourceKey>().FirstOrDefault(k => k.ResourceId.ToString() == "DocumentWellTabSelectedActiveBackground");
					if (leftKey != null && middleKey != null)
					{
						dict[leftKey] = brush;
						dict[middleKey] = brush;
						return;
					}

					if (dict.MergedDictionaries.Count > 0)
					{
						loopThroughDicts(dict.MergedDictionaries.OfType<ResourceDictionary>(),level +1);
					}
				}
			};
			loopThroughDicts(resourceDict.MergedDictionaries.OfType<ResourceDictionary>(),1);

			// 触发刷新操作
			//dockingManager.ApplyTemplate();
			//dockingManager.UpdateLayout();

			//根据关联的TabItem修改属性，但不适合此种情况
			//if (parameter is SolidColorBrush colorBrush)
			//{
			//	LayoutDocument Pane = this.LayoutElement as LayoutDocument;
			//	if (Pane != null)
			//	{
			//		Pane.TabItem.Background = colorBrush;
			//	}
			//}
		}


		#endregion ChangeTabColorCommand

		#endregion Properties

		#region Overrides

		/// <inheritdoc />
		protected override void Close()
		{
			if (_document.Root?.Manager == null) return;
			var dockingManager = _document.Root.Manager;
			dockingManager.ExecuteCloseCommand(_document);
		}

		/// <inheritdoc />
		protected override void OnVisibilityChanged()
		{
			if (_document?.Root != null)
			{
				_document.IsVisible = Visibility == Visibility.Visible;
				if (_document.Parent is LayoutDocumentPane layoutDocumentPane) layoutDocumentPane.ComputeVisibility();
			}
			base.OnVisibilityChanged();
		}

		/// <inheritdoc />
		internal override void Attach(LayoutContent model)
		{
			_document = model as LayoutDocument;
			base.Attach(model);
		}

		/// <inheritdoc />
		internal override void Detach()
		{
			_document = null;
			base.Detach();
		}

		protected override bool CanExecuteDockAsDocumentCommand()
		{
			return (LayoutElement != null && LayoutElement.FindParent<LayoutDocumentPane>() != null && LayoutElement.IsFloating);
		}


		/// <summary>
		/// 初始化命令
		/// </summary>
		protected override void InitDefaultCommands()
		{
			#region Add
			_defaultChangeTabColorCommand = new RelayCommand<object>(ExecuteChangeTabColorCommand, CanExecuteChangeTabColorCommand);
			#endregion
			base.InitDefaultCommands();
		}

		/// <summary>
		/// 卸载控件时清除命令的默认绑定
		/// </summary>
		protected override void ClearDefaultBindings()
		{
			#region Add
			if (ChangeTabColorCommand == _defaultChangeTabColorCommand) BindingOperations.ClearBinding(this, ChangeTabColorCommandProperty);
			#endregion
			base.ClearDefaultBindings();
		}

		/// <summary>
		/// 控件首次加载或需重新绑定时调用，设置命令的默认绑定
		/// </summary>
		protected override void SetDefaultBindings()
		{
			#region Add
			if (ChangeTabColorCommand == null) ChangeTabColorCommand = _defaultChangeTabColorCommand;
			#endregion
			base.SetDefaultBindings();
		}
		#endregion Overrides
	}
}