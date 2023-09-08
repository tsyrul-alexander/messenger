using Messenger.Core.Model;
using Messenger.Core.Store;

namespace Messenger.Store; 

public class MemoryMessengerStore : IMessengerStore {
  public Dictionary<Guid, Room> Rooms { get; set; } = new();
  public Dictionary<Guid, User> Users { get; set; } = new();

  public IEnumerable<Room> GetRooms() {
    return Rooms.Values;
  }

  public Room GetRoom(Guid id) {
    return Rooms[id];
  }

  public Guid CreateRoom(Room room) {
    Rooms.Add(room.Id, room);
    return room.Id;
  }
  
  public User GetUser(Guid userId) {
    return Users[userId];
  }

  public Room GetUsersRoom(Guid userId) {
    return Rooms.Values.FirstOrDefault(room => room.UserIds.Contains(userId));
  }

  public Guid CreateUser(User user) {
    Users.Add(user.Id, user);
    return user.Id;
  }
  
  public void AddUserToRoom(Guid userId, Guid roomId) {
    Rooms[roomId].UserIds.Add(userId);
  }

  public void RemoveUser(Guid userId, Guid roomId) {
    throw new NotImplementedException();
  }

  public Guid CreateMessage(Message message, Guid roomId) {
    var room = Rooms[roomId];
    room.Messages.Add(message);
    return message.Id;
  }

  public IEnumerable<Message> GetMessages(Guid roomId) {
    var room = Rooms[roomId];
    return room.Messages;
  }
}
