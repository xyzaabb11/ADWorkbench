using System;
using System.Collections.Generic;

namespace AD.Workbench.Workbench
{
    public interface IWorkbenchWindow
    {
        string Title
        {
            get;
        }

        [Obsolete("This property always returns false.")]
        bool IsDisposed
        {
            get;
        }

        IViewContent ActiveViewContent
        {
            get;
            set;
        }

        System.Windows.Media.ImageSource Icon
        {
            get;
            set;
        }
        /// <summary>
        /// Is raised when the ActiveViewContent property has changed.
        /// </summary>
        event EventHandler ActiveViewContentChanged;
        IList<IViewContent> ViewContents
        {
            get;
        }
        void SwitchView(int viewNumber);

        bool CloseWindow(bool force);

        void SelectWindow();

        event EventHandler TitleChanged;
    }
}
