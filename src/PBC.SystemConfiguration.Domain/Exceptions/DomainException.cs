using PBC.SystemConfiguration.Domain.Enums;
using PBC.SystemConfiguration.Domain.Extensions;

namespace PBC.SystemConfiguration.Domain.Exceptions;

public abstract class DomainException(string message) : Exception(message)
{
    public int StatusCode { get; set; }
    public ResultEnum MessageEnum { get; set; }
}

public class EmptyFiledException : DomainException
{
    public EmptyFiledException(string filedName) : base(string.Format(ResultEnum.FiledIsEmpty.GetDescription(), filedName))
    {
        StatusCode = 400;
        MessageEnum = ResultEnum.FiledIsEmpty;
    }
}

public class ObjectNotFoundException : DomainException
{
    public ObjectNotFoundException(string objectName) : base(string.Format(ResultEnum.ObjectNotFound.GetDescription(), objectName))
    {
        StatusCode = 404;
        MessageEnum = ResultEnum.ObjectNotFound;
    }
}

public class ObjectAlreadyExistsException : DomainException
{
    public ObjectAlreadyExistsException(string objectName, string fieldName) : base(string.Format(ResultEnum.ObjectAlreadyExists.GetDescription(), objectName, fieldName))
    {
        StatusCode = 409;
        MessageEnum = ResultEnum.ObjectAlreadyExists;
    }
}