import { expect, test } from "@nuxt/test-utils/playwright";
import giveMe from "../utils/giveMe";
import primevue from "../utils/locators/primevue";

test.beforeEach(async({ goto }) => {
  await goto("/specs/data-container", { waitUntil: "hydration" });
});

test.describe("Inputs", () => {
  const id = "Inputs";

  test("inputs rendered", async({ page }) => {
    const component = page.getByTestId(id);

    await expect(component.locator(primevue.select.base)).toBeVisible();
    await expect(component.locator(".b-component--Server-Paginator")).toBeVisible();
  });

  test("informs only when required inputs are not selected", async({ page }) => {
    const component = page.getByTestId(id);

    await expect(component.locator(primevue.message.base)).toHaveText("Select required values to view this data");

    await component.getByTestId("required").fill("any text");
    await expect(component.locator(primevue.message.base)).not.toBeAttached();
  });

  test("listens ready model", async({ page }) => {
    const component = page.getByTestId(id);
    const content = component.getByTestId("content");

    await expect(content).not.toBeVisible();
    await component.getByTestId("required").fill("any text");

    await expect(content).toBeVisible();
  });

  test("redraws when unique key changes", async({ page }) => {
    const component = page.getByTestId(id);

    await component.getByTestId("required").fill("value");

    await expect(component.getByTestId("content")).toHaveText(/value/);
  });

  test("visual for lg", { tag: "@visual" }, async({ page }) => {
    const component = page.getByTestId(id);
    const screen = giveMe.aScreenSize({ name: "lg" });

    await page.setViewportSize({ ...screen });

    await expect(component.getByTestId("required")).toBeVisible();
    await expect(component.getByTestId("optional")).toBeVisible();
    await expect(component).toHaveScreenshot();
  });

  test("visual for mobile", { tag: "@visual" }, async({ page }) => {
    const component = page.getByTestId(id);
    const screen = giveMe.aScreenSize({ name: "sm" });

    await page.setViewportSize({ ...screen });

    await expect(component).toHaveScreenshot();
  });

  test("visual for mobile opened", { tag: "@visual" }, async({ page }) => {
    const component = page.getByTestId(id);
    const screen = giveMe.aScreenSize({ name: "sm" });
    const content = page.locator(primevue.popover.content);
    await page.setViewportSize({ ...screen });

    await page.mouse.wheel(0, 500);
    const filterIcon = component.locator(primevue.button.icon).nth(0);
    await filterIcon.click();

    await expect(content).toHaveScreenshot();
  });

  test("popover visibility based on mobile screen size", async({ page }) => {
    const component = page.getByTestId(id);
    const screen = giveMe.aScreenSize({ name: "sm" });
    await page.setViewportSize({ ...screen });

    const filterIcon = component.locator(primevue.button.icon).nth(0);

    await expect(component.getByTestId("required")).not.toBeAttached();
    await expect(component.getByTestId("optional")).not.toBeAttached();
    await expect(filterIcon).toBeVisible();
  });

  test("popover functionality based on mobile screen size", async({ page }) => {
    const component = page.getByTestId(id);
    const screen = giveMe.aScreenSize({ name: "sm" });
    const content = page.locator(primevue.popover.content);
    await page.setViewportSize({ ...screen });

    const filterIcon = component.locator(primevue.button.icon).nth(0);
    await filterIcon.click();

    await expect(content.getByTestId("required")).toBeVisible();
    await expect(content.getByTestId("optional")).toBeVisible();
  });
});
