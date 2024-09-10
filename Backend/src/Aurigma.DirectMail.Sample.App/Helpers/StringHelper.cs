using Newtonsoft.Json;

public class StringHelper
{
    /// <summary>
    /// Serializes an object of the specified type into a string.
    /// </summary>
    /// <param name="source">Source object.</param>
    /// <typeparam name="T">Object type.</typeparam>
    /// <returns>
    /// Serialized string value if the <paramref name="source"/> is not <c>null</c>;
    /// <c><see cref="string.Empty"/></c> otherwise.
    /// </returns>
    public static string Serialize<T>(T source)
        where T : class
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.NullValueHandling = NullValueHandling.Ignore;
        return source != null ? JsonConvert.SerializeObject(source, settings) : string.Empty;
    }

    /// <summary>
    /// Deserializes a string into an object of the specified type.
    /// </summary>
    /// <param name="source">String in JSON format.</param>
    /// <typeparam name="T">Object type.</typeparam>
    /// <returns>Deserialized object of the specified type.</returns>
    /// <returns>
    /// Deserialized object if the <paramref name="source"/> is not <c>null</c> or <c><see cref="string.Empty"/></c>;
    /// <c>null</c> otherwise.
    /// </returns>
    public static T Deserialize<T>(string source)
        where T : class
    {
        return !string.IsNullOrEmpty(source) ? JsonConvert.DeserializeObject<T>(source) : null;
    }
}
