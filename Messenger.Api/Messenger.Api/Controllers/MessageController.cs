using Messenger.Api.Model;
using Messenger.Api.Utilities;
using Messenger.Core.Model;
using Messenger.Core.Store;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers; 

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase {
  public IMessengerStore Store { get; }

  public MessageController(IMessengerStore store) {
    Store = store;
  }
  
  [HttpGet]
  public GetMessagesResponse Get(Guid roomId) {
    return new GetMessagesResponse {
      Results = Store.GetMessages(roomId).ToList()
    };
  }

  [HttpPost]
  public SendMessageResponse Post([FromBody]SendMessage message) {
    var userId = HttpContext.Request.GetUserId();
    if (userId == null) {
      throw new ArgumentException(nameof(userId));
    }
    var id = Store.CreateMessage(new Message {
      Body = message.Message,
      AuthorId = userId.Value
    }, message.RoomId);
    return new SendMessageResponse {
      Id = id
    };
  }
}
