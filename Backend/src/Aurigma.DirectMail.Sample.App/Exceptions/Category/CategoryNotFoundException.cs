namespace Aurigma.DirectMail.Sample.App.Exceptions.Category;

public class CategoryNotFoundException : Exception
{
    public Guid Id { get; set; }

    public CategoryNotFoundException() { }

    public CategoryNotFoundException(Guid id)
    {
        Id = id;
    }

    public CategoryNotFoundException(Guid id, string message)
        : base(message)
    {
        Id = id;
    }

    public CategoryNotFoundException(Guid id, string message, Exception innerException)
        : base(message, innerException)
    {
        Id = id;
    }
}
