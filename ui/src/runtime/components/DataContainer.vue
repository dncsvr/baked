<template>
  <div class="p-panel">
    <div class="p-panel-header flex gap-2 text-xs items-center justify-end">
      <Inputs
        v-if="inputs.length > 0"
        :inputs="inputs"
        @ready="onReady"
        @changed="onChanged"
      />
    </div>
    <div class="p-panel-content [contain:inline-size]">
      <Bake
        v-if="loaded && ready"
        :key="uniqueKey"
        name="content"
        :descriptor="content"
      />
      <Message
        v-else-if="!ready"
        severity="info"
      >
        <i class="pi pi-info-circle" />
        <span class="ml-3">{{ lc("Select required values to view this data") }}</span>
      </Message>
    </div>
  </div>
</template>
<script setup>
import { ref } from "vue";
import { Message } from "primevue";
import { useContext, useLocalization } from "#imports";
import { Bake, Inputs } from "#components";

const context = useContext();
const { localize: lc } = useLocalization({ group: "DataContainer" });

const { schema } = defineProps({
  schema: { type: null, required: true }
});

const { content, inputs } = schema;

const contextData = context.injectContextData();

const loaded = ref(true);
const ready = ref(inputs.length === 0); // it is ready when there is no parameter
const uniqueKey = ref("");

const values = ref({});
if(inputs.length > 0) {
  contextData.parent["parameters"] = values;
}

function onReady(value) {
  ready.value = value;
}

function onChanged(event) {
  uniqueKey.value = event.uniqueKey;
  values.value = event.values;
}
</script>
<style scoped>
div.p-panel {
  div.p-panel-header {
    @apply p-[1.125rem] bg-transparent border-0 rounded-none
  }

  div.p-panel-content {
    @apply p-[1.125rem] pt-0
  }

  @apply block border rounded bg-[var(--p-panel-background)] text-[var(--p-panel-color)]
}
</style>
