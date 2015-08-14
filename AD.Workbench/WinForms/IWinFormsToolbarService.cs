using System.Windows.Forms;

namespace AD.Workbench.WinForms
{
    public interface IWinFormsToolbarService
    {
        ToolStripItem[] CreateToolStripItems(string path, object parameter, bool throwOnNotFound);
        ToolStrip CreateToolStrip(object parameter, string addInTreePath);

        void UpdateToolbar(ToolStrip toolStrip);
        void UpdateToolbarText(ToolStrip toolStrip);
    }
}
