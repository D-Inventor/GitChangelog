using GitChangelog.Models;

namespace GitChangelog.Components
{
    public class VersionStep : TextBoxStepBase
    {
        public VersionStep(Changelog model)
            : base(model, "Version", "VersionStep", "Version")
        { }

        public override void Commit()
        {
            Model.Version = Value;
        }
    }
}
