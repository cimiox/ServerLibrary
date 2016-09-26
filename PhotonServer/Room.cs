using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotonServer
{
    public class Room : IDisposable
    {
        #region Properties

        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        //TODO:private readonly RoomCacheBase 

        private readonly string name;
        public string Name
        {
            get { return name; }
        }

        private IDisposable removeTimer;
        protected IDisposable RemoveTimer { get; set; }

        private int emptyRoomLiveTime;
        private PoolFiber poolFiber;
        private int emptyRoomLiveTime1;

        public int EmptyRoomLiveTime
        {
            get { return emptyRoomLiveTime; }
            protected set { emptyRoomLiveTime = value; }
        }

        //TODO: PropertyBag<object> property

        //TODO: ActorCollection property

        public PoolFiber ExecutionFiber { get; private set; }

        public bool IsDisposed { get; private set; }

        #endregion Properties

        #region Constructors

        public Room(string name, /*TODO: RoomCacheBase*/ int emptyRoomLiveTime = 0)
            : this(name, new PoolFiber(), emptyRoomLiveTime)
        {
            ExecutionFiber.Start();
        }

        public Room(string name, PoolFiber executionFiber, /*TODO: RoomCacheBase*/ int emptyRoomLiveTime = 0)
        {
            this.name = name;
            ExecutionFiber = executionFiber;
            //TODO: ActorsCollection
            //TODO: PropertyBag
            //TODO: RoomCacheBase
            this.emptyRoomLiveTime = emptyRoomLiveTime;
        }

        ~Room()
        {
            Dispose(false);
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            IsDisposed = true;

            if (dispose)
            {
                ExecutionFiber.Dispose();
                if (removeTimer != null)
                {
                    removeTimer.Dispose();
                    removeTimer = null;
                }
            }
        }

        protected virtual void ExecuteOperation(PhotonPeer peer, OperationRequest operation, SendParameters sendParameters)
        { }

        protected virtual void ProcessMessage(/*TODO: IMessage*/)
        {
        }

        protected void PublishEvent(/*TODO: LiteEventBase, Actor*/SendParameters sendParameters)
        {
            //TODO: var eventData = new EventData(e.Code, e);
            //TODO: actor.Peer.SendEvent(eventData, sendParameters);
        }

        protected void PublishEvent(/*TODO: liteEventBase, IEnumerable<Actor>, SendParameters*/)
        {
            //TODO: PublishEvent
        }

        protected void PublishEvent(EventData e,/*TODO: liteEventBase, IEnumerable<Actor>*/ SendParameters sendParameters)
        {
            //TODO: PublishEvent
        }

        protected void ScheduleRoomRemoval(int roomLiveTime)
        {
            if (RemoveTimer != null)
            {
                RemoveTimer.Dispose();
                RemoveTimer = null;
            }

            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat($"Schedule room removal: roomName={Name}, liveTime={roomLiveTime: N0}");
            }

            RemoveTimer = ExecutionFiber.Schedule(TryRemoveFromCache, roomLiveTime);
        }

        protected void TryRemoveFromCache()
        {
            //TODO: bool removed = roomCache.TryRemoveRoomInstance(this);

            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat($"Tried to remove room: roomName={Name}, removed={"removed"}");
            }
        }

        #endregion Methods
    }
}
