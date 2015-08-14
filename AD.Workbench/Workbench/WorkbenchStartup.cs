using System.Threading;
using System.Windows.Input;
using System.Windows.Interop;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Startup;
using System;
using AD.Workbench.Pads.Output;
using AD.Workbench.Serivces;

namespace AD.Workbench.Workbench
{
    class WorkbenchStartup
    {
        App app; 
        public void InitializeWorkbench()
        {
            app = new App();
            System.Windows.Forms.Integration.WindowsFormsHost.EnableWindowsFormsInterop();
            ComponentDispatcher.ThreadIdle -= ComponentDispatcher_ThreadIdle; // ensure we don't register twice
            ComponentDispatcher.ThreadIdle += ComponentDispatcher_ThreadIdle;
//             LayoutConfiguration.LoadLayoutConfiguration();
            ADService.Services.AddService(typeof(IMessageLoop), new DispatcherMessageLoop(app.Dispatcher, SynchronizationContext.Current));
            InitializeWorkbench(new ADWorkbench(), new AvalonDockLayout());
        }

        static void ComponentDispatcher_ThreadIdle(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.RaiseIdle(e);
        }

        public void InitializeWorkbench(ADWorkbench workbench, IWorkbenchLayout layout)
        {
            ADService.Services.AddService(typeof(IWorkbench), workbench);
            
            workbench.Initialize();
            ADService.Services.AddService(typeof(IOutputPad), OutlinePad.Instance);
            workbench.WorkbenchLayout = layout;
        }

        public void Run()
        {
            foreach (ICommand command in AddInTree.BuildItems<ICommand>("/AD/Workbench/AutostartNothingLoaded", null, false))
            {
                try
                {
                    command.Execute(null);
                }
                catch (Exception ex)
                {
                    MessageService.ShowException(ex);
                }
            }
            app.Run(ADService.Workbench.MainWindow);
        }
    }
}
