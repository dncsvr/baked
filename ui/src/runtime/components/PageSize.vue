<template>
  <Select
    v-model="model"
    :data
    :schema
    class="
      mr-2 shadow-none rounded-md 
      bg-slate-100 border-slate-100 
      dark:bg-zinc-950 dark:border-zinc-950"
  />
</template>
<script setup>
import { Select } from "#components";

const { schema, data } = defineProps({
  schema: { type: null, required: true },
  data: { type: null, required: true }
});
const model = defineModel({ type: null, required: true });
</script>
<style>
.b-component--PageSize {
  .p-select {
    @apply border-none
  }
  .p-select-label {
    font-size: inherit;
  }
}
</style>