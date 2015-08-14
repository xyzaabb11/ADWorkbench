using ICSharpCode.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace AD.Workbench.About
{
    public class ADAbout
    {
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static string GetVersionInformationString()
        {
            string str = "";
            object[] attr = typeof(ADAbout).Assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
            if (attr.Length == 1)
            {
                AssemblyInformationalVersionAttribute aiva = (AssemblyInformationalVersionAttribute)attr[0];
                str += "AD Version        : " + aiva.InformationalVersion + Environment.NewLine;
            }
            str += ".NET Version         : " + Environment.Version.ToString() + Environment.NewLine;
            str += "OS Version           : " + Environment.OSVersion.ToString() + Environment.NewLine;
            string cultureName = null;
            try
            {
                cultureName = CultureInfo.CurrentCulture.Name;
                str += "Current culture      : " + CultureInfo.CurrentCulture.EnglishName + " (" + cultureName + ")" + Environment.NewLine;
            }
            catch { }
            try
            {
                if (cultureName == null || !cultureName.StartsWith(ResourceService.Language))
                {
                    str += "Current UI language  : " + ResourceService.Language + Environment.NewLine;
                }
            }
            catch { }
            try
            {
                if (IntPtr.Size != 4)
                {
                    str += "Running as " + (IntPtr.Size * 8) + " bit process" + Environment.NewLine;
                }
                string PROCESSOR_ARCHITEW6432 = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432");
                if (!string.IsNullOrEmpty(PROCESSOR_ARCHITEW6432))
                {
                    if (PROCESSOR_ARCHITEW6432 == "AMD64")
                        PROCESSOR_ARCHITEW6432 = "x86-64";
                    str += "Running under WOW6432, processor architecture: " + PROCESSOR_ARCHITEW6432 + Environment.NewLine;
                }
            }
            catch { }
            try
            {
                if (SystemInformation.TerminalServerSession)
                {
                    str += "Terminal Server Session" + Environment.NewLine;
                }
                if (SystemInformation.BootMode != BootMode.Normal)
                {
                    str += "Boot Mode            : " + SystemInformation.BootMode + Environment.NewLine;
                }
            }
            catch { }
            str += "Working Set Memory   : " + (Environment.WorkingSet / 1024) + "kb" + Environment.NewLine;
            str += "GC Heap Memory       : " + (GC.GetTotalMemory(false) / 1024) + "kb" + Environment.NewLine;
            return str;
        }
    }
}
