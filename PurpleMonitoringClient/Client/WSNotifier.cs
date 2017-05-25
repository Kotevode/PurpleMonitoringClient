using PurpleMonitoringClient.Model.Executing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace PurpleMonitoringClient.Client
{
    class WSNotifier : INotifier
    {
        MessageWebSocket webSocket;

        public WSNotifier(Uri url, Command command)
        {
            webSocket = new MessageWebSocket();
            webSocket.Control.MessageType = SocketMessageType.Utf8;
            webSocket.MessageReceived += Ws_MessageReceived;
            webSocket.Closed += Ws_Closed;
        }

        async void Execute(Uri url, Command command)
        {
            //await webSocket.ConnectAsync(url);
            //var writer = new DataWriter(webSocket.OutputStream);
            //writer.WriteString(JsonSer)
        }

        private void Ws_Closed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void Ws_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<ClusterCreatedEventArgs> OnClusterCreated;
        public event EventHandler<ProcessingStartedEventArgs> OnProcessingStarted;
        public event EventHandler<JobStatusEventArgs> OnJobStatus;
        public event EventHandler<ProcessingDoneEventArgs> OnProcessingDone;
        public event EventHandler<ClusterFinalizedEventArgs> OnClusterFinalized;
        public event EventHandler<LogMessageEventArgs> OnLogMessage;
    }
}
