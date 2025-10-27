# UI/UX Quick Reference Guide

Quick reference for the Blazer Editor design system.

---

## 🎨 Colors

### Primary
```css
--blue-500: #3b82f6
--blue-600: #2563eb
--blue-700: #1d4ed8
```

### Success
```css
--green-500: #10b981
--green-600: #059669
```

### Purple (Accent)
```css
--purple-500: #8b5cf6
--purple-600: #7c3aed
```

### Grays
```css
--gray-50: #f9fafb
--gray-100: #f3f4f6
--gray-200: #e5e7eb
--gray-300: #d1d5db
--gray-400: #9ca3af
--gray-500: #6b7280
--gray-600: #4b5563
--gray-700: #374151
--gray-800: #1f2937
```

---

## 📏 Spacing

```css
--space-xs: 4px
--space-sm: 8px
--space-md: 12px
--space-lg: 16px
--space-xl: 24px
--space-2xl: 32px
--space-3xl: 48px
```

---

## 🔤 Typography

### Font Sizes
```css
--text-xs: 12px
--text-sm: 13px
--text-base: 14px
--text-lg: 16px
--text-xl: 18px
--text-2xl: 20px
--text-3xl: 24px
--text-4xl: 28px
```

### Font Weights
```css
--font-normal: 400
--font-medium: 500
--font-semibold: 600
--font-bold: 700
```

---

## 🎭 Shadows

### Elevation
```css
--shadow-sm: 0 1px 2px rgba(0, 0, 0, 0.05)
--shadow-md: 0 4px 6px rgba(0, 0, 0, 0.07)
--shadow-lg: 0 10px 15px rgba(0, 0, 0, 0.1)
--shadow-xl: 0 20px 25px rgba(0, 0, 0, 0.15)
```

### Colored Shadows
```css
--shadow-blue: 0 4px 12px rgba(59, 130, 246, 0.3)
--shadow-green: 0 4px 12px rgba(16, 185, 129, 0.2)
--shadow-purple: 0 4px 12px rgba(139, 92, 246, 0.4)
```

---

## 📐 Border Radius

```css
--radius-sm: 6px
--radius-md: 8px
--radius-lg: 10px
--radius-xl: 12px
--radius-2xl: 16px
--radius-full: 9999px
```

---

## ⏱️ Transitions

### Timing
```css
--duration-fast: 0.2s
--duration-normal: 0.3s
--duration-slow: 0.5s
```

### Easing
```css
--ease-smooth: cubic-bezier(0.4, 0, 0.2, 1)
--ease-in: cubic-bezier(0.4, 0, 1, 1)
--ease-out: cubic-bezier(0, 0, 0.2, 1)
```

---

## 🎨 Gradients

### Backgrounds
```css
/* Blue */
linear-gradient(135deg, #3b82f6 0%, #2563eb 100%)

/* Green */
linear-gradient(135deg, #10b981 0%, #34d399 100%)

/* Purple */
linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%)

/* Gray */
linear-gradient(135deg, #ffffff 0%, #f9fafb 100%)
```

### Overlays
```css
/* Selection - Blue */
linear-gradient(135deg, rgba(59, 130, 246, 0.06) 0%, rgba(147, 197, 253, 0.06) 100%)

/* Selection - Green */
linear-gradient(135deg, rgba(16, 185, 129, 0.08) 0%, rgba(52, 211, 153, 0.08) 100%)
```

---

## 🎯 Common Patterns

### Button
```css
padding: 9px 18px;
border-radius: 8px;
font-weight: 600;
transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
```

### Card
```css
background: linear-gradient(135deg, #ffffff 0%, #f9fafb 100%);
border: 2px solid #e5e7eb;
border-radius: 10px;
box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
```

### Input
```css
padding: 10px 14px;
border: 2px solid #e5e7eb;
border-radius: 8px;
transition: all 0.2s;
```

### Input Focus
```css
border-color: #3b82f6;
box-shadow: 0 0 0 4px rgba(59, 130, 246, 0.1);
```

---

## 🎭 Animations

### Hover Lift
```css
transform: translateY(-2px);
box-shadow: 0 6px 16px rgba(0, 0, 0, 0.1);
```

### Hover Scale
```css
transform: scale(1.05);
```

### Hover Glow
```css
box-shadow: 0 0 0 4px rgba(59, 130, 246, 0.1);
```

---

## 📱 Breakpoints

```css
/* Desktop */
@media (min-width: 1400px) { }

/* Laptop */
@media (max-width: 1400px) { }

/* Tablet */
@media (max-width: 1200px) { }

/* Mobile */
@media (max-width: 768px) { }
```

---

## 🎨 Component Classes

### States
```css
.selected { /* Blue glow */ }
.hover { /* Lift + shadow */ }
.active { /* Pressed state */ }
.disabled { /* Reduced opacity */ }
```

### Variants
```css
.btn-primary { /* Blue gradient */ }
.btn-secondary { /* White with border */ }
.btn-success { /* Green gradient */ }
.btn-danger { /* Red gradient */ }
```

---

## 💡 Usage Examples

### Primary Button
```html
<button class="toolbar-btn toolbar-btn-primary">
    Export HTML
</button>
```

### Component Item
```html
<div class="component-item">
    <span class="component-icon">📝</span>
    <span class="component-label">Text</span>
</div>
```

### Form Input
```html
<div class="form-group">
    <label>Font Size</label>
    <input type="text" value="14px" />
</div>
```

### Drop Zone
```html
<div class="drop-zone active">
    <!-- Content -->
</div>
```

---

## 🎯 Best Practices

### DO ✅
- Use gradients for depth
- Add shadows for elevation
- Animate with transforms
- Use cubic-bezier easing
- Provide hover feedback
- Show focus states
- Use consistent spacing
- Follow color system

### DON'T ❌
- Use flat colors everywhere
- Skip hover states
- Use linear easing
- Animate width/height
- Forget focus indicators
- Mix spacing values
- Use random colors
- Overdo animations

---

## 🚀 Quick Tips

1. **Gradients**: Always use 135deg angle
2. **Shadows**: Layer multiple shadows for depth
3. **Hover**: Combine lift + shadow + color
4. **Focus**: Blue glow with 4px spread
5. **Transitions**: 0.2s for quick, 0.3s for normal
6. **Border Radius**: 8px is the sweet spot
7. **Spacing**: Use multiples of 4px
8. **Colors**: Stick to the palette

---

## 📚 Resources

- **Tailwind CSS**: Inspiration for color palette
- **Material Design**: Shadow and elevation system
- **Apple HIG**: Animation timing and easing
- **Radix UI**: Component patterns

---

**Last Updated**: October 26, 2025
**Version**: 1.1.0
