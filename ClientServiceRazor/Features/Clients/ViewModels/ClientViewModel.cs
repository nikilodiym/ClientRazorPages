using System.ComponentModel.DataAnnotations;

namespace ClientServiceRazor.Features.Clients.ViewModels;

public class ClientViewModel
{
    [Required]
    [StringLength(50)]
    public string Surname { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string? Patronymic { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    //TODO
    [DataType(DataType.Date)]
    public DateOnly BirthDate { get; set; }
}