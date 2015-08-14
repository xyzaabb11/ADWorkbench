using ICSharpCode.Core;
using System;
using System.Diagnostics;
using System.IO;


namespace AD.Workbench.Commands
{
    public class LinkCommand : AbstractMenuCommand
    {
        string site;

        public LinkCommand(string site)
        {
            this.site = site;
        }

        public override void Run()
        {
            if (site.StartsWith("home://"))
            {
                string file = Path.Combine(FileUtility.ApplicationRootPath, site.Substring(7).Replace('/', Path.DirectorySeparatorChar));
                try
                {
                    Process.Start(file);
                }
                catch (Exception)
                {
                    MessageService.ShowError("Can't execute/view " + file + "\n Please check that the file exists and that you can open this file.");
                }
            }
            else
            {
//                 FileService.OpenFile(site);
            }
        }
    }
}
