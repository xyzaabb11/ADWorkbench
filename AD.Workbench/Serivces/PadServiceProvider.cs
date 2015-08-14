using System;
using System.Collections.Generic;
using AD.Workbench.Workbench;

namespace AD.Workbench.Serivces
{
    class PadServiceProvider : IServiceProvider
    {
        readonly List<PadDescriptor> pads = new List<PadDescriptor>();

        public PadServiceProvider(IEnumerable<PadDescriptor> descriptors)
        {
            foreach (var descriptor in descriptors)
            {
                if (descriptor.ServiceInterface != null)
                {
                    pads.Add(descriptor);
                }
            }
        }

        public object GetService(Type serviceType)
        {
            foreach (var pad in pads)
            {
                if (serviceType == pad.ServiceInterface)
                    return pad.PadContent;
            }
            return null;
        }
    }
}
