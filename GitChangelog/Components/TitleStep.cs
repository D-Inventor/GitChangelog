using GitChangelog.Models;

using System.Collections.Generic;

namespace GitChangelog.Components
{
    public class TitleStep : TextBoxStepBase
    {
        private bool _isChanged;
        private bool _disableEvents;

        public TitleStep(Changelog model)
            : base(model, "Title", "TitleStep", "Title")
        {
            _isChanged = false;
            _disableEvents = false;
            TextBox.TextChanged += OnTextBoxChange;
        }

        private void OnTextBoxChange(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (_disableEvents) return;

            _isChanged = true;
        }

        public override void Activate()
        {
            base.Activate();

            if (!_isChanged)
            {
                List<string> changes = new List<string>();
                if (Model.Features?.Count > 0) changes.Add($"{Model.Features.Count} new features");
                if (Model.Bugfixes?.Count > 0) changes.Add($"{Model.Bugfixes.Count} bugfixes");
                if (Model.Changes?.Count > 0) changes.Add($"{Model.Changes.Count} other changes");

                _disableEvents = true;
                TextBox.Text = String.Join(", ", changes);
                _disableEvents = false;
            }
        }

        public override void Commit()
        {
            Model.Title = Value;
        }
    }
}
