# Unlayer Merge Tags Analysis

## 🎯 Overview

Merge tags (also called personalization tags or variables) in Unlayer allow users to insert dynamic placeholders into email templates that get replaced with actual data when the email is sent.

---

## 📋 How Unlayer Merge Tags Work

### 1. **Configuration**

Merge tags are configured when initializing the editor:

```javascript
const editor = unlayer.createEditor({
  mergeTags: {
    first_name: {
      name: "First Name",
      value: "{{first_name}}",
      sample: "John"
    },
    last_name: {
      name: "Last Name", 
      value: "{{last_name}}",
      sample: "Doe"
    },
    email: {
      name: "Email Address",
      value: "{{email}}",
      sample: "john.doe@example.com"
    },
    company: {
      name: "Company Name",
      value: "{{company}}",
      sample: "Acme Corp"
    },
    unsubscribe_url: {
      name: "Unsubscribe Link",
      value: "{{unsubscribe_url}}",
      sample: "https://example.com/unsubscribe"
    }
  }
});
```

### 2. **Structure**

Each merge tag has three properties:

- **name**: Display name shown in the UI
- **value**: The actual placeholder text (e.g., `{{first_name}}`)
- **sample**: Example value shown in preview mode

### 3. **Usage in Editor**

Users can insert merge tags in:
- **Text content**: "Hello {{first_name}}!"
- **Button text**: "View {{product_name}}"
- **Links**: href="{{unsubscribe_url}}"
- **Image alt text**: alt="{{product_name}}"
- **Subject lines** (if supported)

### 4. **UI Integration**

Unlayer provides a merge tag picker:
- Dropdown menu in text editor toolbar
- Search/filter functionality
- Click to insert at cursor position
- Shows sample values in preview

### 5. **Preview Mode**

When previewing the email:
- Merge tags are replaced with sample values
- `{{first_name}}` → "John"
- `{{email}}` → "john.doe@example.com"
- Helps visualize final output

### 6. **Export Behavior**

When exporting HTML:
- Merge tags remain as placeholders
- `{{first_name}}` stays as `{{first_name}}`
- Your email service provider replaces them
- Compatible with most ESPs (Mailchimp, SendGrid, etc.)

---

## 🎨 Merge Tag Categories

### Personal Information
```javascript
{
  first_name: { name: "First Name", value: "{{first_name}}", sample: "John" },
  last_name: { name: "Last Name", value: "{{last_name}}", sample: "Doe" },
  full_name: { name: "Full Name", value: "{{full_name}}", sample: "John Doe" },
  email: { name: "Email", value: "{{email}}", sample: "john@example.com" }
}
```

### Company Information
```javascript
{
  company: { name: "Company", value: "{{company}}", sample: "Acme Corp" },
  job_title: { name: "Job Title", value: "{{job_title}}", sample: "Manager" },
  department: { name: "Department", value: "{{department}}", sample: "Sales" }
}
```

### Links & Actions
```javascript
{
  unsubscribe_url: { 
    name: "Unsubscribe Link", 
    value: "{{unsubscribe_url}}", 
    sample: "https://example.com/unsubscribe" 
  },
  view_online_url: { 
    name: "View Online", 
    value: "{{view_online_url}}", 
    sample: "https://example.com/view" 
  },
  preferences_url: { 
    name: "Preferences", 
    value: "{{preferences_url}}", 
    sample: "https://example.com/preferences" 
  }
}
```

### Custom Fields
```javascript
{
  custom_field_1: { 
    name: "Custom Field 1", 
    value: "{{custom_field_1}}", 
    sample: "Custom Value" 
  },
  order_number: { 
    name: "Order Number", 
    value: "{{order_number}}", 
    sample: "#12345" 
  },
  tracking_number: { 
    name: "Tracking Number", 
    value: "{{tracking_number}}", 
    sample: "1Z999AA10123456784" 
  }
}
```

---

## 🔧 Implementation Details

### 1. **Syntax Formats**

Unlayer supports multiple syntax formats:

**Handlebars (Default)**
```
{{first_name}}
{{last_name}}
```

**Mustache**
```
{{first_name}}
{{last_name}}
```

**Liquid (Shopify)**
```
{{ first_name }}
{{ last_name }}
```

**Custom**
```
[first_name]
%first_name%
{first_name}
```

### 2. **Advanced Features**

**Conditional Logic**
```handlebars
{{#if first_name}}
  Hello {{first_name}}!
{{else}}
  Hello there!
{{/if}}
```

**Loops**
```handlebars
{{#each products}}
  <li>{{name}} - ${{price}}</li>
{{/each}}
```

**Fallback Values**
```handlebars
{{first_name|default:"Friend"}}
```

### 3. **Special Merge Tags**

**Date/Time**
```javascript
{
  current_date: { 
    name: "Current Date", 
    value: "{{current_date}}", 
    sample: "January 15, 2024" 
  },
  current_year: { 
    name: "Current Year", 
    value: "{{current_year}}", 
    sample: "2024" 
  }
}
```

**System Tags**
```javascript
{
  sender_name: { 
    name: "Sender Name", 
    value: "{{sender_name}}", 
    sample: "Support Team" 
  },
  sender_email: { 
    name: "Sender Email", 
    value: "{{sender_email}}", 
    sample: "support@example.com" 
  }
}
```

---

## 🎯 User Experience

### 1. **Insertion Methods**

**Toolbar Button**
- Click merge tag icon in text editor
- Dropdown shows available tags
- Click to insert at cursor

**Keyboard Shortcut**
- Type `{{` to trigger autocomplete
- Shows matching tags
- Arrow keys to select, Enter to insert

**Drag & Drop**
- Drag tag from sidebar
- Drop into text area
- Automatically inserts

### 2. **Visual Indicators**

**In Editor**
- Merge tags highlighted with color
- Different color than regular text
- Tooltip shows tag name on hover
- Badge/chip style display

**In Preview**
- Shows sample values
- Can toggle between tags/samples
- Helps visualize final email

### 3. **Validation**

**Real-time Checks**
- Validates tag syntax
- Warns about unknown tags
- Suggests corrections
- Prevents broken tags

---

## 📊 Common Use Cases

### 1. **Welcome Email**
```html
<h1>Welcome, {{first_name}}!</h1>
<p>Thanks for joining {{company}}.</p>
<p>Your email is: {{email}}</p>
```

### 2. **Order Confirmation**
```html
<h1>Order #{{order_number}} Confirmed</h1>
<p>Hi {{first_name}},</p>
<p>Your order will ship to:</p>
<p>{{shipping_address}}</p>
<p>Track your order: {{tracking_url}}</p>
```

### 3. **Newsletter**
```html
<h1>{{newsletter_title}}</h1>
<p>Hi {{first_name}},</p>
<p>Here's what's new this {{current_month}}...</p>
<a href="{{unsubscribe_url}}">Unsubscribe</a>
```

### 4. **Abandoned Cart**
```html
<h1>{{first_name}}, you left items in your cart</h1>
<p>Complete your purchase of {{product_name}}</p>
<a href="{{cart_url}}">Return to Cart</a>
```

---

## 🔄 Integration with Email Service Providers

### Mailchimp
```javascript
mergeTags: {
  first_name: { value: "*|FNAME|*", sample: "John" },
  last_name: { value: "*|LNAME|*", sample: "Doe" },
  email: { value: "*|EMAIL|*", sample: "john@example.com" }
}
```

### SendGrid
```javascript
mergeTags: {
  first_name: { value: "{{first_name}}", sample: "John" },
  last_name: { value: "{{last_name}}", sample: "Doe" },
  email: { value: "{{email}}", sample: "john@example.com" }
}
```

### Campaign Monitor
```javascript
mergeTags: {
  first_name: { value: "[firstname]", sample: "John" },
  last_name: { value: "[lastname]", sample: "Doe" },
  email: { value: "[email]", sample: "john@example.com" }
}
```

### Custom ESP
```javascript
mergeTags: {
  first_name: { value: "%FIRST_NAME%", sample: "John" },
  last_name: { value: "%LAST_NAME%", sample: "Doe" },
  email: { value: "%EMAIL%", sample: "john@example.com" }
}
```

---

## 🎨 UI Components

### 1. **Merge Tag Picker**

**Dropdown Menu**
```
┌─────────────────────────┐
│ 🏷️ Insert Merge Tag     │
├─────────────────────────┤
│ 🔍 Search tags...       │
├─────────────────────────┤
│ 👤 Personal             │
│   • First Name          │
│   • Last Name           │
│   • Email               │
├─────────────────────────┤
│ 🏢 Company              │
│   • Company Name        │
│   • Job Title           │
├─────────────────────────┤
│ 🔗 Links                │
│   • Unsubscribe         │
│   • View Online         │
└─────────────────────────┘
```

### 2. **Tag Display**

**In Text Editor**
```
Hello [First Name]!
      ^^^^^^^^^^^^
      (highlighted chip)
```

**Preview Mode**
```
Hello John!
      ^^^^
      (sample value)
```

### 3. **Tag Manager**

**Settings Panel**
```
┌─────────────────────────────────┐
│ Merge Tags Configuration        │
├─────────────────────────────────┤
│ Tag Name: First Name            │
│ Value:    {{first_name}}        │
│ Sample:   John                  │
│ [Save] [Cancel]                 │
├─────────────────────────────────┤
│ + Add New Tag                   │
└─────────────────────────────────┘
```

---

## 🚀 Implementation for Blazer Editor

### Phase 1: Basic Merge Tags

1. **Data Model**
```csharp
public class MergeTag
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Sample { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
}
```

2. **Configuration**
```csharp
public class EditorOptions
{
    public List<MergeTag> MergeTags { get; set; } = new();
}
```

3. **UI Component**
- Dropdown in text editor toolbar
- Search/filter functionality
- Category grouping
- Click to insert

### Phase 2: Preview Mode

1. **Toggle Preview**
- Button to show/hide sample values
- Replace tags with samples
- Visual indicator of preview mode

2. **Sample Value Display**
- Highlight replaced values
- Tooltip shows original tag
- Toggle back to edit mode

### Phase 3: Advanced Features

1. **Custom Syntax**
- Support multiple formats
- Configurable delimiters
- Validation rules

2. **Tag Management**
- Add/edit/delete tags
- Import/export tag sets
- Tag templates

3. **ESP Integration**
- Preset tag formats
- ESP-specific validation
- Export compatibility

---

## 📚 Best Practices

### 1. **Naming Conventions**
- Use lowercase with underscores: `first_name`
- Be descriptive: `shipping_address` not `addr`
- Consistent prefixes: `user_`, `order_`, `product_`

### 2. **Sample Values**
- Use realistic examples
- Match expected data format
- Include edge cases in testing

### 3. **Documentation**
- Provide tag reference guide
- Include usage examples
- Document ESP compatibility

### 4. **Validation**
- Check tag syntax
- Warn about unknown tags
- Prevent duplicate tags
- Validate before export

### 5. **User Experience**
- Make tags easy to discover
- Provide search/filter
- Show tooltips
- Visual differentiation

---

## 🎯 Summary

Unlayer's merge tag system provides:

✅ **Flexible Configuration** - Define custom tags
✅ **Multiple Syntaxes** - Support various ESPs
✅ **Preview Mode** - See sample values
✅ **Easy Insertion** - Multiple input methods
✅ **Visual Feedback** - Highlighted tags
✅ **Validation** - Prevent errors
✅ **ESP Integration** - Compatible with major providers

**Key Takeaway**: Merge tags are simple placeholders with three properties (name, value, sample) that get replaced by email service providers when sending emails.

---

**Next Steps for Blazer Editor:**
1. Implement basic merge tag data model
2. Add merge tag picker UI component
3. Support tag insertion in text editor
4. Add preview mode with sample values
5. Implement ESP-specific formats
6. Add tag management interface

---

**Last Updated**: October 27, 2025
**Version**: 1.0.0
