using ICSharpCode.Core;
using AD.Workbench.Workbench;

namespace AD.Workbench.Serivces
{
    public class PadDoozer : IDoozer
    {
        /// <summary>
        /// Gets if the doozer handles codon conditions on its own.
        /// If this property return false, the item is excluded when the condition is not met.
        /// </summary>
        public bool HandleConditions
        {
            get
            {
                return false;
            }
        }

        public object BuildItem(BuildItemArgs args)
        {
            return new PadDescriptor(args.Codon);
        }
    }
}
