using Newtonsoft.Json;

namespace Messenger.Core.Model; 

public class Message : IPrimaryEntity, ICreatedAtEntity {
  [JsonProperty("id")]
  public Guid Id { get; set; } = Guid.NewGuid();
  [JsonProperty("createdAt")]
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  [JsonProperty("type")]
  public MessageType Type { get; set; }
  [JsonProperty("body")]
  public string Body { get; set; }
  [JsonProperty("authorId")]
  public Guid AuthorId { get; set; }
  [JsonProperty("authorName")]
  public string AuthorName { get; set; }
  [JsonProperty("roomId")]
  public Guid RoomId { get; set; }
}
