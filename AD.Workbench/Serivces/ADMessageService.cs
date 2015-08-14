using ICSharpCode.Core.WinForms;
using System;
using AD.Workbench.Logging;

namespace AD.Workbench.Serivces
{
    class ADMessageService : WinFormsMessageService
    {
        public override void ShowException(Exception ex, string message)
        {
            ADService.Log.Error(message, ex);
            ADService.Log.Warn("Stack trace of last exception log:\n" + Environment.StackTrace);
            if (ex != null)
                ExceptionBox.ShowErrorBox(ex, message);
            else
                ShowError(message);
        }
    }
}
