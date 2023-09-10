using System.Net.WebSockets;
using System.Text;
using Messenger.Core.Model;
using Messenger.Core.Store;
using Messenger.Server.Model;
using Newtonsoft.Json;

namespace Messenger.Server; 

public class MessengerServer {
  public IMessengerStore Store { get; }
  public Dictionary<Guid, WebSocket> Connections { get; } = new();

  public MessengerServer(IMessengerStore store) {
    Store = store;
  }
  
  public async Task ConnectAsync(WebSocket socket, Guid userId) {
    Connections[userId] = socket;
    await BroadcastUser(userId);
    await ListenAsync(userId);
  }

  private async Task ListenAsync(Guid userId) {
    while (true) {
      var buffer = new ArraySegment<byte>(new byte[1024]);
      var socket = Connections[userId];
      var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
      if (result.MessageType == WebSocketMessageType.Close) {
        await DisconnectUser(userId);
        return;
      }
      var userMessage = GetUserMessage(buffer.ToArray());
      await BroadcastMessage(userMessage, userId);
    }
  }

  protected virtual Task BroadcastUser(Guid senderId) {
    var room = Store.GetUsersRoom(senderId);
    var message = new Message {
      RoomId = room.Id,
      AuthorId = senderId,
      Type = MessageType.User
    };
    return BroadcastMessage(message, senderId);
  }
  
  protected virtual async Task BroadcastMessage(UserMessage userMessage, Guid senderId) {
    var sender = Store.GetUser(senderId);
    var room = Store.GetUsersRoom(senderId);
    var createdAt = DateTime.UtcNow;
    foreach (var recipientId in room.UserIds) {
      if (!userMessage.Messages.TryGetValue(recipientId, out var messageBody)) {
        continue;
      }
      var message = new Message {
        Id = Guid.NewGuid(),
        CreatedAt = createdAt,
        AuthorId = senderId,
        AuthorName = sender.Name,
        Type = MessageType.Message,
        Body = messageBody,
        RoomId = room.Id
      };
      var buffer = GetBuffer(message);
      await BroadcastMessage(recipientId, buffer);
    }
  }
  
  protected virtual async Task BroadcastMessage(Message message, Guid senderId) {
    var buffer = GetBuffer(message);
    var room = Store.GetUsersRoom(senderId);
    foreach (var recipientId in room.UserIds) {
      if (recipientId == senderId) {
        continue;
      }
      await BroadcastMessage(recipientId, buffer);
    }
  }

  protected virtual async Task BroadcastMessage(Guid userId, byte[] message) {
    var socket = Connections[userId];
    var segment = new ArraySegment<byte>(message);
    if (socket.State != WebSocketState.Open) {
      await DisconnectUser(userId);
      return;
    }
    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
  }
  
  protected virtual async Task DisconnectUser(Guid userId) {
    Connections.Remove(userId);
    await BroadcastUser(userId);
  }
  
  private UserMessage GetUserMessage(byte[] buffer) {
    var body = Encoding.UTF8.GetString(buffer);
    return JsonConvert.DeserializeObject<UserMessage>(body);
  }

  private byte[] GetBuffer(Message message) {
    return Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(message));
  }
}
