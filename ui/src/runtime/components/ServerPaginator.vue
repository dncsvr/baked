<template>
  <div class="flex items-center justify-center gap-1">
    <span class="whitespace-nowrap text-xs max-xs:hidden">{{ lc("Page {page}", { page }) }}</span>
    <Button
      rounded
      variant="text"
      icon="pi pi-chevron-left"
      :disabled="page <= 1"
      severity="secondary"
      size="small"
      @click="page--"
    />
    <Button
      rounded
      variant="text"
      icon="pi pi-chevron-right"
      severity="secondary"
      size="small"
      :disabled="!allowNext"
      @click="page++"
    />
  </div>
</template>

<script setup>
import { computed } from "vue";
import { Button } from "primevue";
import { useContext, useLocalization } from "#imports";

const context = useContext();
const { localize: lc } = useLocalization({ group: "ServerPaginator" });

const { data } = defineProps({
  data: { type: null, required: true }
});
const model = defineModel({ type: null, required: true });

const allowNext = computed(() => data.length >= data.take);
const page = computed({
  get: () => Number(model.value) / Number(data.take) + 1,
  set: value => {
    model.value = (value - 1) * Number(data.take);
  }
});

const path = context.injectPath();
const takeStateKey = path + ".take";
const contextData = context.injectContextData();
if(contextData.page[takeStateKey] !== data.take) {
  contextData.page[takeStateKey] = data.take;

  page.value = 1;
}

</script>
