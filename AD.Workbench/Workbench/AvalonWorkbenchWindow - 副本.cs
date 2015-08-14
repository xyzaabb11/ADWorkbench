using System.Windows.Controls;
using AvalonDock;
using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AD.Workbench.Serivces;

namespace AD.Workbench.Workbench
{
    sealed class AvalonWorkbenchWindow : DocumentContent, IWorkbenchWindow, IOwnerState
    {
        AvalonDockLayout dockLayout;
        public AvalonWorkbenchWindow(AvalonDockLayout dockLayout)
        {
            if (dockLayout == null)
                throw new ArgumentNullException("dockLayout");

//             CustomFocusManager.SetRememberFocusedChild(this, true);
            this.IsFloatingAllowed = true;
            this.dockLayout = dockLayout;
            viewContents = new ViewContentCollection(this);

//             SD.ResourceService.LanguageChanged += OnTabPageTextChanged;
        }
        public bool IsDisposed
        {
            get { throw new NotImplementedException(); }
        }

        TabControl viewTabControl;
        public IViewContent ActiveViewContent
        {
            get
            {
                if (viewTabControl != null && viewTabControl.SelectedIndex >= 0 && viewTabControl.SelectedIndex < ViewContents.Count)
                {
                    return ViewContents[viewTabControl.SelectedIndex];
                }
                else if (ViewContents.Count == 1)
                {
                    return ViewContents[0];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                int pos = ViewContents.IndexOf(value);
                if (pos < 0)
                    throw new ArgumentException();
                SwitchView(pos);
            }
        }
        void UnregisterContent(IViewContent content)
        {
            content.WorkbenchWindow = null;

//             content.TabPageTextChanged -= OnTabPageTextChanged;
//             content.TitleNameChanged -= OnTitleNameChanged;
//             content.InfoTipChanged -= OnInfoTipChanged;
//             content.IsDirtyChanged -= OnIsDirtyChanged;
// 
//             this.dockLayout.Workbench.OnViewClosed(new ViewContentEventArgs(content));
        }

        void ClearContent()
        {
//             this.Content = null;
//             if (viewTabControl != null)
//             {
//                 foreach (TabItem page in viewTabControl.Items)
//                 {
//                     SD.WinForms.SetContent(page, null);
//                 }
//                 viewTabControl = null;
//             }
        }
        void UpdateActiveViewContent()
        {
            UpdateTitleAndInfoTip();

            IViewContent newActiveViewContent = this.ActiveViewContent;
//             if (newActiveViewContent != null)
//                 IsLocked = newActiveViewContent.IsReadOnly;
// 
//             if (oldActiveViewContent != newActiveViewContent && ActiveViewContentChanged != null)
//             {
//                 ActiveViewContentChanged(this, EventArgs.Empty);
//             }
//             oldActiveViewContent = newActiveViewContent;
//             CommandManager.InvalidateRequerySuggested();
        }

        void UpdateTitleAndInfoTip()
        {
            UpdateInfoTip();
            UpdateTitle();
        }

        void UpdateInfoTip()
        {
            IViewContent content = ActiveViewContent;
            if (content != null)
            {
                string newInfoTip = content.InfoTip;

                if (newInfoTip != this.InfoTip)
                {
                    this.InfoTip = newInfoTip;
                    if (DragEnabledArea != null)
                        DragEnabledArea.ToolTip = this.InfoTip;

                    OnInfoTipChanged();
                }
            }
        }

        void UpdateTitle()
        {
            IViewContent content = ActiveViewContent;
            if (content != null)
            {
                string newTitle = content.TitleName;
                if (content.IsDirty)
                    newTitle += "*";
                if (newTitle != Title)
                {
                    Title = newTitle;
                    OnTitleChanged();
                }
            }
        }

        void OnTitleChanged()
        {
            if (TitleChanged != null)
            {
                TitleChanged(this, EventArgs.Empty);
            }
        }

        void OnInfoTipChanged()
        {
            if (InfoTipChanged != null)
            {
                InfoTipChanged(this, EventArgs.Empty);
            }
        }
        public event EventHandler InfoTipChanged;
        sealed class ViewContentCollection : Collection<IViewContent>
        {
            readonly AvalonWorkbenchWindow window;

            internal ViewContentCollection(AvalonWorkbenchWindow window)
            {
                this.window = window;
            }

            protected override void ClearItems()
            {
                foreach (IViewContent vc in this)
                {
                    window.UnregisterContent(vc);
                }

                base.ClearItems();
                window.ClearContent();
//                 window.UpdateActiveViewContent();
            }

            protected override void InsertItem(int index, IViewContent item)
            {
                base.InsertItem(index, item);
// 
//                 window.RegisterNewContent(item);

                if (Count == 1)
                {
                    ADService.WinForms.SetContent(window, item.Control, item);
                }
                else
                {
//                     if (Count == 2)
//                     {
//                         window.CreateViewTabControl();
//                         IViewContent oldItem = this[0];
//                         if (oldItem == item) oldItem = this[1];
// 
//                         TabItem oldPage = new TabItem();
//                         oldPage.Header = StringParser.Parse(oldItem.TabPageText);
//                         SD.WinForms.SetContent(oldPage, oldItem.Control, oldItem);
//                         window.viewTabControl.Items.Add(oldPage);
//                     }
// 
//                     TabItem newPage = new TabItem();
//                     newPage.Header = StringParser.Parse(item.TabPageText);
//                     SD.WinForms.SetContent(newPage, item.Control, item);
// 
//                     window.viewTabControl.Items.Insert(index, newPage);
                }
                window.UpdateActiveViewContent();
            }

            protected override void RemoveItem(int index)
            {
                window.UnregisterContent(this[index]);

                base.RemoveItem(index);

                if (Count < 2)
                {
                    window.ClearContent();
                    if (Count == 1)
                    {
//                         SD.WinForms.SetContent(window, this[0].Control, this[0]);
                    }
                }
//                 else
//                 {
//                     window.viewTabControl.Items.RemoveAt(index);
//                 }
//                 window.UpdateActiveViewContent();
            }

            protected override void SetItem(int index, IViewContent item)
            {
                window.UnregisterContent(this[index]);

                base.SetItem(index, item);

//                 window.RegisterNewContent(item);

                if (Count == 1)
                {
//                     window.ClearContent();
//                     SD.WinForms.SetContent(window, item.Control, item);
                }
                else
                {
//                     TabItem page = (TabItem)window.viewTabControl.Items[index];
//                     SD.WinForms.SetContent(page, item.Control, item);
//                     page.Header = StringParser.Parse(item.TabPageText);
                }
//                 window.UpdateActiveViewContent();
            }
        }
        readonly ViewContentCollection viewContents;
        public IList<IViewContent> ViewContents
        {
            get { return viewContents; }
        }

        public void SwitchView(int viewNumber)
        {
            throw new NotImplementedException();
        }

        public bool CloseWindow(bool force)
        {
            throw new NotImplementedException();
        }

        public void SelectWindow()
            {
                Activate(); 
        }

        public event EventHandler TitleChanged;

        public Enum InternalState
        {
            get { throw new NotImplementedException(); }
        }
    }
}
