<template>
  <AwaitLoading>
    <template #loading>
      <div class="min-w-60">
        <Skeleton class="min-h-10" />
      </div>
    </template>
    <Labeler
      :label
      :path
      :mode="labelMode"
      :variant="labelVariant"
    >
      <InputNumber
        v-model="model"
        v-bind="$attrs"
        class="min-w-60"
        @input="onInput"
      />
    </Labeler>
  </AwaitLoading>
</template>
<script setup>
import { InputNumber, Skeleton } from "primevue";
import { useContext } from "#imports";
import { AwaitLoading, Labeler } from "#components";

const context = useContext();

const { schema } = defineProps({
  schema: { type: null, required: true }
});
const model = defineModel({ type: null, required: true });

const { label, labelMode, labelVariant } = schema;

const path = context.injectPath();

function onInput(event) {
  model.value = event.value;
}
</script>
