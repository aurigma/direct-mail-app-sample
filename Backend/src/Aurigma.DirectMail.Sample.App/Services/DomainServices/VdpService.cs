using System.Data;
using System.Web;
using Aurigma.DesignAtoms.Model;
using Aurigma.DirectMail.Sample.App.Helpers;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.VDP;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Variable;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;
using Newtonsoft.Json.Linq;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class VdpService(
    IDesignAtomsServiceAdapter designAtomsServiceAdapter,
    ITokenAdapter tokenAdapter
) : IVdpService
{
    private readonly IDesignAtomsServiceAdapter _designAtomsServiceAdapter =
        designAtomsServiceAdapter;
    private readonly ITokenAdapter _tokenAdapter = tokenAdapter;

    public async Task SendVdpDataAsync(VdpSendDataAppModel model)
    {
        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var adapterModel = new VdpSendDataAdapterModel()
        {
            DesignId = model.DesignId,
            UserId = model.UserId,
            DataSet = model.DataSet,
        };

        await _designAtomsServiceAdapter.SendVdpDataAsync(adapterModel, token);
    }

    public IEnumerable<Variable> GetRecipientVariableData(
        Recipient recipient,
        List<VdpBuildDataSetImageAppModel> images
    )
    {
        var imageVariables = images.Select(i => new Variable()
        {
            Name = i.Name.Split('.').First(),
            Type = ToVariableType(i.Type),
            Value = i.Value,
        });

        var textVariables = new List<Variable>
        {
            new Variable
            {
                Name = RecipientVariableNamesHelper.FullName,
                Value = recipient.FullName,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.FirstName,
                Value = recipient.FirstName,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.Title,
                Value = recipient.Title,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.Signature,
                Value = recipient.Signature,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.Zip,
                Value = recipient.Zip,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.State,
                Value = recipient.State,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.City,
                Value = recipient.City,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.AddressLine1,
                Value = recipient.AddressLine1,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.AddressLine2,
                Value = recipient.AddressLine2,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.ReturnZip,
                Value = recipient.ReturnZip,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.ReturnState,
                Value = recipient.ReturnState,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.ReturnCity,
                Value = recipient.ReturnCity,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.ReturnAddressLine1,
                Value = recipient.ReturnAddressLine1,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.ReturnAddressLine2,
                Value = recipient.ReturnAddressLine2,
                Type = VariableType.InString,
            },
            new Variable
            {
                Name = RecipientVariableNamesHelper.FsCode,
                Value = recipient.FsCode,
                Type = VariableType.InString,
            },
        };

        var qrCodeVariables = new List<Variable>
        {
            new Variable
            {
                Name = RecipientVariableNamesHelper.QrCodeUrl,
                Value = recipient.QRCodeUrl,
                Type = VariableType.Barcode,
                BarcodeFormat = VariableBarcodeFormat.QR_CODE,
                BarcodeSubType = VariableBarcodeSubType.Url,
            },
        };

        var variables = imageVariables.Concat(textVariables).Concat(qrCodeVariables);
        return variables;
    }

    public DesignAtoms.Model.DataSet BuildVdpDataSet(
        int[] surfaceIndexes,
        Dictionary<Guid, List<Variable>> variables
    )
    {
        var dataSet = new DesignAtoms.Model.DataSet()
        {
            SurfacesData = new SurfaceDataCollection()
            {
                new SurfaceData()
                {
                    SurfaceBinding = new SurfaceBinding() { SurfaceIndexes = surfaceIndexes },
                    IterateOverSurfacesFirst = false,
                    Data = new List<ItemsData>(
                        variables.Select(v => new ItemsData
                        {
                            DataItems = ConstructDataItems(v.Value),
                            Placeholders = ConstructPlaceholders(v.Value),
                        })
                    ),
                },
            },
        };

        return dataSet;
    }

    public VariableValidationResult ValidateVariables(List<Variable> variables)
    {
        var missingRecipientVariableNames = new List<string>();
        var missingSourceVariableNames = new List<string>();

        var recipientVariableNames = RecipientVariableNamesHelper.GetNames();

        foreach (var variableName in recipientVariableNames)
        {
            if (
                !variables.Any(v =>
                    string.Equals(v.Name, variableName, StringComparison.OrdinalIgnoreCase)
                )
            )
                missingRecipientVariableNames.Add(variableName);
        }

        foreach (var variable in variables)
        {
            if (
                !recipientVariableNames.Any(name =>
                    string.Equals(variable.Name, name, StringComparison.OrdinalIgnoreCase)
                )
            )
                missingSourceVariableNames.Add(variable.Name);
        }

        return new VariableValidationResult
        {
            MissingRecipientVariableNames = missingRecipientVariableNames,
            MissingSourceVariableNames = missingSourceVariableNames,
        };
    }

    public IEnumerable<VariableInfo> GetAvailableVariables()
    {
        var imageVariables = GetImageVariables();
        var textVariables = GetTextVariables();
        var qrCodeVariables = GetQrCodeVariables();

        var variables = imageVariables.Concat(textVariables).Concat(qrCodeVariables);

        return variables;
    }

    private Dictionary<string, string> ConstructPlaceholders(List<Variable> variables)
    {
        var placeholders = new Dictionary<string, string>();
        foreach (var variable in variables)
        {
            switch (variable.Type)
            {
                case VariableType.InString:
                {
                    placeholders.Add(variable.Name, variable.Value);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        return placeholders;
    }

    private Dictionary<string, JToken> ConstructDataItems(List<Variable> variables)
    {
        var dataSet = new Dictionary<string, JToken>();

        foreach (var variable in variables)
        {
            var encodedVariableValue = HttpUtility.JavaScriptStringEncode(variable.Value);
            switch (variable.Type)
            {
                case VariableType.Text:
                {
                    dataSet.Add(
                        variable.Name,
                        JToken.Parse($"{{ \"text\": \"{encodedVariableValue}\" }}")
                    );
                    break;
                }
                case VariableType.PrivateImage:
                {
                    dataSet.Add(
                        variable.Name,
                        JToken.Parse($"{{ \"image\": \"user:{encodedVariableValue}\" }}")
                    );
                    break;
                }
                case VariableType.PublicImage:
                {
                    dataSet.Add(
                        variable.Name,
                        JToken.Parse($"{{ \"image\": \"public:{encodedVariableValue}\" }}")
                    );
                    break;
                }
                case VariableType.ExternalImage:
                {
                    dataSet.Add(
                        variable.Name,
                        JToken.Parse($"{{ \"image\": \"{encodedVariableValue}\" }}")
                    );
                    break;
                }
                case VariableType.PrivatePlaceholder:
                {
                    dataSet.Add(
                        variable.Name,
                        JToken.Parse($"{{ \"image\": \"user:{encodedVariableValue}\" }}")
                    );
                    break;
                }
                case VariableType.PublicPlaceholder:
                {
                    dataSet.Add(
                        variable.Name,
                        JToken.Parse($"{{ \"image\": \"public:{encodedVariableValue}\" }}")
                    );
                    break;
                }
                case VariableType.ExternalPlaceholder:
                {
                    dataSet.Add(
                        variable.Name,
                        JToken.Parse($"{{ \"image\": \"{encodedVariableValue}\" }}")
                    );
                    break;
                }
                case VariableType.Barcode:
                {
                    dataSet.Add(
                        variable.Name,
                        JToken.Parse(
                            $"{{ \"barcodeData\": {{ \"barcodeValue\" : \"{encodedVariableValue}\" ,"
                                + $" \"barcodeFormat\" : \"{variable.BarcodeFormat.Value}\",  \"barcodeSubType\" : \"{variable.BarcodeSubType.Value}\" }} }}"
                        )
                    );
                    break;
                }
                case VariableType.BarcodePlaceholder:
                {
                    dataSet.Add(
                        variable.Name,
                        JToken.Parse(
                            $"{{ \"barcodeData\": {{ \"barcodeValue\" : \"{encodedVariableValue}\" ,"
                                + $" \"barcodeFormat\" : \"{variable.BarcodeFormat.Value}\",  \"barcodeSubType\" : \"{variable.BarcodeSubType.Value}\" }} }}"
                        )
                    );
                    break;
                }
                case VariableType.InString:
                {
                    break;
                }
                default:
                {
                    throw new NotImplementedException(
                        $"Unsupported variable type. variable={StringHelper.Serialize(variable)}"
                    );
                }
            }
        }

        return dataSet;
    }

    private List<VariableInfo> GetImageVariables()
    {
        var imageVariables = new List<VariableInfo>
        {
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.Image,
                Type = VariableType.PrivateImage,
            },
        };

        return imageVariables;
    }

    private List<VariableInfo> GetQrCodeVariables()
    {
        var qrCodes = new List<VariableInfo>()
        {
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.QrCodeUrl,
                Type = VariableType.Barcode,
                BarcodeFormat = VariableBarcodeFormat.QR_CODE,
                BarcodeSubType = VariableBarcodeSubType.Url,
            },
        };

        return qrCodes;
    }

    private List<VariableInfo> GetTextVariables()
    {
        var textVariables = new List<VariableInfo>
        {
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.FullName,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.FirstName,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.Title,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.Signature,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.Zip,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.State,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.City,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.AddressLine1,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.AddressLine2,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.ReturnZip,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.ReturnState,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.ReturnCity,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.ReturnAddressLine1,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.ReturnAddressLine2,
                Type = VariableType.InString,
            },
            new VariableInfo
            {
                Name = RecipientVariableNamesHelper.FsCode,
                Type = VariableType.InString,
            },
        };

        return textVariables;
    }

    private VariableType ToVariableType(VdpImageType source) =>
        source switch
        {
            VdpImageType.Private => VariableType.PrivateImage,
            VdpImageType.Public => VariableType.PublicImage,
            VdpImageType.ExternalUrl => VariableType.ExternalImage,
            VdpImageType.PrivatePlaceholder => VariableType.PrivatePlaceholder,
            VdpImageType.PublicPlaceholder => VariableType.PublicPlaceholder,
            VdpImageType.ExternalUrlPlaceholder => VariableType.ExternalPlaceholder,
            _ => throw new NotImplementedException(
                $"Unsupported vdp image type. vdpImageType={source}"
            ),
        };
}
