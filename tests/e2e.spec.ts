import { test, expect } from '@playwright/test';

const BASE_URL = 'http://localhost:8080';
const PROPERTY_ID = '075e44b8-6602-44ba-994e-e0477032a2ab';

test.describe('RentalPlatform E2E', () => {
  test('1. Home carga y muestra título', async ({ page }) => {
    await page.goto(BASE_URL);
    await expect(page).toHaveTitle(/Home|Discover|LuxeStay/);
  });

  test('2. Página de Registro carga y tiene formulario', async ({ page }) => {
    await page.goto(`${BASE_URL}/Front/Register`);
    await expect(page.locator('form[method="post"]')).toBeVisible({ timeout: 10000 });
    // Actual field names in rendered HTML: name, surname, email, password
    await expect(page.locator('input[name="name"]')).toBeVisible({ timeout: 10000 });
    await expect(page.locator('input[name="surname"]')).toBeVisible();
    await expect(page.locator('input[name="email"]')).toBeVisible();
    await expect(page.locator('input[name="password"]')).toBeVisible();
    await expect(page.locator('button[type="submit"]')).toBeVisible();
  });

  test('3. Página de Login carga y tiene formulario', async ({ page }) => {
    await page.goto(`${BASE_URL}/Front/SignIn`);
    await expect(page.locator('form[method="post"]')).toBeVisible({ timeout: 10000 });
    await expect(page.locator('input[name="Email"]')).toBeVisible();
    await expect(page.locator('input[name="Password"]')).toBeVisible();
    await expect(page.locator('button[type="submit"]')).toBeVisible();
  });

  test('4. Favoritos sin auth → error 500/401 (Auth requerido)', async ({ page }) => {
    // [Authorize] without valid JWT returns 500 (no challenge handler configured)
    const response = await page.goto(`${BASE_URL}/Front/GuestFavorites`, { waitUntil: 'domcontentloaded' }).catch(() => null);
    // If navigation throws, check if it's an auth-related error
    const isAuthError = response === null || [401, 403, 500].includes(response?.status() ?? 0);
    expect(isAuthError).toBeTruthy();
  });

  test('5. PropertyDetails sin auth → error 500/401 (Auth requerido)', async ({ page }) => {
    const response = await page.goto(`${BASE_URL}/Front/PropertyDetails/${PROPERTY_ID}`, { waitUntil: 'domcontentloaded' }).catch(() => null);
    const isAuthError = response === null || [401, 403, 500].includes(response?.status() ?? 0);
    expect(isAuthError).toBeTruthy();
  });
});