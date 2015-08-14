using ICSharpCode.Core;
using AD.Workbench.Logging;
using System;
using AD.Workbench.Serivces;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Resources;
using AD.Workbench.Workbench;
using System.Windows.Input;

namespace AD.Workbench.Startup
{
    public class Startup
    {
        [STAThread]
        static void Main(string[] args)
        {

            StartupWorkbench();

        }

        public static void StartupWorkbench()
        {
//             if (!CheckEnivronment())
//             {
//                 return;
//             }

            try
            {
                Run();
            }
            catch (Exception ex)
            {
                Application.Run(new ExceptionBox(ex, "未处理的异常终止了AD", true));
            }
        }

        static bool CheckEnivronment()
        {
            if (Environment.Version < new Version(4, 0, 30319, 17626))
            {
                MessageBox.Show("此版本AD需要 .NET 4.5 RC。 当前环境为: " + Environment.Version, "AD");
                return false;
            }
            string windir = Environment.GetFolderPath(Environment.SpecialFolder.Windows, Environment.SpecialFolderOption.DoNotVerify);
            if (Environment.GetEnvironmentVariable("WINDIR") != windir)
            {
                Environment.SetEnvironmentVariable("WINDIR", windir);
            }
            return true;
        }

        static void Run()
        {
            LoggingService.Info("应用程序启动...");
            StartupSettings startup = InitStartupSetting();
            InitSerivces(startup);
            RunWorkbench();
        }

        static void RunWorkbench()
        {
            LoggingService.Info("加载程序界面...");
            WorkbenchStartup wbs = new WorkbenchStartup();
            wbs.InitializeWorkbench();
            wbs.Run();
        }
        static StartupSettings InitStartupSetting()
        {
            StartupSettings startup = new StartupSettings();
            Assembly exe = typeof(Startup).Assembly;
            startup.ApplicationRootPath = Path.Combine(Path.GetDirectoryName(exe.Location), "");
            startup.AllowUserAddIns = true;

            string configDirectory = ConfigurationManager.AppSettings["settingsPath"];
            if (String.IsNullOrEmpty(configDirectory))
            {
                startup.ConfigDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                       "AD" + RevisionClass.Major);
            }
            else
            {
                startup.ConfigDirectory = Path.Combine(Path.GetDirectoryName(exe.Location), configDirectory);
            }

            startup.AddAddInsFromDirectory(Path.Combine(startup.ApplicationRootPath, "."));
            return startup;
        }

        static void InitSerivces(StartupSettings properties)
        {
            ADServicesContainer container = new ADServicesContainer();
            container.AddService(typeof(ILoggingService), new Log4netLoggingService());
            container.AddService(typeof(IMessageService), new ADMessageService());
            container.AddService(typeof(IAnalyticsMonitor), new AnalyticsMonitor());
            ServiceSingleton.ServiceProvider = container;

            LoggingService.Info("应用程序初始化...");
            ExceptionBox.RegisterExceptionBoxForUnhandledExceptions();

            CoreStartup startup = new CoreStartup(properties.ApplicationName);

            if (properties.ApplicationRootPath != null)
            {
                FileUtility.ApplicationRootPath = properties.ApplicationRootPath;
            }

            string configDirectory = properties.ConfigDirectory;
            string dataDirectory = properties.DataDirectory;
            string propertiesName;
            if (properties.PropertiesName != null)
            {
                propertiesName = properties.PropertiesName;
            }
            else
            {
                propertiesName = properties.ApplicationName + "Properties";
            }

            if (properties.ApplicationRootPath != null)
            {
                FileUtility.ApplicationRootPath = properties.ApplicationRootPath;
            }

            if (configDirectory == null)
                configDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                               properties.ApplicationName);
            var propertyService = new AD.Workbench.Serivces.PropertyService(
                DirectoryName.Create(configDirectory),
                DirectoryName.Create(dataDirectory ?? Path.Combine(FileUtility.ApplicationRootPath, "data")),
                propertiesName);

            startup.StartCoreServices(propertyService);
            Assembly exe = Assembly.Load(properties.ResourceAssemblyName);
            ADService.ResourceService.RegisterNeutralStrings(new ResourceManager("AD.Workbench.Resources.StringResources", exe));
            ADService.ResourceService.RegisterNeutralImages(new ResourceManager("AD.Workbench.Resources.BitmapResources", exe));
//             CommandWrapper.LinkCommandCreator = (link => new LinkCommand(link));
            CommandWrapper.WellKnownCommandCreator = ICSharpCode.Core.Presentation.MenuService.GetKnownCommand;
            CommandWrapper.RegisterConditionRequerySuggestedHandler = (eh => CommandManager.RequerySuggested += eh);
            CommandWrapper.UnregisterConditionRequerySuggestedHandler = (eh => CommandManager.RequerySuggested -= eh);

            LoggingService.Info("查找插件...");
            foreach (string file in properties.addInFiles)
            {
                startup.AddAddInFile(file);
            }
            foreach (string dir in properties.addInDirectories)
            {
                startup.AddAddInsFromDirectory(dir);
            }

            if (properties.AllowAddInConfigurationAndExternalAddIns)
            {
                startup.ConfigureExternalAddIns(Path.Combine(configDirectory, "AddIns.xml"));
            }
            if (properties.AllowUserAddIns)
            {
                startup.ConfigureUserAddIns(Path.Combine(configDirectory, "AddInInstallTemp"),
                    Path.Combine(configDirectory, "AddIns"));
            }

            LoggingService.Info("加载插件树...");
            startup.RunInitialization();
            LoggingService.Info("初始化应用程序完成.");
        }
    }
}
