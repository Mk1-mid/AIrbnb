---
name: Nocturne Elite
colors:
  surface: '#131313'
  surface-dim: '#131313'
  surface-bright: '#3a3939'
  surface-container-lowest: '#0e0e0e'
  surface-container-low: '#1c1b1b'
  surface-container: '#201f1f'
  surface-container-high: '#2a2a2a'
  surface-container-highest: '#353534'
  on-surface: '#e5e2e1'
  on-surface-variant: '#d0c5af'
  inverse-surface: '#e5e2e1'
  inverse-on-surface: '#313030'
  outline: '#99907c'
  outline-variant: '#4d4635'
  surface-tint: '#e9c349'
  primary: '#f2ca50'
  on-primary: '#3c2f00'
  primary-container: '#d4af37'
  on-primary-container: '#554300'
  inverse-primary: '#735c00'
  secondary: '#b8c8de'
  on-secondary: '#233242'
  secondary-container: '#39485a'
  on-secondary-container: '#a7b6cc'
  tertiary: '#c8cee6'
  on-tertiary: '#2a3043'
  tertiary-container: '#adb2ca'
  on-tertiary-container: '#3f4459'
  error: '#ffb4ab'
  on-error: '#690005'
  error-container: '#93000a'
  on-error-container: '#ffdad6'
  primary-fixed: '#ffe088'
  primary-fixed-dim: '#e9c349'
  on-primary-fixed: '#241a00'
  on-primary-fixed-variant: '#574500'
  secondary-fixed: '#d4e4fa'
  secondary-fixed-dim: '#b8c8de'
  on-secondary-fixed: '#0d1d2d'
  on-secondary-fixed-variant: '#39485a'
  tertiary-fixed: '#dce1fb'
  tertiary-fixed-dim: '#c0c6de'
  on-tertiary-fixed: '#151b2d'
  on-tertiary-fixed-variant: '#40465a'
  background: '#131313'
  on-background: '#e5e2e1'
  surface-variant: '#353534'
typography:
  display-lg:
    fontFamily: Playfair Display
    fontSize: 48px
    fontWeight: '700'
    lineHeight: '1.1'
    letterSpacing: -0.02em
  display-lg-mobile:
    fontFamily: Playfair Display
    fontSize: 32px
    fontWeight: '700'
    lineHeight: '1.2'
  headline-md:
    fontFamily: Playfair Display
    fontSize: 32px
    fontWeight: '600'
    lineHeight: '1.2'
  headline-sm:
    fontFamily: Playfair Display
    fontSize: 24px
    fontWeight: '600'
    lineHeight: '1.3'
  body-lg:
    fontFamily: DM Sans
    fontSize: 18px
    fontWeight: '400'
    lineHeight: '1.6'
    letterSpacing: 0.01em
  body-md:
    fontFamily: DM Sans
    fontSize: 16px
    fontWeight: '400'
    lineHeight: '1.6'
    letterSpacing: 0.01em
  label-md:
    fontFamily: DM Sans
    fontSize: 14px
    fontWeight: '500'
    lineHeight: '1.4'
    letterSpacing: 0.05em
  label-sm:
    fontFamily: DM Sans
    fontSize: 12px
    fontWeight: '600'
    lineHeight: '1.2'
    letterSpacing: 0.08em
rounded:
  sm: 0.125rem
  DEFAULT: 0.25rem
  md: 0.375rem
  lg: 0.5rem
  xl: 0.75rem
  full: 9999px
spacing:
  base: 4px
  xs: 4px
  sm: 8px
  md: 16px
  lg: 24px
  xl: 40px
  2xl: 64px
  gutter: 24px
  margin-mobile: 16px
  margin-desktop: 48px
---

## Brand & Style
The design system embodies "Atmospheric Opulence," a direction tailored for high-end hospitality management and guest experiences. The personality is exclusive, authoritative, and sophisticated, favoring the visual language of luxury editorial and boutique concierge services.

The style is a fusion of **Glassmorphism** and **High-Contrast Minimalism**. It utilizes deep, infinite black backgrounds contrasted against razor-sharp golden accents. Surface treatments rely on translucent layers and backdrop blurs to create a sense of physical depth and "midnight" atmosphere. Every interaction should feel intentional and weighty, evoking the feeling of a premium physical service.

## Colors
The palette is rooted in a "Darkest Hour" philosophy. The base surfaces use `#0A0A0A` and `#050505` to provide a true black foundation. Atmospheric gradients transition from the deep charcoal base to a `#020617` midnight blue to simulate natural, low-light environments.

**Primary Accent (Luxury Gold):** Used sparingly for critical focus points, active states, and premium borders. 
**Secondary (Midnight Indigo):** Employed for surface overlays, subtle glows behind floating containers, and secondary interactive states.
**Typography:** Primary information uses Stone-100 for high legibility against black, while metadata and secondary labels use Neutral-400 to maintain visual hierarchy and reduce eye strain.

## Typography
The typography strategy creates an "Editorial Luxury" feel. 
- **Headlines:** Use Playfair Display. The high-contrast serifs provide a classic, high-fashion aesthetic. These should be set with tight leading and slight negative tracking for a compact, authoritative look.
- **Body & Interface:** Use DM Sans. This geometric sans-serif ensures maximum legibility for dense data. Tracking is increased slightly (+1% to +2%) to enhance the premium, airy feel.
- **Labels:** Always utilize the uppercase variant with generous letter spacing to act as "architectural" markers within the UI.

## Layout & Spacing
The layout follows a **structured 12-column fixed grid** for desktop and a **4-column fluid grid** for mobile. Despite the luxury focus, the system maintains a "High-Value Density," ensuring that information-rich dashboards remain functional.

Spacing uses a strict 4px baseline. Large internal paddings (24px+) are reserved for container edges to give content room to breathe, while internal element spacing (e.g., label to input) is kept tight (8px) to maintain a sense of precision and professional density.

## Elevation & Depth
Elevation is not conveyed through traditional drop shadows but through **Tonal Opacity and Blurs**.

- **Level 0 (Base):** Deepest black (#050505).
- **Level 1 (Cards/Sections):** Midnight Indigo (#0B1B2B) with 40% opacity and a 20px backdrop-blur.
- **Level 2 (Modals/Overlays):** 60% opacity with a fine 1px golden border at 30% alpha.
- **Interactive States:** High-elevation elements utilize a "Golden Glow"—a soft, diffused outer shadow using the primary gold color at very low opacity (10-15%) to simulate a backlit effect.

## Shapes
The shape language is **Orthogonal and Sophisticated**. Rounded corners are kept to a minimum to maintain a sharp, architectural appearance.

- **Small Components (Buttons/Inputs):** 4px (rounded-sm) for a crisp, tailored look.
- **Large Components (Cards/Modals):** 8px (rounded-md) to provide a subtle soften to the "midnight" environment without appearing overly casual.
- **Accent Lines:** Use 1px ultra-fine horizontal and vertical rules for section separation rather than heavy boxes.

## Components
- **Buttons:** Primary buttons are solid Gold with black text. Secondary buttons are transparent with a 1px Gold border at 50% opacity. Tracking is increased on button labels.
- **Cards:** Feature `backdrop-blur-xl` and a 1px top-border (linear gradient from Gold to transparent) to catch the "light."
- **Input Fields:** Bottom-border only or very subtle ghost-borders. Focus state triggers a subtle Golden Glow and moves the label upwards in uppercase `label-sm`.
- **Chips/Status:** Use the Indigo secondary color as a base with Primary Gold text for active statuses.
- **Interactive Lists:** Hover states should trigger a 5% opacity Gold background fill and a sharp vertical Gold line on the left edge.
- **Luxury Accents:** Use thin dividers (0.5pt - 1pt) with 20% opacity to maintain structure without cluttering the dark workspace.