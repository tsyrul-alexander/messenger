namespace Messenger.Api.Model; 

public class SendMessage {
  public Guid RoomId { get; set; }
  public string Message { get; set; }
}
