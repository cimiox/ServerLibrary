﻿using Photon.SocketServer;
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

        public Vector3Net Position { get; private set; }
        public string CharacterName { get; private set; }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            World.Instance.RemoveClient(this);
            var sendParameters = new SendParameters();
            sendParameters.Unreliable = true;
            WorldExitHandler(sendParameters);
            log.Debug("Disconnect");
        }

        private void WorldExitHandler(SendParameters sendParameters)
        {
            var eventData = new EventData((byte)EventCode.WorldExit);
            eventData.Parameters = new Dictionary<byte, object>()
                    {
                        { (byte)ParameterCode.CharacterName, CharacterName }
                    };
            eventData.SendTo(World.Instance.Clients, sendParameters);
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

                    if (messageChat.Count == 0)
                        break;

                    var messagesChat = messageChat.Aggregate((i, j) =>i + "\r\n" + j);
                    var chatEventData = new EventData((byte)EventCode.ChatMessage);
                    chatEventData.Parameters = new Dictionary<byte, object>() { { (byte)ParameterCode.ChatMessage, messageChat } };
                    chatEventData.SendTo(new Client[] { this }, sendParameters);
                    break;

                case (byte)OperationCode.Move:
                    var moveRequest = new Move(Protocol, operationRequest);

                    if (!moveRequest.IsValid)
                    {
                        SendOperationResponse(moveRequest.GetResponse(ErrorCode.InvalidParameters), sendParameters);
                        return;
                    }

                    Position = new Vector3Net(moveRequest.X, moveRequest.Y, moveRequest.Z);
                    var moveEventData = new EventData((byte)EventCode.Move);
                    moveEventData.Parameters = new Dictionary<byte, object>()
                    { 
                        { (byte)ParameterCode.PosX, Position.X },
                        { (byte)ParameterCode.PosY, Position.Y },
                        { (byte)ParameterCode.PosZ, Position.Z },
                        { (byte)ParameterCode.CharacterName, CharacterName }
                    };
                    moveEventData.SendTo(World.Instance.Clients, sendParameters);
                    break;
                case (byte)OperationCode.WorldEnter:
                    var enterEventData = new EventData((byte)EventCode.WorldEnter);
                    enterEventData.Parameters = new Dictionary<byte, object>()
                    {
                        { (byte)ParameterCode.PosX, Position.X },
                        { (byte)ParameterCode.PosY, Position.Y },
                        { (byte)ParameterCode.PosZ, Position.Z },
                        { (byte)ParameterCode.CharacterName, CharacterName }
                    };
                    enterEventData.SendTo(World.Instance.Clients, sendParameters);
                    break;
                case (byte)OperationCode.WorldExit:
                    WorldExitHandler(sendParameters);
                    break;
                case (byte)OperationCode.ListPlayers:
                    ListPlayersHandler(sendParameters);
                    break;
                default:
                    log.Debug("Unknown OperationRequest received:" + operationRequest.OperationCode);
                    break;
            }
        }

        private void ListPlayersHandler(SendParameters sendParameters)
        {
            OperationResponse response = new OperationResponse((byte)OperationCode.ListPlayers);

            var players = World.Instance.Clients;
            var dicPlayers = new Dictionary<string, object[]>();

            foreach (var p in players)
            {
                if (!p.CharacterName.Equals(CharacterName))
                    dicPlayers.Add(p.CharacterName, new object[] { p.Position.X, p.Position.Y, p.Position.Z });
            }

            response.Parameters = new Dictionary<byte, object> { { (byte)ParameterCode.ListPlayers, dicPlayers } };
            SendOperationResponse(response, sendParameters);
        }
    }
}
