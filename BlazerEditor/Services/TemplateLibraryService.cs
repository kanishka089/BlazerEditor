using BlazerEditor.Models;

namespace BlazerEditor.Services;

/// <summary>
/// Service providing pre-built email templates
/// </summary>
public class TemplateLibraryService
{
    public List<EmailTemplate> GetTemplates()
    {
        return new List<EmailTemplate>
        {
            GetWelcomeTemplate(),
            GetNewsletterTemplate(),
            GetPromotionalTemplate(),
            GetTransactionalTemplate()
        };
    }

    private EmailTemplate GetWelcomeTemplate()
    {
        var design = new EmailDesign
        {
            Body = new EmailBody
            {
                Values = new BodyValues
                {
                    BackgroundColor = "#f5f5f7",
                    ContentWidth = "600px"
                },
                Rows = new List<Row>
                {
                    new Row
                    {
                        Cells = new List<int> { 1 },
                        Values = new RowValues
                        {
                            BackgroundColor = "#007aff",
                            Padding = "60px 20px"
                        },
                        Columns = new List<Column>
                        {
                            new Column
                            {
                                Contents = new List<Content>
                                {
                                    new Content
                                    {
                                        Type = "heading",
                                        Values = new ContentValues
                                        {
                                            Text = "Welcome!",
                                            FontSize = "48px",
                                            Color = "#ffffff",
                                            TextAlign = "center"
                                        }
                                    },
                                    new Content
                                    {
                                        Type = "text",
                                        Values = new ContentValues
                                        {
                                            Text = "<p>We're excited to have you on board.</p>",
                                            FontSize = "18px",
                                            Color = "#ffffff",
                                            TextAlign = "center",
                                            ContainerPadding = "20px 10px"
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new Row
                    {
                        Cells = new List<int> { 1 },
                        Values = new RowValues
                        {
                            BackgroundColor = "#ffffff",
                            Padding = "40px 20px"
                        },
                        Columns = new List<Column>
                        {
                            new Column
                            {
                                Contents = new List<Content>
                                {
                                    new Content
                                    {
                                        Type = "text",
                                        Values = new ContentValues
                                        {
                                            Text = "<p>Thank you for joining our community. We're here to help you get started.</p>",
                                            FontSize = "16px",
                                            LineHeight = "150%"
                                        }
                                    },
                                    new Content
                                    {
                                        Type = "button",
                                        Values = new ContentValues
                                        {
                                            Text = "Get Started",
                                            TextAlign = "center",
                                            ContainerPadding = "30px 10px",
                                            Href = new LinkAction
                                            {
                                                Values = new LinkValues { Href = "#" }
                                            },
                                            ButtonColors = new ButtonColors
                                            {
                                                BackgroundColor = "#007aff",
                                                Color = "#ffffff"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        return new EmailTemplate
        {
            Id = "welcome",
            Name = "Welcome Email",
            Description = "Welcome new users to your platform",
            Category = "Transactional",
            Design = design
        };
    }

    private EmailTemplate GetNewsletterTemplate()
    {
        var design = new EmailDesign
        {
            Body = new EmailBody
            {
                Values = new BodyValues
                {
                    BackgroundColor = "#f9f9f9",
                    ContentWidth = "600px"
                },
                Rows = new List<Row>
                {
                    new Row
                    {
                        Cells = new List<int> { 1 },
                        Values = new RowValues
                        {
                            BackgroundColor = "#1d1d1f",
                            Padding = "30px 20px"
                        },
                        Columns = new List<Column>
                        {
                            new Column
                            {
                                Contents = new List<Content>
                                {
                                    new Content
                                    {
                                        Type = "heading",
                                        Values = new ContentValues
                                        {
                                            Text = "Monthly Newsletter",
                                            FontSize = "32px",
                                            Color = "#ffffff",
                                            TextAlign = "center"
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new Row
                    {
                        Cells = new List<int> { 1 },
                        Values = new RowValues
                        {
                            BackgroundColor = "#ffffff",
                            Padding = "40px 20px"
                        },
                        Columns = new List<Column>
                        {
                            new Column
                            {
                                Contents = new List<Content>
                                {
                                    new Content
                                    {
                                        Type = "heading",
                                        Values = new ContentValues
                                        {
                                            Text = "What's New This Month",
                                            FontSize = "24px",
                                            Color = "#1d1d1f"
                                        }
                                    },
                                    new Content
                                    {
                                        Type = "text",
                                        Values = new ContentValues
                                        {
                                            Text = "<p>Here are the latest updates and news from our team.</p>",
                                            FontSize = "16px",
                                            LineHeight = "150%"
                                        }
                                    },
                                    new Content
                                    {
                                        Type = "divider",
                                        Values = new ContentValues
                                        {
                                            Color = "#e0e0e0",
                                            ContainerPadding = "20px 0"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        return new EmailTemplate
        {
            Id = "newsletter",
            Name = "Newsletter",
            Description = "Monthly newsletter template",
            Category = "Marketing",
            Design = design
        };
    }

    private EmailTemplate GetPromotionalTemplate()
    {
        var design = new EmailDesign
        {
            Body = new EmailBody
            {
                Values = new BodyValues
                {
                    BackgroundColor = "#ffffff",
                    ContentWidth = "600px"
                },
                Rows = new List<Row>
                {
                    new Row
                    {
                        Cells = new List<int> { 1 },
                        Values = new RowValues
                        {
                            BackgroundColor = "#ff3b30",
                            Padding = "50px 20px"
                        },
                        Columns = new List<Column>
                        {
                            new Column
                            {
                                Contents = new List<Content>
                                {
                                    new Content
                                    {
                                        Type = "heading",
                                        Values = new ContentValues
                                        {
                                            Text = "Special Offer!",
                                            FontSize = "42px",
                                            Color = "#ffffff",
                                            TextAlign = "center"
                                        }
                                    },
                                    new Content
                                    {
                                        Type = "text",
                                        Values = new ContentValues
                                        {
                                            Text = "<p>Get 50% off on all products</p>",
                                            FontSize = "24px",
                                            Color = "#ffffff",
                                            TextAlign = "center"
                                        }
                                    },
                                    new Content
                                    {
                                        Type = "button",
                                        Values = new ContentValues
                                        {
                                            Text = "Shop Now",
                                            TextAlign = "center",
                                            ContainerPadding = "20px 10px",
                                            Href = new LinkAction
                                            {
                                                Values = new LinkValues { Href = "#" }
                                            },
                                            ButtonColors = new ButtonColors
                                            {
                                                BackgroundColor = "#ffffff",
                                                Color = "#ff3b30"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        return new EmailTemplate
        {
            Id = "promotional",
            Name = "Promotional",
            Description = "Promotional offer template",
            Category = "Marketing",
            Design = design
        };
    }

    private EmailTemplate GetTransactionalTemplate()
    {
        var design = new EmailDesign
        {
            Body = new EmailBody
            {
                Values = new BodyValues
                {
                    BackgroundColor = "#f5f5f7",
                    ContentWidth = "600px"
                },
                Rows = new List<Row>
                {
                    new Row
                    {
                        Cells = new List<int> { 1 },
                        Values = new RowValues
                        {
                            BackgroundColor = "#ffffff",
                            Padding = "40px 20px"
                        },
                        Columns = new List<Column>
                        {
                            new Column
                            {
                                Contents = new List<Content>
                                {
                                    new Content
                                    {
                                        Type = "heading",
                                        Values = new ContentValues
                                        {
                                            Text = "Order Confirmation",
                                            FontSize = "28px",
                                            Color = "#1d1d1f"
                                        }
                                    },
                                    new Content
                                    {
                                        Type = "text",
                                        Values = new ContentValues
                                        {
                                            Text = "<p>Thank you for your order. Your order has been confirmed.</p>",
                                            FontSize = "16px",
                                            LineHeight = "150%"
                                        }
                                    },
                                    new Content
                                    {
                                        Type = "button",
                                        Values = new ContentValues
                                        {
                                            Text = "View Order",
                                            TextAlign = "left",
                                            ContainerPadding = "20px 10px",
                                            Href = new LinkAction
                                            {
                                                Values = new LinkValues { Href = "#" }
                                            },
                                            ButtonColors = new ButtonColors
                                            {
                                                BackgroundColor = "#34c759",
                                                Color = "#ffffff"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        return new EmailTemplate
        {
            Id = "transactional",
            Name = "Order Confirmation",
            Description = "Transactional order confirmation",
            Category = "Transactional",
            Design = design
        };
    }
}

public class EmailTemplate
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public EmailDesign Design { get; set; } = new();
}
