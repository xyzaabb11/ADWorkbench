using System.Windows;

namespace AvalonDock
{
    public class AvalonDockWindow : Window
    {
        static AvalonDockWindow()
        {
            ShowInTaskbarProperty.OverrideMetadata(typeof(AvalonDockWindow), new FrameworkPropertyMetadata(false));
        
        }

        internal AvalonDockWindow()
        { }
    }
}
