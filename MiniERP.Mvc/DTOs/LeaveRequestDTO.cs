using System.ComponentModel.DataAnnotations;

namespace MiniERP.Mvc.DTOs;

public record LeaveRequestCreateDTO(
    [Required] int EmployeeId,
    [Required] int LeaveTypeId,
    [Required] DateTime FromDate,
    [Required] DateTime ToDate,
    [StringLength(255)] string? Reason
);

public record LeaveRequestUpdateDTO(
    DateTime? FromDate,
    DateTime? ToDate,
    string? Reason
);
