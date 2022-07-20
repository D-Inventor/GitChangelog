using GitChangelog.Models;

namespace GitChangelog.Components
{
    public class FeaturesStep
        : StringListStepBase
    {
        public FeaturesStep(Changelog model)
            : base(model, "New features", "FeaturesStep", "Features")
        { }

        public override void Commit()
        {
            Model.Features = Values;
        }
    }
}
