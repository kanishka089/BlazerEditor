# Merge Tags Flow Diagram

## 🔄 Complete Flow: From Configuration to Email Delivery

```
┌─────────────────────────────────────────────────────────────────┐
│                    1. CONFIGURATION PHASE                        │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
                    ┌──────────────────┐
                    │  Developer Sets  │
                    │   Merge Tags     │
                    └──────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────┐
        │  mergeTags: {                           │
        │    first_name: {                        │
        │      name: "First Name",                │
        │      value: "{{first_name}}",           │
        │      sample: "John"                     │
        │    }                                    │
        │  }                                      │
        └─────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    2. EDITOR PHASE                               │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
                    ┌──────────────────┐
                    │  User Opens      │
                    │  Email Editor    │
                    └──────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────┐
        │         Editor UI Displays:             │
        │                                         │
        │  ┌───────────────────────────────┐     │
        │  │ 🏷️ Insert Merge Tag          │     │
        │  ├───────────────────────────────┤     │
        │  │ • First Name                  │     │
        │  │ • Last Name                   │     │
        │  │ • Email                       │     │
        │  └───────────────────────────────┘     │
        └─────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    3. INSERTION PHASE                            │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
                    ┌──────────────────┐
                    │  User Clicks     │
                    │  "First Name"    │
                    └──────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────┐
        │  Text Content:                          │
        │                                         │
        │  "Hello {{first_name}}!"                │
        │         ^^^^^^^^^^^^^^                  │
        │         (inserted tag)                  │
        └─────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    4. PREVIEW PHASE                              │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
                    ┌──────────────────┐
                    │  User Clicks     │
                    │  "Preview"       │
                    └──────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────┐
        │  Replace tags with samples:             │
        │                                         │
        │  "Hello {{first_name}}!"                │
        │           ↓                             │
        │  "Hello John!"                          │
        │         ^^^^                            │
        │         (sample value)                  │
        └─────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    5. EXPORT PHASE                               │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
                    ┌──────────────────┐
                    │  User Clicks     │
                    │  "Export HTML"   │
                    └──────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────┐
        │  HTML Output:                           │
        │                                         │
        │  <p>Hello {{first_name}}!</p>           │
        │           ^^^^^^^^^^^^^^                │
        │           (tag preserved)               │
        └─────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    6. STORAGE PHASE                              │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
                    ┌──────────────────┐
                    │  Save to         │
                    │  Database        │
                    └──────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────┐
        │  Stored HTML:                           │
        │                                         │
        │  {                                      │
        │    "html": "<p>Hello {{first_name}}!</p>│
        │    "design": { ... }                    │
        │  }                                      │
        └─────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    7. EMAIL SENDING PHASE                        │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
                    ┌──────────────────┐
                    │  Load Template   │
                    │  from Database   │
                    └──────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────┐
        │  Template HTML:                         │
        │  "<p>Hello {{first_name}}!</p>"         │
        └─────────────────────────────────────────┘
                              │
                              ▼
                    ┌──────────────────┐
                    │  Get Recipient   │
                    │  Data            │
                    └──────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────┐
        │  Recipient Data:                        │
        │                                         │
        │  {                                      │
        │    "first_name": "Alice",               │
        │    "last_name": "Smith",                │
        │    "email": "alice@example.com"         │
        │  }                                      │
        └─────────────────────────────────────────┘
                              │
                              ▼
                    ┌──────────────────┐
                    │  ESP Replaces    │
                    │  Merge Tags      │
                    └──────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────┐
        │  Final HTML:                            │
        │                                         │
        │  "Hello {{first_name}}!"                │
        │           ↓                             │
        │  "Hello Alice!"                         │
        │         ^^^^^                           │
        │         (actual data)                   │
        └─────────────────────────────────────────┘
                              │
                              ▼
                    ┌──────────────────┐
                    │  Send Email to   │
                    │  alice@example.com│
                    └──────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    8. RECIPIENT RECEIVES                         │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────┐
        │  Email Inbox:                           │
        │                                         │
        │  ┌───────────────────────────────┐     │
        │  │ From: support@example.com     │     │
        │  │ Subject: Welcome!             │     │
        │  ├───────────────────────────────┤     │
        │  │                               │     │
        │  │ Hello Alice!                  │     │
        │  │                               │     │
        │  │ Welcome to our service...     │     │
        │  │                               │     │
        │  └───────────────────────────────┘     │
        └─────────────────────────────────────────┘
```

---

## 🔍 Detailed Component Interactions

### Editor Component
```
┌────────────────────────────────────────────────┐
│           Email Editor Component               │
├────────────────────────────────────────────────┤
│                                                │
│  ┌──────────────┐      ┌──────────────┐       │
│  │   Toolbar    │      │  Properties  │       │
│  │              │      │    Panel     │       │
│  │ [🏷️ Tags]   │      │              │       │
│  └──────────────┘      └──────────────┘       │
│                                                │
│  ┌──────────────────────────────────┐         │
│  │        Canvas Area               │         │
│  │                                  │         │
│  │  "Hello {{first_name}}!"         │         │
│  │                                  │         │
│  └──────────────────────────────────┘         │
│                                                │
└────────────────────────────────────────────────┘
```

### Merge Tag Picker
```
User Action: Click "Insert Merge Tag"
     │
     ▼
┌─────────────────────────┐
│ Show Dropdown Menu      │
├─────────────────────────┤
│ • Load configured tags  │
│ • Group by category     │
│ • Enable search         │
└─────────────────────────┘
     │
     ▼
User Action: Select "First Name"
     │
     ▼
┌─────────────────────────┐
│ Insert Tag at Cursor    │
├─────────────────────────┤
│ • Get cursor position   │
│ • Insert tag value      │
│ • Update content        │
│ • Highlight tag         │
└─────────────────────────┘
```

### Preview Toggle
```
User Action: Click "Preview"
     │
     ▼
┌─────────────────────────┐
│ Parse Content           │
├─────────────────────────┤
│ • Find all merge tags   │
│ • Match with config     │
│ • Get sample values     │
└─────────────────────────┘
     │
     ▼
┌─────────────────────────┐
│ Replace Tags            │
├─────────────────────────┤
│ {{first_name}} → John   │
│ {{last_name}} → Doe     │
│ {{email}} → john@...    │
└─────────────────────────┘
     │
     ▼
┌─────────────────────────┐
│ Render Preview          │
├─────────────────────────┤
│ • Show replaced content │
│ • Add visual indicator  │
│ • Enable toggle back    │
└─────────────────────────┘
```

---

## 🎯 Data Flow

### Configuration → Editor
```
Developer Config
    │
    ├─ mergeTags: { ... }
    │
    ▼
Editor Options
    │
    ├─ Parse tags
    ├─ Validate format
    ├─ Store in state
    │
    ▼
UI Components
    │
    ├─ Populate dropdown
    ├─ Enable insertion
    └─ Setup preview
```

### Editor → Export
```
User Content
    │
    ├─ "Hello {{first_name}}!"
    │
    ▼
Export Function
    │
    ├─ Preserve tags
    ├─ Generate HTML
    ├─ Include metadata
    │
    ▼
Output HTML
    │
    └─ <p>Hello {{first_name}}!</p>
```

### Export → ESP → Recipient
```
Template HTML
    │
    ├─ {{first_name}}
    ├─ {{last_name}}
    ├─ {{email}}
    │
    ▼
ESP Processing
    │
    ├─ Load recipient data
    ├─ Match tag keys
    ├─ Replace values
    │
    ▼
Final Email
    │
    ├─ Alice
    ├─ Smith
    └─ alice@example.com
```

---

## 🔄 State Management

### Editor State
```javascript
{
  content: "Hello {{first_name}}!",
  mergeTags: {
    first_name: {
      name: "First Name",
      value: "{{first_name}}",
      sample: "John"
    }
  },
  previewMode: false,
  previewContent: null
}
```

### Preview State
```javascript
{
  content: "Hello {{first_name}}!",
  mergeTags: { ... },
  previewMode: true,
  previewContent: "Hello John!"
}
```

---

## 🎨 Visual States

### Edit Mode
```
┌────────────────────────────┐
│ Hello {{first_name}}!      │
│       ^^^^^^^^^^^^^^       │
│       (blue highlight)     │
└────────────────────────────┘
```

### Preview Mode
```
┌────────────────────────────┐
│ Hello John!                │
│       ^^^^                 │
│       (green highlight)    │
│       [sample value]       │
└────────────────────────────┘
```

### Hover State
```
┌────────────────────────────┐
│ Hello {{first_name}}!      │
│       ^^^^^^^^^^^^^^       │
│       ┌──────────────┐     │
│       │ First Name   │     │
│       │ Sample: John │     │
│       └──────────────┘     │
└────────────────────────────┘
```

---

## 🚀 Implementation Checklist

### Phase 1: Basic Setup
- [ ] Define MergeTag data model
- [ ] Add mergeTags to EditorOptions
- [ ] Parse and validate configuration
- [ ] Store tags in component state

### Phase 2: UI Components
- [ ] Create merge tag picker dropdown
- [ ] Add toolbar button
- [ ] Implement search/filter
- [ ] Group tags by category

### Phase 3: Insertion
- [ ] Get cursor position in text editor
- [ ] Insert tag value at cursor
- [ ] Update content state
- [ ] Highlight inserted tags

### Phase 4: Preview
- [ ] Add preview toggle button
- [ ] Parse content for tags
- [ ] Replace with sample values
- [ ] Show visual indicators
- [ ] Toggle back to edit mode

### Phase 5: Export
- [ ] Preserve tags in HTML export
- [ ] Include tag metadata
- [ ] Validate tag syntax
- [ ] Test with various ESPs

### Phase 6: Advanced Features
- [ ] Custom tag syntax
- [ ] Tag management UI
- [ ] Import/export tag sets
- [ ] ESP-specific formats
- [ ] Conditional logic support

---

**This flow diagram shows the complete journey of merge tags from configuration to final email delivery!**

---

**Last Updated**: October 27, 2025
**Version**: 1.0.0
