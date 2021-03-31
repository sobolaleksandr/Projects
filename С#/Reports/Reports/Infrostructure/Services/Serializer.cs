using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Reports.Infrostructure
{
    public class Serializer
    {
        public List<ControlPoint> KpList = new List<ControlPoint>();

        public Serializer(Tag Item, string[] KpRegex, string[] ObjectRegex, string[] ObjectPrefix)
        {
            foreach (string regex in KpRegex)
            {
                var ListFoundKp = FillGroup(
                    regex,
                    new string[0],
                    Item.Path,
                    Item.DaData.ConvertToString());

                if (ListFoundKp.Count == 0)
                    continue;

                ControlPoint currentKp = new ControlPoint
                {
                    Name = ListFoundKp[0].Name,
                    GroupList = new List<Area>()
                };

                foreach (string objRegex in ObjectRegex)
                {
                    var groups = FillGroup(
                        objRegex,
                        ObjectPrefix,
                        ListFoundKp[0].Path,
                        ListFoundKp[0].Items.ToArray());

                    currentKp.GroupList.AddRange(groups);
                }

                if (currentKp.GroupList.Count == 0)
                    continue;

                KpList.Add(currentKp); 
            }
        }

        public List<Area> FillGroup(string objectCondition, string[] otherConditions, List<string> Paths, string[] Values)
        {
            List<Area> GroupList = new List<Area>();
            int length = Values.Length;

            for (int i = 0; i < length; i++)
            {
                string GroupName = FindMatches(objectCondition, Values[i]);

                if (GroupName != "")
                    foreach(string condition in otherConditions)
                        GroupName += FindMatches(condition, Values[i]);

                if (GroupName == "")
                    continue;

                var Group = GroupList.Find(g => g.Name == GroupName);

                if (Group.Items == null)
                {
                    Group = new Area
                    {
                        Name = GroupName,
                        Path = new List<string> { Paths[i].Replace(".Description", "") },
                        Items = new List<string> { Values[i].Replace(GroupName, "") }
                    };
                    GroupList.Add(Group);
                }
                else
                {
                    Group.Items.Add(Values[i].Replace(GroupName, ""));
                    Group.Path.Add(Paths[i].Replace(".Description", ""));
                }
            }

            return GroupList;
        }

        string FindMatches(string Condition, string Desc)
        {
            Regex regex = new Regex(Condition);
            MatchCollection matches = regex.Matches(Desc);

            if (matches.Count > 0)
                return matches[0].Value;

            return "";
        }
    }

}
