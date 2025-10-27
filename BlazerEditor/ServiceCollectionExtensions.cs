using Microsoft.Extensions.DependencyInjection;
using BlazerEditor.Services;

namespace BlazerEditor;

/// <summary>
/// Extension methods for registering BlazerEditor services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds BlazerEditor services to the service collection
    /// </summary>
    public static IServiceCollection AddBlazerEditor(this IServiceCollection services)
    {
        services.AddScoped<HtmlExportService>();
        services.AddScoped<DesignService>();
        services.AddScoped<TemplateLibraryService>();
        services.AddScoped<MergeTagService>();
        
        return services;
    }
}
