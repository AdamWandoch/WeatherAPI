using System.Net;

namespace Domain.DTO;

public sealed class ResponseDTO<T> where T : class
{
    public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
}