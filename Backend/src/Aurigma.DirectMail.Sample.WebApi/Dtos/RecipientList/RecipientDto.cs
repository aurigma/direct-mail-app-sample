using System;

namespace Aurigma.DirectMail.Sample.WebApi.Dtos.RecipientList;

public class RecipientDto
{
    /// <summary>
    /// Recipient identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Recipient first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Recipient full name.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Recipient signature.
    /// </summary>
    public string Signature { get; set; }

    /// <summary>
    /// Recipient state of residence.
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// Recipient city of residence.
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Address first line.
    /// </summary>
    public string AddressLine1 { get; set; }

    /// <summary>
    /// Address second line.
    /// </summary>
    public string AddressLine2 { get; set; }

    /// <summary>
    /// Postal zip code.
    /// </summary>
    public string Zip { get; set; }

    /// <summary>
    /// Return state.
    /// </summary>
    public string ReturnState { get; set; }

    /// <summary>
    /// Return city.
    /// </summary>
    public string ReturnCity { get; set; }

    /// <summary>
    /// Return postal zip code.
    /// </summary>
    public string ReturnZip { get; set; }

    /// <summary>
    /// Return address first line.
    /// </summary>
    public string ReturnAddressLine1 { get; set; }

    /// <summary>
    /// Return address second line.
    /// </summary>
    public string ReturnAddressLine2 { get; set; }

    /// <summary>
    /// Project title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// QR code line.
    /// </summary>
    public string QRCodeUrl { get; set; }
}
