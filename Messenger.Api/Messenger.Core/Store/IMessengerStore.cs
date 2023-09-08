using Messenger.Core.Model;

namespace Messenger.Core.Store; 

public interface IMessengerStore {
  IEnumerable<Room> GetRooms();
  IEnumerable<User> GetUsers(Guid roomId);
  IEnumerable<MessageDetails> GetMessages(Guid roomId);
  Room GetRoom(Guid id);
  Guid CreateRoom(Room room);
  Guid CreateUser(User user);
  void AddUserToRoom(Guid userId, Guid roomId);
  void RemoveUserFromRoom(Guid userId, Guid roomId);
  void RemoveUser(Guid userId);
  Guid CreateMessage(Message message, Guid roomId);
  User GetUser(Guid getUserId);
  Room GetUsersRoom(Guid userId);
}
