namespace Messenger.Api.Model; 

public class BaseCollectionResponse<T> {
  public List<T> Results { get; set; }
}
