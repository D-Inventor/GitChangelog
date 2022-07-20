namespace GitChangelog.Components
{
    public interface IChangelogStep
    {
        void Activate();
        void Deactivate();
        void Commit();
    }
}
