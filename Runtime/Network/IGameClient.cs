namespace mazing.common.Runtime.Network
{
    public interface IGameClient : IInit
    {
        void Send(IPacket _Packet, bool _Async = true);
    }
}