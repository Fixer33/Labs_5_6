using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public enum ServerPacket : byte
    {
        IdExistsResponse = 0,
        ChatCreated = 1,
        ChatUpdated = 2,
        ChatDeleted = 3,
        UserNameResponse = 4,
        UserNameChanged = 5,
        ChatDataNotification = 6,
        ChatMessageSent = 7,
    }

    public enum ClientPacket : byte
    {
        ClientIdInit = 0,
        ClientNameChange = 1,
        CheckIdExists = 2,
        ChatCreate = 3,
        ChatUpdate = 4,
        ChatDelete = 5,
        UserNameRequest = 6,
        ChatDataRequest = 7,
        ChatMessageSent = 8,
    }
}
