# 📚 Warframe Utils - Documentation Index

Welcome! This index will help you find the right documentation for your needs.

---

## 🚀 Getting Started

### New to this project?
1. **Start here**: [README.md](README.md) - Project overview
2. **Then read**: [PROJECT_COMPLETE.md](PROJECT_COMPLETE.md) - What's new
3. **Setup**: [SETUP_GUIDE.md](SETUP_GUIDE.md) - Complete installation guide

### Quick Setup (Just want to run it!)
👉 **[SETUP_GUIDE.md](SETUP_GUIDE.md)** - Follow steps 1-2-3 and you're done!

Or use the setup scripts:
- **Windows**: `.\setup-frontend.ps1`
- **Mac/Linux**: `./setup-frontend.sh`

---

## 📖 Documentation by Topic

### 🎯 For Users

| Document | Purpose | Time to Read |
|----------|---------|--------------|
| [README.md](README.md) | Project overview, features, quick start | 10 min |
| [SETUP_GUIDE.md](SETUP_GUIDE.md) | Step-by-step installation | 15-30 min |
| [PROJECT_COMPLETE.md](PROJECT_COMPLETE.md) | What's new, features summary | 5 min |

### 👨‍💻 For Developers

| Document | Purpose | Time to Read |
|----------|---------|--------------|
| [DEVELOPER_GUIDE.md](DEVELOPER_GUIDE.md) | Quick reference, commands, tips | Reference |
| [warframe-frontend/README.md](warframe-frontend/README.md) | Frontend architecture, components | 10 min |
| [CHANGELOG.md](CHANGELOG.md) | All changes made during rebuild | 15 min |
| [UI_GUIDE.md](UI_GUIDE.md) | UI design, colors, components | Reference |

### 🎨 For Designers

| Document | Purpose |
|----------|---------|
| [UI_GUIDE.md](UI_GUIDE.md) | Color palette, layout, components, responsive design |

### 🔧 For System Administrators

| Document | Purpose |
|----------|---------|
| [SETUP_GUIDE.md](SETUP_GUIDE.md) | PostgreSQL setup, deployment |
| [README.md](README.md) | Configuration, environment variables |

---

## 📂 By Document Type

### Overview Documents
- **[README.md](README.md)** - Main documentation (Full Stack)
- **[PROJECT_COMPLETE.md](PROJECT_COMPLETE.md)** - Rebuild summary
- **[CHANGELOG.md](CHANGELOG.md)** - Detailed changes log

### Setup & Installation
- **[SETUP_GUIDE.md](SETUP_GUIDE.md)** - Complete setup guide
- **[QUICK_START_ALERTS.md](QUICK_START_ALERTS.md)** - Quick start (includes original alert setup)

### Developer Resources
- **[DEVELOPER_GUIDE.md](DEVELOPER_GUIDE.md)** - Commands, tips, reference
- **[warframe-frontend/README.md](warframe-frontend/README.md)** - Frontend docs

### Design Resources
- **[UI_GUIDE.md](UI_GUIDE.md)** - Visual design guide

### Legacy Documentation (Original System)
- **[README_ALERTS.md](README_ALERTS.md)** - Price alert system
- **[ALERT_SYSTEM_SETUP.md](ALERT_SYSTEM_SETUP.md)** - Alert setup details
- **[ALERT_SYSTEM_IMPLEMENTATION.md](ALERT_SYSTEM_IMPLEMENTATION.md)** - Alert implementation

---

## 🎯 Common Tasks

### "I want to install and run the application"
1. Read: [SETUP_GUIDE.md](SETUP_GUIDE.md)
2. Follow the 3-step process
3. Done! 🎉

### "I want to understand the architecture"
1. Read: [README.md](README.md#architecture)
2. Check: [warframe-frontend/README.md](warframe-frontend/README.md#project-structure)
3. Review: [CHANGELOG.md](CHANGELOG.md#api-integration)

### "I want to customize the UI"
1. Read: [UI_GUIDE.md](UI_GUIDE.md)
2. Check: [warframe-frontend/README.md](warframe-frontend/README.md#styling)
3. Edit: `warframe-frontend/src/app/globals.css`

### "I want to add a new feature"
1. Read: [DEVELOPER_GUIDE.md](DEVELOPER_GUIDE.md#adding-new-features)
2. Check existing code structure
3. Follow the patterns

### "I want to deploy to production"
1. Read: [README.md](README.md#deployment)
2. Build backend: `dotnet publish -c Release`
3. Build frontend: `npm run build`
4. Deploy both separately

---

## 🔍 Quick Reference

### File Locations

**Frontend:**
```
warframe-frontend/
├── src/app/page.tsx          # Main page
├── src/components/           # All components
├── src/app/globals.css       # Global styles
└── package.json              # Dependencies
```

**Backend:**
```
Warframe Utils .NET/
├── Program.cs                # App config
├── Controllers/API/          # API endpoints
├── Services/                 # Business logic
└── appsettings.json         # Configuration
```

### Key Commands

**Frontend:**
```bash
npm run dev       # Start development
npm run build     # Build for production
npm start         # Run production build
```

**Backend:**
```bash
dotnet run                  # Start server
dotnet ef database update   # Run migrations
dotnet build                # Build project
```

### Ports
- Frontend: **http://localhost:3000**
- Backend: **http://localhost:5000** (API)
- Backend HTTPS: **https://localhost:5001**

---

## 📊 Documentation Completion

✅ **Setup & Installation**
- Complete installation guide
- Quick start guide
- Setup scripts (Windows/Unix)

✅ **Developer Documentation**
- API reference
- Component documentation
- Quick reference guide

✅ **User Documentation**
- Feature overview
- Usage instructions
- Troubleshooting

✅ **Design Documentation**
- UI guide
- Color palette
- Component showcase

---

## 🎓 Learning Path

### Beginner Path
1. [README.md](README.md) - Understand what this project does
2. [SETUP_GUIDE.md](SETUP_GUIDE.md) - Install and run it
3. [PROJECT_COMPLETE.md](PROJECT_COMPLETE.md) - Explore features

### Intermediate Path
1. [warframe-frontend/README.md](warframe-frontend/README.md) - Learn frontend
2. [DEVELOPER_GUIDE.md](DEVELOPER_GUIDE.md) - Development practices
3. [UI_GUIDE.md](UI_GUIDE.md) - Understand the design

### Advanced Path
1. [CHANGELOG.md](CHANGELOG.md) - See all technical changes
2. Study the source code
3. Add new features

---

## 🔗 External Resources

### Technologies Used
- [Next.js Documentation](https://nextjs.org/docs)
- [React Documentation](https://react.dev)
- [Tailwind CSS](https://tailwindcss.com/docs)
- [shadcn/ui](https://ui.shadcn.com)
- [.NET Documentation](https://docs.microsoft.com/dotnet)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)

### APIs
- [Warframe Market API](https://docs.warframe.market)
- [Warframe Status API](https://docs.warframestat.us)

---

## 📝 Documentation Standards

All our documentation follows these principles:

✅ **Clear Structure**
- Table of contents for long docs
- Sections with descriptive headings
- Code examples where relevant

✅ **Multiple Formats**
- Step-by-step guides
- Quick references
- Visual diagrams

✅ **Searchable**
- Descriptive titles
- Consistent terminology
- Clear keywords

---

## 🆘 Need Help?

### Can't find what you're looking for?

1. **Use Ctrl+F** to search within documents
2. **Check the README** - Covers most topics
3. **Read error messages** - They're usually helpful
4. **Check browser console** - F12 for frontend errors
5. **Check terminal output** - Backend logs

### Common Questions

**Q: How do I start the application?**
A: See [SETUP_GUIDE.md](SETUP_GUIDE.md#installation-steps)

**Q: How do I customize colors?**
A: See [UI_GUIDE.md](UI_GUIDE.md#color-palette)

**Q: How do I add a new component?**
A: See [DEVELOPER_GUIDE.md](DEVELOPER_GUIDE.md#adding-new-features)

**Q: Why isn't my API call working?**
A: See [SETUP_GUIDE.md](SETUP_GUIDE.md#troubleshooting)

---

## 📅 Document Versions

All documentation is current as of **January 2026** (Updated with latest project structure).

### Last Updated
- README.md: January 13, 2026
- SETUP_GUIDE.md: January 13, 2026
- DEVELOPER_GUIDE.md: January 13, 2026
- UI_GUIDE.md: January 13, 2026
- PROJECT_COMPLETE.md: January 13, 2026
- CHANGELOG.md: January 13, 2026

---

## ✨ What's Next?

After reading the documentation:

1. **Install the application** - Follow SETUP_GUIDE.md
2. **Explore the UI** - Try all features
3. **Customize** - Make it your own
4. **Extend** - Add new features
5. **Share** - Deploy to production

---

## 🎯 Quick Links

| Need to... | Go to... |
|------------|----------|
| Install | [SETUP_GUIDE.md](SETUP_GUIDE.md) |
| Understand project | [README.md](README.md) |
| See what's new | [PROJECT_COMPLETE.md](PROJECT_COMPLETE.md) |
| Developer reference | [DEVELOPER_GUIDE.md](DEVELOPER_GUIDE.md) |
| Frontend docs | [warframe-frontend/README.md](warframe-frontend/README.md) |
| UI design | [UI_GUIDE.md](UI_GUIDE.md) |
| See all changes | [CHANGELOG.md](CHANGELOG.md) |

---

**Happy reading! 📚**

**If you find this documentation helpful, give it a ⭐ on GitHub!**
