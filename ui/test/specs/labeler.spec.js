import { expect, test } from "@nuxt/test-utils/playwright";
import primevue from "../utils/locators/primevue";

test.beforeEach(async({ goto }) => {
  await goto("/specs/labeler", { waitUntil: "hydration" });
});

test.describe("Select Input Label Mode", () => {
  [
    { id: "Select: No Label Mode", expected: null },
    { id: "Select: Float:In Mode", expected: "Float:In Mode" },
    { id: "Select: Ifta Mode", expected: "Ifta Mode" },
    { id: "Select: Float:On Mode", expected: "Float:On Mode" }
  ].forEach(({ id, expected }) => {
    test(`testing correct label text ${expected}`, async({ page }) => {
      if(expected === null ) { return; }

      const component = page.getByTestId(id);
      const options = page.locator(primevue.select.option);
      await expect(component.locator(primevue.select.base)).toBeAttached();
      await component.click();
      await options.nth(1).click();
      await page.waitForTimeout(100); // waits for animation to finish

      await expect(component.getByText(expected)).toBeVisible();
    });

    test(`visual ${expected}`, { tag: "@visual" }, async({ page }) => {
      const component = page.getByTestId(id);
      const options = page.locator(primevue.select.option);
      await expect(component.locator(primevue.select.base)).toBeAttached();
      await component.click();
      await options.nth(1).click();
      await page.waitForTimeout(100); // waits for animation to finish

      await expect(component.locator(primevue.select.label)).toHaveScreenshot();
    });
  });
});

test.describe("SelectButton Input Label Mode", () => {
  [
    { id: "Select: No Label Mode", expected: null },
    { id: "SelectButton: Ifta Mode", expected: "Ifta Mode" }
  ].forEach(({ id, expected }) => {
    test(`testing correct label text ${expected}`, async({ page }) => {
      if(expected === null ) { return; }

      const component = page.getByTestId(id);
  
      await expect(component.getByText(expected)).toBeVisible();
    });
  
    test(`visual ${expected}`, { tag: "@visual" }, async({ page }) => {
      const component = page.getByTestId(id);
  
      await expect(component).toHaveScreenshot();
    });
  });
});