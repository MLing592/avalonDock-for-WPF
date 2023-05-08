| Downloads                                                                                                                                               | NuGet Packages
| :------------------------------------------------------------------------------------------------------------------------------------------------------ | :--------------------------------------------------------------------------------
| [![NuGet](https://img.shields.io/nuget/dt/Dirkster.AvalonDock.svg)](http://nuget.org/packages/Dirkster.AvalonDock)                                      | [Dirkster.AvalonDock](http://nuget.org/packages/Dirkster.AvalonDock)
| [![NuGet](https://img.shields.io/nuget/dt/Dirkster.AvalonDock.Themes.Aero.svg)](http://nuget.org/packages/Dirkster.AvalonDock.Themes.Aero)              | [Dirkster.AvalonDock.Themes.Aero](http://nuget.org/packages/Dirkster.AvalonDock.Themes.Aero)
| [![NuGet](https://img.shields.io/nuget/dt/Dirkster.AvalonDock.Themes.Expression.svg)](http://nuget.org/packages/Dirkster.AvalonDock.Themes.Expression)  | [Dirkster.AvalonDock.Themes.Expression](http://nuget.org/packages/Dirkster.AvalonDock.Themes.Expression)
| [![NuGet](https://img.shields.io/nuget/dt/Dirkster.AvalonDock.Themes.Metro.svg)](http://nuget.org/packages/Dirkster.AvalonDock.Themes.Metro)            | [Dirkster.AvalonDock.Themes.Metro](http://nuget.org/packages/Dirkster.AvalonDock.Themes.Metro)
| [![NuGet](https://img.shields.io/nuget/dt/Dirkster.AvalonDock.Themes.VS2010.svg)](http://nuget.org/packages/Dirkster.AvalonDock.Themes.VS2010)          | [Dirkster.AvalonDock.Themes.VS2010](http://nuget.org/packages/Dirkster.AvalonDock.Themes.VS2010)
| [![NuGet](https://img.shields.io/nuget/dt/Dirkster.AvalonDock.Themes.VS2013.svg)](http://nuget.org/packages/Dirkster.AvalonDock.Themes.VS2013)          | [Dirkster.AvalonDock.Themes.VS2013](http://nuget.org/packages/Dirkster.AvalonDock.Themes.VS2013) (see [Wiki](https://github.com/Dirkster99/AvalonDock/wiki/WPF-VS-2013-Dark-Light-Demo-Client) )

![Net4](https://badgen.net/badge/Framework/.Net&nbsp;4/blue) ![NetCore3](https://badgen.net/badge/Framework/NetCore&nbsp;3/blue) ![Net4](https://badgen.net/badge/Framework/.NET&nbsp;5/blue)

## 引用源地址：
[![Dikster99 AvalonDock 4.72](https://github.com/Dirkster99/AvalonDock/tree/v4.72.0)]

# Feature Added - 基于Dirkster.AvalonDock的VS2013主题修改而来的VS2022主题

如图所示案例为/Test Projects/VS2022Test，全图为VS2022，红框内为Test程序，进行对比。

<table width="100%">
   <tr>
      <td>theme</td>
      <td>display</td>
   </tr>
   <tr>
      <td>Dark</td>
      <td><img src="https://gitee.com/liu_meiling/avalon-dock-for-wpf/raw/master/source/VS2022-Dark.png" width="400"></td>
   </tr>
   <tr>
      <td>Light</td>
      <td><img src="https://gitee.com/liu_meiling/avalon-dock-for-wpf/raw/master/source/VS2022-Light.png" width="400"></td>

   </tr>
   <tr>
      <td>Blue</td>
      <td><img src="https://gitee.com/liu_meiling/avalon-dock-for-wpf/raw/master/source/VS2022-Blue.png" width="400"></td>
   </tr>

</table>

## Theming

Using the *AvalonDock.Themes.VS2012* theme is very easy with *Dark* and *Light* and *Blue* themes.
Just load *Light* or *Dark* or *Blue* brush resources in you resource dictionary to take advantage of existing definitions.

```XAML
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/AvalonDock.Themes.VS2022;component/DarkBrushs.xaml" />
    </ResourceDictionary.MergedDictionaries>
```

```XAML
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/AvalonDock.Themes.VS2022;component/LightBrushs.xaml" />
    </ResourceDictionary.MergedDictionaries>
```

```XAML
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/AvalonDock.Themes.VS2022;component/BlueBrushs.xaml" />
    </ResourceDictionary.MergedDictionaries>
```

These definitions do not theme all controls used within this library. You should use a standard theming library, such as:
- [MahApps.Metro](https://github.com/MahApps/MahApps.Metro),
- [MLib](https://github.com/Dirkster99/MLib), or
- [MUI](https://github.com/firstfloorsoftware/mui)

to also theme standard elements, such as, button and textblock etc.

# update History

## 2023-5-4

- 1.新建AvalonDock.Themes.VS2022，修改AvalonDock.Themes.VS2022/BlueBrushs，DarkBrushs，LightBrushs主题部分配色，增加彩色标签等颜色
- 2.修改AvalonDock.Themes.VS2022/Themes/Generic.xaml下AvalonDockThemeVS2022DocumentPaneControlStyle属性ItemContainerStyle，修改了文档选项卡样式，左侧增加彩色标签
- 3.修改AvalonDock.Themes.VS2022/Themes/Generic.xaml下AvalonDockThemeVS2022DocumentContextMenu，增加文档右键菜单-"设置选项卡颜色"
- 4.修改AvalonDock/Controls/LayoutDocumentItem.cs，增加ChangeTabColorCommand命令

## 2023-5-8

- 1.修改AvalonDock/Controls/DocumentPaneTabPanel.cs，原版容器为Panel，由一行显示文档，多余隐藏，现在使用WrapPanel作为面板容器
- 2.新增AvalonDock/Controls/LayoutDocumentItem.cs，增加ChangeTabColorCommand命令对应方法，寻找资源并统一替换颜色，实现统一更换选项卡颜色
- 3.新增右键LayoutDocument时，LayoutDocument被选中





