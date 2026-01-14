# 🎨 Warframe Utils - Visual UI Guide

This document showcases the new UI design and components.

---

## 🌟 Color Palette

### Primary Colors
```
Primary Blue:    #3b82f6  (rgb(59, 130, 246))
Secondary:       #a855f7  (rgb(168, 85, 247))
Accent Pink:     #ec4899  (rgb(236, 72, 153))
```

### Background Gradients
```
Main Background: from-slate-950 via-blue-950 to-slate-950
Hero Gradient:   from-blue-400 via-purple-400 to-pink-400
Card Gradient:   from-blue-900/20 to-purple-900/20
```

### Theme Colors
```css
Light Mode:
- Background: White (#ffffff)
- Text: Dark gray (#1f2937)
- Card: White with shadow

Dark Mode:
- Background: Very dark blue (#0f172a)
- Text: Light gray (#f1f5f9)
- Card: Dark blue with border
```

---

## 📱 Layout Structure

```
┌─────────────────────────────────────────┐
│           NAVBAR                         │
│  [Logo] Warframe Utils    [Theme] [🌙]  │
└─────────────────────────────────────────┘
┌─────────────────────────────────────────┐
│                                          │
│         HERO SECTION                     │
│    Warframe Utils (Gradient Text)       │
│    Real-time market prices...           │
│                                          │
└─────────────────────────────────────────┘
┌──────────┬──────────┬──────────┐
│  CETUS   │  VOID    │  VENUS   │
│  CYCLE   │  TRADER  │  CYCLE   │
│  [Blue]  │[Purple]  │ [Pink]   │
└──────────┴──────────┴──────────┘
┌─────────────────────────────────────────┐
│      MARKET SEARCH                       │
│  [Search Bar..................] [Search] │
│                                          │
│  ┌─ Mod Details ────────────────────┐  │
│  │ Serration (Legendary)             │  │
│  │ Trading Tax: 8000 credits         │  │
│  └───────────────────────────────────┘  │
│                                          │
│  [Sell Orders] [Buy Orders]             │
│  ┌─────────────────────────────────┐   │
│  │ Player1     Status: ingame      │   │
│  │             120p  Qty: 1    [→] │   │
│  ├─────────────────────────────────┤   │
│  │ Player2     Status: online      │   │
│  │             115p  Qty: 2    [→] │   │
│  └─────────────────────────────────┘   │
└─────────────────────────────────────────┘
┌─────────────────────────────────────────┐
│      PRICE ALERTS                        │
│  ┌─ Create New Alert ───────────────┐  │
│  │ [Item Name....................]   │  │
│  │ [Alert Price..........]           │  │
│  │ [+ Create Alert]                  │  │
│  └──────────────────────────────────┘  │
│                                          │
│  Active Alerts                           │
│  ┌─────────────────────────────────┐   │
│  │ Serration                        │   │
│  │ Alert: 100p | Current: 120p     │   │
│  │ Created: Jan 13                [×]│   │
│  └─────────────────────────────────┘   │
└─────────────────────────────────────────┘
```

---

## 🎯 Component Showcase

### 1. Hero Section
```
╔════════════════════════════════════════╗
║                                         ║
║       🎮 Warframe Utils 🎮             ║
║   (Animated Gradient Text - XL Size)   ║
║                                         ║
║  Real-time market prices, trading      ║
║  orders, and game status monitoring    ║
║                                         ║
╚════════════════════════════════════════╝

Features:
- Large 7xl heading on desktop, 5xl on mobile
- Animated gradient text (blue → purple → pink)
- Pulsing animation
- Centered layout
- Responsive typography
```

### 2. Game Status Cards (3 Cards)

#### Cetus Cycle Card
```
┌─────────────────────┐
│ 🕐 Cetus Cycle      │ ← Blue theme
├─────────────────────┤
│                     │
│      Day/Night      │ ← Large text
│                     │
│   2h 34m left       │ ← Small gray text
│                     │
└─────────────────────┘

States: Day/Night
Colors: Blue (#3b82f6)
Icon: Clock
```

#### Void Trader Card
```
┌─────────────────────┐
│ 👤 Void Trader      │ ← Purple theme
├─────────────────────┤
│                     │
│      Active         │ ← Large text
│                     │
│   Larunda Relay     │ ← Location
│                     │
└─────────────────────┘

States: Active/Away
Colors: Purple (#a855f7)
Icon: Users
```

#### Venus Cycle Card
```
┌─────────────────────┐
│ ❄️ Venus Cycle      │ ← Pink theme
├─────────────────────┤
│                     │
│    Warm/Cold        │ ← Large text
│                     │
│   45m 30s left      │ ← Time remaining
│                     │
└─────────────────────┘

States: Warm/Cold
Colors: Pink (#ec4899)
Icon: Snowflake
```

### 3. Market Search Section
```
┌───────────────────────────────────────┐
│ 🔍 Market Search                      │
├───────────────────────────────────────┤
│                                       │
│ [Search Box........................] │
│                               [Search]│
│                                       │
│ ╔═ Mod Details ════════════════════╗│
│ ║ Serration                         ║│
│ ║ Rarity: Legendary                 ║│
│ ║ Trading Tax: 8000 credits         ║│
│ ╚═══════════════════════════════════╝│
│                                       │
│ [Sell Orders (25)] [Buy Orders (10)] │
│                                       │
│ ┌─ Order ──────────────────────────┐│
│ │ PlayerName    Status: ingame     ││
│ │                      120p  Qty: 1││
│ └──────────────────────────────────┘│
│ ┌─ Order ──────────────────────────┐│
│ │ AnotherPlayer Status: offline    ││
│ │                      115p  Qty: 2││
│ └──────────────────────────────────┘│
│                                       │
└───────────────────────────────────────┘

Features:
- Live search
- Tabbed interface (Buy/Sell)
- Player status indicators
- Hover effects on orders
- Gradient header
```

### 4. Notification Bell & Price Alerts
```
┌───────────────────────────────────────┐
│ 🔔 Notification Bell (Top Nav)        │
├───────────────────────────────────────┤
│ [Alerts Tab] [Notifications Tab]      │
│                                       │
│ ═══ ALERTS TAB ═══                    │
│ ┌─ Alert ──────────────────────────┐│
│ │ Serration                         ││
│ │ Alert: 100p | Current: 120p      ││
│ │ [✏️ Edit] [🗑️ Delete]            ││
│ └──────────────────────────────────┘│
│                                       │
│ ═══ NOTIFICATIONS TAB ═══             │
│ ┌─ Notification ────────────────────┐│
│ │ 🎯 Serration                      ││
│ │ Price dropped to 95p              ││
│ │ [✓ Mark Read]                    ││
│ └──────────────────────────────────┘│
│                                       │
└───────────────────────────────────────┘

Features:
- Tabbed interface (Alerts/Notifications)
- Create, modify, and delete alerts
- View all triggered notifications
- Mark notifications as read
- Visual indicators for triggered alerts
- Pop-up toast notifications on trigger
```

---

## 🎨 Interactive Elements

### Buttons
```
┌──────────────┐
│   Primary    │  ← Blue background, white text
└──────────────┘

┌──────────────┐
│  Secondary   │  ← Gray background
└──────────────┘

┌──────────────┐
│  Destructive │  ← Red background
└──────────────┘

┌──────────────┐
│   Outline    │  ← Border only
└──────────────┘

[   Ghost   ]    ← Transparent, hover effect

[   Icon   ]     ← Square, icon only
```

### Hover States
```
Normal Card:
┌─────────────┐
│   Content   │
└─────────────┘

Hover Card:
┌═════════════┐  ← Brighter border
│   Content   │  ← Subtle scale up
└═════════════┘  ← Shadow increases
```

---

## 📱 Responsive Design

### Desktop (>1024px)
```
┌─────────────────────────────────────────┐
│           Full width navbar              │
├─────────────────────────────────────────┤
│                                          │
│       Large hero text (7xl)              │
│                                          │
├──────────┬──────────┬──────────┤
│  Card 1  │  Card 2  │  Card 3  │ ← 3 columns
├──────────┴──────────┴──────────┤
│                                          │
│         Full width search                │
│                                          │
└─────────────────────────────────────────┘
```

### Tablet (640px - 1024px)
```
┌─────────────────────────────┐
│      Compact navbar          │
├─────────────────────────────┤
│                              │
│    Medium hero text (6xl)    │
│                              │
├──────────────┬──────────────┤
│   Card 1     │   Card 2     │ ← 2 columns
├──────────────┼──────────────┤
│   Card 3     │              │
├──────────────┴──────────────┤
│                              │
│      Compact search          │
│                              │
└─────────────────────────────┘
```

### Mobile (<640px)
```
┌─────────────┐
│   Navbar    │
├─────────────┤
│             │
│  Hero (5xl) │
│             │
├─────────────┤
│   Card 1    │
├─────────────┤  ← 1 column
│   Card 2    │
├─────────────┤
│   Card 3    │
├─────────────┤
│             │
│   Search    │
│             │
└─────────────┘
```

---

## 🎭 Animations

### Page Load
1. Fade in (0.3s)
2. Slide up (0.5s)
3. Cards stagger (0.1s each)

### Hover Effects
- Scale: 1.02
- Transition: 200ms
- Border glow increase

### Theme Switch
- Smooth color transition (300ms)
- No layout shift

---

## 🎨 CSS Classes Used

### Layout
```css
.container        /* Max width container */
.mx-auto          /* Center horizontally */
.px-4 py-8        /* Padding */
.space-y-8        /* Vertical spacing */
.gap-4            /* Grid gap */
```

### Flexbox & Grid
```css
.flex             /* Flexbox */
.grid             /* CSS Grid */
.grid-cols-4      /* 4 columns */
.items-center     /* Align center */
.justify-between  /* Space between */
```

### Colors & Effects
```css
.bg-gradient-to-br        /* Background gradient */
.text-blue-400            /* Text color */
.border-blue-500/20       /* Border with opacity */
.backdrop-blur-sm         /* Blur effect */
.hover:border-blue-500/40 /* Hover state */
.transition-all           /* Smooth transition */
```

---

## 🖼️ Typography

### Headings
```
7xl: Hero title (72px)
5xl: Section headers (48px)
2xl: Card titles (24px)
xl:  Large text (20px)
lg:  Medium text (18px)
base: Body text (16px)
sm:  Small text (14px)
xs:  Extra small (12px)
```

### Font Weights
```
font-bold:      700
font-semibold:  600
font-medium:    500
font-normal:    400
```

---

## 📐 Spacing System

```
space-y-1:  0.25rem (4px)
space-y-2:  0.5rem  (8px)
space-y-3:  0.75rem (12px)
space-y-4:  1rem    (16px)
space-y-6:  1.5rem  (24px)
space-y-8:  2rem    (32px)
```

---

## 🎯 Accessibility

### ARIA Labels
- All buttons have descriptive labels
- Icons have aria-hidden="true"
- Form inputs have associated labels

### Keyboard Navigation
- Tab order is logical
- Focus visible on all interactive elements
- Enter key activates buttons

### Color Contrast
- All text meets WCAG AA standards
- Minimum contrast ratio: 4.5:1
- Enhanced contrast for small text

---

## 🎨 Design Principles

1. **Consistency**: Same patterns throughout
2. **Clarity**: Clear visual hierarchy
3. **Feedback**: Hover states, loading states
4. **Simplicity**: Clean, uncluttered
5. **Accessibility**: Usable by everyone

---

**This UI was built with ❤️ using Next.js, Tailwind CSS, and shadcn/ui**
