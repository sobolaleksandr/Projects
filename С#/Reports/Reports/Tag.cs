using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public class Tag//Класс типов тегов на сервере
    {
        public string Name;//Сущность тега бывает описание, значение и множество других параметров. По каждому параметру можно получить оперативные данные
        public List<string> Path;//Массив путей (путь до конкретного тега)
        public Data ValDA;//Оперативные значения
        public Data ValHDA;//Исторические значения

        public Tag(string Name)
        {
            this.Name = Name;
            Path = new List<string>();
            ValDA = new Data(0);
            ValHDA = new Data(0);
        }
    }
}
