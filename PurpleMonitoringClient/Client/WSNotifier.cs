using PurpleMonitoringClient.Model.Executing;
using System;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PurpleMonitoringClient.Client
{
    class WSNotifier : INotifier
    {
        MessageWebSocket webSocket;

        public WSNotifier()
        {
            webSocket = new MessageWebSocket();
            webSocket.Control.MessageType = SocketMessageType.Utf8;
            webSocket.MessageReceived += Ws_MessageReceived;
            webSocket.Closed += Ws_Closed;
        }

        public async Task ExecuteAsync(String hostName, Command command)
        {
            await webSocket.ConnectAsync(new UriBuilder("ws", hostName, 8080, "execute").Uri);
            var messageWriter = new DataWriter(webSocket.OutputStream);
            messageWriter.WriteString(JsonConvert.SerializeObject(command));
            await messageWriter.StoreAsync();
        }

        private void Ws_Closed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            if (args.Code == 4000)
                OnTerminated?.Invoke(this, new ClusterEventArgs<Terminated>(
                    DateTime.Now,
                    new Terminated { Succeed = true }));
            else
                OnTerminated?.Invoke(this, new ClusterEventArgs<Terminated>(
                    DateTime.Now,
                    new Terminated { Succeed = false, Message = args.Reason }));
        }

        private void Ws_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            try
            {
                var reader = args.GetDataReader();
                reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                var jsonString = reader.ReadString(reader.UnconsumedBufferLength);
                if (String.IsNullOrEmpty(jsonString))
                    return;
                var json = JObject.Parse(jsonString);
                Broadcast(json);
            }
            catch (JsonException)
            {
                webSocket.Close(4000, "Wrong message format");
            }
        }

        private void Broadcast(JObject json)
        {
            string type = json["type"].ToString();
            switch (type)
            {
                case "cluster_created":
                    OnClusterCreated?.Invoke(this, json.ToObject<ClusterEventArgs<ClusterCreated>>());
                    break;
                case "processing_started":
                    OnProcessingStarted?.Invoke(this, json.ToObject<ClusterEventArgs<ProcessingStarted>>());
                    break;
                case "job_status_changed":
                    OnJobStatusChanged?.Invoke(this, json.ToObject<ClusterEventArgs<JobStatusChanged>>());
                    break;
                case "processing_done":
                    OnProcessingDone?.Invoke(this, json.ToObject<ClusterEventArgs<ProcessingDone>>());
                    break;
                case "cluster_finalized":
                    OnClusterFinalized?.Invoke(this, json.ToObject<ClusterEventArgs<ClusterFinalized>>());
                    break;
                case "log_message":
                    OnLogMessage?.Invoke(this, json.ToObject<ClusterEventArgs<LogMessage>>());
                    break;
                default:
                    break;
            }
        }

        public event EventHandler<ClusterEventArgs<ClusterCreated>> OnClusterCreated;
        public event EventHandler<ClusterEventArgs<ProcessingStarted>> OnProcessingStarted;
        public event EventHandler<ClusterEventArgs<JobStatusChanged>> OnJobStatusChanged;
        public event EventHandler<ClusterEventArgs<ProcessingDone>> OnProcessingDone;
        public event EventHandler<ClusterEventArgs<ClusterFinalized>> OnClusterFinalized;
        public event EventHandler<ClusterEventArgs<LogMessage>> OnLogMessage;
        public event EventHandler<ClusterEventArgs<Terminated>> OnTerminated;
    
    }
}
