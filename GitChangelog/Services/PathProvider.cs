using Microsoft.VisualStudio.Shell.Interop;

using System.IO;

namespace GitChangelog.Services
{
    public class PathProvider
    {
        public string GetSolutionFolder()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            IVsSolution solution = (IVsSolution)Package.GetGlobalService(typeof(IVsSolution));

            solution.GetSolutionInfo(out string _, out string file, out string _);
            return Path.GetDirectoryName(file);
        }

        public string[] GetAbsoluteProjectFilePaths()
        {
            return Directory.GetFiles(GetSolutionFolder(), "*.csproj", SearchOption.AllDirectories);
        }
    }
}
