using Reports.FileHandler;

namespace Reports.Servers
{
    public class DAServer : Server
    {
        Opc.Da.Server server;

        Opc.Da.ItemValueResult[] Data;

        public DAServer(ServerProperties serverProperties)
            : base(serverProperties)
        {}

        public override void CreateServer(Opc.URL url) =>
            server = new Opc.Da.Server(new OpcCom.Factory(), url);

        public override void Connect() =>
            server.Connect();

        public override void Disconnect() =>
            server.Disconnect();

        public object[] GetData(System.Collections.Generic.List<string> list)
        {
            if (server.IsConnected == false)
                TryToConnect();

            Opc.Da.Item[] tags = ConvertTags(list);
            Data = server.Read(tags);

            return ConvertedDataToObjectArray();
        }

        object[] ConvertedDataToObjectArray()
        {
            int length = Data.Length;
            object[] result = new object[length];

            for (int i = 0; i < length; i++)
                result[i] = Data[i].Value;

            return result;
        }

        Opc.Da.Item[] ConvertTags(System.Collections.Generic.List<string> list)
        {
            int count = list.Count;
            Opc.Da.Item[] tags = new Opc.Da.Item[count];

            for (int i = 0; i < count; i++)
                tags[i] = new Opc.Da.Item(
                    new Opc.ItemIdentifier(list[i]));

            return tags;
        }

    }

}
