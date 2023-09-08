using Messenger.Api.Utilities;
using Messenger.Core.Store;
using Messenger.Server;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers; 

[ApiController]
public class SessionController : ControllerBase {
  public MessengerServer MessengerServer { get; }
  public IMessengerStore Store { get; }

  public SessionController(MessengerServer messengerServer, IMessengerStore store) {
    MessengerServer = messengerServer;
    Store = store;
  }
  
  [HttpGet]
  [Route("/listen")]
  public async Task Get([FromQuery]Guid roomId) {
    if (HttpContext.WebSockets.IsWebSocketRequest) {
      using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
      var userId = Request.GetUserId();
      if (userId == null) {
        throw new Exception("No user id");
      }
      Store.AddUserToRoom(userId.Value, roomId);
      await MessengerServer.ConnectAsync(webSocket, userId.Value);
      Store.RemoveUserFromRoom(userId.Value, roomId);
      Store.RemoveUser(userId.Value);
    } else {
      HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
  }
}
