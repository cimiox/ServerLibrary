using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotonServer
{
    public class Actor
    {
        private readonly List<ActorGroup> groups = new List<ActorGroup>();

        public int ActorNr { get; set; }
        public PeerBase Peer { get; set; }
        //TODO: public PropertyBag<object> Properties {get; private set;}
        #region Constructors
        public Actor()
        {
            //TODO: Properties = new PropertyBag<object>();
        }

        public Actor(PeerBase peer)
            : this()
        {
            Peer = peer;
        }
        #endregion Constructors

        #region Methods

        public override string ToString()
        {
            return string.Format($"Actor {ActorNr}: Peer{Peer}");
        }

        public void AddGroup(ActorGroup group)
        {
            groups.Add(group);
            group.Add(this);
        }

        public void RemoveGroups(byte[] groupIds)
        {
            if (groupIds == null)
            {
                return;
            }

            if (groupIds.Length == 0)
            {
                RemoveAllGroups();
                return;
            }

            foreach (var group in groupIds)
            {
                RemoveGroup(group);
            }
        }

        private void RemoveAllGroups()
        {
            foreach (var group in groups)
            {
                group.RemoveActorByPeer((PhotonPeer)Peer);
            }
        }

        private void RemoveGroup(byte group)
        {
            int actorGroupIndex = groups.FindIndex(g => g.GroupId == group);
            if (actorGroupIndex == -1)
            {
                return;
            }

            groups[actorGroupIndex].RemoveActorByPeer((PhotonPeer)Peer);
            groups.RemoveAt(actorGroupIndex);
        }

        #endregion Methods
    }
}
