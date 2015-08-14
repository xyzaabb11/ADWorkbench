using System.Collections.Generic;
using System.Linq;
using ICSharpCode.Core;
using ICSharpCode.Core.Presentation;
using System;
using System.Diagnostics;
using System.Windows;
using AD.Workbench.Serivces;

namespace AD.Workbench.Workbench
{
    /// <summary>
    /// ADWorkbench.xaml 的交互逻辑
    /// </summary>
    public partial class ADWorkbench : Window, IWorkbench, System.Windows.Forms.IWin32Window
    {
        const string MainMenuPath = "/AD/Workbench/MainMenu";
        const string viewContentPath = "/AD/Workbench/Pads";

        public event EventHandler<ViewContentEventArgs> ViewOpened;
        public event EventHandler ActiveWorkbenchWindowChanged;
        public event EventHandler ActiveViewContentChanged;
        public event EventHandler ActiveContentChanged;
        internal void OnViewOpened(ViewContentEventArgs e)
        {
            if (ViewOpened != null)
            {
                ViewOpened(this, e);
            }
        }
        public event EventHandler<ViewContentEventArgs> ViewClosed;

        internal void OnViewClosed(ViewContentEventArgs e)
        {
            if (ViewClosed != null)
            {
                ViewClosed(this, e);
            }
        }
        public ADWorkbench()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            var padDescriptors = AddInTree.BuildItems<PadDescriptor>(viewContentPath, this, false);
            ((ADServicesContainer)ADService.Services).AddFallbackProvider(new PadServiceProvider(padDescriptors));
            foreach (PadDescriptor content in padDescriptors)
            {
                ShowPad(content);
            }
            mainMenu.ItemsSource = MenuService.CreateMenuItems(this, this, MainMenuPath, activationMethod: "MainMenu", immediatelyExpandMenuBuildersForShortcuts: true);
        }

        public Window MainWindow
        {
            get { return this; }
        }

        IWorkbenchLayout workbenchLayout;

        public IWorkbenchLayout WorkbenchLayout
        {
            get
            {
                return workbenchLayout;
            }
            set
            {
                ADService.MainThread.VerifyAccess();

                if (workbenchLayout != null)
                {
                    workbenchLayout.ActiveContentChanged -= OnActiveWindowChanged;
                    workbenchLayout.Detach();
                }
                if (value != null)
                {
                    value.Attach(this);
                    value.ActiveContentChanged += OnActiveWindowChanged;
                }
                workbenchLayout = value;
                OnActiveWindowChanged(null, null);
            }
        }

        IWorkbenchWindow activeWorkbenchWindow;

        public IWorkbenchWindow ActiveWorkbenchWindow
        {
            get
            {
                ADService.MainThread.VerifyAccess();
                return activeWorkbenchWindow;
            }
            private set
            {
                if (activeWorkbenchWindow != value)
                {
                    if (activeWorkbenchWindow != null)
                    {
                        activeWorkbenchWindow.ActiveViewContentChanged -= WorkbenchWindowActiveViewContentChanged;
                    }

                    activeWorkbenchWindow = value;

                    if (value != null)
                    {
                        value.ActiveViewContentChanged += WorkbenchWindowActiveViewContentChanged;
                    }

                    if (ActiveWorkbenchWindowChanged != null)
                    {
                        ActiveWorkbenchWindowChanged(this, EventArgs.Empty);
                    }
                    WorkbenchWindowActiveViewContentChanged(null, null);
                }
            }
        }

        void WorkbenchWindowActiveViewContentChanged(object sender, EventArgs e)
        {
            if (workbenchLayout != null)
            {
                // update ActiveViewContent
                IWorkbenchWindow window = this.ActiveWorkbenchWindow;
                if (window != null)
                    this.ActiveViewContent = window.ActiveViewContent;
                else
                    this.ActiveViewContent = null;

                // update ActiveContent
                this.ActiveContent = workbenchLayout.ActiveContent;
            }
        }

        bool activeWindowWasChanged;

        void OnActiveWindowChanged(object sender, EventArgs e)
        {
            if (activeWindowWasChanged)
                return;
            activeWindowWasChanged = true;
            Dispatcher.BeginInvoke(new Action(
                delegate
                {
                    activeWindowWasChanged = false;
                    if (workbenchLayout != null)
                    {
                        this.ActiveContent = workbenchLayout.ActiveContent;
                        this.ActiveWorkbenchWindow = workbenchLayout.ActiveWorkbenchWindow;
                    }
                    else
                    {
                        this.ActiveContent = null;
                        this.ActiveWorkbenchWindow = null;
                    }
                }));
        }

        IViewContent activeViewContent;

        public IViewContent ActiveViewContent
        {
            get
            {
                ADService.MainThread.VerifyAccess();
                return activeViewContent;
            }
            private set
            {
                if (activeViewContent != value)
                {
                    activeViewContent = value;

                    if (ActiveViewContentChanged != null)
                    {
                        ActiveViewContentChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        IServiceProvider activeContent;

        public IServiceProvider ActiveContent
        {
            get
            {
                ADService.MainThread.VerifyAccess();
                return activeContent;
            }
            private set
            {
                if (activeContent != value)
                {
                    activeContent = value;

                    if (ActiveContentChanged != null)
                    {
                        ActiveContentChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public void ShowView(IViewContent content)
        {
            ShowView(content, true);
        }

        public void ShowView(IViewContent content, bool switchToOpenedView)
        {
            if (content == null)
                throw new ArgumentNullException("content");
            if (ViewContentCollection.Contains(content))
                throw new ArgumentException("ViewContent was already shown");
            System.Diagnostics.Debug.Assert(WorkbenchLayout != null);

//             LoadViewContentMemento(content);

            WorkbenchLayout.ShowView(content, switchToOpenedView);
        }

        List<PadDescriptor> padDescriptorCollection = new List<PadDescriptor>();
        public IList<PadDescriptor> PadContentCollection
        {
            get
            {
                ADService.MainThread.VerifyAccess();
                return padDescriptorCollection.AsReadOnly();
            }
        }

        public void ShowPad(PadDescriptor content)
        {
            ADService.MainThread.VerifyAccess();
            if (content == null)
                throw new ArgumentNullException("content");
            if (padDescriptorCollection.Contains(content))
                throw new ArgumentException("Pad is already loaded");

            padDescriptorCollection.Add(content);

            if (WorkbenchLayout != null)
            {
                WorkbenchLayout.ShowPad(content);
            }
        }

        public PadDescriptor GetPad(Type type)
        {
            ADService.MainThread.VerifyAccess();
            if (type == null)
                throw new ArgumentNullException("type");
            foreach (PadDescriptor pad in PadContentCollection)
            {
                if (pad.Class == type.FullName)
                {
                    return pad;
                }
            }
            return null;
        }
        public void ActivatePad(PadDescriptor content)
        {
            if (workbenchLayout != null)
                workbenchLayout.ActivatePad(content);
        }


        public System.Collections.Generic.ICollection<IViewContent> ViewContentCollection
        {
            get { return WorkbenchWindowCollection.SelectMany(w => w.ViewContents).ToList().AsReadOnly(); }
        }


        public System.Collections.Generic.IList<IWorkbenchWindow> WorkbenchWindowCollection
        {
            get
            {
                if (workbenchLayout != null)
                    return workbenchLayout.WorkbenchWindows;
                else
                    return new IWorkbenchWindow[0];
            }
        }

        [Conditional("DEBUG")]
        internal static void FocusDebug(string format, params object[] args)
        {
#if DEBUG
// 			if (enableFocusDebugOutput)
			LoggingService.DebugFormatted(format, args);
#endif
        }

        public System.Windows.Forms.IWin32Window MainWin32Window { get { return this; } }

        IntPtr System.Windows.Forms.IWin32Window.Handle
        {
            get
            {
                var wnd = System.Windows.PresentationSource.FromVisual(this) as System.Windows.Interop.IWin32Window;
                if (wnd != null)
                    return wnd.Handle;
                else
                    return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Stores the memento for the view content.
        /// Such mementos are automatically loaded in ShowView().
        /// </summary>
        public void StoreMemento(IViewContent viewContent)
        {
//             IMementoCapable mementoCapable = viewContent.GetService<IMementoCapable>();
//             if (mementoCapable != null && LoadDocumentProperties)
//             {
//                 if (viewContent.PrimaryFileName == null)
//                     return;
// 
//                 string key = GetMementoKeyName(viewContent);
//                 LoggingService.Debug("Saving memento of '" + viewContent.ToString() + "' to key '" + key + "'");
// 
//                 Properties memento = mementoCapable.CreateMemento();
//                 Properties p = this.LoadOrCreateViewContentMementos();
//                 p.SetNestedProperties(key, memento);
//                 FileUtility.ObservedSave(new NamedFileOperationDelegate(p.Save), this.ViewContentMementosFileName, FileErrorPolicy.Inform);
//             }
        }

        internal static string GetElementName(object element)
        {
            if (element == null)
                return "<null>";
            else
                return element.GetType().FullName + ": " + element.ToString();
        }


    }
}
