﻿using System;
using AD.Workbench.Serivces;

namespace AD.Workbench.Workbench
{
    public abstract class AbstractPadContent : IPadContent
    {
        /// <inheritdoc/>
        public abstract object Control
        {
            get;
        }

        /// <inheritdoc/>
        public virtual object InitiallyFocusedControl
        {
            get
            {
                return null;
            }
        }

        public virtual void Dispose()
        {
        }

        public void BringToFront()
        {
            PadDescriptor d = this.PadDescriptor;
            if (d != null)
                d.BringPadToFront();
        }

        protected virtual PadDescriptor PadDescriptor
        {
            get
            {
                return ADService.Workbench.GetPad(GetType());
            }
        }

        public virtual object GetService(Type serviceType)
        {
            if (serviceType.IsInstanceOfType(this))
                return this;
            else
                return null;
        }
    }
}
