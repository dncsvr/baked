<template>
  <Select
    v-model="model"
    :data
    :schema
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
}
</style>