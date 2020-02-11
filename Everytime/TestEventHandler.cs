using Smod2.EventHandlers;
using Smod2.Events;

namespace EveryTime
{
    internal class TestEventHandler : IEventHandler, IEventHandlerRoundEnd, IEventHandlerRoundRestart, IEventHandlerRoundStart, IEventHandlerWaitingForPlayers
    {
        private EveryTime everyTime;

        public TestEventHandler(EveryTime everyTime)
        {
            this.everyTime = everyTime;
        }

        public void OnRoundEnd(RoundEndEvent ev)
        {
            everyTime.Error("round ended");
        }

        public void OnRoundRestart(RoundRestartEvent ev)
        {
            everyTime.Error("round restart");
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            everyTime.Error("round start");
        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            everyTime.Error("waiting on players");
        }
    }
}