using System.Text.Json;

namespace CommerceConsole.Infrastructure.Persistence;

/// <summary>
/// Provides JSON file list persistence for repository data.
/// </summary>
public sealed class JsonFileStore
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// Initializes a JSON file store rooted at the provided directory.
    /// </summary>
    public JsonFileStore(string? dataDirectory = null)
    {
        DataDirectory = dataDirectory ?? Path.Combine(Directory.GetCurrentDirectory(), "data");
        Directory.CreateDirectory(DataDirectory);
    }

    /// <summary>
    /// Gets the directory where persistence files are stored.
    /// </summary>
    public string DataDirectory { get; }

    /// <summary>
    /// Loads a list from a JSON file if it exists.
    /// </summary>
    public List<T> LoadList<T>(string fileName)
    {
        string path = GetFilePath(fileName);
        if (!File.Exists(path))
        {
            return new List<T>();
        }

        try
        {
            using FileStream stream = File.OpenRead(path);
            return JsonSerializer.Deserialize<List<T>>(stream, _serializerOptions) ?? new List<T>();
        }
        catch (JsonException)
        {
            // Recover to empty data if file content is malformed.
            return new List<T>();
        }
    }

    /// <summary>
    /// Saves a list to a JSON file.
    /// </summary>
    public void SaveList<T>(string fileName, IEnumerable<T> items)
    {
        string path = GetFilePath(fileName);
        string tempPath = path + "." + Guid.NewGuid().ToString("N") + ".tmp";

        string json = JsonSerializer.Serialize(items, _serializerOptions);
        File.WriteAllText(tempPath, json);
        File.Move(tempPath, path, true);
    }

    private string GetFilePath(string fileName)
    {
        return Path.Combine(DataDirectory, fileName);
    }
}
