namespace Messenger.Core.Model; 

public class Message : IPrimaryEntity, ICreatedAtEntity {
  public Guid Id { get; set; } = Guid.NewGuid();
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public MessageType Type { get; set; }
  public string Body { get; set; }
  public Guid AuthorId { get; set; }
  public Guid RoomId { get; set; }
}
