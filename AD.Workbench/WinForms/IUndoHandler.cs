namespace AD.Workbench.WinForms
{
    public interface IUndoHandler
    {
        bool EnableUndo
        {
            get;
        }

        bool EnableRedo
        {
            get;
        }

        void Undo();
        void Redo();
    }
}
