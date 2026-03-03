using System.ComponentModel.DataAnnotations;

namespace ClientServiceRazor.Features.Clients.ViewModels;

public class AddressViewModel
{
    public int ClientId { get; set; }

    [Required]
    public string Country { get; set; } = null!;

    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}
