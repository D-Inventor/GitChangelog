using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GitChangelog.Components
{
    public class StringList : StackPanel
    {
        public StringList()
        {
            Children.Add(CreateTextBox());
        }

        private TextBox CreateTextBox()
        {
            var result = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(5),
                TextWrapping = TextWrapping.NoWrap,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 380
            };

            result.TextChanged += TextBoxTextChanged;
            result.LostFocus += TextBoxLostFocus;

            return result;
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (Children.Count > 1 && sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text) && !ReferenceEquals(Children[Children.Count - 1], tb))
            {
                Children.Remove(tb);
            }
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb && !string.IsNullOrWhiteSpace(tb.Text) && ReferenceEquals(Children[Children.Count - 1], tb))
            {
                TextBox newTextBox = CreateTextBox();
                Children.Add(newTextBox);
            }
        }

        public IEnumerable<string> GetValues()
        {
            return Children.OfType<TextBox>()
                           .Select(tb => tb.Text.Trim())
                           .Where(t => !string.IsNullOrEmpty(t));
        }
    }
}
