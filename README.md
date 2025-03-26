
# ClearBlazor

A Blazor UI library with a difference. 

You may be wondering, 'Why another Blazor UI Library' since there are already plenty of good free libraries, such as [MudBlazor](https://MudBlazor.com) , [Blazorise](https://blazorise.com) , [Blazored](https://github.com/Blazored) , [MatBlazor](https://www.matblazor.com) , [Microsoft Fluent UI Blazor Components](https://github.com/microsoft/fluentui-blazor) and [Ant Design Blazor](https://antblazor.com) to name a few.

I have written a lot of WPF code and I am very familiar with how the WPF layout system works. Once learnt, it was quite easy to layout applications and was consistent in the way it worked.  (see [WPF Layout](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/layout?view=netframeworkdesktop-4.8))
Moving to Blazor I found the HTML/CSS Layout was tricky and inconsistent so hence this library. I got some ideas from this site. [WpfGridLayout](https://github.com/aboudoux/WpfGridLayout.Blazor)

WPF like layout controls that have been implemented are:

* Grid
* WrapPanel
* DockPanel
* StackPanel
* UniformGrid
* ScrollViewer

Many other controls have also been implemented. See the [DEMO](https://icy-sea-0e9ce5410.4.azurestaticapps.net)

# Getting started

The source code contains 3 simple example projects under the folder 'GettingStartedExamples'. They provide examples for a Blazor WASM project, a Blazor Server project and a Blazor Auto(Wasm and Server) project.
Note that Static Server Rendering (SSR) is not supported.

Perform the the following steps to setup a project for ClearBlazor:
1. Include the lastest ClearBlazor nuget package
2. Add '@using ClearBlazor' to the _Imports.razor file
3. In MainLayout.razor add the RootComponent as the base component and add other ClearBlazor components inside the RootComponent as required.
   

