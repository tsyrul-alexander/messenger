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
    Connections.Add(userId, socket);
    await BroadcastNewUser(userId);
    await ListenAsync(userId);
  }

  private async Task ListenAsync(Guid userId) {
    while (true) {
      var buffer = new ArraySegment<byte>(new byte[1024]);
      var socket = Connections[userId];
      var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
      if (result.MessageType == WebSocketMessageType.Close) {
        CloseSocket(userId);
        return;
      }
      var userMessage = GetUserMessage(buffer.ToArray());
      var room = Store.GetUsersRoom(userId);
      var message = new Message {
        RoomId = room.Id,
        AuthorId = userId,
        Type = MessageType.User,
        Body = userMessage.Message
      };
      Store.CreateMessage(message, room.Id);
      await BroadcastMessage(message);
    }
  }

  protected virtual Task BroadcastNewUser(Guid userId) {
    var room = Store.GetUsersRoom(userId);
    var message = new Message {
      RoomId = room.Id,
      AuthorId = userId,
      Type = MessageType.System,
      Body = $"Welcome to {room.Name}!"
    };
    message.Id = Store.CreateMessage(message, room.Id);
    return BroadcastMessage(message);
  }
  
  protected virtual async Task BroadcastMessage(Message message) {
    var buffer = GetBuffer(message);
    foreach (var connection in Connections) {
      await BroadcastMessage(connection.Key, buffer);
    }
  }

  protected virtual async Task BroadcastMessage(Guid userId, byte[] message) {
    var socket = Connections[userId];
    var segment = new ArraySegment<byte>(message);
    if (socket.State != WebSocketState.Open) {
      CloseSocket(userId);
      return;
    }
    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
  }
  
  protected virtual void CloseSocket(Guid userId) {
    throw new NotImplementedException();
  }
  
  private UserMessage GetUserMessage(byte[] buffer) {
    var body = Encoding.UTF8.GetString(buffer);
    return JsonConvert.DeserializeObject<UserMessage>(body);
  }

  private byte[] GetBuffer(Message message) {
    return Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(message));
  }
}
