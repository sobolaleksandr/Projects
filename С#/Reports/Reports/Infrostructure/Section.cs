using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports.Infrostructure
{
    public static class SectionExtension
    {
        public static Tag GetItem(this Section section, string ItemName) =>
        section.Items.Find(p => p.Name == ItemName);
    }

    public class Section
    {
        public string Name;
        public List<Tag> Items = new List<Tag>();

        public Section(string[] TagNames, string SectionName)
        {
            Name = SectionName;

            foreach (string name in TagNames)
                Items.Add(new Tag(name));
        }
    }
}
