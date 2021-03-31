using Reports.FileHandler;

namespace Reports.Servers
{
    public class HDAServer : Server
    {
        Opc.Hda.Server server;
        Opc.Hda.ItemValueCollection collection;

        public HDAServer(ServerProperties serverProperties)
            : base(serverProperties)
        {  }

        public override void CreateServer(Opc.URL url) =>
            server = new Opc.Hda.Server(new OpcCom.Factory(), url);

        public override void Connect() =>
            server.Connect();

        public override void Disconnect() =>
            server.Disconnect();

        public object[] GetData(System.DateTime startTime, System.DateTime endTime,
            System.Collections.Generic.List<string> list)
        {
            Opc.Hda.Time OpcStartTime = new Opc.Hda.Time(startTime);
            Opc.Hda.Time OpcEndTime = new Opc.Hda.Time(endTime);

            if (server.IsConnected == false)
                TryToConnect();

            Opc.ItemIdentifier[] HDA_tags = ConvertTags(list);
            Opc.IdentifiedResult[] items = server.CreateItems(HDA_tags);
            Opc.Hda.ItemValueCollection[] RawData = server.ReadRaw(OpcStartTime, OpcEndTime, 100, true, items);

            return Filter(RawData);
        }

        Opc.ItemIdentifier[] ConvertTags(System.Collections.Generic.List<string> list)
        {
            int count = list.Count;
            Opc.ItemIdentifier[] tags = new Opc.ItemIdentifier[count];

            for (int i = 0; i < count; i++)
                tags[i] = new Opc.ItemIdentifier(list[i].ToString());

            return tags;
        }

        object[] Filter(Opc.Hda.ItemValueCollection[] collectionArray)
        {
            int lengthOfCollection = collectionArray.Length;
            object[] filteredData = new object[lengthOfCollection];

            for (int collectionIndex = 0; collectionIndex < lengthOfCollection; collectionIndex++)
            {
                collection = collectionArray[collectionIndex];
                filteredData[collectionIndex] = FindGoodValue();
            }

            return filteredData;
        }

        object FindGoodValue()
        {
            int length = collection.Count;

            for (int itemIndex = 0; itemIndex < length; itemIndex++)
            {
                if (IsGoodValue(itemIndex))
                {
                    return collection[itemIndex].Value;
                }
            }

            return null;
        }

        bool IsGoodValue(int itemIndex) =>
            (collection[itemIndex].HistorianQuality.ToString() != "NoBound");
    }
}
