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
    // Current field names: firstName, lastName, email, password
    await expect(page.locator('input[name="firstName"]')).toBeVisible({ timeout: 10000 });
    await expect(page.locator('input[name="lastName"]')).toBeVisible();
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

  test('4. Favoritos sin auth → redirect a SignIn', async ({ page }) => {
    const response = await page.goto(`${BASE_URL}/Front/GuestFavorites`, { waitUntil: 'networkidle' });
    // Cookie auth redirects to SignIn with ReturnUrl (200 on final page)
    await expect(page).toHaveURL(/.*SignIn\?ReturnUrl=.*GuestFavorites/);
  });

  test('5. PropertyDetails sin auth → redirect a SignIn', async ({ page }) => {
    const response = await page.goto(`${BASE_URL}/Front/PropertyDetails/${PROPERTY_ID}`, { waitUntil: 'networkidle' });
    // Cookie auth redirects to SignIn with ReturnUrl (200 on final page)
    await expect(page).toHaveURL(/.*SignIn\?ReturnUrl=.*PropertyDetails/);
  });
});