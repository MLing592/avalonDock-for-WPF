﻿using System.ComponentModel;

namespace AvalonDock.MVVMTestApp
{
	class ViewModelBase : INotifyPropertyChanged
	{

		protected virtual void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}


		public event PropertyChangedEventHandler PropertyChanged;
	}
}
