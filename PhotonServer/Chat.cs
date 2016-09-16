﻿using ExitGames.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhotonServer
{
    public class Chat
    {
        public static readonly Chat Instance = new Chat();

        private List<string> Messages { get; set; }

        private readonly ReaderWriterLockSlim readWrtiteLock;
        private const int MAX_RECENT_MESSAGES = 10;
        private const int MAX_BUFFERED_MESSAGES = 1000;

        public Chat()
        {
            Messages = new List<string>();
            readWrtiteLock = new ReaderWriterLockSlim();
        }

        ~Chat()
        {
            readWrtiteLock.Dispose();
        }

        public List<string> GetRecentMessages()
        {
            using (ReadLock.TryEnter(this.readWrtiteLock, 1000))
            {
                List<string> list = new List<string>(MAX_RECENT_MESSAGES);
                if (Messages.Count == 0)
                {
                    return list;
                }

                if (Messages.Count <= MAX_RECENT_MESSAGES)
                {
                    list.AddRange(Messages);
                    return list;
                }

                return Messages.Skip(Messages.Count-MAX_RECENT_MESSAGES).ToList();
            }
        }

        public void AddMessage(string message)
        {
            using (WriteLock.TryEnter(this.readWrtiteLock, 1000))
            {
                Messages.Add(message);

                if (Messages.Count > MAX_BUFFERED_MESSAGES)
                {
                    Messages = Messages.Skip(Messages.Count - MAX_RECENT_MESSAGES).ToList();
                }
            }
        }
    }
}
