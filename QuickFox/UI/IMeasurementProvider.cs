using System;
using QuickFox.Components;
using QuickFox.Rendering;

namespace QuickFox.UI
{
    public interface IMeasurementProvider
    {
        Size Measure(IRenderableComponent component);
    }
}
