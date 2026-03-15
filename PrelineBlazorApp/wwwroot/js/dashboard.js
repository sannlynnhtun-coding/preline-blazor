(() => {
  const STORAGE_KEY_V1 = "pb.admin.theme.v1";
  const STORAGE_KEY_V2 = "pb.admin.theme.v2";

  const palettes = {
    slate: { accent: "#334155" },
    sky: { accent: "#0369a1" },
    emerald: { accent: "#047857" },
    amber: { accent: "#b45309" },
    rose: { accent: "#be123c" }
  };

  const defaultPreference = {
    mode: "system",
    defaultPalette: "slate",
    defaultCustomAccent: null,
    pagePaletteOverrides: {},
    pageCustomAccentOverrides: {}
  };

  function deepClone(value) {
    if (typeof structuredClone === "function") {
      return structuredClone(value);
    }

    return JSON.parse(JSON.stringify(value));
  }

  function normalizeRoute(route) {
    if (!route) return "/dashboard";
    const path = route.split("?")[0].split("#")[0].trim();
    if (!path || path === "/") return "/dashboard";
    return path.startsWith("/") ? path : `/${path}`;
  }

  function parseColor(color) {
    if (!color) return null;
    const input = String(color).trim();

    if (input.startsWith("#")) {
      const hex = input.slice(1);
      if (hex.length === 3) {
        const r = parseInt(hex[0] + hex[0], 16);
        const g = parseInt(hex[1] + hex[1], 16);
        const b = parseInt(hex[2] + hex[2], 16);
        return { r, g, b };
      }

      if (hex.length >= 6) {
        const r = parseInt(hex.slice(0, 2), 16);
        const g = parseInt(hex.slice(2, 4), 16);
        const b = parseInt(hex.slice(4, 6), 16);
        return { r, g, b };
      }
    }

    const rgbMatch = input.match(/rgba?\(([^)]+)\)/i);
    if (rgbMatch) {
      const parts = rgbMatch[1].split(",").map((x) => Number.parseFloat(x.trim()));
      if (parts.length >= 3) {
        return {
          r: Math.max(0, Math.min(255, Math.round(parts[0]))),
          g: Math.max(0, Math.min(255, Math.round(parts[1]))),
          b: Math.max(0, Math.min(255, Math.round(parts[2])))
        };
      }
    }

    return null;
  }

  function rgbToHex(rgb, fallback = "#334155") {
    if (!rgb) return fallback;

    const toHex = (value) => value.toString(16).padStart(2, "0");
    return `#${toHex(rgb.r)}${toHex(rgb.g)}${toHex(rgb.b)}`;
  }

  function normalizeHexColor(value) {
    if (!value) return null;
    const input = String(value).trim();
    if (!input) return null;

    const withHash = input.startsWith("#") ? input : `#${input}`;
    if (withHash.length === 4) {
      const expanded = `#${withHash[1]}${withHash[1]}${withHash[2]}${withHash[2]}${withHash[3]}${withHash[3]}`;
      return /^[#][0-9a-fA-F]{6}$/.test(expanded) ? expanded.toLowerCase() : null;
    }

    return /^[#][0-9a-fA-F]{6}$/.test(withHash) ? withHash.toLowerCase() : null;
  }

  function withAlpha(color, alpha, fallback = "rgba(51, 65, 85, 0.2)") {
    const rgb = parseColor(color);
    if (!rgb) return fallback;
    return `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, ${alpha})`;
  }

  function mixColors(colorA, colorB, ratio, fallback = "#334155") {
    const a = parseColor(colorA);
    const b = parseColor(colorB);
    if (!a || !b) return fallback;

    const clamped = Math.max(0, Math.min(1, ratio));
    const r = Math.round(a.r + (b.r - a.r) * clamped);
    const g = Math.round(a.g + (b.g - a.g) * clamped);
    const bValue = Math.round(a.b + (b.b - a.b) * clamped);

    return rgbToHex({ r, g, b: bValue }, fallback);
  }

  function getLuminance(color) {
    const rgb = parseColor(color);
    if (!rgb) return 0;

    const linearize = (channel) => {
      const c = channel / 255;
      return c <= 0.03928 ? c / 12.92 : ((c + 0.055) / 1.055) ** 2.4;
    };

    const r = linearize(rgb.r);
    const g = linearize(rgb.g);
    const b = linearize(rgb.b);
    return (0.2126 * r) + (0.7152 * g) + (0.0722 * b);
  }

  function getContrastRatio(foreground, background) {
    const l1 = getLuminance(foreground);
    const l2 = getLuminance(background);
    const lighter = Math.max(l1, l2);
    const darker = Math.min(l1, l2);
    return (lighter + 0.05) / (darker + 0.05);
  }

  function pickOnAccent(accent) {
    const darkText = "#0f172a";
    const lightText = "#ffffff";
    const darkRatio = getContrastRatio(darkText, accent);
    const lightRatio = getContrastRatio(lightText, accent);
    return darkRatio >= lightRatio ? darkText : lightText;
  }

  function sanitizePaletteOverrides(value) {
    if (!value || typeof value !== "object") {
      return {};
    }

    const result = {};
    Object.entries(value).forEach(([route, palette]) => {
      if (!route || !palettes[palette]) return;
      result[normalizeRoute(route)] = palette;
    });

    return result;
  }

  function sanitizeColorOverrides(value) {
    if (!value || typeof value !== "object") {
      return {};
    }

    const result = {};
    Object.entries(value).forEach(([route, color]) => {
      const normalized = normalizeHexColor(color);
      if (!route || !normalized) return;
      result[normalizeRoute(route)] = normalized;
    });

    return result;
  }

  function normalizePreference(raw) {
    const preference = deepClone(defaultPreference);
    const source = raw && typeof raw === "object" ? raw : {};

    preference.mode = ["system", "light", "dark"].includes(source.mode) ? source.mode : "system";
    preference.defaultPalette = palettes[source.defaultPalette] ? source.defaultPalette : "slate";
    preference.defaultCustomAccent = normalizeHexColor(source.defaultCustomAccent);
    preference.pagePaletteOverrides = sanitizePaletteOverrides(source.pagePaletteOverrides);
    preference.pageCustomAccentOverrides = sanitizeColorOverrides(source.pageCustomAccentOverrides);

    return preference;
  }

  function savePreference(preference) {
    window.localStorage.setItem(STORAGE_KEY_V2, JSON.stringify(preference));
  }

  function loadPreference() {
    try {
      const rawV2 = window.localStorage.getItem(STORAGE_KEY_V2);
      if (rawV2) {
        return normalizePreference(JSON.parse(rawV2));
      }

      const rawV1 = window.localStorage.getItem(STORAGE_KEY_V1);
      if (rawV1) {
        const migrated = normalizePreference(JSON.parse(rawV1));
        savePreference(migrated);
        return migrated;
      }

      return deepClone(defaultPreference);
    } catch {
      return deepClone(defaultPreference);
    }
  }

  function applyMode(mode) {
    const root = document.documentElement;
    const shouldDark = mode === "dark" || (mode === "system" && window.matchMedia("(prefers-color-scheme: dark)").matches);

    root.classList.toggle("dark", shouldDark);
    root.dataset.mode = mode;
    root.dataset.effectiveMode = shouldDark ? "dark" : "light";

    return shouldDark;
  }

  function resolveTheme(preference, route) {
    const normalized = normalizeRoute(route);

    const pageCustomAccent = normalizeHexColor(preference.pageCustomAccentOverrides?.[normalized]);
    if (pageCustomAccent) {
      return {
        route: normalized,
        accent: pageCustomAccent,
        palette: preference.pagePaletteOverrides?.[normalized] || null,
        source: "page-custom"
      };
    }

    const pagePalette = preference.pagePaletteOverrides?.[normalized];
    if (pagePalette && palettes[pagePalette]) {
      return {
        route: normalized,
        accent: palettes[pagePalette].accent,
        palette: pagePalette,
        source: "page-palette"
      };
    }

    const defaultCustom = normalizeHexColor(preference.defaultCustomAccent);
    if (defaultCustom) {
      return {
        route: normalized,
        accent: defaultCustom,
        palette: preference.defaultPalette || "slate",
        source: "global-custom"
      };
    }

    const defaultPalette = palettes[preference.defaultPalette] ? preference.defaultPalette : "slate";
    return {
      route: normalized,
      accent: palettes[defaultPalette].accent,
      palette: defaultPalette,
      source: "global-palette"
    };
  }

  function applyResolvedTheme(resolvedTheme) {
    const root = document.documentElement;
    const isDark = root.classList.contains("dark");
    const styles = getComputedStyle(root);
    const surface = rgbToHex(parseColor(styles.getPropertyValue("--surface")) || parseColor(isDark ? "#0f172a" : "#ffffff"));

    const accent = normalizeHexColor(resolvedTheme.accent) || palettes.slate.accent;
    const accentSoft = mixColors(accent, surface, isDark ? 0.78 : 0.88, isDark ? "#1f2937" : "#e2e8f0");
    const accentSoftStrong = mixColors(accent, surface, isDark ? 0.64 : 0.76, isDark ? "#334155" : "#cbd5e1");
    const onAccent = pickOnAccent(accent);

    root.style.setProperty("--theme-accent", accent);
    root.style.setProperty("--theme-accent-soft", accentSoft);
    root.style.setProperty("--theme-accent-soft-strong", accentSoftStrong);
    root.style.setProperty("--theme-on-accent", onAccent);

    root.dataset.palette = resolvedTheme.palette || "";
    root.dataset.themeSource = resolvedTheme.source;
    root.dataset.themeAccent = accent;

    return {
      ...resolvedTheme,
      accent,
      accentSoft,
      onAccent,
      isDark
    };
  }

  function notifyThemeChanged(route, resolvedTheme) {
    if (window.dashboardCharts && typeof window.dashboardCharts.applyThemeToAll === "function") {
      window.dashboardCharts.applyThemeToAll();
    }

    window.dispatchEvent(new CustomEvent("dashboard:theme-changed", {
      detail: {
        route: normalizeRoute(route || window.location.pathname),
        mode: document.documentElement.dataset.mode,
        effectiveMode: document.documentElement.dataset.effectiveMode,
        palette: resolvedTheme?.palette || document.documentElement.dataset.palette || "",
        accent: resolvedTheme?.accent || document.documentElement.dataset.themeAccent || "",
        source: resolvedTheme?.source || document.documentElement.dataset.themeSource || ""
      }
    }));
  }

  function applyPreference(preference, route) {
    applyMode(preference.mode);
    const resolved = resolveTheme(preference, route);
    const applied = applyResolvedTheme(resolved);
    notifyThemeChanged(route, applied);
    return applied;
  }

  window.dashboardTheme = {
    getPreference() {
      return loadPreference();
    },

    setMode(mode) {
      const preference = loadPreference();
      preference.mode = ["system", "light", "dark"].includes(mode) ? mode : "system";
      savePreference(preference);
      applyPreference(preference, window.location.pathname);
    },

    setDefaultPalette(palette) {
      const preference = loadPreference();
      preference.defaultPalette = palettes[palette] ? palette : "slate";
      savePreference(preference);
      applyPreference(preference, window.location.pathname);
    },

    setDefaultCustomAccent(hexColor) {
      const preference = loadPreference();
      preference.defaultCustomAccent = normalizeHexColor(hexColor);
      savePreference(preference);
      applyPreference(preference, window.location.pathname);
    },

    setPagePalette(route, palette) {
      const preference = loadPreference();
      const normalized = normalizeRoute(route);

      if (!palette || !palettes[palette]) {
        delete preference.pagePaletteOverrides[normalized];
      } else {
        preference.pagePaletteOverrides[normalized] = palette;
      }

      savePreference(preference);
      applyPreference(preference, normalized);
    },

    setPageCustomAccent(route, hexColor) {
      const preference = loadPreference();
      const normalized = normalizeRoute(route);
      const normalizedColor = normalizeHexColor(hexColor);

      if (!normalizedColor) {
        delete preference.pageCustomAccentOverrides[normalized];
      } else {
        preference.pageCustomAccentOverrides[normalized] = normalizedColor;
      }

      savePreference(preference);
      applyPreference(preference, normalized);
    },

    apply(route) {
      const preference = loadPreference();
      applyPreference(preference, route);
    }
  };

  window.dashboardUi = {
    reinitialize() {
      if (window.HSStaticMethods && typeof window.HSStaticMethods.autoInit === "function") {
        window.HSStaticMethods.autoInit();
      }
    }
  };

  const chartRegistry = new Map();

  function parseOptions(optionsJson) {
    if (!optionsJson) return {};

    try {
      return JSON.parse(optionsJson);
    } catch {
      return {};
    }
  }

  function buildDerivedPalette(accent, isDark) {
    const lightTarget = "#ffffff";
    const darkTarget = "#0f172a";

    return [
      accent,
      mixColors(accent, isDark ? lightTarget : darkTarget, isDark ? 0.22 : 0.16, accent),
      mixColors(accent, isDark ? darkTarget : lightTarget, isDark ? 0.15 : 0.28, accent),
      mixColors(accent, lightTarget, 0.36, accent),
      mixColors(accent, darkTarget, 0.35, accent),
      mixColors(accent, isDark ? lightTarget : darkTarget, isDark ? 0.48 : 0.46, accent)
    ];
  }

  function getThemeTokens() {
    const root = document.documentElement;
    const styles = getComputedStyle(root);

    const accent = rgbToHex(parseColor(styles.getPropertyValue("--theme-accent")) || parseColor("#334155"));
    const accentSoft = rgbToHex(parseColor(styles.getPropertyValue("--theme-accent-soft")) || parseColor("#e2e8f0"));
    const textStrong = rgbToHex(parseColor(styles.getPropertyValue("--text-strong")) || parseColor("#0f172a"));
    const textMuted = rgbToHex(parseColor(styles.getPropertyValue("--text-muted")) || parseColor("#64748b"));
    const border = rgbToHex(parseColor(styles.getPropertyValue("--border-color")) || parseColor("#e2e8f0"));
    const surface = rgbToHex(parseColor(styles.getPropertyValue("--surface")) || parseColor("#ffffff"));

    const isDark = root.classList.contains("dark");
    const series = buildDerivedPalette(accent, isDark);

    return {
      isDark,
      accent,
      accentSoft,
      textStrong,
      textMuted,
      border,
      surface,
      grid: mixColors(border, isDark ? "#000000" : "#ffffff", isDark ? 0.18 : 0.24, border),
      series
    };
  }

  function applyAxisTheme(axisConfig, tokens, isYAxis = false) {
    if (!axisConfig) {
      return;
    }

    const targets = Array.isArray(axisConfig) ? axisConfig : [axisConfig];
    targets.forEach((axis) => {
      axis.lineColor = tokens.border;
      axis.tickColor = tokens.border;
      if (isYAxis || axis.gridLineColor !== undefined) {
        axis.gridLineColor = tokens.grid;
      }

      axis.labels = axis.labels || {};
      axis.labels.style = {
        ...(axis.labels.style || {}),
        color: tokens.textMuted
      };

      axis.title = axis.title || {};
      axis.title.style = {
        ...(axis.title.style || {}),
        color: tokens.textMuted
      };
    });
  }

  function normalizeSeriesType(series, options, chartKind) {
    return String(series?.type || options?.chart?.type || chartKind || "").toLowerCase();
  }

  function stylePieData(series, colors) {
    if (!Array.isArray(series.data)) {
      return;
    }

    series.data = series.data.map((point, index) => {
      const color = colors[index % colors.length];
      if (Array.isArray(point)) {
        return {
          name: point[0],
          y: point[1],
          color
        };
      }

      if (point && typeof point === "object") {
        return {
          ...point,
          color
        };
      }

      return { y: point, color };
    });
  }

  function applySeriesTheme(series, index, options, chartKind, tokens) {
    const type = normalizeSeriesType(series, options, chartKind);
    const color = tokens.series[index % tokens.series.length];

    series.color = color;

    if (type.includes("pie")) {
      series.colors = tokens.series;
      stylePieData(series, tokens.series);
      return;
    }

    if (type.includes("area")) {
      series.fillColor = series.fillColor || withAlpha(color, 0.18);
      series.marker = {
        ...(series.marker || {}),
        lineColor: color
      };
      return;
    }

    if (type.includes("column") || type.includes("bar")) {
      series.borderColor = withAlpha(color, 0.8);
      return;
    }
  }

  function applyThemeToOptions(baseOptions, chartKind) {
    const options = deepClone(baseOptions || {});
    const tokens = getThemeTokens();

    options.chart = {
      ...(options.chart || {}),
      backgroundColor: "transparent",
      style: {
        ...(options.chart?.style || {}),
        color: tokens.textStrong
      }
    };

    options.colors = tokens.series;

    options.title = {
      ...(options.title || {}),
      style: {
        ...(options.title?.style || {}),
        color: tokens.textStrong
      }
    };

    options.subtitle = {
      ...(options.subtitle || {}),
      style: {
        ...(options.subtitle?.style || {}),
        color: tokens.textMuted
      }
    };

    options.legend = {
      ...(options.legend || {}),
      itemStyle: {
        ...(options.legend?.itemStyle || {}),
        color: tokens.textStrong
      },
      itemHoverStyle: {
        ...(options.legend?.itemHoverStyle || {}),
        color: tokens.accent
      }
    };

    options.tooltip = {
      ...(options.tooltip || {}),
      backgroundColor: withAlpha(tokens.surface, 0.96),
      borderColor: tokens.border,
      style: {
        ...(options.tooltip?.style || {}),
        color: tokens.textStrong
      }
    };

    applyAxisTheme(options.xAxis, tokens, false);
    applyAxisTheme(options.yAxis, tokens, true);

    options.plotOptions = {
      ...(options.plotOptions || {}),
      pie: {
        ...(options.plotOptions?.pie || {}),
        dataLabels: {
          ...(options.plotOptions?.pie?.dataLabels || {}),
          style: {
            ...(options.plotOptions?.pie?.dataLabels?.style || {}),
            color: tokens.textStrong
          }
        }
      }
    };

    if (Array.isArray(options.series)) {
      options.series.forEach((series, index) => applySeriesTheme(series, index, options, chartKind, tokens));
    }

    return options;
  }

  function attachPointCallback(options, dotNetRef) {
    if (!dotNetRef) return;

    options.plotOptions = options.plotOptions || {};
    options.plotOptions.series = options.plotOptions.series || {};
    options.plotOptions.series.point = options.plotOptions.series.point || {};
    options.plotOptions.series.point.events = options.plotOptions.series.point.events || {};

    options.plotOptions.series.point.events.click = function () {
      const payload = JSON.stringify({
        x: this.x,
        y: this.y,
        name: this.name,
        series: this.series?.name
      });
      dotNetRef.invokeMethodAsync("HandlePointClick", payload);
    };
  }

  function updateChartTheme(entry) {
    const themedOptions = applyThemeToOptions(entry.baseOptions, entry.chartKind);
    entry.chart.update(themedOptions, true, true);
  }

  window.dashboardCharts = {
    renderOrUpdate(chartId, chartKind, optionsJson, dotNetRef) {
      if (!window.Highcharts) return;

      const baseOptions = parseOptions(optionsJson);
      const themedOptions = applyThemeToOptions(baseOptions, chartKind);
      attachPointCallback(themedOptions, dotNetRef);

      const existingEntry = chartRegistry.get(chartId);
      if (existingEntry) {
        existingEntry.baseOptions = deepClone(baseOptions);
        existingEntry.chartKind = chartKind;
        existingEntry.chart.update(themedOptions, true, true);
        return;
      }

      const chart = window.Highcharts.chart(chartId, themedOptions);
      chartRegistry.set(chartId, {
        chart,
        baseOptions: deepClone(baseOptions),
        chartKind
      });
    },

    applyTheme(chartId) {
      if (chartId) {
        const entry = chartRegistry.get(chartId);
        if (!entry) return;
        updateChartTheme(entry);
        return;
      }

      chartRegistry.forEach((entry) => {
        updateChartTheme(entry);
      });
    },

    applyThemeToAll() {
      this.applyTheme();
    },

    destroy(chartId) {
      const entry = chartRegistry.get(chartId);
      if (!entry) return;
      entry.chart.destroy();
      chartRegistry.delete(chartId);
    },

    reflowAll() {
      chartRegistry.forEach((entry) => entry.chart.reflow());
    }
  };

  window.addEventListener("dashboard:theme-changed", () => {
    window.dashboardCharts.applyThemeToAll();
  });

  let resizeTimer;
  window.addEventListener("resize", () => {
    clearTimeout(resizeTimer);
    resizeTimer = setTimeout(() => {
      window.dashboardCharts.reflowAll();
    }, 120);
  });

  const darkQuery = window.matchMedia("(prefers-color-scheme: dark)");
  const handleSystemThemeChange = () => {
    const preference = loadPreference();
    if (preference.mode !== "system") {
      return;
    }

    applyPreference(preference, window.location.pathname);
  };

  if (typeof darkQuery.addEventListener === "function") {
    darkQuery.addEventListener("change", handleSystemThemeChange);
  } else if (typeof darkQuery.addListener === "function") {
    darkQuery.addListener(handleSystemThemeChange);
  }

  document.addEventListener("DOMContentLoaded", () => {
    window.dashboardTheme.apply(window.location.pathname);
    window.dashboardUi.reinitialize();
  });
})();
