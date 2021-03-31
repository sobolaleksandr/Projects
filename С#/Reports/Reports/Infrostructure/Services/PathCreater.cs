using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports.Infrostructure
{
    public class PathCreater
    {
        public List<Section> sections;

        public PathCreater(List<Section> sections)
        {
            this.sections = sections;

            AppendSection();
        }

        void AppendSection()
        {
            foreach (Section section in sections)
                foreach (Tag item in section.Items)
                    AppendList(item);
        }

        void AppendList(Tag item)
        {
            switch (item.Name)
            {
                case "CE":
                    item.Path = AppendTag(item.Path, ".state.flg0");
                    break;
                case "State":
                    item.Path = AppendTag(item.Path, ".state");
                    break;
                case "Status":
                    item.Path = AppendTag(item.Path, ".state.status");
                    break;
                case "Mode":
                    item.Path = AppendTag(item.Path, ".state.mode");
                    break;
                case "Desc":
                    item.Path = AppendTag(item.Path, ".Description");
                    break;
                case "Fail":
                    item.Path = AppendTag(item.Path, ".fail");
                    break;
                case "Value":
                    break;
                case "EUnit":
                    item.Path = AppendTag(item.Path, ".EUnit");
                    break;
                case "MskSet":
                    item.Path = AppendTag(item.Path, ".msk_set");
                    break;
                default:
                    break;
            }
        }

        List<string> AppendTag(List<string> List, string prefix)
        {
            int count = List.Count;

            for (int i = 0; i < count; i++)
                List[i] = String.Concat(List[i].ToString(), prefix);

            return List;
        }
    }

}
