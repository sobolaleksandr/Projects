using Reports.Infrostructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Reports.FileHandler
{
    public class IniHandler
    {
        readonly string[] Splitter;
        readonly bool filteredPaths;
        readonly string IniPath;

        Section section;

        public IniHandler(string IniPath, string[] Splitter, bool filteredPaths)
        {
            this.Splitter = Splitter;
            this.filteredPaths = filteredPaths;
            this.IniPath = IniPath;
        }

        public List<Section> Parse()
        {
            System.IO.StreamReader streamReader =
               new System.IO.StreamReader(IniPath, System.Text.Encoding.GetEncoding(1251));

            string head = "";
            string headed = "Headed";
            string prefix = "";

            List<Section> sections = new List<Section>();

            while (!streamReader.EndOfStream)
            {
                string tag = streamReader.ReadLine();
                tag = Regex.Replace(tag, @"[ \r\n\t]", "");

                if (String.IsNullOrWhiteSpace(tag))
                    continue;

                if (IsHead(tag))
                {
                    head = tag.Replace("[", "").Replace("]", "");

                    if (head == "Servers" || head == "Hosts")
                    {
                        head = headed;
                        continue;
                    }
                    else if (head == "Prefix")
                    {
                        tag = streamReader.ReadLine();
                        tag = Regex.Replace(tag, @"[ \r\n\t]", "");
                        prefix = tag + ".";
                        continue;
                    }

                    section = new Section(Splitter, head);
                    sections.Add(section);
                    continue;
                }
                else if (head == headed)
                    continue;
                else
                    foreach (Tag item in section.Items)
                        if (tag.Contains(item.Name) || !filteredPaths)
                            item.Path.Add(prefix + tag);
            }

            streamReader.Dispose();
            return sections;
        }

        private bool IsHead(string fileString) =>
            fileString.Contains("[");
    }

}
