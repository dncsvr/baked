<template>
  <component
    :is="labelComponent"
    :pt="pt"
  >
    <template #default>
      <slot />
      <label :for="path">{{ label }}</label>
    </template>
  </component>
</template>

<script setup>
import { computed } from "vue";
import { FloatLabel, IftaLabel } from "primevue";

const { label, mode = "ifta" } = defineProps({
  label: { type: String, required: true },
  path: { type: String, required: true },
  mode: { type: String, required: false },
  pt: { type: Object, default: () => {} }
});

const labelComponent = computed(() => {
  switch (mode) {
  case "float":
    return FloatLabel;
  case "none":
    return "div";
  default:
    return IftaLabel;
  }
});
</script>