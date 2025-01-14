using ACE.Common.Extensions;
using ACE.Server.Network.GameMessages;
using ARC.Client.Entity;
using Character = ARC.Client.Entity.Character;
using InboundMessage = ACE.Server.Network.ClientMessage;

namespace ARC.Client.Network.GameMessages.Inbound;

public class ServerName : InboundGameMessage
{
    public static new GameMessageOpcode Opcode = GameMessageOpcode.ServerName;

    public int CurrentConnections { get; private set; }
    public int MaxConnections { get; private set; }
    public string Name { get; private set; }

    /// <see cref="ACE.Server.Network.GameMessages.Messages.GameMessageServerName"/>
    public override void Handle(InboundMessage message, Session session)
    {
        var reader = new BinaryReader(message.Data);

        CurrentConnections = reader.ReadInt32();
        MaxConnections = reader.ReadInt32();
        Name = reader.ReadString16L();
    }

    public override string ToString()
    {
        return $@"

        <<< GameMessage: ServerName [0x{(int)Opcode:X4}:{Opcode}]
            Server name:          {Name}
            Connected users:      {CurrentConnections} / {MaxConnections}

        ";
    }
}
