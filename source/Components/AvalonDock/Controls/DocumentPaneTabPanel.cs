/************************************************************************
   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at https://opensource.org/licenses/MS-PL
 ************************************************************************/

using AvalonDock.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AvalonDock.Controls
{
	/// <summary>
	/// Provides a panel that contains the TabItem Headers of the <see cref="LayoutDocumentPaneControl"/>.
	/// 除VS2022外使用
	/// </summary>
	public class DocumentPaneTabPanel : Panel
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
			#region 原Panel
			Size desideredSize = new Size();
			foreach (FrameworkElement child in Children)//遍历所有子元素，并计算其大小需求
			{
				child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));// 测量子元素的大小需求
				desideredSize.Width += child.DesiredSize.Width;//累加子元素的宽度作为这个面板的理想宽度

				desideredSize.Height = Math.Max(desideredSize.Height, child.DesiredSize.Height);//计算出最大需要的高度，作为这个面板的理想高度
			}

			return new Size(Math.Min(desideredSize.Width, availableSize.Width), desideredSize.Height);//返回一个新尺寸，即最小控件面板和其子元素所需的尺寸
			#endregion
		}

		//确定每个子元素的最终位置，并设置其大小。按照布局策略排列子元素
		//在方法内部应该：
		//1.遍历所有子元素，并使用它们的MeasuredSize和位置偏移计算出每个子元素应该渲染的最终区域。
		//2.对于所有子元素，将这些最终区域指定为每个子元素的Arrange方法的输入参数之一。在这里，可以调整子元素的大小或位置，以满足特定的布局需求。此外，您还可以根据子元素的其他属性进行自定义处理，例如旋转或倾斜。
		//3.最后，返回Panel所需的最终尺寸，以便系统了解如何呈现整个Panel。
		protected override Size ArrangeOverride(Size finalSize)
		{
			#region 原Panel
			//遍历所有可见子元素
		   var visibleChildren = Children.Cast<UIElement>().Where(ch => ch.Visibility != System.Windows.Visibility.Collapsed);
			// 定义偏移量，并标记是否跳过所有其他元素
			var offset = 0.0;
			var skipAllOthers = false;
			foreach (TabItem doc in visibleChildren)
			{
				// 如果已经遍历到最后，或者在添加当前项之后剩余的空间不足，则标记后面的元素为隐藏
				if (skipAllOthers || offset + doc.DesiredSize.Width > finalSize.Width)
				{
					// 在当前项为选定内容并且未可见时，在布局中重新排列其他项以使其能够正确显示
					bool isLayoutContentSelected = false;
					var layoutContent = doc.Content as LayoutContent;
					if (layoutContent != null)
						isLayoutContentSelected = layoutContent.IsSelected;
					if (isLayoutContentSelected && !doc.IsVisible)
					{
						// 找到父容器和父选择器
						var parentContainer = layoutContent.Parent as ILayoutContainer;
						var parentSelector = layoutContent.Parent as ILayoutContentSelector;
						var parentPane = layoutContent.Parent as ILayoutPane;
						int contentIndex = parentSelector.IndexOf(layoutContent);
						if (contentIndex > 0 &&
							parentContainer.ChildrenCount > 1)
						{
							// 将选定的布局内容移到第一个位置
							parentPane.MoveChild(contentIndex, 0);
							parentSelector.SelectedContentIndex = 0;
							// 重新排列子元素以反映新的布局
							return ArrangeOverride(finalSize);
						}
					}
					doc.Visibility = System.Windows.Visibility.Hidden;
					skipAllOthers = true;
				}
				// 否则，将元素设置为可见，并使用其所需大小来安排它
				else
				{
					doc.Visibility = System.Windows.Visibility.Visible;
					doc.Arrange(new Rect(offset, 0.0, doc.DesiredSize.Width, finalSize.Height));
					offset += doc.ActualWidth + doc.Margin.Left + doc.Margin.Right;
				}
			}
			// 返回最终尺寸
			return finalSize;
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


	//VS2022使用
	public class DocumentPaneTabWrapPanel : WrapPanel
	{
		public DocumentPaneTabWrapPanel() 
		{
			this.FlowDirection = System.Windows.FlowDirection.LeftToRight; 
		}

		protected override Size MeasureOverride(Size constraint)
		{
			double currentLineLength = 0;                // 当前行已占用的宽度
			double maxLineHeight = 0;                    // 所有行总计的高度
			double currentLineMaxHeight = 0;             // 当前行最大高度
			var infiniteConstraintHgt = new Size(constraint.Width, double.PositiveInfinity);   // 限制宽度，但允许无限高度的约束

			// 遍历所有子元素 
			foreach (UIElement child in InternalChildren)
			{
				// 如果该子元素不可见，则跳过
				if (!child.IsVisible) { continue; }

				// 判断是否为固定与非固定分界线，或元素累计宽度超出容器宽度
				var current = (child as TabItem)?.Content as LayoutDocument;
				var ine = Math.Max(Children.IndexOf(child) - 1, 0);
				var front = (Children[ine] as TabItem).Content as LayoutDocument;
				if ((current?.IsFixed == false && front.IsFixed == true) || (currentLineLength + child.DesiredSize.Width > constraint.Width))
				{
					// 如果需要换行，则更新总高度、当前行最大高度和当前行宽度
					maxLineHeight += currentLineMaxHeight;
					currentLineLength = 0;
					currentLineMaxHeight = 0;
				}

				// 测量子元素，使用允许无限高度的约束
				child.Measure(infiniteConstraintHgt);

				// 更新当前行宽度和最大高度
				var childDesiredSize = child.DesiredSize;
				currentLineLength += childDesiredSize.Width;
				currentLineMaxHeight = Math.Max(currentLineMaxHeight, childDesiredSize.Height);
			}

			// 将最后一行的最大高度加入总高度中，得到最终的尺寸大小
			maxLineHeight += currentLineMaxHeight;
			var finalSize = new Size(constraint.Width, Math.Min(maxLineHeight, constraint.Height));

			return finalSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			var baseSize = base.ArrangeOverride(finalSize);//获取原生Wrappanel计算
			#region 自定义Wrappanel
			double currentX = 0; // 当前控件将要布局的子元素的 X 轴坐标
			double currentY = 0; // 当前控件将要布局的子元素的 Y 轴坐标
			double currentLineLength = 0; // 当前行已有元素的总长度
			double currentLineMaxHeight = 0; // 当前行的最大高度
			int currentLineNumber = 0; // 当前行编号

			foreach (UIElement child in InternalChildren)
			{
				if (!child.IsVisible) { continue; } // 如果不需要重新排列，则继续处理下一个子元素

				double width = child.DesiredSize.Width;
				double height = child.DesiredSize.Height;
				bool needsNewLine = false;

				var current = (child as TabItem)?.Content as LayoutDocument;
				var ine = Math.Max(Children.IndexOf(child) - 1, 0);
				var front = (Children[ine] as TabItem)?.Content as LayoutDocument;
				//执行完LayoutDocumentItem的OnExecuteFixCommand方法后才开始MeasureOverride和ArrangeOverride
				//判断当前是否到了固定与非固定分界点
				//当固定/取消固定标签时，current为选中固定/取消固定的标签，front为index为0即也是选中固定/取消固定的标签
				if ((currentLineLength + width > finalSize.Width) || (current?.IsFixed == false && front?.IsFixed == true))
				{
					needsNewLine = true;
					currentLineNumber++; // 控件换行了
					currentY += currentLineMaxHeight; // 更新 Y 坐标
					currentLineLength = 0; // 当前行长度清零
					currentLineMaxHeight = 0; // 当前行最大高度清零					
				}

				if (!needsNewLine) // 如果不需要换行，则排在当前行的尾部
				{
					child.Arrange(new Rect(currentX, currentY, width, height));
					currentX += width;
					currentLineLength += width;
					currentLineMaxHeight = Math.Max(currentLineMaxHeight, height);
				}
				else // 否则将子元素放到下一行的开头
				{
					child.Arrange(new Rect(0, currentY + currentLineMaxHeight, width, height)); // 这里 X 坐标直接设为 0 就行，因为肯定是左对齐的
					currentX = width;
					currentLineLength = width;
					currentLineMaxHeight = height;
					baseSize.Height += currentLineMaxHeight; //增加一行的高度
				}
			}
			#endregion
			return baseSize;

		} 
		
	}

	//测试使用
	public class CustomWrapPanel : Panel
	{
		private List<bool> m_visibleList; // 保存子元素是否可见
		private List<List<FrameworkElement>> m_rowList; // 每一行上的元素集合

		public CustomWrapPanel()
		{
			m_visibleList = new List<bool>();
			m_rowList = new List<List<FrameworkElement>>();
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			m_visibleList.Clear();
			m_rowList.Clear();

			double totalWidth = 0;
			int curRow = 0;
			double curLineHeight = 0;

			foreach (FrameworkElement child in Children)
			{
				// 获取子元素的期望大小，并将可视状态添加到列表中
				child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
				m_visibleList.Add(true);

				// 如果当前行宽度加上该元素宽度已经超出最大宽度，则进入下一行
				if (totalWidth + child.DesiredSize.Width > availableSize.Width)
				{
					// 如果存在行高，则记录该行中元素的总高度
					if (curRow > 0)
					{
						//m_rowList[curRow - 1].ForEach(c => c.DesiredSize = new Size(c.DesiredSize.Width, curLineHeight));
					}

					// 进入下一行
					curRow++;
					totalWidth = 0;
					curLineHeight = 0;
				}

				// 将元素添加到当前行中，并累加该行宽度
				if (curRow == m_rowList.Count)
				{
					m_rowList.Add(new List<FrameworkElement>());
				}
				m_rowList[curRow].Add(child);
				totalWidth += child.DesiredSize.Width;

				// 如果该元素高度大于当前行高度，则更新当前行高度
				double childHeight = child.DesiredSize.Height + child.Margin.Top + child.Margin.Bottom;
				curLineHeight = Math.Max(curLineHeight, childHeight);
			}

			// 更新最后一行的高度
			if (curRow > 0)
			{
				//m_rowList[curRow - 1].ForEach(c => c.DesiredSize = new Size(c.DesiredSize.Width, curLineHeight));
			}

			// 返回总大小，这里只考虑所有元素存在一个列中的情况，在实际排列中，将根据实际位置和大小计算布局
			double desiredHeight = m_rowList.Count * curLineHeight;
			return new Size(availableSize.Width, desiredHeight);
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			double curX = 0;
			double curY = 0;
			double curLineHeight = 0;

			for (int i = 0; i < Children.Count; i++)
			{
				var child = Children[i] as FrameworkElement;

				// 设置子元素位置
				if (m_visibleList[i])
				{
					child.Arrange(new Rect(curX + child.Margin.Left, curY + child.Margin.Top,
						child.DesiredSize.Width, child.DesiredSize.Height));

					// 更新下一个元素的 x 和 y 坐标值，以及当前行高度
					curX += child.DesiredSize.Width + child.Margin.Left + child.Margin.Right;
					curLineHeight = Math.Max(curLineHeight, child.DesiredSize.Height + child.Margin.Top + child.Margin.Bottom);
				}

				// 如果该元素是该行最后一个，则进入下一行
				if (i == Children.Count - 1 || curX + Children[i + 1].DesiredSize.Width > finalSize.Width)
				{
					curX = 0;
					curY += curLineHeight;
					curLineHeight = 0;
				}
			}

			return finalSize;
		}
	}



}