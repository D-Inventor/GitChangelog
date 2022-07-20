using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitChangelog.Models
{
    public class Changelog
    {
        public string Version { get; set; }
        public List<string> Bugfixes { get; set; }
        public List<string> Features { get; set; }
        public List<string> Changes { get; set; }
        public string Title { get; set; }

        public string ToMessage()
        {
            StringBuilder sb = new();
            sb.AppendLine(Title);
            foreach (var item in Bugfixes.Concat(Features).Concat(Changes))
            {
                sb.AppendLine();
                sb.Append("- ");
                sb.Append(item);
            }

            return sb.ToString();
        }
    }
}
