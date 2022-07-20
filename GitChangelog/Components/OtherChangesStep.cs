using GitChangelog.Models;

namespace GitChangelog.Components
{
    public class OtherChangesStep
        : StringListStepBase
    {
        public OtherChangesStep(Changelog model)
            : base(model, "Other changes", "OtherChangesStep", "OtherChanges")
        { }

        public override void Commit()
        {
            Model.Changes = Values;
        }
    }
}
