using System.Text.Json;
using BlazerEditor.Models;

namespace BlazerEditor.Services;

/// <summary>
/// Service for managing email designs
/// </summary>
public class DesignService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public string SerializeDesign(EmailDesign design)
    {
        return JsonSerializer.Serialize(design, _jsonOptions);
    }

    public EmailDesign? DeserializeDesign(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<EmailDesign>(json, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public EmailDesign CreateEmptyDesign()
    {
        return new EmailDesign
        {
            Body = new EmailBody
            {
                Rows = new List<Row>()
            }
        };
    }

    public void IncrementCounter(EmailDesign design, string counterName)
    {
        if (design.Counters.ContainsKey(counterName))
        {
            design.Counters[counterName]++;
        }
        else
        {
            design.Counters[counterName] = 1;
        }
    }
}
