using GitChangelog.Models;

using System.Windows;
using System.Windows.Controls;

namespace GitChangelog.Components
{
    public abstract class StepBase : GroupBox, IChangelogStep
    {
        protected Changelog Model { get; }

        public StepBase(Changelog model, string header, string name)
        {
            Name = name;
            Header = header;

            Deactivate();
            Model = model;
        }

        public virtual void Activate()
        {
            IsEnabled = true;
            Visibility = Visibility.Visible;
        }

        public virtual void Deactivate()
        {
            IsEnabled = false;
            Visibility = Visibility.Collapsed;
        }

        public abstract void Commit();
    }
}
