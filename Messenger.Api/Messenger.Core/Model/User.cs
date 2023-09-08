namespace Messenger.Core.Model;

public class User : IPrimaryEntity, IDisplayEntity {
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Name { get; set; }
}
