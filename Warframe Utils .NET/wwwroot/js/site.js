// ============================================================================
// WARFRAME UTILS .NET - SITE JAVASCRIPT
// ============================================================================
// This file contains all client-side JavaScript functionality for the site.
// Features:
// - Theme toggle (light/dark mode)
// - Local storage persistence
// - System preference detection
// ============================================================================

// ============================================================================
// THEME TOGGLE FUNCTIONALITY
// ============================================================================

/**
 * Theme Manager
 * 
 * Handles light/dark mode switching with:
 * - Local storage persistence (remembers user preference)
 * - System preference detection (uses OS dark mode setting if no preference)
 * - Smooth transitions between themes
 */

// Get reference to the theme toggle button
const themeToggle = document.getElementById("theme-toggle");

// Check if user has previously selected a theme
const userPref = localStorage.getItem("theme");

// Check if the operating system is set to dark mode
const systemPrefDark = window.matchMedia("(prefers-color-scheme: dark)").matches;

/**
 * Apply the specified theme to the page.
 * 
 * @param {string} theme - "dark" or "light"
 * 
 * Modifies:
 * - body element: adds/removes "dark" class
 * - navbar element: toggles appropriate Bootstrap classes
 * 
 * CSS classes are defined in wwwroot/css/site.css
 * - Light mode: body without "dark" class
 * - Dark mode: body with "dark" class
 */
function applyTheme(theme) {
    const body = document.body;
    const navbar = document.getElementById("main-navbar");

    // Add smooth transition class while switching themes
    body.classList.add("transition");

    if (theme === "dark") {
        // Dark mode: add "dark" class to body, update navbar colors
        body.classList.add("dark");
        navbar.classList.remove("navbar-light", "bg-white");
        navbar.classList.add("navbar-dark", "bg-dark");
    } else {
        // Light mode: remove "dark" class from body, revert navbar colors
        body.classList.remove("dark");
        navbar.classList.remove("navbar-dark", "bg-dark");
        navbar.classList.add("navbar-light", "bg-white");
    }
}

/**
 * Get the currently applied theme.
 * 
 * @returns {string} "dark" or "light"
 * 
 * Checks if body element has "dark" class to determine current theme.
 */
function currentTheme() {
    return document.body.classList.contains("dark") ? "dark" : "light";
}

// ============================================================================
// THEME INITIALIZATION
// ============================================================================

/**
 * Initialize theme on page load.
 * 
 * Priority order:
 * 1. User preference (if previously set) - stored in localStorage
 * 2. System preference (from OS settings)
 * 3. Default: light mode
 */
if (userPref) {
    // User has previously selected a theme - use their preference
    applyTheme(userPref);
} else {
    // No user preference - check system dark mode setting
    // If OS is set to dark mode, use dark; otherwise use light
    applyTheme(systemPrefDark ? "dark" : "light");
}

// ============================================================================
// THEME TOGGLE EVENT LISTENER
// ============================================================================

/**
 * Theme toggle button click handler.
 * 
 * When user clicks the theme toggle button (🌓):
 * 1. Gets current theme
 * 2. Switches to opposite theme
 * 3. Applies new theme to page
 * 4. Saves preference to localStorage for next visit
 * 
 * localStorage persists across browser sessions - preference survives page reloads
 */
themeToggle.addEventListener("click", () => {
    // Determine if currently in dark mode
    const isDark = currentTheme() === "dark";

    // Toggle theme (opposite of current)
    const newTheme = isDark ? "light" : "dark";

    // Apply the new theme to the page
    applyTheme(newTheme);

    // Save user preference to browser storage
    // Retrieved on next page load via userPref variable above
    localStorage.setItem("theme", newTheme);
});

// ============================================================================
// ADDITIONAL UTILITIES (Future Use)
// ============================================================================

/**
 * Helper function to detect if dark mode is currently active.
 * Useful for conditional JavaScript logic based on theme.
 * 
 * @returns {boolean} true if dark mode is active, false if light mode
 */
function isDarkMode() {
    return currentTheme() === "dark";
}

/**
 * Helper function to log the current theme (for debugging).
 * Useful during development to verify theme state.
 */
function logCurrentTheme() {
    console.log(`Current theme: ${currentTheme()}`);
    console.log(`Dark mode active: ${isDarkMode()}`);
    console.log(`User preference: ${userPref || "none"}`);
    console.log(`System dark mode: ${systemPrefDark}`);
}

// Uncomment the line below for debugging theme issues:
// logCurrentTheme();


