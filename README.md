# Preline Blazor Admin Dashboard

A production-style admin dashboard built with **Blazor (.NET 8)**, **Preline 4.x**, **Tailwind CSS v4**, and **Highcharts**.

## Project Overview

This project is a UI-first SaaS operations dashboard that demonstrates:

- A responsive admin shell (sidebar, topbar, breadcrumbs, mobile menu)
- Multi-page business views (Dashboard, Analytics, Customers, Orders, Billing, Team, Tasks, Activity, Settings)
- A reusable, Bootstrap-inspired Blazor component kit (`Ui*` components)
- Theme personalization with mode + palette + custom color overrides
- Theme-aware Highcharts integration with runtime chart restyling
- Search + pagination behavior across operational tables

The app currently uses seeded mock data through services, so the UI can be developed and validated before backend/API integration.

## Tech Stack

- **.NET 8 Blazor Web App** (Interactive Server)
- **Preline** (`preline@^4.1.2`)
- **Tailwind CSS v4** (`@tailwindcss/cli`, `@tailwindcss/forms`)
- **Highcharts** (`highcharts@^12.5.0`)

## Key Features

### 1. Reusable Component Library
Typed reusable components are in:

- `PrelineBlazorApp/Components/UI/Kit`

Examples include:

- `UiButton`, `UiBadge`, `UiAlert`, `UiCard`
- `UiInput`, `UiSelect`, `UiTextarea`, `UiCheckbox`, `UiRadio`, `UiSwitch`, `UiFormGroup`
- `UiTable`, `UiPagination`, `UiBreadcrumb`, `UiTabs`, `UiAccordion`, `UiListGroup`
- `UiModal`, `UiDrawer`, `UiDropdown`, `UiToast`

Shared enums/models are in:

- `PrelineBlazorApp/Models/UI/Kit`

### 2. Theme System
Theme behavior supports:

- `System / Light / Dark` mode
- Global default palette
- Global custom accent color
- Per-page palette override
- Per-page custom accent override
- Persistent preferences in `localStorage`

Precedence:

`page custom > page palette > global custom > global palette > slate fallback`

### 3. Highcharts Integration
Charts are hosted through a reusable Blazor wrapper and styled from runtime theme tokens.
When mode/palette/accent changes, charts restyle without page reload.

### 4. Responsive UX
- Sidebar overlay + burger menu on mobile
- Responsive cards, tables, and controls
- Theme-aware contrast handling for dark and light modes

## Project Structure

- `PrelineBlazorApp/Components/Layout` - app shell and navigation
- `PrelineBlazorApp/Components/Pages` - route pages
- `PrelineBlazorApp/Components/UI` - legacy wrappers and shared UI pieces
- `PrelineBlazorApp/Components/UI/Kit` - new reusable typed UI kit
- `PrelineBlazorApp/Components/Charts` - Highcharts Blazor component
- `PrelineBlazorApp/Models` - dashboard/theme/ui models
- `PrelineBlazorApp/Services` - navigation, theme, and mock dashboard data services
- `PrelineBlazorApp/wwwroot/css` - Tailwind source (`site.css`) and output (`output.css`)
- `PrelineBlazorApp/wwwroot/js` - dashboard/theme/chart/ui interop

## Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js (npm)

### Install Dependencies

```bash
cd PrelineBlazorApp
npm install
```

### Build Frontend Assets

```bash
npm run build:assets
```

### Run the App

```bash
dotnet run --project PrelineBlazorApp.csproj
```

### Development Scripts

From `PrelineBlazorApp`:

- `npm run build:css` - compile Tailwind once
- `npm run watch:css` - watch Tailwind changes
- `npm run copy:vendor` - copy Preline/Highcharts vendor JS into `wwwroot/js/vendor`
- `npm run build:assets` - copy vendor JS + build CSS

### Notes

- This pass focuses on frontend architecture and UX.
- Authentication, authorization, and backend API integration are intentionally out of scope.
- Data is currently mocked and ready to be swapped with real APIs later.

