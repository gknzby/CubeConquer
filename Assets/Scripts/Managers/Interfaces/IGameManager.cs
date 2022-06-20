namespace CubeConquer.Managers
{
    public interface IGameManager : IManager
    {
        void SendGameAction(GameAction gameAction);
    }
}
