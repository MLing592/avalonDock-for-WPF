using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;
using AakStudio.Shell.UI.Themes.AvalonDock;
using AvalonDock;
using AvalonDock.Layout.Serialization;
using AvalonDock.Themes;

namespace DockingDemo;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private static readonly List<Theme> Themes = new()
    {
        new VisualStudio2019Blue(),
        new VisualStudio2019Dark(),
        new VisualStudio2019Light(),
        new VisualStudio2022Blue(),
        new VisualStudio2022Dark(),
        new VisualStudio2022Light()
    };

    public MainWindow()
    {
        InitializeComponent();
    }

    private void ThemeItem_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            Application.Current.Resources.MergedDictionaries[0].Source = Themes[int.Parse((string)menuItem.Tag)].GetResourceUri();
        }
    }
	string fileName = "layout.xml";
	private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
	{
		using (TextWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
		{
			XmlLayoutSerializer xmlLayout = new XmlLayoutSerializer(dockingManager);
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Encoding = Encoding.UTF8;
			XmlWriter xmlWriter = XmlWriter.Create(writer, settings);
			xmlLayout.Serialize(xmlWriter);
			xmlWriter.Close();
			writer.Close();
		}
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		if (File.Exists(fileName))
		{
			XmlLayoutSerializer serializer = new XmlLayoutSerializer(dockingManager);
			using (var fs = new FileStream(fileName, FileMode.Open))
			{
				serializer.Deserialize(fs);
			}
		}

	}
}
