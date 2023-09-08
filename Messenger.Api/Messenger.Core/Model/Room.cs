namespace Messenger.Core.Model;

public class Room : IPrimaryEntity, IDisplayEntity {
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Name { get; set; }
  public List<Guid> UserIds { get; set; } = new();
  public List<Message> Messages { get; set; } = new();
}
