using Messenger.Core.Model;

namespace Messenger.Core.Store; 

public interface IMessengerStore {
  IEnumerable<Room> GetRooms();
  IEnumerable<User> GetUsers(Guid roomId);
  Room GetRoom(Guid id);
  Guid CreateRoom(Room room);
  Guid CreateUser(User user);
  void AddUserToRoom(Guid userId, Guid roomId);
  void RemoveUserFromRoom(Guid userId, Guid roomId);
  void RemoveUser(Guid userId);
  User GetUser(Guid getUserId);
  Room GetUsersRoom(Guid userId);
}
