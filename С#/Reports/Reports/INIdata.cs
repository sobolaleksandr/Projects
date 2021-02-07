using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public class INIData//Класс для работы с ini-файлом
    {
        readonly string[] Splitter;
        readonly bool filteredPaths;
        readonly string INIPath;
        string tag;

        List<Section> sections = new List<Section>();
        Section section;

        public INIData(string INIPath, string[] Splitter, bool filteredPaths)//Получаем путь, условие по которому фильтруем теги, также узнаем нужна ли фильтрация по bool filteredPaths
        {
            this.Splitter = Splitter;
            this.filteredPaths = filteredPaths;
            this.INIPath = INIPath;
        }

        public List<Section> Parse()//Парсим ini-файл. Теги разделены на секции. Пример прилагаю
        {
            System.IO.StreamReader streamReader = System.IO.File.OpenText(INIPath);

            while (!streamReader.EndOfStream)
            {
                tag = streamReader.ReadLine();
                tag = Regex.Replace(tag, @"[ \r\n\t]", "");
                Create();
            }

            streamReader.Dispose();
            return sections;
        }

        void Create()//Либо создаем новую секцию, либо заполняем текущую
        {
            if (tag.Contains("["))
            {
                GetSection();
            }
            else
            {
                FillPaths();
            }
        }

        void GetSection()//Создаем новую секцию
        {
            string Name = tag.Replace("[", "").Replace("]", "");
            section = new Section(Splitter, Name);

            sections.Add(section);
        }

        void FillPaths()//Заполняем секцию
        {
            foreach (Tag item in section.Items)
            {
                if (tag.Contains(item.Name) || !filteredPaths)
                {
                    item.Path.Add(tag);
                }
            }
        }

    }
}
