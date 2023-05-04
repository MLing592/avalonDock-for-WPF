﻿namespace MLibTest
{
	using MLib.Interfaces;
	using MLibTest.Models;
	using MLibTest.ViewModels;
	using MWindowInterfacesLib.Interfaces;
	using Settings.Interfaces;
	using Settings.UserProfile;
	using System;
	using System.Diagnostics;
	using System.Globalization;
	using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		#region fields
		private readonly ViewModels.AppViewModel _appVM;
		private readonly MainWindow _mainWindow;
		#endregion fields

		#region constructors
		static App()
		{
			// Create service model to ensure available services
			ServiceInjector.InjectServices();
		}

		/// <summary>
		/// Class constructor
		/// </summary>
		public App()
		{
			_mainWindow = new MainWindow();
			_appVM = new ViewModels.AppViewModel(new AppLifeCycleViewModel());
			LayoutLoaded = new LayoutLoader(@".\AvalonDock.Layout.config");
		}
		#endregion constructors

		/// <summary>
		/// Gets an object that loads the AvalonDock Xml layout string
		/// in an aysnchronous background task.
		/// </summary>
		internal LayoutLoader LayoutLoaded { get; set; }

		#region methods
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			// Set shutdown mode here (and reset further below) to enable showing custom dialogs (messageboxes)
			// durring start-up without shutting down application when the custom dialogs (messagebox) closes
			ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;

			var settings = GetService<ISettingsManager>(); // add the default themes
			var appearance = GetService<IAppearanceManager>();

			try
			{
				// Construct Application ViewModel and mainWindow
				_appVM.AppLifeCycle.LoadConfigOnAppStartup(settings, appearance);

				appearance.SetTheme(settings.Themes
									, settings.Options.GetOptionValue<string>("Appearance", "ThemeDisplayName")
									, ThemeViewModel.GetCurrentAccentColor(settings));

				_appVM.SetSessionData(settings.SessionData);
				_appVM.AppTheme.InitThemes(settings);

				// Customize services specific items for this application
				// Program message box service for Modern UI (Metro Light and Dark)
				//                var msgBox = GetService<IMessageBoxService>();
				//                msgBox.Style = MsgBoxStyle.WPFThemed;

			}
			catch (Exception exp)
			{
				Debug.WriteLine(exp);
			}

			try
			{
				var selectedLanguage = settings.Options.GetOptionValue<string>("Options", "LanguageSelected");

				Thread.CurrentThread.CurrentCulture = new CultureInfo(selectedLanguage);
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);
			}
			catch (Exception exp)
			{
				Debug.WriteLine(exp);
			}

			// Create the optional appearance viewmodel and apply
			// current settings to start-up with correct colors etc...
			////var appearSettings = new AppearanceViewModel(settings.Themes);
			////appearSettings.ApplyOptionsFromModel(settings.Options);

			// Initialize WPF theming and friends ...
			var defaultTheme = settings.Options.GetOptionValue<string>("Appearance", "ThemeDisplayName");
			_appVM.InitForMainWindow(appearance, defaultTheme);

			if (MainWindow != null && _appVM != null)
			{
				ConstructMainWindowSession(_appVM, _mainWindow, settings);

				// and show it to the user ...
				MainWindow.Loaded += MainWindow_LoadedAsync;
				MainWindow.Closing += OnClosing;

				// When the ViewModel asks to be closed, close the window.
				// Source: http://msdn.microsoft.com/en-us/magazine/dd419663.aspx
				MainWindow.Closed += delegate
				{
					// Save session data and close application
					OnClosed(_appVM, _mainWindow);

                    if (_appVM is IDisposable dispose)
                        dispose.Dispose();

                    _mainWindow.DataContext = null;
 				};

				MainWindow.Show();
			}

			AppCore.CreateAppDataFolder();
		}

		/// <summary>
		/// Method is invoked when the mainwindow is loaded and visble to the user.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void MainWindow_LoadedAsync(object sender, RoutedEventArgs e)
		{
			try
			{
				ShutdownMode = ShutdownMode.OnLastWindowClose;
			}
			catch (Exception exp)
			{
				Debug.WriteLine(exp);
			}

			await LayoutLoaded.LoadLayoutAsync();

			// Load and layout AvalonDock elements when MainWindow has loaded
			_mainWindow.OnLoadLayoutAsync();
		}

		/// <summary>
		/// COnstruct MainWindow an attach datacontext to it.
		/// </summary>
		/// <param name="workSpace"></param>
		/// <param name="win"></param>
		private void ConstructMainWindowSession(AppViewModel workSpace,
												IViewSize win,
												ISettingsManager settings)
		{
			try
			{
				Application.Current.MainWindow = _mainWindow;
				_mainWindow.DataContext = _appVM;

                // Establish command binding to accept user input via commanding framework
                // workSpace.InitCommandBinding(win);

                settings.SessionData.WindowPosSz.TryGetValue(settings.SessionData.MainWindowName
                                                           , out ViewPosSizeModel viewSz);

                viewSz.SetWindowsState(win);

				string lastActiveFile = settings.SessionData.LastActiveSolution;

				MainWindow mainWin = win as MainWindow;
			}
			catch (Exception exp)
			{
				Debug.WriteLine(exp);
			}
		}

		/// <summary>
		/// Save session data on closing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
                if (base.MainWindow.DataContext is AppViewModel wsVM)
                {
                    if (MainWindow is IMetroWindow MainWindowCanClose)
                    {
                        if (MainWindowCanClose.IsContentDialogVisible == true)
                        {
                            e.Cancel = true;     // Lets not close with open dialog
                            return;
                        }
                    }

                    // Close all open files and check whether application is ready to close
                    if (wsVM.AppLifeCycle.Exit_CheckConditions(wsVM) == true)
                    {
                        // (other than exception and error handling)
                        wsVM.AppLifeCycle.OnRequestClose(true);

                        _mainWindow.OnSaveLayout();
                        e.Cancel = false;
                    }
                    else
                    {
                        wsVM.AppLifeCycle.CancelShutDown();
                        e.Cancel = true;
                    }
                }
            }
			catch (Exception exp)
			{
				Debug.WriteLine(exp);
			}
		}

		/// <summary>
		/// Execute closing function and persist session data to be reloaded on next restart
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClosed(AppViewModel appVM, IViewSize win)
		{
			try
			{
				var settings = GetService<ISettingsManager>();

                settings.SessionData.WindowPosSz.TryGetValue(settings.SessionData.MainWindowName
                                                           , out ViewPosSizeModel viewSz);
                viewSz.GetWindowsState(win);

				_appVM.GetSessionData(settings.SessionData);

				// Save/initialize program options that determine global programm behaviour
				appVM.AppLifeCycle.SaveConfigOnAppClosed(win);
			}
			catch (Exception exp)
			{
				Debug.WriteLine(exp);
			}
		}

		/// <summary>
		/// This method gets the service locator instance
		/// that is  used in turn to get an application specific service instance.
		/// </summary>
		/// <typeparam name="TServiceContract"></typeparam>
		/// <returns></returns>
		private TServiceContract GetService<TServiceContract>() where TServiceContract : class
		{
			return ServiceLocator.ServiceContainer.Instance.GetService<TServiceContract>();
		}
		#endregion methods
	}
}
