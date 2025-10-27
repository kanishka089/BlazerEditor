// ============================================
// BlazerEditor Merge Tag Autocomplete System
// Provides Unlayer-style merge tag autocomplete
// ============================================

// Global state management
window.blazerEditorAutocomplete = window.blazerEditorAutocomplete || {};
window.blazerEditorCallbacks = window.blazerEditorCallbacks || {};

// Helper functions
function isArray(val) {
    return Array.isArray(val);
}

function isObject(val) {
    return val && typeof val === 'object' && !Array.isArray(val);
}

// Normalize individual tag (handles Name/name, Value/value, Sample/sample)
function mapTagToUnlayer(tag) {
    if (!tag) return null;
    
    // Support both C# PascalCase and JavaScript camelCase
    const name = tag.Name || tag.name;
    const value = tag.Value || tag.value;
    const sample = tag.Sample || tag.sample || value;
    
    if (!name || !value) return null;
    
    return { name, value, sample };
}

// Main transformation function - handles all formats
function buildFormattedMergeTags(mergeTags) {
    // FORMAT 1: Flat array of merge tags (C# List<MergeTag> serialized)
    // Example: [ { Name: "...", Value: "...", Sample: "..." }, ... ]
    if (isArray(mergeTags) && mergeTags.length) {
        // Check if it's a flat array of tags (not grouped)
        if (!mergeTags[0].mergeTags && !mergeTags[0].MergeTags) {
            const tags = mergeTags.map(mapTagToUnlayer).filter(Boolean);
            if (tags.length) {
                // Return as array - will be handled by flattenMergeTags
                return tags;
            }
        }
        
        // FORMAT 2: Grouped as array
        // Example: [ { name: 'Group', mergeTags:[{...},{...}] }, ... ]
        if (mergeTags[0].mergeTags || mergeTags[0].MergeTags) {
            const groups = mergeTags.map(g => ({
                name: g.name || g.Name,
                mergeTags: (g.mergeTags || g.MergeTags || [])
                    .map(mapTagToUnlayer)
                    .filter(Boolean)
            })).filter(g => g.name && g.mergeTags.length);
            
            if (groups.length) return groups;
        }
    }

    // FORMAT 3: Grouped as object-of-arrays
    // Example: { GroupName: [ {Name/Value/Sample}, ... ], ... }
    if (isObject(mergeTags) && Object.values(mergeTags).some(v => isArray(v))) {
        const groups = Object.keys(mergeTags).map(groupName => ({
            name: groupName,
            mergeTags: (mergeTags[groupName] || [])
                .map(mapTagToUnlayer)
                .filter(Boolean)
        })).filter(g => g.name && g.mergeTags.length);
        
        if (groups.length) return groups;
    }

    // FORMAT 4: Flat dictionary
    // Example: { key: { Name/Value/Sample } }
    if (isObject(mergeTags)) {
        const flat = {};
        Object.keys(mergeTags).forEach(key => {
            const mapped = mapTagToUnlayer(mergeTags[key]);
            if (mapped) flat[key] = mapped;
        });
        if (Object.keys(flat).length) return flat;
    }

    // Fallback: empty object
    return {};
}

// Flatten merge tags for autocomplete menu
function flattenMergeTags(formatted) {
    let list = [];
    
    if (Array.isArray(formatted)) {
        // Check if it's a flat array of tags or grouped array
        if (formatted.length > 0 && (formatted[0].mergeTags || formatted[0].MergeTags)) {
            // Array of groups
            formatted.forEach(g => {
                (g.mergeTags || g.MergeTags || []).forEach(t => 
                    list.push({ 
                        name: t.name, 
                        value: t.value, 
                        group: g.name || g.Name || '' 
                    })
                );
            });
        } else {
            // Flat array of tags
            formatted.forEach(t => {
                const mapped = mapTagToUnlayer(t);
                if (mapped) {
                    list.push({ 
                        name: mapped.name, 
                        value: mapped.value, 
                        group: t.Category || t.category || '' 
                    });
                }
            });
        }
    } else if (isObject(formatted)) {
        const keys = Object.keys(formatted);
        const groupLike = keys.some(k => Array.isArray(formatted[k]));
        
        if (groupLike) {
            // Object with array values (groups)
            keys.forEach(groupName => {
                const arr = formatted[groupName] || [];
                arr.forEach(t => 
                    list.push({ 
                        name: t.name || t.Name, 
                        value: t.value || t.Value, 
                        group: groupName 
                    })
                );
            });
        } else {
            // Flat object
            keys.forEach(k => {
                const t = formatted[k];
                const mt = mapTagToUnlayer(t);
                if (mt) list.push({ 
                    name: mt.name, 
                    value: mt.value, 
                    group: '' 
                });
            });
        }
    }
    
    // Fallback default if empty
    if (!list.length) {
        list = [
            { name: 'First Name', value: '{{first_name}}', group: '' },
            { name: 'Last Name', value: '{{last_name}}', group: '' }
        ];
    }
    
    return list;
}

// Inject CSS styles for autocomplete menu
function ensureMentionStylesInjected() {
    if (document.getElementById('blazer-editor-mention-styles')) return;
    
    const style = document.createElement('style');
    style.id = 'blazer-editor-mention-styles';
    style.textContent = `
        .blazer-mention-menu {
            position: absolute;
            z-index: 99999;
            background: #fff;
            border: 1px solid #ddd;
            border-radius: 6px;
            box-shadow: 0 4px 16px rgba(0,0,0,.15);
            min-width: 240px;
            max-width: 320px;
            max-height: 280px;
            overflow: auto;
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
        }
        .blazer-mention-category {
            padding: 6px 10px;
            font-size: 11px;
            font-weight: 600;
            color: #666;
            text-transform: uppercase;
            letter-spacing: 0.03em;
            background: #f5f5f7;
            border-bottom: 1px solid #e0e0e0;
            margin-top: 4px;
        }
        .blazer-mention-category:first-child {
            margin-top: 0;
        }
        .blazer-mention-item {
            padding: 8px 10px;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: space-between;
            transition: background 0.15s;
        }
        .blazer-mention-item em {
            font-size: 11px;
            color: #666;
            margin-left: 8px;
            font-style: normal;
        }
        .blazer-mention-item:hover,
        .blazer-mention-item.active {
            background: #f5f5f7;
        }
        .blazer-mention-empty {
            padding: 8px 10px;
            color: #999;
            text-align: center;
        }
    `;
    document.head.appendChild(style);
}

// Initialize autocomplete for a specific editor instance
window.blazerEditorAutocomplete.init = function(editorId, mergeTagsJson) {
    console.log(`[BlazerEditor] Initializing autocomplete for editor: ${editorId}`);
    
    ensureMentionStylesInjected();
    
    // Parse merge tags from JSON string
    let mergeTags = {};
    if (mergeTagsJson) {
        try {
            mergeTags = JSON.parse(mergeTagsJson);
            console.log(`[BlazerEditor] Merge tags parsed:`, mergeTags);
        } catch (error) {
            console.error(`[BlazerEditor] Error parsing merge tags:`, error);
            mergeTags = {
                first_name: { name: 'First Name', value: '{{first_name}}', sample: 'John' },
                last_name: { name: 'Last Name', value: '{{last_name}}', sample: 'Doe' }
            };
        }
    }
    
    // Transform merge tags to Unlayer format
    console.log(`[BlazerEditor] Raw merge tags received:`, mergeTags);
    const formatted = buildFormattedMergeTags(mergeTags);
    console.log(`[BlazerEditor] Formatted tags:`, formatted);
    const flattened = flattenMergeTags(formatted);
    
    console.log(`[BlazerEditor] Flattened tags (${flattened.length} items):`, flattened);
    
    // Initialize state for this editor
    window.blazerEditorAutocomplete[editorId] = {
        allItems: flattened,
        open: false,
        query: '',
        selectedIndex: 0,
        menuEl: null,
        startSelection: null,
        prevKey: '',
        editorElement: null
    };
    
    // Install autocomplete trigger
    installAutocompleteTrigger(editorId, flattened);
};

// Install autocomplete trigger system
function installAutocompleteTrigger(editorId, allItems) {
    const state = window.blazerEditorAutocomplete[editorId];
    if (!state) return;
    
    // Create menu DOM element
    const menu = document.createElement('div');
    menu.className = 'blazer-mention-menu';
    menu.style.display = 'none';
    document.body.appendChild(menu);
    state.menuEl = menu;
    
    // Find editor element
    const editorElement = document.getElementById(editorId);
    if (!editorElement) {
        console.warn(`[BlazerEditor] Editor element not found: ${editorId}`);
        return;
    }
    
    state.editorElement = editorElement;
    
    // Wait for Radzen HTML Editor to initialize
    let attachTries = 0;
    const tryAttach = setInterval(() => {
        attachTries++;
        
        // Try to find the Radzen HTML Editor iframe or contenteditable div
        const radzenEditor = editorElement.querySelector('.rz-html-editor');
        const contentEditable = editorElement.querySelector('[contenteditable="true"]');
        
        if (radzenEditor || contentEditable) {
            clearInterval(tryAttach);
            setupMentionListeners(editorId, radzenEditor || contentEditable);
        } else if (attachTries > 50) {
            clearInterval(tryAttach);
            console.warn(`[BlazerEditor] Could not attach autocomplete listeners to ${editorId}`);
        }
    }, 200);
}

// Setup event listeners for autocomplete
function setupMentionListeners(editorId, editorDoc) {
    const state = window.blazerEditorAutocomplete[editorId];
    if (!state) return;
    
    // KEYDOWN HANDLER - Main logic
    editorDoc.addEventListener('keydown', (ev) => {
        const key = ev.key;
        if (!window.blazerEditorAutocomplete[editorId]) return;
        const s = window.blazerEditorAutocomplete[editorId];

        // MENU IS OPEN - Handle navigation and selection
        if (s.open) {
            if (key === 'Escape') {
                ev.preventDefault();
                closeMenu();
                return;
            }
            
            if (key === 'ArrowDown') {
                ev.preventDefault();
                s.selectedIndex = Math.min(filteredItems().length - 1, s.selectedIndex + 1);
                renderMenu();
                return;
            }
            
            if (key === 'ArrowUp') {
                ev.preventDefault();
                s.selectedIndex = Math.max(0, s.selectedIndex - 1);
                renderMenu();
                return;
            }
            
            if (key === 'Enter' || key === 'Tab') {
                ev.preventDefault();
                const list = filteredItems();
                if (list.length) insertSelected(list[s.selectedIndex]);
                return;
            }
            
            if (key === 'Backspace') {
                ev.preventDefault();
                if (s.query.length > 0) {
                    s.query = s.query.slice(0, -1);
                    renderMenu();
                } else {
                    closeMenu();
                }
                return;
            }
            
            // Accept alphanumeric and underscore
            if (key.length === 1 && /[a-zA-Z0-9_]/.test(key)) {
                ev.preventDefault();
                s.query += key;
                renderMenu();
                return;
            }
            
            // Any other key closes menu
            closeMenu();
            return;
        }

        // MENU IS CLOSED - Detect {{ trigger
        if (key === '{' && s.prevKey === '{') {
            ev.preventDefault();
            openMenu();
            setTimeout(() => { 
                renderMenu(); 
                positionMenu(); 
            }, 0);
        }

        // Track previous key for {{ detection
        s.prevKey = key;
    });

    // KEYUP HANDLER - Update menu position
    editorDoc.addEventListener('keyup', () => {
        if (!state.open) return;
        positionMenu();
    });

    // CLICK OUTSIDE - Close menu
    document.addEventListener('mousedown', (e) => {
        if (!state.open) return;
        if (state.menuEl && !state.menuEl.contains(e.target)) {
            closeMenu();
        }
    });

    // Helper functions
    function positionMenu() {
        try {
            const selection = window.getSelection();
            if (!selection || selection.rangeCount === 0) return;
            
            const range = selection.getRangeAt(0).cloneRange();
            const rect = range.getBoundingClientRect();
            
            state.menuEl.style.left = Math.round(rect.left) + 'px';
            state.menuEl.style.top = Math.round(rect.bottom + 6) + 'px';
        } catch (err) {
            console.warn('[BlazerEditor] Error positioning menu:', err);
        }
    }

    function openMenu() {
        state.open = true;
        state.query = '';
        state.selectedIndex = 0;
        renderMenu();
        positionMenu();
    }

    function closeMenu() {
        state.open = false;
        state.query = '';
        state.selectedIndex = 0;
        if (state.menuEl) state.menuEl.style.display = 'none';
    }

    function filteredItems() {
        const q = state.query.trim().toLowerCase();
        const items = state.allItems || [];
        
        if (!q) return items.slice(0, 100); // Show first 100 items across all categories
        
        // Filter items while preserving all items for grouping
        return items.filter(x => 
            (x.name || '').toLowerCase().includes(q) || 
            (x.value || '').toLowerCase().includes(q) ||
            (x.group || '').toLowerCase().includes(q)
        ).slice(0, 100);
    }

    function renderMenu() {
        if (!state.menuEl) return;
        
        const list = filteredItems();
        
        if (!state.open) {
            state.menuEl.style.display = 'none';
            return;
        }
        
        state.menuEl.innerHTML = '';
        
        if (list.length === 0) {
            const empty = document.createElement('div');
            empty.className = 'blazer-mention-empty';
            empty.textContent = 'No matches';
            state.menuEl.appendChild(empty);
        } else {
            // Group items by category
            const grouped = {};
            list.forEach(item => {
                const group = item.group || 'General';
                if (!grouped[group]) {
                    grouped[group] = [];
                }
                grouped[group].push(item);
            });
            
            // Track index across all items
            let globalIndex = 0;
            
            // Render each category
            Object.keys(grouped).sort().forEach(groupName => {
                const groupItems = grouped[groupName];
                
                // Category header
                const header = document.createElement('div');
                header.className = 'blazer-mention-category';
                header.textContent = groupName;
                state.menuEl.appendChild(header);
                
                // Category items
                groupItems.forEach(item => {
                    const el = document.createElement('div');
                    el.className = 'blazer-mention-item' + 
                        (globalIndex === state.selectedIndex ? ' active' : '');
                    
                    const nameSpan = document.createElement('span');
                    nameSpan.textContent = item.name;
                    
                    const valueEm = document.createElement('em');
                    valueEm.textContent = item.value;
                    
                    el.appendChild(nameSpan);
                    el.appendChild(valueEm);
                    
                    el.addEventListener('mousedown', (ev) => {
                        ev.preventDefault();
                        insertSelected(item);
                    });
                    
                    state.menuEl.appendChild(el);
                    globalIndex++;
                });
            });
        }
        
        state.menuEl.style.display = 'block';
    }

    function insertSelected(item) {
        try {
            const selection = window.getSelection();
            if (!selection || selection.rangeCount === 0) return;
            
            const range = selection.getRangeAt(0);

            // Delete the typed {{ + query
            const deleteCount = (state.query || '').length + 2; // {{ = 2 chars
            try {
                const r = range.cloneRange();
                if (r.startContainer === r.endContainer) {
                    const startOffset = Math.max(0, r.startOffset - deleteCount);
                    r.setStart(r.startContainer, startOffset);
                    r.setEnd(r.endContainer, r.endOffset);
                    r.deleteContents();
                }
            } catch {}

            // Insert the merge tag value
            const textNode = document.createTextNode(item.value);
            const r2 = window.getSelection().getRangeAt(0);
            r2.insertNode(textNode);
            
            // Move caret after inserted text
            r2.setStartAfter(textNode);
            r2.collapse(true);
            selection.removeAllRanges();
            selection.addRange(r2);
        } catch (e) {
            console.warn('[BlazerEditor] Failed to insert mention:', e);
        }
        
        closeMenu();
    }
}

// Public API: Initialize autocomplete
window.blazerEditorAutocompleteInitialize = function(editorId, mergeTagsJson) {
    window.blazerEditorAutocomplete.init(editorId, mergeTagsJson);
};

// Public API: Insert merge tag manually
window.blazerEditorInsertMergeTag = function(editorId, mergeTagValue) {
    try {
        const selection = window.getSelection();
        if (!selection || selection.rangeCount === 0) return;
        
        const range = selection.getRangeAt(0);
        const textNode = document.createTextNode(mergeTagValue);
        range.insertNode(textNode);
        
        // Move caret after inserted text
        range.setStartAfter(textNode);
        range.collapse(true);
        selection.removeAllRanges();
        selection.addRange(range);
    } catch (e) {
        console.warn('[BlazerEditor] Failed to insert merge tag:', e);
    }
};

console.log('[BlazerEditor] Merge tag autocomplete system loaded');


