namespace PBC.SystemConfiguration.Application.Interfaces;

public interface IResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
}