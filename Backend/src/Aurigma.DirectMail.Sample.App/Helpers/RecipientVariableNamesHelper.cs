using System.Reflection;

namespace Aurigma.DirectMail.Sample.App.Helpers;

public static class RecipientVariableNamesHelper
{
    public const string FirstName = "First";
    public const string Zip = "Zip";
    public const string State = "ST";
    public const string City = "City";
    public const string AddressLine1 = "Address1";
    public const string AddressLine2 = "Address2";
    public const string FullName = "RecipientFull";
    public const string ReturnZip = "ReturnZip";
    public const string ReturnState = "ReturnST";
    public const string ReturnCity = "ReturnCity";
    public const string ReturnAddressLine1 = "ReturnAddress1";
    public const string ReturnAddressLine2 = "ReturnAddress2";
    public const string Title = "Title";
    public const string Signature = "Signature";
    public const string FsCode = "fs_code";
    public const string Image = "Your Image";
    public const string QrCodeUrl = "QR code URL";

    public static List<string> GetNames()
    {
        var type = typeof(RecipientVariableNamesHelper);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        var constantValues = fields
            .Where(f => f.IsLiteral && !f.IsInitOnly)
            .Select(f => f.GetValue(null).ToString())
            .ToList();

        return constantValues;
    }
}
