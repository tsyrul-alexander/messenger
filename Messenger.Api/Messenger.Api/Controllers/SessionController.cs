using Messenger.Api.Utilities;
using Messenger.Core.Store;
using Messenger.Server;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers; 

[ApiController]
public class SessionController : ControllerBase {
  public MessengerServer MessengerServer { get; }

  public SessionController(MessengerServer messengerServer) {
    MessengerServer = messengerServer;
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
      await MessengerServer.ConnectAsync(webSocket, userId.Value, roomId);
    } else {
      HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
  }
}
