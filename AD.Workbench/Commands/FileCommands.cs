using ICSharpCode.Core;
using AD.Workbench.Serivces;

namespace AD.Workbench.Commands
{
    public class CreateNewFile : AbstractMenuCommand
    {
        public override void Run()
        {
            ADService.MessageService.ShowMessage("CreateNewFile Clicked");
        }
    }
}
