---
name: Aura Rental Systems
colors:
  surface: '#f8f9ff'
  surface-dim: '#cbdbf5'
  surface-bright: '#f8f9ff'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#eff4ff'
  surface-container: '#e5eeff'
  surface-container-high: '#dce9ff'
  surface-container-highest: '#d3e4fe'
  on-surface: '#0b1c30'
  on-surface-variant: '#45464d'
  inverse-surface: '#213145'
  inverse-on-surface: '#eaf1ff'
  outline: '#76777d'
  outline-variant: '#c6c6cd'
  surface-tint: '#565e74'
  primary: '#000000'
  on-primary: '#ffffff'
  primary-container: '#131b2e'
  on-primary-container: '#7c839b'
  inverse-primary: '#bec6e0'
  secondary: '#515f74'
  on-secondary: '#ffffff'
  secondary-container: '#d5e3fd'
  on-secondary-container: '#57657b'
  tertiary: '#000000'
  on-tertiary: '#ffffff'
  tertiary-container: '#00201e'
  on-tertiary-container: '#00938e'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#dae2fd'
  primary-fixed-dim: '#bec6e0'
  on-primary-fixed: '#131b2e'
  on-primary-fixed-variant: '#3f465c'
  secondary-fixed: '#d5e3fd'
  secondary-fixed-dim: '#b9c7e0'
  on-secondary-fixed: '#0d1c2f'
  on-secondary-fixed-variant: '#3a485c'
  tertiary-fixed: '#84f5ee'
  tertiary-fixed-dim: '#66d8d2'
  on-tertiary-fixed: '#00201e'
  on-tertiary-fixed-variant: '#00504d'
  background: '#f8f9ff'
  on-background: '#0b1c30'
  surface-variant: '#d3e4fe'
typography:
  display-lg:
    fontFamily: Geist
    fontSize: 48px
    fontWeight: '700'
    lineHeight: '1.1'
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Geist
    fontSize: 32px
    fontWeight: '600'
    lineHeight: '1.2'
    letterSpacing: -0.01em
  headline-lg-mobile:
    fontFamily: Geist
    fontSize: 24px
    fontWeight: '600'
    lineHeight: '1.2'
  headline-md:
    fontFamily: Geist
    fontSize: 24px
    fontWeight: '600'
    lineHeight: '1.3'
  body-lg:
    fontFamily: Inter
    fontSize: 18px
    fontWeight: '400'
    lineHeight: '1.6'
  body-md:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '400'
    lineHeight: '1.5'
  body-sm:
    fontFamily: Inter
    fontSize: 14px
    fontWeight: '400'
    lineHeight: '1.5'
  label-md:
    fontFamily: Geist
    fontSize: 14px
    fontWeight: '500'
    lineHeight: '1'
    letterSpacing: 0.01em
  label-sm:
    fontFamily: Geist
    fontSize: 12px
    fontWeight: '600'
    lineHeight: '1'
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  base: 4px
  container-max: 1280px
  gutter: 24px
  margin-mobile: 16px
  margin-desktop: 40px
  stack-sm: 8px
  stack-md: 16px
  stack-lg: 32px
---

## Brand & Style

This design system is built for a high-end short-term rental ecosystem that prioritizes reliability over novelty. The brand personality is professional, architectural, and precise, targeting property managers and discerning guests who value clarity and efficiency.

The visual style follows a **Modern Corporate** aesthetic with a lean toward **Minimalism**. It avoids the vibrant, "vacation-mode" tropes of typical travel platforms in favor of a utilitarian, data-first approach. The interface relies on generous white space, rigorous alignment, and a sophisticated monochromatic foundation to establish an atmosphere of institutional trust and operational excellence.

## Colors

The palette is anchored in deep, authoritative blues and balanced by a systematic range of slate grays.

- **Primary (#0F172A):** Used for core branding, primary action buttons, and high-level navigation. It provides the "weight" necessary for a professional feel.
- **Secondary (#334155):** Utilized for sub-navigation and secondary interactive elements.
- **Accent/Tertiary (#38B2AC):** A muted teal used sparingly for success states or subtle highlights to prevent the UI from feeling strictly clinical.
- **Neutral (#64748B):** Reserved for supporting text, icons, and borders to maintain a soft but legible hierarchy.
- **Background:** Primarily white (#FFFFFF) with subtle off-white (#F8FAFC) sectioning to create clear visual containment without heavy lines.

## Typography

This design system employs a dual-sans-serif approach to maximize both technical precision and readability. 

**Geist** is used for headlines, labels, and numeric data to leverage its geometric, developer-centric clarity. **Inter** handles all body copy and long-form content, ensuring maximum legibility across all browser engines. 

Hierarchy is established through weight and subtle letter-spacing adjustments rather than drastic size changes. For data-rich dashboards, use `label-sm` for table headers and metadata descriptors to maintain a structured, organized appearance.

## Layout & Spacing

The layout utilizes a **Fixed Grid** model for desktop views to maintain a curated, professional density, transitioning to a fluid model for tablet and mobile devices.

- **Desktop (1280px+):** 12-column grid, 24px gutters, 40px outer margins.
- **Tablet (768px - 1279px):** 8-column grid, 16px gutters, 24px outer margins.
- **Mobile (Up to 767px):** 4-column grid, 12px gutters, 16px outer margins.

The spacing rhythm is strictly based on a **4px baseline**. Use `stack-md` (16px) for the majority of component internal spacing and `stack-lg` (32px) for vertical rhythm between distinct content blocks.

## Elevation & Depth

Visual hierarchy is achieved through **Tonal Layers** and **Low-Contrast Outlines**. Instead of heavy shadows, the design system uses soft, multi-layered "ambient" shadows to lift cards off the background without creating visual noise.

- **Level 0 (Base):** #F8FAFC or #FFFFFF background.
- **Level 1 (Cards/Surface):** White background with a 1px border (#E2E8F0) and a subtle shadow (Y: 1px, Blur: 3px, Color: rgba(15, 23, 42, 0.05)).
- **Level 2 (Popovers/Modals):** White background with a more pronounced shadow (Y: 4px, Blur: 12px, Color: rgba(15, 23, 42, 0.08)).

Interactive elements should use a "lift" effect on hover, where the shadow deepens slightly and the border color darkens to #CBD5E1.

## Shapes

The shape language is structured and "Rounded" (8px to 12px) to soften the professional aesthetic and make the platform feel accessible.

- **Standard Elements (Buttons, Inputs):** 8px (0.5rem).
- **Containers (Cards, Modals):** 12px (0.75rem).
- **Small Elements (Chips, Tags):** 4px (0.25rem).

Avoid full-pill shapes unless used for status indicators (e.g., "Available" badges). The consistent use of 8px-12px radii ensures the UI feels modern and cohesive without the playfulness of fully rounded corners.

## Components

- **Buttons:** Primary buttons use #0F172A background with white text and no border. Secondary buttons use a transparent background with a #E2E8F0 border and #334155 text. 
- **Input Fields:** Use a 1px border (#E2E8F0) with 8px rounding. Focus states should transition the border to #0F172A and add a 2px soft blue ring.
- **Professional Cards:** Cards are the primary vessel for listings. They must feature a 1px border, 12px rounding, and a subtle "Level 1" shadow. Content should be padded by 24px.
- **Data Dashboards:** Use condensed versions of Geist for numerical data. Tables should have horizontal borders only (#F1F5F9) to maintain a clean flow.
- **Navigation:** Top-tier navigation uses a clean horizontal bar with #0F172A for active states. Sidebar navigation for property managers should use subtle #F8FAFC hover states and 8px rounding on menu items.
- **Chips:** Used for property features (e.g., "WiFi", "Parking"). These should have a light gray background (#F1F5F9), 4px rounding, and `label-sm` typography.