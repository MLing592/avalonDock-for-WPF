﻿namespace MLibTest.Demos.ViewModels.AD
{
	internal class ToolViewModel : PaneViewModel
	{
		#region Fields
		private bool _isVisible = false;
		#endregion Fields

		#region constructors
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="name"></param>
		public ToolViewModel(string name)
		{
			Name = name;
			Title = name;
		}

		/// <summary>
		/// Hidden default class constructor
		/// </summary>
		protected ToolViewModel()
		{
		}
		#endregion constructors

		#region properties
		/// <summary>
		/// Gets the name of this tool window.
		/// </summary>
		public string Name { get; }

		#region IsVisible
		/// <summary>
		/// Gets/sets whether this tool window is visible or not.
		/// </summary>
		public bool IsVisible
		{
			get => _isVisible;
			set
			{
				if (_isVisible != value)
				{
					_isVisible = value;
					RaisePropertyChanged(() => IsVisible);
				}
			}
		}
		#endregion IsVisible
		#endregion properties
	}
}
