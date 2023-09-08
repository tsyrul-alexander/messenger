namespace Messenger.Core.Model; 

public class Message : IPrimaryEntity {
  public Guid Id { get; set; } = Guid.NewGuid();
  public MessageType Type { get; set; }
  public string Body { get; set; }
  public Guid AuthorId { get; set; }
  public Guid RoomId { get; set; }
}
