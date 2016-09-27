using ExitGames.Diagnostics.Counter;
using ExitGames.Diagnostics.Monitoring;

namespace PhotonServer.Diagnostic
{
    public static class Counter
    {
        [PublishCounter("Games")]
        public static readonly NumericCounter Games = new NumericCounter("Games");
    }
}
