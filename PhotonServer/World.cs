using ExitGames.Logging;
using ExitGames.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhotonServer
{
    public class World
    {
        public static readonly World Instance = new World();

        public List<Client> Clients { get; private set; }

        private readonly ReaderWriterLockSlim readWriteLock;

        public World()
        {
            Clients = new List<Client>();
            readWriteLock = new ReaderWriterLockSlim();
        }

        public Client TryGetByName(string name)
        {
            using (ReadLock.TryEnter(this.readWriteLock, 1000))
            {
                return Clients.Find(n => n.CharacterName.Equals(name));
            }
        }

        public bool IsContain(string name)
        {
            using (ReadLock.TryEnter(this.readWriteLock, 1000))
            {
                return Clients.Exists(n => n.CharacterName.Equals(name));
            }
        }

        public void AddClient(Client client)
        {
            using (WriteLock.TryEnter(this.readWriteLock, 1000))
            {
                Clients.Add(client);
            }
        }

        public void RemoveClient(Client client)
        {
            using (WriteLock.TryEnter(this.readWriteLock, 1000))
            {
                Clients.Remove(client);
            }
        }

        ~World()
        {
            readWriteLock.Dispose();
        }
    }
}
