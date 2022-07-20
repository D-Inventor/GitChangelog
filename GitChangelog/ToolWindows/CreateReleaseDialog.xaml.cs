using GitChangelog.Components;
using GitChangelog.Models;
using GitChangelog.Services;

using Microsoft.VisualStudio.PlatformUI;

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GitChangelog
{
    public partial class CreateReleaseDialog : DialogWindow
    {
        private readonly List<IChangelogStep> _steps;
        private int _currentStep;
        private readonly PathProvider pathProvider;
        private readonly Changelog _model;

        private IChangelogStep CurrentStep => _steps[_currentStep];

        public CreateReleaseDialog(PathProvider pathProvider, Changelog model)
        {
            InitializeComponent();
            this.pathProvider = pathProvider;
            _model = model;
            _currentStep = 0;
            _steps = new List<IChangelogStep>
            {
                BindStep(new VersionStep(_model)),
                BindStep(new BugsStep(_model)),
                BindStep(new FeaturesStep(_model)),
                BindStep(new OtherChangesStep(_model)),
                BindStep(new TitleStep(_model))
            };

            CurrentStep.Activate();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            if (_currentStep > 0)
            {
                ShowButton(previousButton);
            }
            else
            {
                HideButton(previousButton);
            }

            if (_currentStep < _steps.Count - 1)
            {
                ShowButton(nextButton);
                HideButton(okButton);
            }
            else
            {
                ShowButton(okButton);
                HideButton(nextButton);
            }
        }

        private void ShowButton(Button button)
        {
            button.IsEnabled = true;
            button.Visibility = Visibility.Visible;
        }

        private void HideButton(Button button)
        {
            button.IsEnabled = false;
            button.Visibility = Visibility.Collapsed;
        }

        private async void okButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentStep.Commit();
            if (await VS.MessageBox.ShowConfirmAsync("Confirm changelog", _model.ToMessage()))
            {
                Close();
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentStep.Commit();
            CurrentStep.Deactivate();

            _currentStep++;

            CurrentStep.Activate();
            UpdateButtons();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentStep.Commit();
            CurrentStep.Deactivate();

            _currentStep--;

            CurrentStep.Activate();
            UpdateButtons();
        }

        private T BindStep<T>(T step)
            where T : UIElement, IChangelogStep
        {
            step.SetValue(Grid.ColumnProperty, 0);
            step.SetValue(Grid.RowProperty, 0);
            Main.Children.Add(step);

            return step;
        }
    }
}
