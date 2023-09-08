using Messenger.Core.Model;

namespace Messenger.Core.Store; 

public interface IMessengerStore {
  IEnumerable<Room> GetRooms();
  Room GetRoom(Guid id);
  Guid CreateRoom(Room room);
  Guid CreateUser(User user);
  void AddUserToRoom(Guid userId, Guid roomId);
  void RemoveUser(Guid userId, Guid roomId);
  Guid CreateMessage(Message message, Guid roomId);
  IEnumerable<Message> GetMessages(Guid roomId);
  User GetUser(Guid getUserId);
  Room GetUsersRoom(Guid userId);
}
