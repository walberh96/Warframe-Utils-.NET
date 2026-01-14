# Warframe Utils - Next.js Frontend

A modern, beautiful frontend for the Warframe Utils application built with Next.js 14, TypeScript, and Tailwind CSS.

## 🎨 Features

- **Modern UI**: Beautiful gradient-based design with glass morphism effects
- **Dark Mode**: Native dark mode support with theme switching
- **Responsive**: Mobile-first design that works on all devices
- **Real-time Data**: Live game status updates and market prices
- **Price Alerts**: Create and manage price alerts for your favorite items
- **Market Search**: Search and browse Warframe market items with live orders
- **Performance**: Built with Next.js 14 App Router for optimal performance

## 🚀 Quick Start

### Prerequisites

- Node.js 18.17 or later
- npm or yarn
- .NET backend running on http://localhost:5000

### Installation

1. Navigate to the frontend directory:
```bash
cd warframe-frontend
```

2. Install dependencies:
```bash
npm install
```

3. Start the development server:
```bash
npm run dev
```

4. Open your browser and visit:
```
http://localhost:3000
```

## 📁 Project Structure

```
warframe-frontend/
├── src/
│   ├── app/                    # Next.js App Router pages
│   │   ├── layout.tsx          # Root layout with theme provider
│   │   ├── page.tsx            # Home page
│   │   └── globals.css         # Global styles and Tailwind
│   ├── components/             # React components
│   │   ├── ui/                 # Reusable UI components (shadcn/ui)
│   │   │   ├── button.tsx
│   │   │   ├── card.tsx
│   │   │   ├── input.tsx
│   │   │   ├── tabs.tsx
│   │   │   └── toast.tsx
│   │   ├── navbar.tsx          # Navigation bar with theme toggle
│   │   ├── game-status-section.tsx    # Game status cards
│   │   ├── search-section.tsx         # Market search interface
│   │   ├── NotificationBell.tsx       # Alert & notification management
│   │   ├── theme-provider.tsx         # Theme context provider
│   │   └── auth/                      # Authentication components
│   ├── hooks/                  # Custom React hooks
│   │   ├── use-toast.ts        # Toast notification hook
│   │   └── useNotifications.ts # Notification management hook
│   ├── contexts/               # React contexts
│   │   └── AuthContext.tsx     # Authentication context
│   └── lib/                    # Utility functions
│       └── utils.ts            # Helper functions
├── public/                     # Static assets
├── next.config.mjs             # Next.js configuration
├── tailwind.config.ts          # Tailwind CSS configuration
├── tsconfig.json               # TypeScript configuration
└── package.json                # Dependencies and scripts
```

## 🎯 Key Components

### Game Status Section
Displays real-time information about:
- Cetus Cycle (Day/Night)
- Void Trader (Baro Ki'Teer)
- Arbitration missions
- Server status

### Market Search
- Search for mods and items with autocomplete suggestions
- View item images, descriptions, rarity, and trading tax
- View buy and sell orders
- See player status (online/offline)
- Sort by price and quantity
- Create price alerts directly from search results

### Price Alerts & Notifications
- Create price alerts for items
- Get notified when prices drop (pop-up notifications)
- Manage all alerts through notification bell
- Modify alert prices
- View active alerts and triggered notifications
- Mark notifications as read

## 🔌 API Integration

The frontend connects to the .NET backend through API proxying configured in `next.config.mjs`:

```javascript
async rewrites() {
  return [
    {
      source: '/api/:path*',
      destination: 'http://localhost:5000/api/:path*',
    },
  ];
}
```

### Available Endpoints

- `GET /api/GameStatus` - Get current game status
- `GET /api/Search?modName=xxx` - Search for items
- `GET /api/Search/items` - Get all items
- `GET /api/Alert` - Get user's price alerts
- `POST /api/Alert` - Create new price alert
- `PUT /api/Alert/{id}` - Update price alert
- `DELETE /api/Alert/{id}` - Delete price alert
- `GET /api/Alert/notifications/unread` - Get unread notifications
- `POST /api/Alert/notifications/{id}/read` - Mark notification as read
- `GET /api/User/me` - Get current user info
- `GET /api/User/check` - Check authentication status

## 🎨 Styling

The application uses:
- **Tailwind CSS**: Utility-first CSS framework
- **shadcn/ui**: High-quality UI components
- **Radix UI**: Unstyled, accessible component primitives
- **Custom Theme**: Warframe-inspired color palette

### Theme Customization

Edit `src/app/globals.css` to customize colors:

```css
:root {
  --primary: 221.2 83.2% 53.3%;  /* Blue */
  --secondary: 210 40% 96.1%;     /* Gray */
  /* ... more colors */
}
```

## 📦 Build for Production

1. Build the application:
```bash
npm run build
```

2. Start the production server:
```bash
npm start
```

3. The optimized build will be available at `http://localhost:3000`

## 🔧 Configuration

### Environment Variables

Create a `.env.local` file for environment-specific configuration:

```env
NEXT_PUBLIC_API_URL=http://localhost:5000
```

### Backend URL

Update the API proxy in `next.config.mjs` if your backend runs on a different port:

```javascript
destination: 'http://localhost:YOUR_PORT/api/:path*',
```

## 🌐 Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)

## 📱 Responsive Design

The UI is fully responsive with breakpoints:
- Mobile: < 640px
- Tablet: 640px - 1024px
- Desktop: > 1024px

## 🎭 Features in Detail

### Theme Switching
- Toggle between light and dark modes
- System preference detection
- Persistent theme selection

### Real-time Updates
- Game status updates every 60 seconds
- Price alerts checked by backend service every 30 seconds
- Notifications polled every 30 seconds
- Live order data from Warframe Market API
- Immediate price check on alert creation/modification

### Accessibility
- ARIA labels and roles
- Keyboard navigation support
- Screen reader friendly
- Focus management

## 🛠️ Development

### Adding New Components

1. Create component in `src/components/`:
```typescript
export function MyComponent() {
  return <div>My Component</div>;
}
```

2. Import and use in pages:
```typescript
import { MyComponent } from '@/components/my-component';
```

### Using UI Components

Import from `@/components/ui/`:
```typescript
import { Button } from '@/components/ui/button';
import { Card } from '@/components/ui/card';
```

## 🐛 Troubleshooting

### API Connection Issues

If the frontend can't connect to the backend:

1. Ensure the .NET backend is running
2. Check CORS settings in `Program.cs`
3. Verify the port in `next.config.mjs`

### Build Errors

```bash
# Clear cache and rebuild
rm -rf .next node_modules
npm install
npm run build
```

### Type Errors

```bash
# Check TypeScript errors
npm run lint
```

## 📚 Learn More

- [Next.js Documentation](https://nextjs.org/docs)
- [Tailwind CSS](https://tailwindcss.com/docs)
- [shadcn/ui](https://ui.shadcn.com/)
- [Radix UI](https://www.radix-ui.com/)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## 📄 License

This project is part of Warframe Utils and shares the same license.

---

**Built with ❤️ using Next.js, TypeScript, and Tailwind CSS**
