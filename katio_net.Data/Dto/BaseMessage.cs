using System.Net;

namespace katio.Data.Dto;

public class BaseMessage<T>
    where T : class
{
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public int TotalElements { get; set; }
    public IEnumerable<T> ResponseElements { get; set; }
}

public static class BaseMessageStatus
{
    public const string OK_200 = "200 OK";
    public const string BAD_REQUEST_400 = "400 Bar Request";
    public const string INTERNAL_SERVER_ERROR_500 = "500 Internal Server Error";
    public const string NOT_FOUND_404 = "404 Not Found";
    public const string BOOK_NOT_FOUND = "404 Book Not Found";
    public const string AUTHOR_NOT_FOUND = "404 Author Not Found";
    public const string AUDIOBOOK_NOT_FOUND = "404 AudioBook Not Found";
    public const string NARRATOR_NOT_FOUND = "404 Narrator Not Found";
    public const string GENRE_NOT_FOUND = "404 Genre Not Found";
}