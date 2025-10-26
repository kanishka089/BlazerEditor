using System.Text;
using BlazerEditor.Models;

namespace BlazerEditor.Services;

/// <summary>
/// Service for exporting email designs to HTML
/// </summary>
public class HtmlExportService
{
    public string ExportToHtml(EmailDesign design)
    {
        var html = new StringBuilder();
        
        // Start HTML document
        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">");
        html.AppendLine("<head>");
        html.AppendLine("  <meta charset=\"UTF-8\">");
        html.AppendLine("  <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
        html.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        html.AppendLine("  <title></title>");
        html.AppendLine("  <style type=\"text/css\">");
        html.AppendLine(GetEmailStyles(design));
        html.AppendLine("  </style>");
        html.AppendLine("  <!--[if mso]>");
        html.AppendLine("  <noscript>");
        html.AppendLine("  <xml>");
        html.AppendLine("    <o:OfficeDocumentSettings>");
        html.AppendLine("      <o:PixelsPerInch>96</o:PixelsPerInch>");
        html.AppendLine("    </o:OfficeDocumentSettings>");
        html.AppendLine("  </xml>");
        html.AppendLine("  </noscript>");
        html.AppendLine("  <![endif]-->");
        html.AppendLine("</head>");
        
        // Body
        html.AppendLine($"<body style=\"margin: 0; padding: 0; background-color: {design.Body.Values.BackgroundColor};\">");
        
        // Preheader
        if (!string.IsNullOrEmpty(design.Body.Values.PreheaderText))
        {
            html.AppendLine($"  <div style=\"display: none; max-height: 0; overflow: hidden;\">{design.Body.Values.PreheaderText}</div>");
        }
        
        // Main table
        html.AppendLine("  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
        html.AppendLine("    <tr>");
        html.AppendLine("      <td align=\"center\" style=\"padding: 0;\">");
        html.AppendLine($"        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"{design.Body.Values.ContentWidth}\" style=\"max-width: {design.Body.Values.ContentWidth};\">");
        
        // Rows
        foreach (var row in design.Body.Rows)
        {
            html.AppendLine(RenderRow(row));
        }
        
        html.AppendLine("        </table>");
        html.AppendLine("      </td>");
        html.AppendLine("    </tr>");
        html.AppendLine("  </table>");
        html.AppendLine("</body>");
        html.AppendLine("</html>");
        
        return html.ToString();
    }

    private string GetEmailStyles(EmailDesign design)
    {
        return @"
        body { margin: 0; padding: 0; }
        table { border-collapse: collapse; }
        img { border: 0; display: block; outline: none; text-decoration: none; }
        p { margin: 0; padding: 0; }
        @media only screen and (max-width: 600px) {
            .mobile-hide { display: none !important; }
            .mobile-center { text-align: center !important; }
            .mobile-full-width { width: 100% !important; max-width: 100% !important; }
        }
        ";
    }

    private string RenderRow(Row row)
    {
        var html = new StringBuilder();
        var bgColor = !string.IsNullOrEmpty(row.Values.BackgroundColor) 
            ? $"background-color: {row.Values.BackgroundColor};" 
            : "";
        
        html.AppendLine($"          <tr class=\"row\" style=\"{bgColor}\">");
        html.AppendLine($"            <td style=\"padding: {row.Values.Padding};\">");
        
        if (row.Columns.Count > 1)
        {
            // Multi-column layout
            html.AppendLine("              <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
            html.AppendLine("                <tr>");
            
            foreach (var column in row.Columns)
            {
                var columnWidth = 100.0 / row.Columns.Count;
                html.AppendLine($"                  <td width=\"{columnWidth:F0}%\" valign=\"top\">");
                html.AppendLine(RenderColumn(column));
                html.AppendLine("                  </td>");
            }
            
            html.AppendLine("                </tr>");
            html.AppendLine("              </table>");
        }
        else if (row.Columns.Count == 1)
        {
            // Single column
            html.AppendLine(RenderColumn(row.Columns[0]));
        }
        
        html.AppendLine("            </td>");
        html.AppendLine("          </tr>");
        
        return html.ToString();
    }

    private string RenderColumn(Column column)
    {
        var html = new StringBuilder();
        var bgColor = !string.IsNullOrEmpty(column.Values.BackgroundColor) 
            ? $"background-color: {column.Values.BackgroundColor};" 
            : "";
        
        html.AppendLine($"                    <div style=\"padding: {column.Values.Padding}; {bgColor}\">");
        
        foreach (var content in column.Contents)
        {
            html.AppendLine(RenderContent(content));
        }
        
        html.AppendLine("                    </div>");
        
        return html.ToString();
    }

    private string RenderContent(Content content)
    {
        return content.Type switch
        {
            "text" => RenderText(content),
            "image" => RenderImage(content),
            "button" => RenderButton(content),
            "divider" => RenderDivider(content),
            "column-divider" => RenderColumnDivider(content),
            "heading" => RenderHeading(content),
            "layout" => RenderNestedLayout(content),
            _ => ""
        };
    }

    private string RenderText(Content content)
    {
        var style = $"font-size: {content.Values.FontSize}; " +
                   $"color: {content.Values.Color}; " +
                   $"text-align: {content.Values.TextAlign}; " +
                   $"line-height: {content.Values.LineHeight}; " +
                   $"padding: {content.Values.ContainerPadding};";
        
        return $"<div style=\"{style}\">{content.Values.Text}</div>";
    }

    private string RenderImage(Content content)
    {
        if (content.Values.Src == null) return "";
        
        var style = $"padding: {content.Values.ContainerPadding}; text-align: {content.Values.TextAlign};";
        var imgStyle = $"max-width: {content.Values.Src.MaxWidth}; height: auto;";
        
        var img = $"<img src=\"{content.Values.Src.Url}\" alt=\"{content.Values.AltText}\" style=\"{imgStyle}\" />";
        
        if (content.Values.Href != null && !string.IsNullOrEmpty(content.Values.Href.Values.Href))
        {
            img = $"<a href=\"{content.Values.Href.Values.Href}\" target=\"{content.Values.Href.Values.Target}\">{img}</a>";
        }
        
        return $"<div style=\"{style}\">{img}</div>";
    }

    private string RenderButton(Content content)
    {
        var colors = content.Values.ButtonColors ?? new ButtonColors();
        var href = content.Values.Href?.Values.Href ?? "#";
        var target = content.Values.Href?.Values.Target ?? "_blank";
        
        var style = $"padding: {content.Values.ContainerPadding}; text-align: {content.Values.TextAlign};";
        var buttonStyle = $"display: inline-block; " +
                         $"padding: 12px 24px; " +
                         $"background-color: {colors.BackgroundColor}; " +
                         $"color: {colors.Color}; " +
                         $"text-decoration: none; " +
                         $"border-radius: 4px; " +
                         $"font-size: {content.Values.FontSize};";
        
        return $"<div style=\"{style}\"><a href=\"{href}\" target=\"{target}\" style=\"{buttonStyle}\">{content.Values.Text}</a></div>";
    }

    private string RenderDivider(Content content)
    {
        var style = $"padding: {content.Values.ContainerPadding};";
        var thickness = content.Values.FontSize ?? "1px";
        
        if (content.Values.TextAlign == "vertical")
        {
            return $"<div style=\"{style} text-align: center;\"><div style=\"border-left: {thickness} solid {content.Values.Color}; height: 100px; width: 0; display: inline-block;\"></div></div>";
        }
        else
        {
            return $"<div style=\"{style}\"><hr style=\"border: 0; border-top: {thickness} solid {content.Values.Color}; margin: 0;\" /></div>";
        }
    }

    private string RenderHeading(Content content)
    {
        var style = $"font-size: {content.Values.FontSize}; " +
                   $"color: {content.Values.Color}; " +
                   $"text-align: {content.Values.TextAlign}; " +
                   $"line-height: {content.Values.LineHeight}; " +
                   $"padding: {content.Values.ContainerPadding}; " +
                   $"margin: 0;";
        
        return $"<h2 style=\"{style}\">{content.Values.Text}</h2>";
    }

    private string RenderColumnDivider(Content content)
    {
        var style = $"padding: {content.Values.ContainerPadding}; text-align: center;";
        var thickness = content.Values.FontSize ?? "2px";
        var height = content.Values.LineHeight ?? "150px";
        
        return $"<div style=\"{style}\"><div style=\"border-left: {thickness} solid {content.Values.Color}; height: {height}; width: 0; display: inline-block;\"></div></div>";
    }

    private string RenderNestedLayout(Content content)
    {
        if (content.Columns == null || !content.Columns.Any())
        {
            return "";
        }

        var html = new StringBuilder();
        var style = $"padding: {content.Values.ContainerPadding};";
        
        html.AppendLine($"<div style=\"{style}\">");
        html.AppendLine("  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
        html.AppendLine("    <tr>");
        
        foreach (var column in content.Columns)
        {
            var columnWidth = 100.0 / content.Columns.Count;
            html.AppendLine($"      <td width=\"{columnWidth:F0}%\" valign=\"top\">");
            
            foreach (var nestedContent in column.Contents)
            {
                html.AppendLine(RenderContent(nestedContent));
            }
            
            html.AppendLine("      </td>");
        }
        
        html.AppendLine("    </tr>");
        html.AppendLine("  </table>");
        html.AppendLine("</div>");
        
        return html.ToString();
    }
}
