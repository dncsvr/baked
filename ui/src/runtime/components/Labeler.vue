<template>
  <component
    :is="labelComponent"
    :dt="labelDT"
    :pt
    :variant
    class="w-full"
  >
    <template #default>
      <slot />
      <label
        v-if="mode !== 'none'"
        class="max-sm:truncate max-sm:w-5/6"
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

const { label, mode, dt } = defineProps({
  label: { type: String, required: true },
  path: { type: String, required: true },
  mode: { type: String, default: "float" },
  variant: { type: String, default: "on" },
  pt: { type: Object, default: () => { } },
  dt: { type: Object, default: () => { } }
});

const labelDT = computed(() => {
  return Object.assign({
    colorScheme: {
      light: {
        transition: {
          duration: 0
        }
      },
      dark: {
        transition: {
          duration: 0
        }
      }
    }
  }, dt);
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