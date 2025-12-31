using System.ComponentModel;

namespace PBC.SystemConfiguration.Domain.Enums;

public enum ResultEnum
{
    [Description("Successful")]
    Success,
    
    [Description("Created successfully")] 
    CreatedSuccessfully,  
    
    [Description("Updated successfully")]
    UpdatedSuccessfully,
    
    [Description("Deleted successfully")]
    DeletedSuccessfully,
    
    [Description("Unexpected error")]
    UnexpectedError,
}