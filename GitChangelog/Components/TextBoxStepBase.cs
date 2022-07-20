using GitChangelog.Models;

using System.Windows;
using System.Windows.Controls;

namespace GitChangelog.Components
{
    public abstract class TextBoxStepBase : StepBase
    {
        public TextBoxStepBase(Changelog model, string header, string name, string fieldname)
            : base(model, header, name)
        {
            Content = new TextBox
            {
                Name = fieldname,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10),
                TextWrapping = TextWrapping.NoWrap,
                Width = 380,
            };
        }

        protected TextBox TextBox => Content as TextBox;

        protected string Value => TextBox?.Text;
    }
}
