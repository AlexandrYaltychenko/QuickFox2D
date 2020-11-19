using System;
using System.Collections.Generic;

namespace QuickFox.Resources
{
    public class ResourcePool
    {
        public IDictionary<string, IResource> Resource { get; }
    }
}
