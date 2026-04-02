# Preline Blazor Admin Dashboard

A high-performance, client-side admin dashboard template built with **.NET 8 Blazor**, **Tailwind CSS v4**, and **Preline UI**. This application runs entirely in the user's browser, making it ideal for static hosting environments and decoupled frontend architectures.

## Project Overview

This dashboard is a comprehensive UI foundation for SaaS operations, designed for speed and flexibility. It demonstrates:

- **Full Browser Execution**: A standalone frontend that processes all logic and UI interactions locally.
- **Modern Design System**: Built on Preline 4.x and Tailwind CSS v4 for a sleek, enterprise-grade aesthetic.
- **Dynamic Personalization**: Deeply integrated theme system allowing runtime changes to modes (Light/Dark/System), color palettes, and custom accent colors.
- **Intelligent Data Viz**: Theme-aware Highcharts integration that automatically restyles charts based on the application's active palette.
- **Production-Ready Components**: A robust library of reusable, typed Blazor components (`UiButton`, `UiCard`, `UiTable`, etc.).
- **Zero-Backend Dependency**: Currently powered by seeded mock data services, enabling immediate UI development and validation.

## Tech Stack

- **.NET 8 Blazor** (Client-side execution)
- **Preline UI** (`preline@^4.1.2`)
- **Tailwind CSS v4** (Modern CSS architecture)
- **Highcharts** (Responsive, themeable data visualization)

## Key Features

### 1. Reusable UI Kit
A comprehensive suite of typed components is available in `PrelineBlazorWasmApp/Components/UI/Kit`. These include:
- **Navigation**: Breadcrumbs, Tabs, Pagination.
- **Forms**: Buttons, Inputs, Swatches, Selects, Checkboxes, Switches.
- **Feedback**: Alerts, Badges, Toasts, Progress indicators.
- **Containers**: Cards, Modals, Drawers, Accordions, List groups.

### 2. Advanced Theme Engine
The application features a pervasive theme service that manages:
- **Visual Modes**: Smooth transitions between Light, Dark, and System modes.
- **Palette Presets**: Selectable color schemes (e.g., Indigo, Rose, Amber).
- **Custom Overrides**: Ability for users to pick exact HEX codes for the primary brand color.
- **Persistence**: All preferences are saved to local storage for a consistent user experience.

### 3. Responsive Architecture
- **Adaptive Sidebar**: Collapsible menu with mobile overlay support.
- **Fluid Layouts**: Responsive grids and tables that maintain usability across all screen sizes.
- **Contrast Optimization**: Hand-tuned accessibility for both light and dark backgrounds.

## Getting Started

### Prerequisites

- **.NET 8 SDK**
- **Node.js** (for Tailwind and asset management)

### 1. Install Dependencies

Navigate to the project directory and install the necessary npm packages:

```bash
cd PrelineBlazorWasmApp
npm install
```

### 2. Build Assets

Generate the CSS and copy vendor scripts to the web root:

```bash
npm run build:assets
```

### 3. Run the Application

Launch the development server:

```bash
dotnet run --project PrelineBlazorWasmApp.csproj
```

The application will be accessible at the URL provided in the console (usually `http://localhost:5000` or `https://localhost:5001`).

## Development Workflow

From the `PrelineBlazorWasmApp` directory:

- `npm run watch:css`: Watch for Tailwind CSS changes during development.
- `npm run build:css`: Compile and minify production CSS.
- `npm run copy:vendor`: Refresh vendor scripts (Preline, Highcharts) in the `wwwroot/js` folder.

---
*Note: This project is focused on the frontend architecture and user experience. Authentication and real-time API integrations are intended to be implemented during the next phase of development.*
