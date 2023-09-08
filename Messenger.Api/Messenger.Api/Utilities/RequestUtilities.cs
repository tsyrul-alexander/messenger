namespace Messenger.Api.Utilities; 

public static class RequestUtilities {
  public const string UserCookieKey = "user_id";
  
  public static Guid? GetUserId(this HttpRequest request) {
    return request.Cookies.TryGetValue(UserCookieKey, out var value) ? Guid.Parse(value) : null;
  }
  
  public static void SetUserId(this HttpResponse request, Guid userId) {
    request.Cookies.Append(UserCookieKey, userId.ToString());
  }
}
