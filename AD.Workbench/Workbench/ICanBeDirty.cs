using System;

namespace AD.Workbench.Workbench
{
    public interface ICanBeDirty
    {
        bool IsDirty
        {
            get;
        }
        event EventHandler IsDirtyChanged;
    }
}
