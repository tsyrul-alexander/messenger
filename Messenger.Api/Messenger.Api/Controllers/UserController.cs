using Messenger.Api.Model;
using Messenger.Api.Utilities;
using Messenger.Core.Model;
using Messenger.Core.Store;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers; 

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase {
  protected IMessengerStore Store { get; }

  public UserController(IMessengerStore store) {
    Store = store;
  }

  [HttpGet]
  public GetUsersResponse Get(Guid roomId) {
    return new GetUsersResponse {
      Results = Store.GetUsers(roomId).ToList()
    };
  }
  
  [HttpPost]
  public CreateUserResponse Post([FromBody] CreateUser request) {
    var userId = Store.CreateUser(new User {
      Name = request.Name,
      PublicKey = request.PublicKey
    });
    Response.SetUserId(userId);
    return new CreateUserResponse {
      Id = userId
    };
  }
}
