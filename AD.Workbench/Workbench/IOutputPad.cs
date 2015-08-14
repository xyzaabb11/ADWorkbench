using ICSharpCode.Core;

namespace AD.Workbench.Workbench
{
    /// <summary>
    /// The 'Output' pad.
    /// Allows showing a text log to the user.
    /// </summary>
    /// <remarks>This service is thread-safe.</remarks>
    [SDService("SD.OutputPad")]
    public interface IOutputPad
    {
        void ClearText();
        /// <summary>
        /// Appends text to this category.
        /// </summary>
        void AppendText(string text);

        /// <summary>
        /// Appends text to this category, followed by a newline.
        /// </summary>
        void AppendLine(string text);
    }

}
