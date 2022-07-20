using EnvDTE;

using GitChangelog.Models;
using GitChangelog.Services;

using LibGit2Sharp;

using Microsoft;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace GitChangelog
{
    [Command(PackageIds.openCreateReleaseDialog)]
    internal sealed class OpenCreateReleaseDialogCommand : BaseCommand<OpenCreateReleaseDialogCommand>
    {
        private readonly PathProvider pathProvider;

        public OpenCreateReleaseDialogCommand()
            : this(new PathProvider())
        { }

        public OpenCreateReleaseDialogCommand(PathProvider pathProvider)
        {
            this.pathProvider = pathProvider;
        }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = (DTE)await this.Package.GetServiceAsync(typeof(DTE));
            Assumes.Present(dte);

            if (!dte.Documents.OfType<Document>().All(d => d.Saved))
            {
                await VS.MessageBox.ShowWarningAsync("Unsaved changes", "Save your changes before using this tool");
                return;
            }

            var model = new Changelog();
            await VS.Windows.ShowDialogAsync(new CreateReleaseDialog(pathProvider, model));

            try
            {
                // make changes
                var dir = pathProvider.GetSolutionFolder();

                var projects = Directory.GetFiles(dir, "*.csproj", SearchOption.AllDirectories);
                List<string> editedProjects = new();

                foreach (var project in projects)
                {
                    XmlDocument document = new();
                    document.Load(project);
                    var root = document.DocumentElement;
                    var releaseNotes = root.SelectSingleNode("//PropertyGroup/PackageReleaseNotes");

                    if (releaseNotes is not null)
                    {
                        releaseNotes.InnerText = model.ToMessage();
                        document.Save(project);
                        editedProjects.Add(project);
                    }
                }

                using (var repo = new Repository(dir))
                {
                    var signature = repo.Config.BuildSignature(DateTimeOffset.Now);

                    // git commit
                    foreach (var project in editedProjects)
                    {
                        var relativePath = project.Replace(dir, string.Empty).TrimStart(Path.DirectorySeparatorChar);
                        LibGit2Sharp.Commands.Stage(repo, relativePath);
                    }
                    repo.Commit("Update changelog", signature, signature);

                    // git create tag
                    repo.ApplyTag(model.Version, signature, model.ToMessage());
                }
            }
            catch (Exception ex)
            {
                await VS.MessageBox.ShowErrorAsync("Something went wrong", ex.Message);
            }
        }
    }
}
