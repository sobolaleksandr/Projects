using Reports.FileHandler;
using System.Diagnostics;

namespace Reports.Servers
{
    public abstract class Server : ServerLogger
    {
        readonly Opc.URL MasterUrl;
        readonly Opc.URL SlaveUrl;

        public Server(ServerProperties serverProperties):base()
        {
            MasterUrl = new Opc.URL(serverProperties.Path 
                + serverProperties.MasterIp + "/" + serverProperties.Type);

            SlaveUrl = new Opc.URL(serverProperties.Path 
                + serverProperties.SlaveIp + "/" + serverProperties.Type);

            CreateServer(MasterUrl);
        }

        public bool TryToConnect()
        {
            try
            {
                Connect();
                log.WriteEntry($"Подключились по MasterIP", EventLogEntryType.Information);
                return true;
            }
            catch (System.Exception e)
            {
                log.WriteEntry($"Ошибка при подключении к DA Серверу по MasterIP , '{e.Message}'", EventLogEntryType.Error);

                if (TryToConnectSlave())
                {
                    log.WriteEntry($"Подключились по SlaveIP", EventLogEntryType.Information);
                    return true;
                }

                return false;
            }
        }

        public abstract void CreateServer(Opc.URL url);

        bool TryToConnectSlave()
        {
            CreateServer(SlaveUrl);

            try
            {
                Connect();
                log.WriteEntry($"Подключились по SlaveIP", EventLogEntryType.Information);
                return true;
            }
            catch (System.Exception e)
            {
                log.WriteEntry($"Ошибка при подключении к DA Серверу по SlaveIP , '{e.Message}'", EventLogEntryType.Error);
                return false;
            }
        }

        public void TryToDisconnect()
        {
            try
            {
                Disconnect();
                log.WriteEntry($"Успешно отключились", EventLogEntryType.Information);
            }
            catch (System.Exception e)
            {
                log.WriteEntry($"Ошибка при отключиении , '{e.Message}'", EventLogEntryType.Error);
            }
        }

        public abstract void Connect();
        public abstract void Disconnect();
    }

}
