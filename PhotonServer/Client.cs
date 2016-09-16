using Photon.SocketServer;
using System;
using PhotonHostRuntimeInterfaces;
using ExitGames.Logging;
using System.Collections.Generic;
using PhotonServerLib.Common;
using PhotonServer.Operations;
using System.Linq;

namespace PhotonServer
{
    public class Client : ClientPeer
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        public Client(InitRequest initRequest) : base(initRequest)
        {
            log.Debug("Client ip:" + initRequest.RemoteIP);
        }

        public string CharacterName { get; set; }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            World.Instance.RemoveClient(this);
            log.Debug("Disconnect");
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            switch (operationRequest.OperationCode)
            {
                case (byte)OperationCode.Login:
                    var loginRequest = new Login(Protocol, operationRequest);

                    if (!loginRequest.IsValid)
                    {
                        SendOperationResponse(loginRequest.GetResponse(ErrorCode.InvalidParameters), sendParameters);
                        return;
                    }

                    CharacterName = loginRequest.CharacterName;

                    if (World.Instance.IsContain(CharacterName))
                    {
                        SendOperationResponse(loginRequest.GetResponse(ErrorCode.NameIsExist), sendParameters);
                        return;
                    }

                    World.Instance.AddClient(this);

                    var response = new OperationResponse(operationRequest.OperationCode);
                    SendOperationResponse(response, sendParameters);

                    log.Info("User name: " + CharacterName);
                    break;
                case (byte)OperationCode.SendChatMessage:
                    var chatRequest = new ChatMessage(Protocol, operationRequest);

                    if (!chatRequest.IsValid)
                    {
                        SendOperationResponse(chatRequest.GetResponse(ErrorCode.InvalidParameters), sendParameters);
                        return;
                    }

                    string message = chatRequest.Message;

                    string[] param = message.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (param.Length == 2)
                    {
                        string targeName = param[0];
                        message = param[1];
                        if (World.Instance.IsContain(targeName))
                        {
                            var targetClient = World.Instance.TryGetByName(targeName);
                            if (targetClient == null)
                            {
                                return;
                            }

                            message = CharacterName + "[PM]:" + message;

                            var personalEventData = new EventData((byte)EventCode.ChatMessage);
                            personalEventData.Parameters = new Dictionary<byte, object>() { { (byte)ParameterCode.ChatMessage, message } };
                            personalEventData.SendTo(new Client[] { this, targetClient }, sendParameters);
                        }

                        return;
                    }

                    message = CharacterName + ": " + message;
                    Chat.Instance.AddMessage(message);

                    var eventData = new EventData((byte)EventCode.ChatMessage);
                    eventData.Parameters = new Dictionary<byte, object>() { { (byte)ParameterCode.ChatMessage, message } };
                    eventData.SendTo(World.Instance.Clients, sendParameters);
                    break;

                case (byte)OperationCode.GetRecentChatMessages:
                    var messageChat = Chat.Instance.GetRecentMessages();
                    messageChat.Reverse();
                    var messagesChat = messageChat.Aggregate((i, j) =>i + "\r\n" + j);
                    var chatEventData = new EventData((byte)EventCode.ChatMessage);
                    chatEventData.Parameters = new Dictionary<byte, object>() { { (byte)ParameterCode.ChatMessage, messageChat } };
                    chatEventData.SendTo(new Client[] { this }, sendParameters);
                    break;
                default:
                    log.Debug("Unknown OperationRequest received:" + operationRequest.OperationCode);
                    break;
            }
        }
    }
}
