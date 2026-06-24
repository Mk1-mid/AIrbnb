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
        "primary-fixed-dim": "#bec6e0",
        background: "#f8f9ff",
        primary: "#000000",
        "primary-container": "#131b2e",
        "surface-container-high": "#dce9ff",
        "on-tertiary-fixed": "#00201e",
        "on-primary-fixed-variant": "#3f465c",
        "surface-container-low": "#eff4ff",
        "error-container": "#ffdad6",
        "outline-variant": "#c6c6cd",
        "on-tertiary-fixed-variant": "#00504d",
        "on-primary-container": "#7c839b",
        "secondary-fixed": "#d5e3fd",
        error: "#ba1a1a",
        "inverse-primary": "#bec6e0",
        "surface-container-highest": "#d3e4fe",
        "surface-variant": "#d3e4fe",
        "on-surface": "#0b1c30",
        "tertiary-fixed-dim": "#66d8d2",
        "on-tertiary": "#ffffff",
        "on-secondary-container": "#57657b",
        "surface-container": "#e5eeff",
        "primary-fixed": "#dae2fd",
        "on-error": "#ffffff",
        "on-surface-variant": "#45464d",
        "on-secondary-fixed": "#0d1c2f",
        "on-primary": "#ffffff",
        "tertiary-container": "#00201e",
        "on-secondary-fixed-variant": "#3a485c",
        surface: "#f8f9ff",
        tertiary: "#000000",
        "surface-dim": "#cbdbf5",
        "surface-tint": "#565e74",
        "inverse-surface": "#213145",
        secondary: "#515f74",
        "on-secondary": "#ffffff",
        "on-error-container": "#93000a",
        "secondary-container": "#d5e3fd",
        "surface-bright": "#f8f9ff",
        "inverse-on-surface": "#eaf1ff",
        "tertiary-fixed": "#84f5ee",
        outline: "#76777d",
        "surface-container-lowest": "#ffffff",
        "on-background": "#0b1c30",
        "secondary-fixed-dim": "#b9c7e0",
        "on-tertiary-container": "#00938e",
        "on-primary-fixed": "#131b2e",
        "on-secondary-container": "#57657b",
        "surface-container-low": "#eff4ff"
      },
      borderRadius: {
        DEFAULT: "0.25rem",
        lg: "0.5rem",
        xl: "0.75rem",
        full: "9999px"
      },
      spacing: {
        "margin-desktop": "40px",
        "stack-md": "16px",
        "stack-lg": "32px",
        "stack-sm": "8px",
        gutter: "24px",
        "margin-mobile": "16px",
        "container-max": "1280px",
        base: "4px"
      },
      fontFamily: {
        "label-sm": ["Geist", "sans-serif"],
        "display-lg": ["Geist", "sans-serif"],
        "headline-md": ["Geist", "sans-serif"],
        "headline-lg": ["Geist", "sans-serif"],
        "label-md": ["Geist", "sans-serif"],
        "headline-lg-mobile": ["Geist", "sans-serif"],
        "body-lg": ["Inter", "sans-serif"],
        "body-sm": ["Inter", "sans-serif"],
        "body-md": ["Inter", "sans-serif"]
      },
      fontSize: {
        "label-sm": ["12px", { lineHeight: "1", fontWeight: "600" }],
        "display-lg": ["48px", { lineHeight: "1.1", letterSpacing: "-0.02em", fontWeight: "700" }],
        "headline-md": ["24px", { lineHeight: "1.3", fontWeight: "600" }],
        "headline-lg": ["32px", { lineHeight: "1.2", letterSpacing: "-0.01em", fontWeight: "600" }],
        "label-md": ["14px", { lineHeight: "1", letterSpacing: "0.01em", fontWeight: "500" }],
        "headline-lg-mobile": ["24px", { lineHeight: "1.2", fontWeight: "600" }],
        "body-lg": ["18px", { lineHeight: "1.6", fontWeight: "400" }],
        "body-sm": ["14px", { lineHeight: "1.5", fontWeight: "400" }],
        "body-md": ["16px", { lineHeight: "1.5", fontWeight: "400" }]
      }
    }
  },
  plugins: ["forms", "container-queries"]
}
