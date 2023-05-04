﻿namespace MLibTest.Demos.ViewModels
{
	using AvalonDock.Tools;
	using Microsoft.Win32;
    using MLibTest.Demos.ViewModels.AD;
    using MLibTest.Demos.ViewModels.Interfaces;
	using MLibTest.ViewModels.Base;
	using MWindowInterfacesLib.MsgBox.Enums;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
	using System.Windows.Input;

	#region Helper Test Classes
	/// <summary>
	/// This class is uses to create a type safe list
	/// of enumeration members for selection in combobox.
	/// </summary>
	public class MessageImageCollection
	{
		public string Name { get; set; }
		public MsgBoxImage EnumKey { get; set; }
	}

	/// <summary>
	/// Test class to enumerate over message box buttons enumeration.
	/// </summary>
	public class MessageButtonCollection
	{
		public string Name { get; set; }
		public MsgBoxButtons EnumKey { get; set; }
	}

	/// <summary>
	/// Test class to enumerate over message box result enumeration.
	/// The <seealso cref="MsgBoxResult"/> enumeration is used to define
	/// a default button (if any).
	/// </summary>
	public class MessageResultCollection
	{
		public string Name { get; set; }
		public MsgBoxResult EnumKey { get; set; }
	}

	/// <summary>
	/// Test class to enumerate over languages (and their locale) that
	/// are supported with specific (non-English) button and tool tip strings.
	/// 
	/// The class definition is based on BCP 47 which in turn is used to
	/// set the UI and thread culture (which in turn selects the correct string resource in MsgBox assembly).
	/// </summary>
	public class LanguageCollection
	{
		public string Language { get; set; }
		public string Locale { get; set; }
		public string Name { get; set; }

		/// <summary>
		/// Get BCP47 language tag for this language
		/// See also http://en.wikipedia.org/wiki/IETF_language_tag
		/// </summary>
		public string BCP47
		{
			get
			{
				if (string.IsNullOrEmpty(this.Locale) == false)
					return String.Format("{0}-{1}", this.Language, this.Locale);
				else
					return String.Format("{0}", this.Language);
			}
		}

		/// <summary>
		/// Get BCP47 language tag for this language
		/// See also http://en.wikipedia.org/wiki/IETF_language_tag
		/// </summary>
		public string DisplayName
		{
			get
			{
				return String.Format("{0} {1}", this.Name, this.BCP47);
			}
		}
	}
	#endregion Helper Test Classes

	/// <summary>
	/// The WorkSpaceViewModel implements AvalonDock demo specific properties, events and methods.
	/// </summary>
	internal class WorkSpaceViewModel : MLibTest.ViewModels.Base.ViewModelBase, IWorkSpaceViewModel
	{
		#region fields
		private readonly ObservableCollection<DocumentViewModel> _files = new ObservableCollection<DocumentViewModel>();
		private ToolViewModel[] _tools = null;

		private ICommand _openCommand = null;
		private ICommand _newCommand = null;

		private FileStatsViewModel _fileStats = null;
		private ColorPickerViewModel _ColorPicker = null;
		private Tool1_ViewModel _Tool1;
		private Tool2_ViewModel _Tool2;
		private Tool3_ViewModel _Tool3;

		private DocumentViewModel _activeDocument = null;

		private int _newDocumentCounter = 0;
		#endregion fields

		#region constructors
		/// <summary>
		/// Class constructor
		/// </summary>
		public WorkSpaceViewModel()
		{
		}
		#endregion constructors

		/// <summary>
		/// Event is raised when AvalonDock (or the user) selects a new document.
		/// </summary>
		public event EventHandler ActiveDocumentChanged;

		#region Properties
		/// <summary>
		/// Gets/Sets the currently active document.
		/// </summary>
		public DocumentViewModel ActiveDocument
		{
			get => _activeDocument;
			set             // This can also be set by the user via the view
			{
				if (_activeDocument != value)
				{
					_activeDocument = value;
					RaisePropertyChanged(nameof(ActiveDocument));

                    ActiveDocumentChanged?.Invoke(this, EventArgs.Empty);
                }
			}
		}

		/// <summary>
		/// Gets a collection of all currently available document viewmodels
		/// </summary>
		public IEnumerable<DocumentViewModel> Files
		{
			get
			{
				return _files;
			}
		}

		/// <summary>
		/// Gets an enumeration of all currently available tool window viewmodels.
		/// </summary>
		public IEnumerable<ToolViewModel> Tools
		{
			get
			{
				if (_tools == null)
					_tools = new ToolViewModel[] { FileStats, ColorPicker, _Tool1, _Tool2, _Tool3 };

				return _tools;
			}
		}

		/// <summary>Closing all documents without user interaction to support reload of layout via menu.</summary>
		public void CloseAllDocuments()
		{
			ActiveDocument = null;
			_files.Clear();
		}

		/// <summary>
		/// Gets an instance of the file stats tool window viewmodels.
		/// </summary>
		public FileStatsViewModel FileStats
		{
			get
			{
				if (_fileStats == null)
					_fileStats = new FileStatsViewModel(this as IWorkSpaceViewModel);

				return _fileStats;
			}
		}

		/// <summary>
		/// Gets an instance of the color picker tool window viewmodels.
		/// </summary>
		public ColorPickerViewModel ColorPicker
		{
			get
			{
				if (_ColorPicker == null)
					_ColorPicker = new ColorPickerViewModel(this as IWorkSpaceViewModel);

				return _ColorPicker;
			}
		}

		/// <summary>
		/// Gets an instance of the tool1 tool window viewmodel.
		/// </summary>
		public Tool1_ViewModel Tool1
		{
			get
			{
				if (_Tool1 == null)
					_Tool1 = new Tool1_ViewModel(this as IWorkSpaceViewModel);

				return _Tool1;
			}
		}

		/// <summary>
		/// Gets an instance of the tool2 tool window viewmodel.
		/// </summary>
		public Tool2_ViewModel Tool2
		{
			get
			{
				if (_Tool2 == null)
					_Tool2 = new Tool2_ViewModel(this as IWorkSpaceViewModel);

				return _Tool2;
			}
		}

		/// <summary>
		/// Gets an instance of the tool3 tool window viewmodel.
		/// </summary>
		public Tool3_ViewModel Tool3
		{
			get
			{
				if (_Tool3 == null)
					_Tool3 = new Tool3_ViewModel(this as IWorkSpaceViewModel);

				return _Tool3;
			}
		}

		/// <summary>
		/// Gets a open document command to open files from the file system.
		/// </summary>
		public ICommand OpenCommand
		{
			get
			{
				if (_openCommand == null)
				{
					_openCommand = new RelayCommand<object>(async (p) => await OnOpenAsync(p), (p) => CanOpen(p));
				}

				return _openCommand;
			}
		}

		/// <summary>
		/// Gets a new document command to create new documents from scratch.
		/// </summary>
		public ICommand NewCommand
		{
			get
			{
				if (_newCommand == null)
				{
					_newCommand = new RelayCommand<object>((p) => OnNew(p), (p) => CanNew(p));
				}

				return _newCommand;
			}
		}
		#endregion Properties

		#region methods
		/// <summary>
		/// Checks if a document can be closed and asks the user whether
		/// to save before closing if the document appears to be dirty.
		/// </summary>
		/// <param name="fileToClose"></param>
		public void Close(DocumentViewModel fileToClose)
		{
			if (fileToClose.IsDirty)
			{
				var res = MessageBox.Show(string.Format("Save changes for file '{0}'?", fileToClose.FileName), "AvalonDock Test App", MessageBoxButton.YesNoCancel);
				if (res == MessageBoxResult.Cancel)
					return;

				if (res == MessageBoxResult.Yes)
				{
					Save(fileToClose);
				}
			}

			_files.Remove(fileToClose);
		}

		/// <summary>
		/// Add a new document viewmodel into the collection of files.
		/// </summary>
		/// <param name="fileToAdd"></param>
		public void AddFile(DocumentViewModel fileToAdd)
		{
			if (fileToAdd == null) return;

			// Don't add this twice
			if (_files.Any(f => f.ContentId == fileToAdd.ContentId))
				return;

			_files.Add(fileToAdd);
		}

		/// <summary>
		/// Saves a document and resets the dirty flag.
		/// </summary>
		/// <param name="fileToSave"></param>
		/// <param name="saveAsFlag"></param>
		public void Save(DocumentViewModel fileToSave, bool saveAsFlag = false)
		{
			if (fileToSave.FilePath == null || saveAsFlag)
			{
				var dlg = new SaveFileDialog();
				if (dlg.ShowDialog().GetValueOrDefault())
					fileToSave.FilePath = dlg.SafeFileName;
			}

			System.IO.File.WriteAllText(fileToSave.FilePath, fileToSave.TextContent);
			ActiveDocument.IsDirty = false;
		}

		#region OpenCommand
		/// <summary>
		/// Determines if application can currently open a document or not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private bool CanOpen(object parameter)
		{
			return true;
		}

		private async Task OnOpenAsync(object parameter)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog().GetValueOrDefault())
			{
				var fileViewModel = await OpenAsync(dlg.FileName);
				ActiveDocument = fileViewModel;
			}
		}

		/// <summary>
		/// Open a file and return its content in a viewmodel.
		/// </summary>
		/// <param name="filepath"></param>
		/// <returns></returns>
		public async Task<DocumentViewModel> OpenAsync(string filepath)
		{
			// Check if we have already loaded this file and return it if so
			var fileViewModel = _files.FirstOrDefault(fm => fm.FilePath == filepath);
			if (fileViewModel != null)
				return fileViewModel;

			fileViewModel = new DocumentViewModel(this as IWorkSpaceViewModel, filepath, true);
			bool result = await fileViewModel.OpenFileAsync(filepath);

			if (result)
			{
				_files.Add(fileViewModel);
				return fileViewModel;
			}

			return null;
		}
		#endregion  OpenCommand

		#region NewCommand
		/// <summary>
		/// Determines if application can currently create a new document or not.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private bool CanNew(object parameter)
		{
			return true;
		}

		private void OnNew(object parameter)
		{
			string path = string.Format("Untitled{0}.txt", _newDocumentCounter++);

			var newFile = new DocumentViewModel(this as IWorkSpaceViewModel, path, false);
			_files.Add(newFile);
			ActiveDocument = newFile;
		}

		#endregion
		#endregion methods
	}
}
