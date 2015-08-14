using ICSharpCode.Core;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using AD.Workbench.WinForms;
using AD.Workbench.Workbench;

namespace AD.Workbench.Serivces
{
    public static class ADService
    {

        public static IServiceContainer Services
        {
            get { return GetRequiredService<IServiceContainer>(); }
        }

        public static void InitializeForUnitTests()
        {
            var container = new ADServicesContainer();
            container.AddFallbackProvider(ServiceSingleton.FallbackServiceProvider);
            container.AddService(typeof(IPropertyService), new PropertyServiceImpl());
            container.AddService(typeof(IAddInTree), new AddInTreeImpl(null));
            ServiceSingleton.ServiceProvider = container;
        }

        public static T GetService<T>() where T : class
        {
            return ServiceSingleton.ServiceProvider.GetService<T>();
        }
        public static T GetRequiredService<T>() where T : class
        {
            return ServiceSingleton.ServiceProvider.GetRequiredService<T>();
        }
        public static Task<T> GetFutureService<T>() where T : class
        {
            return GetRequiredService<ADServicesContainer>().GetFutureService<T>();
        }
        public static ILoggingService Log
        {
            get { return GetRequiredService<ILoggingService>(); }
        }

        public static IMessageService MessageService
        {
            get { return GetRequiredService<IMessageService>(); }
        }

        public static ICSharpCode.Core.IResourceService ResourceService
        {
            get { return GetRequiredService<ICSharpCode.Core.IResourceService>(); }
        }
        public static IAnalyticsMonitor AnalyticsMonitor
        {
            get { return GetRequiredService<IAnalyticsMonitor>(); }
        }

        /// <summary>
        /// Gets the <see cref="IMessageLoop"/> representing the main UI thread.
        /// </summary>
        public static IMessageLoop MainThread
        {
            get { return GetRequiredService<IMessageLoop>(); }
        }
        public static IWorkbench Workbench
        {
            get { return GetRequiredService<IWorkbench>(); }
        }
        public static IWinFormsService WinForms
        {
            get { return GetRequiredService<IWinFormsService>(); }
        }

        public static IClipboard Clipboard
        {
            get { return GetRequiredService<IClipboard>(); }
        }

        /// <inheritdoc see="IOutputPad"/>
        public static IOutputPad OutputPad
        {
            get { return GetRequiredService<IOutputPad>(); }
        }
    }
}
