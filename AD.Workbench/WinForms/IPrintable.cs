using System.Drawing.Printing;

namespace AD.Workbench.WinForms
{
    public interface IPrintable
    {
        /// <summary>
        /// Returns the PrintDocument for this object, see the .NET reference
        /// for more information about printing.
        /// </summary>
        PrintDocument PrintDocument
        {
            get;
        }
    }
}
