<template>
  <component
    :is="labelComponent"
    :dt
    :pt
    :variant
  >
    <template #default>
      <slot />
      <label
        v-if="mode !== 'none'"
        :for="path"
      >
        {{ label }}
      </label>
    </template>
  </component>
</template>

<script setup>
import { computed } from "vue";
import { FloatLabel, IftaLabel } from "primevue";

const { label, mode } = defineProps({
  label: { type: String, required: true },
  path: { type: String, required: true },
  mode: { type: String, default: "float" },
  variant: { type: String, default: "on" },
  pt: { type: Object, default: () => { } },
  dt: { type: Object, default: () => { } }
});

const labelComponent = computed(() => {
  if(!label) { return "div"; }

  switch (mode) {
  case "none":
    return "div";
  case "ifta":
    return IftaLabel;
  case "float":
  default:
    return FloatLabel;
  }
});
</script>