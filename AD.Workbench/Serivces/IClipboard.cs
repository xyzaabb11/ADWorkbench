using System.Windows;

namespace AD.Workbench.Serivces
{
    public interface IClipboard
    {
        /// <inheritdoc cref="System.Windows.Clipboard.Clear"/>
        void Clear();

        /// <inheritdoc cref="System.Windows.Clipboard.GetDataObject"/>
        IDataObject GetDataObject();

        /// <inheritdoc cref="System.Windows.Clipboard.SetDataObject(object)"/>
        void SetDataObject(object data);

        /// <inheritdoc cref="System.Windows.Clipboard.SetDataObject(object, bool)"/>
        void SetDataObject(object data, bool copy);

        /// <inheritdoc cref="System.Windows.Clipboard.ContainsText"/>
        bool ContainsText();
        /// <inheritdoc cref="System.Windows.Clipboard.GetText"/>
        string GetText();
        /// <inheritdoc cref="System.Windows.Clipboard.SetText(string)"/>
        void SetText(string text);
    }

}
