/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: "class",
  content: [
    "src/RentalPlatform.Web/Pages/**/*.cshtml",
    "src/RentalPlatform.Web/Pages/Shared/**/*.cshtml",
    "src/RentalPlatform.Web/Pages/Front/**/*.cshtml",
    "src/RentalPlatform.Web/Views/**/*.cshtml"
  ],
  theme: {
    extend: {
      colors: {
        "inverse-on-surface": "#313030",
        "on-secondary-fixed-variant": "#39485a",
        "on-background": "#e5e2e1",
        "secondary-fixed": "#d4e4fa",
        "outline-variant": "#4d4635",
        "error": "#ffb4ab",
        "surface": "#131313",
        "error-container": "#93000a",
        "on-primary-fixed-variant": "#574500",
        "secondary-fixed-dim": "#b8c8de",
        "on-secondary": "#233242",
        "primary": "#f2ca50",
        "outline": "#99907c",
        "on-secondary-container": "#a7b6cc",
        "primary-fixed": "#ffe088",
        "on-surface": "#e5e2e1",
        "surface-variant": "#353534",
        "on-tertiary-fixed": "#151b2d",
        "primary-container": "#d4af37",
        "surface-container": "#201f1f",
        "inverse-primary": "#735c00",
        "on-tertiary-fixed-variant": "#40465a",
        "inverse-surface": "#e5e2e1",
        "surface-container-low": "#1c1b1b",
        "on-tertiary": "#2a3043",
        "tertiary-fixed": "#dce1fb",
        "surface-container-lowest": "#0e0e0e",
        "on-surface-variant": "#d0c5af",
        "on-primary-fixed": "#241a00",
        "surface-container-high": "#2a2a2a",
        "tertiary-container": "#adb2ca",
        "on-secondary-fixed": "#0d1d2d",
        "on-error": "#690005",
        "tertiary-fixed-dim": "#c0c6de",
        "on-primary": "#3c2f00",
        "surface-container-highest": "#353534",
        "on-primary-container": "#554300",
        "on-error-container": "#ffdad6",
        "tertiary": "#c8cee6",
        "surface-tint": "#e9c349",
        "surface-dim": "#131313",
        "on-tertiary-container": "#3f4459",
        "primary-fixed-dim": "#e9c349",
        "background": "#131313",
        "secondary-container": "#39485a",
        "secondary": "#b8c8de",
        "surface-bright": "#3a3939"
      },
      borderRadius: {
        DEFAULT: "0.125rem",
        lg: "0.25rem",
        xl: "0.5rem",
        full: "0.75rem"
      },
      spacing: {
        xs: "4px",
        sm: "8px",
        md: "16px",
        "margin-mobile": "16px",
        lg: "24px",
        "2xl": "64px",
        base: "4px",
        "margin-desktop": "48px",
        xl: "40px",
        gutter: "24px"
      },
      fontFamily: {
        "body-md": ["DM Sans", "sans-serif"],
        "body-lg": ["DM Sans", "sans-serif"],
        "label-sm": ["DM Sans", "sans-serif"],
        "label-md": ["DM Sans", "sans-serif"],
        "headline-sm": ["Playfair Display", "serif"],
        "headline-md": ["Playfair Display", "serif"],
        "display-lg-mobile": ["Playfair Display", "serif"],
        "display-lg": ["Playfair Display", "serif"]
      },
      fontSize: {
        "body-md": ["16px", { lineHeight: "1.6", letterSpacing: "0.01em", fontWeight: "400" }],
        "body-lg": ["18px", { lineHeight: "1.6", letterSpacing: "0.01em", fontWeight: "400" }],
        "label-sm": ["12px", { lineHeight: "1.2", letterSpacing: "0.08em", fontWeight: "600" }],
        "label-md": ["14px", { lineHeight: "1.4", letterSpacing: "0.05em", fontWeight: "500" }],
        "headline-sm": ["24px", { lineHeight: "1.3", fontWeight: "600" }],
        "headline-md": ["32px", { lineHeight: "1.2", fontWeight: "600" }],
        "display-lg-mobile": ["32px", { lineHeight: "1.2", fontWeight: "700" }],
        "display-lg": ["48px", { lineHeight: "1.1", letterSpacing: "-0.02em", fontWeight: "700" }]
      }
    }
  },
  plugins: ["forms", "container-queries"]
}
