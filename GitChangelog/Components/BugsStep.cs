using GitChangelog.Models;

namespace GitChangelog.Components
{
    internal class BugsStep : StringListStepBase
    {
        public BugsStep(Changelog model)
            : base(model, "Fixed bugs", "BugfixesStep", "BugFixes")
        { }

        public override void Commit()
        {
            Model.Bugfixes = Values;
        }
    }
}
