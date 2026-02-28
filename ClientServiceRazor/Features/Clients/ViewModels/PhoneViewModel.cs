using System.ComponentModel.DataAnnotations;
using ClientServiceRazor.Features.Clients.Models;

namespace ClientServiceRazor.Features.Clients.ViewModels;

public class PhoneViewModel
{
    [Required]
    [StringLength(50)]
    public string Number { get; set; } = null!;
    [Required]
    public CountryCode CountryCode { get; set; }
}