namespace Aurigma.DirectMail.Sample.App.Exceptions.Company;

public class CompanyNotFoundException : Exception
{
    public Guid Id { get; set; }

    public CompanyNotFoundException() { }

    public CompanyNotFoundException(Guid id)
    {
        Id = id;
    }

    public CompanyNotFoundException(Guid id, string message)
        : base(message)
    {
        Id = id;
    }

    public CompanyNotFoundException(Guid id, string message, Exception innerException)
        : base(message, innerException)
    {
        Id = id;
    }
}
