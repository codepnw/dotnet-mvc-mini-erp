using System.ComponentModel.DataAnnotations;

namespace MiniERP.Mvc.DTOs;

public class LeaveRequestCreateDto
{
    [Required(ErrorMessage = "Employee id is required")]
    public required int EmployeeId { get; init; }

    [Required(ErrorMessage = "Leave request id is required")]
    public required int LeaveTypeId { get; init; }

    [Required(ErrorMessage = "From date is required")]
    public required DateTime FromDate { get; init; }

    [Required(ErrorMessage = "To date is required")]
    public required DateTime ToDate { get; init; }

    [StringLength(255)] public string? Reason { get; init; }
};

public class LeaveRequestUpdateDto
{
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    [StringLength(255)] public string? Reason { get; init; }
};