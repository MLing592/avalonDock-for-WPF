/************************************************************************
   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at https://opensource.org/licenses/MS-PL
 ************************************************************************/

using AvalonDock.Layout;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AvalonDock.Controls
{
	/// <summary>
	/// Provides a panel that contains the TabItem Headers of the <see cref="LayoutDocumentPaneControl"/>.
	/// 修改继承自Panel为Wrappanel
	/// </summary>
	public class DocumentPaneTabPanel : WrapPanel
	{
		#region Constructors

		/// <summary>
		/// Static constructor
		/// </summary>
		public DocumentPaneTabPanel()
		{
			this.FlowDirection = System.Windows.FlowDirection.LeftToRight;
		}

		#endregion Constructors

		#region Overrides

		//确定子元素大小需求并计算Panel大小,接收Size类型参数availableSize，表示Panel可用空间的大小
		//在方法内部，应该：
		//1.检查Panel是否为空以及存在子元素。如果没有子元素，您可以将可用大小作为理想大小返回，或者返回具有零高度和宽度的尺寸。
		//2.遍历子元素列表并使用每个子元素的DesiredSize属性来确定其测量大小向量。
		//3.基于子元素的大小计算出整个Panel的大小。
		protected override Size MeasureOverride(Size availableSize)
		{
			return base.MeasureOverride(availableSize);
			#region 原Panel代码
			//Size desideredSize = new Size();
			//foreach (FrameworkElement child in Children)//遍历所有子元素，并计算其大小需求
			//{
			//	child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));// 测量子元素的大小需求
			//	desideredSize.Width += child.DesiredSize.Width;//累加子元素的宽度作为这个面板的理想宽度

			//	desideredSize.Height = Math.Max(desideredSize.Height, child.DesiredSize.Height);//计算出最大需要的高度，作为这个面板的理想高度
			//}

			//return new Size(Math.Min(desideredSize.Width, availableSize.Width), desideredSize.Height);//返回一个新尺寸，即最小控件面板和其子元素所需的尺寸
			#endregion
		}

		//确定每个子元素的最终位置，并设置其大小。按照布局策略排列子元素
		//在方法内部应该：
		//1.遍历所有子元素，并使用它们的MeasuredSize和位置偏移计算出每个子元素应该渲染的最终区域。
		//2.对于所有子元素，将这些最终区域指定为每个子元素的Arrange方法的输入参数之一。在这里，可以调整子元素的大小或位置，以满足特定的布局需求。此外，您还可以根据子元素的其他属性进行自定义处理，例如旋转或倾斜。
		//3.最后，返回Panel所需的最终尺寸，以便系统了解如何呈现整个Panel。
		protected override Size ArrangeOverride(Size finalSize)
		{
			return base.ArrangeOverride(finalSize);
			#region 原Panel代码
			// 遍历所有可见子元素
			//var visibleChildren = Children.Cast<UIElement>().Where(ch => ch.Visibility != System.Windows.Visibility.Collapsed);
			//// 定义偏移量，并标记是否跳过所有其他元素
			//var offset = 0.0;
			//var skipAllOthers = false;
			//foreach (TabItem doc in visibleChildren)
			//{
			//	// 如果已经遍历到最后，或者在添加当前项之后剩余的空间不足，则标记后面的元素为隐藏
			//	if (skipAllOthers || offset + doc.DesiredSize.Width > finalSize.Width)
			//	{
			//		// 在当前项为选定内容并且未可见时，在布局中重新排列其他项以使其能够正确显示
			//		bool isLayoutContentSelected = false;
			//		var layoutContent = doc.Content as LayoutContent;
			//		if (layoutContent != null)
			//			isLayoutContentSelected = layoutContent.IsSelected;
			//		if (isLayoutContentSelected && !doc.IsVisible)
			//		{
			//			// 找到父容器和父选择器
			//			var parentContainer = layoutContent.Parent as ILayoutContainer;
			//			var parentSelector = layoutContent.Parent as ILayoutContentSelector;
			//			var parentPane = layoutContent.Parent as ILayoutPane;
			//			int contentIndex = parentSelector.IndexOf(layoutContent);
			//			if (contentIndex > 0 &&
			//				parentContainer.ChildrenCount > 1)
			//			{
			//				// 将选定的布局内容移到第一个位置
			//				parentPane.MoveChild(contentIndex, 0);
			//				parentSelector.SelectedContentIndex = 0;
			//				// 重新排列子元素以反映新的布局
			//				return ArrangeOverride(finalSize);
			//			}
			//		}
			//		doc.Visibility = System.Windows.Visibility.Hidden;
			//		skipAllOthers = true;
			//	}
			//	// 否则，将元素设置为可见，并使用其所需大小来安排它
			//	else
			//	{
			//		doc.Visibility = System.Windows.Visibility.Visible;
			//		doc.Arrange(new Rect(offset, 0.0, doc.DesiredSize.Width, finalSize.Height));
			//		offset += doc.ActualWidth + doc.Margin.Left + doc.Margin.Right;
			//	}
			//}
			//// 返回最终尺寸
			//return finalSize;
			#endregion
		}


		protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
		{
			//if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed &&
			//    LayoutDocumentTabItem.IsDraggingItem())
			//{
			//    var contentModel = LayoutDocumentTabItem.GetDraggingItem().Model;
			//    var manager = contentModel.Root.Manager;
			//    LayoutDocumentTabItem.ResetDraggingItem();
			//    System.Diagnostics.Trace.WriteLine("OnMouseLeave()");

			//    manager.StartDraggingFloatingWindowForContent(contentModel);
			//}

			base.OnMouseLeave(e);
		}

		#endregion Overrides

	}
}