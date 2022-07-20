using GitChangelog.Models;

using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GitChangelog.Components
{
    public abstract class StringListStepBase : StepBase
    {
        public StringListStepBase(Changelog model, string header, string name, string fieldname)
            : base(model, header, name)
        {
            Content = new StringList
            {
                Name = fieldname,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10)
            };
        }

        protected List<string> Values => (Content as StringList)?.GetValues().ToList();
    }
}
