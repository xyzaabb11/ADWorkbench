using System;

namespace AD.Workbench.Workbench
{
    public class ViewContentEventArgs : EventArgs
    {
        IViewContent content;

        public IViewContent Content
        {
            get
            {
                return content;
            }
        }

        public ViewContentEventArgs(IViewContent content)
        {
            if (content == null)
                throw new ArgumentNullException("content");
            this.content = content;
        }
    }
}
