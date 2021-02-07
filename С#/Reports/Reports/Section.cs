using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public class Section
    {
        public string Name;
        public List<Tag> Items;

        public Section(string[] TagNames, string SectionName)//Под каждую сущность создаем свой объект Tag
        {
            Name = SectionName;
            Items = new List<Tag>();

            foreach (string name in TagNames)
            {
                Tag tag = new Tag(name);
                Items.Add(tag);
            }
        }
    }
    public static class SectionExtension//Найти определенную сущность в секции (к примеру мы не будем получать историческое описание тега, нам достаточно оперативного
    {
        public static Tag GetItem(this Section section, string ItemName)
        {
            Tag Item = section.Items.Find(p => p.Name == ItemName);
            return Item;
        }
    }
}
