using Messenger.Api.Model;
using Messenger.Core.Model;
using Messenger.Core.Store;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers; 

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase {
  public IMessengerStore Store { get; }

  public RoomController(IMessengerStore store) {
    Store = store;
  }
  
  [HttpGet]
  public GetRoomsResponse Get() {
    return new GetRoomsResponse {
      Results = Store.GetRooms().ToList()
    };
  }
  
  [HttpPost]
  public CreateRoomResponse Post([FromBody]CreateRoom request) {
    var roomId = Store.CreateRoom(new Room {
      Id = Guid.NewGuid(),
      Name = request.Name
    });
    return new CreateRoomResponse {
      Id = roomId
    };
  }
}
