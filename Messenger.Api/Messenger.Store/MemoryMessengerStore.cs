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
    return Rooms[roomId].UserIds.Select(id => Users[id]);
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

  public void RemoveUserFromRoom(Guid userId, Guid roomId) {
    Rooms[roomId].UserIds.Remove(userId);
  }

  public void RemoveUser(Guid userId) {
    Users.Remove(userId);
  }

  public Guid CreateMessage(Message message, Guid roomId) {
    var room = Rooms[roomId];
    room.Messages.Add(message);
    return message.Id;
  }

  public IEnumerable<MessageDetails> GetMessages(Guid roomId) {
    var room = Rooms[roomId];
    return room.Messages.Select(message => {
      var author = Users[message.AuthorId];
      return new MessageDetails {
        Id = message.Id,
        CreatedAt = message.CreatedAt,
        Type = message.Type,
        Body = message.Body,
        RoomId = message.RoomId,
        AuthorId = message.AuthorId,
        AuthorName = author.Name,
      };
    });
  }
}
