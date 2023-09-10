using Messenger.Core.Model;
using Messenger.Core.Store;

namespace Messenger.Store; 

public class MemoryMessengerStore : IMessengerStore {
  public Dictionary<Guid, Room> Rooms { get; set; } = new();
  public Dictionary<Guid, User> Users { get; set; } = new();

  public IEnumerable<Room> GetRooms() {
    return Rooms.Values;
  }

  public IEnumerable<User> GetUsers(Guid roomId) {
    return Rooms.TryGetValue(roomId, out var room) 
      ? room.UserIds.Select(id => 
        Users.TryGetValue(id, out var user) ? user : null).Where(user => user != null) 
      : Array.Empty<User>();
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
    var room = Rooms[roomId];
    if (room.UserIds.Contains(userId)) {
      return;
    }
    room.UserIds.Add(userId);
  }

  public void RemoveUserFromRoom(Guid userId, Guid roomId) {
    Rooms[roomId].UserIds.Remove(userId);
  }

  public void RemoveUser(Guid userId) {
    Users.Remove(userId);
  }
}
