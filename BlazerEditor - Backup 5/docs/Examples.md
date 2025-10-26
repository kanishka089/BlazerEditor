# Examples

## Basic Email Editor

Simple email editor with export functionality.

```razor
@page "/editor"
@inject IJSRuntime JS

<EmailEditor @ref="editor" Options="options" />

<button @onclick="Export">Export HTML</button>

@code {
    private EmailEditor? editor;
    private EditorOptions options = new() 
    { 
        MinHeight = 600 
    };

    private async Task Export()
    {
        var result = await editor!.ExportHtmlAsync();
        await JS.InvokeVoidAsync("console.log", result.Html);
    }
}
```

## Save and Load from Database

```razor
@page "/email-templates"
@inject IEmailTemplateService TemplateService

<EmailEditor @ref="editor" Options="options" />

<div class="actions">
    <button @onclick="Save">Save Template</button>
    <button @onclick="Load">Load Template</button>
</div>

@code {
    private EmailEditor? editor;
    private EditorOptions options = new();
    private int templateId = 1;

    private async Task Save()
    {
        var design = await editor!.SaveDesignAsync();
        var json = JsonSerializer.Serialize(design);
        
        await TemplateService.SaveTemplateAsync(new EmailTemplate
        {
            Id = templateId,
            Name = "My Template",
            DesignJson = json,
            UpdatedAt = DateTime.UtcNow
        });
    }

    private async Task Load()
    {
        var template = await TemplateService.GetTemplateAsync(templateId);
        if (template != null)
        {
            await editor!.LoadDesignAsync(template.DesignJson);
        }
    }
}
```

## Custom Theme

```razor
@page "/custom-theme"

<EmailEditor @ref="editor" Options="options" />

@code {
    private EmailEditor? editor;
    private EditorOptions options = new()
    {
        Appearance = new AppearanceConfig
        {
            Theme = "modern_dark"
        },
        MinHeight = 700
    };
}
```

## With Merge Tags

```razor
@page "/personalized-emails"

<EmailEditor @ref="editor" Options="options" />

@code {
    private EmailEditor? editor;
    private EditorOptions options = new()
    {
        MergeTags = new List<MergeTag>
        {
            new() { Name = "First Name", Value = "{{firstName}}", Sample = "John" },
            new() { Name = "Last Name", Value = "{{lastName}}", Sample = "Doe" },
            new() { Name = "Email", Value = "{{email}}", Sample = "john@example.com" },
            new() { Name = "Company", Value = "{{company}}", Sample = "Acme Inc" }
        }
    };
}
```

## Newsletter Builder

Complete newsletter builder with template management.

```razor
@page "/newsletter-builder"
@inject INewsletterService NewsletterService
@inject NavigationManager Navigation

<div class="newsletter-builder">
    <div class="header">
        <h1>Newsletter Builder</h1>
        <div class="actions">
            <button @onclick="SaveDraft">Save Draft</button>
            <button @onclick="Preview">Preview</button>
            <button @onclick="Send">Send Newsletter</button>
        </div>
    </div>

    <EmailEditor @ref="editor" 
                 Options="options"
                 OnReady="OnEditorReady" />
</div>

@code {
    private EmailEditor? editor;
    private EditorOptions options = new()
    {
        Appearance = new AppearanceConfig { Theme = "modern_light" },
        MinHeight = 800,
        Features = new FeaturesConfig
        {
            TextEditor = true,
            ImageEditor = true,
            UndoRedo = true,
            Preview = true
        }
    };

    private void OnEditorReady()
    {
        // Load existing newsletter if editing
        if (!string.IsNullOrEmpty(newsletterId))
        {
            LoadNewsletter();
        }
    }

    private async Task SaveDraft()
    {
        var design = await editor!.SaveDesignAsync();
        var json = JsonSerializer.Serialize(design);
        
        await NewsletterService.SaveDraftAsync(new Newsletter
        {
            DesignJson = json,
            Status = NewsletterStatus.Draft,
            UpdatedAt = DateTime.UtcNow
        });
    }

    private async Task Preview()
    {
        var result = await editor!.ExportHtmlAsync();
        // Open preview in new window
        await JS.InvokeVoidAsync("open", $"/preview?html={Uri.EscapeDataString(result.Html)}");
    }

    private async Task Send()
    {
        var result = await editor!.ExportHtmlAsync();
        await NewsletterService.SendNewsletterAsync(result.Html);
        Navigation.NavigateTo("/newsletters");
    }
}
```

## Transactional Email Templates

```razor
@page "/email-templates/{TemplateType}"
@inject ITemplateService TemplateService

<div class="template-editor">
    <h2>@TemplateType Template</h2>
    
    <EmailEditor @ref="editor" Options="options" />
    
    <div class="template-actions">
        <button @onclick="SaveTemplate">Save Template</button>
        <button @onclick="TestEmail">Send Test Email</button>
    </div>
</div>

@code {
    [Parameter] public string TemplateType { get; set; } = "";
    
    private EmailEditor? editor;
    private EditorOptions options = new()
    {
        MergeTags = new List<MergeTag>
        {
            new() { Name = "User Name", Value = "{{userName}}" },
            new() { Name = "Action URL", Value = "{{actionUrl}}" },
            new() { Name = "Support Email", Value = "{{supportEmail}}" }
        }
    };

    protected override async Task OnInitializedAsync()
    {
        // Load existing template
        var template = await TemplateService.GetTemplateByTypeAsync(TemplateType);
        if (template != null && editor != null)
        {
            await editor.LoadDesignAsync(template.DesignJson);
        }
    }

    private async Task SaveTemplate()
    {
        var design = await editor!.SaveDesignAsync();
        var result = await editor.ExportHtmlAsync();
        
        await TemplateService.SaveTemplateAsync(new EmailTemplate
        {
            Type = TemplateType,
            DesignJson = JsonSerializer.Serialize(design),
            HtmlContent = result.Html,
            UpdatedAt = DateTime.UtcNow
        });
    }

    private async Task TestEmail()
    {
        var result = await editor!.ExportHtmlAsync();
        await TemplateService.SendTestEmailAsync(result.Html, "test@example.com");
    }
}
```

## Multi-Language Templates

```razor
@page "/multilang-templates"

<div class="language-selector">
    <select @bind="selectedLanguage" @bind:after="LoadTemplate">
        <option value="en">English</option>
        <option value="es">Spanish</option>
        <option value="fr">French</option>
        <option value="de">German</option>
    </select>
</div>

<EmailEditor @ref="editor" Options="options" />

@code {
    private EmailEditor? editor;
    private string selectedLanguage = "en";
    private Dictionary<string, string> templates = new();
    
    private EditorOptions options = new()
    {
        Locale = "en-US"
    };

    private async Task LoadTemplate()
    {
        // Save current before switching
        if (editor != null)
        {
            var current = await editor.SaveDesignAsync();
            templates[selectedLanguage] = JsonSerializer.Serialize(current);
        }

        // Load new language template
        if (templates.ContainsKey(selectedLanguage))
        {
            await editor!.LoadDesignAsync(templates[selectedLanguage]);
        }
        
        // Update locale
        options.Locale = selectedLanguage switch
        {
            "es" => "es-ES",
            "fr" => "fr-FR",
            "de" => "de-DE",
            _ => "en-US"
        };
    }
}
```

## Integration with File Upload

```razor
@page "/editor-with-upload"
@inject IFileUploadService FileService

<EmailEditor @ref="editor" Options="options" />

<InputFile OnChange="HandleFileUpload" accept="image/*" />

@code {
    private EmailEditor? editor;
    private EditorOptions options = new();

    private async Task HandleFileUpload(InputFileChangeEventArgs e)
    {
        var file = e.File;
        var imageUrl = await FileService.UploadImageAsync(file);
        
        // You can programmatically add image to design
        Console.WriteLine($"Image uploaded: {imageUrl}");
    }
}
```
