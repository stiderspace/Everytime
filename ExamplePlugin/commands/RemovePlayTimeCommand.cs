using Smod2.Commands;

namespace EveryTime
{
    internal class RemovePlayTimeCommand : ICommandHandler
    {
        private EveryTime everyTime;

        public RemovePlayTimeCommand(EveryTime everyTime)
        {
            this.everyTime = everyTime;
        }

        public string GetCommandDescription()
        {
            throw new System.NotImplementedException();
        }

        public string GetUsage()
        {
            throw new System.NotImplementedException();
        }

        public string[] OnCall(ICommandSender sender, string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}