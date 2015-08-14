using System.ComponentModel.Design;
using ICSharpCode.Core.Presentation;
using System;

namespace AD.Workbench.Workbench
{
    public abstract class AbstractViewContent : IViewContent
    {
        public abstract object Control
        {
            get;
        }

        public virtual object InitiallyFocusedControl
        {
            get { return null; }
        }
        #region IDisposable
        public event EventHandler Disposed;

        bool isDisposed;

        public bool IsDisposed
        {
            get { return isDisposed; }
        }

        public virtual void Dispose()
        {
            _workbenchWindow = null;
//             UnregisterOnActiveViewContentChanged();
//             if (AutomaticallyRegisterViewOnFiles)
//             {
//                 this.Files.Clear();
//             }
            isDisposed = true;
            if (Disposed != null)
            {
                Disposed(this, EventArgs.Empty);
            }
        }
        #endregion

        #region IsDirty
        bool IsDirtyInternal
        {
            get
            {
//                 foreach (OpenedFile file in this.Files)
//                 {
//                     if (file.IsDirty)
//                         return true;
//                 }
                return false;
            }
        }

        bool isDirty;

        public virtual bool IsDirty
        {
            get { return isDirty; }
        }

        void OnIsDirtyChanged(object sender, EventArgs e)
        {
            bool newIsDirty = IsDirtyInternal;
            if (newIsDirty != isDirty)
            {
                isDirty = newIsDirty;
                RaiseIsDirtyChanged();
            }
        }

        /// <summary>
        /// Raise the IsDirtyChanged event. Call this method only if you have overridden the IsDirty property
        /// to implement your own handling of IsDirty.
        /// </summary>
        protected void RaiseIsDirtyChanged()
        {
            if (IsDirtyChanged != null)
                IsDirtyChanged(this, EventArgs.Empty);
        }

        public event EventHandler IsDirtyChanged;
        #endregion
        #region IServiceProvider

        IServiceContainer services = new ServiceContainer();
        public object GetService(Type serviceType)
        {
            object obj = services.GetService(serviceType);
            if (obj != null)
                return obj;
            if (serviceType.IsInstanceOfType(this))
                return this;
            return null;
        }
        public IServiceContainer Services
        {
            get { return services; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException();
                services = value;
            }
        }

        #endregion

        IWorkbenchWindow _workbenchWindow;

        IWorkbenchWindow IViewContent.WorkbenchWindow
        {
            get { return _workbenchWindow; }
            set
            {
                if (_workbenchWindow != value)
                {
                    _workbenchWindow = value;
                    OnWorkbenchWindowChanged();
                }
            }
        }

        public IWorkbenchWindow WorkbenchWindow {
			get { return _workbenchWindow; }
		}

        protected virtual void OnWorkbenchWindowChanged()
        {
        }
        string tabPageText = "TabPageText";

        public event EventHandler TabPageTextChanged;

        /// <summary>
        /// Gets/Sets the title of the current tab page.
        /// This value will be passed through the string parser before being displayed.
        /// </summary>
        public string TabPageText
        {
            get { return tabPageText; }
            set
            {
                if (tabPageText != value)
                {
                    tabPageText = value;

                    if (TabPageTextChanged != null)
                    {
                        TabPageTextChanged(this, EventArgs.Empty);
                    }
                }
            }
        }
        #region TitleName
		public event EventHandler TitleNameChanged;
		
		void OnTitleNameChanged(EventArgs e)
		{
			if (TitleNameChanged != null) {
				TitleNameChanged(this, e);
			}
		}
		
		string titleName;
		LanguageDependentExtension titleNameLocalizeExtension;
		
		string IViewContent.TitleName {
			get {
				if (titleName != null) {
					return titleName;
// 				} else if (files.Count > 0) {
// 					return Path.GetFileName(files[0].FileName);
				} else {
					return "[Default Title]";
				}
			}
		}
		
		public string TitleName {
			get { return titleName; }
			set {
				if (titleNameLocalizeExtension != null) {
					titleNameLocalizeExtension.PropertyChanged -= OnTitleNameLocalizationChanged;
					titleNameLocalizeExtension = null;
				}
				if (titleName != value) {
					titleName = value;
					OnTitleNameChanged(EventArgs.Empty);
				}
			}
		}
		
		/// <summary>
		/// Sets a localized title that will update automatically when the language changes.
		/// </summary>
		/// <param name="text">The input to the string parser which will localize title.</param>
		protected void SetLocalizedTitle(string text)
		{
			titleNameLocalizeExtension = new StringParseExtension(text) { UsesAccessors = false };
			titleNameLocalizeExtension.PropertyChanged += OnTitleNameLocalizationChanged;
			OnTitleNameLocalizationChanged(null, null);
		}
		
		void OnTitleNameLocalizationChanged(object sender, EventArgs e)
		{
			string value = titleNameLocalizeExtension.Value;
			if (titleName != value) {
				titleName = value;
				OnTitleNameChanged(EventArgs.Empty);
			}
		}
		#endregion   
        #region InfoTip
        public event EventHandler InfoTipChanged;
        void OnInfoTipChanged()
        {
            if (InfoTipChanged != null)
            {
                InfoTipChanged(this, EventArgs.Empty);
            }
        }
        string infoTip;
        LanguageDependentExtension infoTipLocalizeExtension;
        string IViewContent.InfoTip
        {
            get
            {
                if (infoTip != null)
                    return infoTip;
                else
                    return null;
            }
        }
        public string InfoTip
        {
            get { return infoTip; }
            set
            {
                if (infoTipLocalizeExtension != null)
                {
                    infoTipLocalizeExtension.PropertyChanged -= OnInfoTipLocalizationChanged;
                    infoTipLocalizeExtension = null;
                }
                if (infoTip != value)
                {
                    infoTip = value;
                    OnInfoTipChanged();
                }
            }
        }

        
        		protected void SetLocalizedInfoTip(string text)
		{
			infoTipLocalizeExtension = new StringParseExtension(text) { UsesAccessors = false };
			infoTipLocalizeExtension.PropertyChanged += OnInfoTipLocalizationChanged;
			OnInfoTipLocalizationChanged(null, null);
		}

		void OnInfoTipLocalizationChanged(object sender, EventArgs e)
		{
			string value = infoTipLocalizeExtension.Value;
			if (infoTip != value)
			{
				infoTip = value;
				OnInfoTipChanged();
			}
		}
		#endregion
        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}
