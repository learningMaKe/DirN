using DirN.Utils;
using DirN.Utils.CommandLine;
using DirN.Utils.DirManager;
using DirN.Utils.KManager;
using DirN.Utils.NgManager;
using DirN.Utils.PreManager;
using DirN.Views;
using Fclp;
using Fclp.Internals;
using System.Configuration;
using System.Data;
using System.Windows;
using Wpf.Ui;

namespace DirN;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public ApplicationParameter CommandLineArgs { get; private set; } = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        var res = ParameterParser.Parse(e.Args,out var paramter);
        if (!res.HasErrors)
        {
            CommandLineArgs = paramter;
        }

        base.OnStartup(e);
    }


    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterInstance(CommandLineArgs);
        containerRegistry.RegisterSingleton<IContentDialogService, ContentDialogService>();
        containerRegistry.RegisterSingleton<KeyManager>();
        containerRegistry.RegisterSingleton<DirectoryManager>();
        containerRegistry.RegisterSingleton<PreviewerManager>();
        containerRegistry.RegisterSingleton<INodeGraphicsManager, NodeGraphicsManager>();

    }
}

