using System;
using System.Collections;
using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;

public class Recipient
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string FullName { get; set; }
    public string Signature { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string Zip { get; set; }
    public string ReturnState { get; set; }
    public string ReturnCity { get; set; }
    public string ReturnZip { get; set; }
    public string ReturnAddressLine1 { get; set; }

    public string ReturnAddressLine2 { get; set; }
    public string Title { get; set; }
    public string FsCode { get; set; }

    public string QRCodeUrl { get; set; }

    public IEnumerable<RecipientImage> Images { get; set; }
}
