<template>
  <UiSpec
    title="Labeler"
    :variants
    use-model
    no-loading-variant
  />
</template>
<script setup>
import giveMe from "@utils/giveMe";

const variants = [
  {
    name: "Select: No Label Mode",
    descriptor: giveMe.aSelect({
      labelMode: null
    })
  },
  {
    name: "Select: Float:On Mode",
    descriptor: giveMe.aSelect({
      label: "Float:On Mode"
    })
  },
  {
    name: "Select: Ifta Mode",
    descriptor: giveMe.aSelect({
      labelMode: "ifta",
      label: "Ifta Mode"
    })
  },
  {
    name: "Select: Float:In Mode",
    descriptor: giveMe.aSelect({
      labelVariant: "in",
      label: "Float:In Mode"
    })
  },
  {
    name: "SelectButton: Ifta Mode",
    descriptor: giveMe.aSelectButton({
      labelMode: "ifta",
      label: "Ifta Mode"
    })
  },
  {
    name: "SelectButton: No Label Mode",
    descriptor: giveMe.aSelectButton({
      labelMode: null,
      label: null
    })
  },
  {
    name: "InputText: No Label Mode",
    descriptor: giveMe.anInputText({
      labelMode: null
    })
  },
  {
    name: "InputText: Float:On Mode",
    descriptor: giveMe.anInputText({
      label: "Float:On Mode"
    })
  },
  {
    name: "InputText: Float:In Mode",
    descriptor: giveMe.anInputText({
      labelVariant: "in",
      label: "Float:In Mode"
    })
  },
  {
    name: "InputText: Ifta Mode",
    descriptor: giveMe.anInputText({
      labelMode: "ifta",
      label: "Ifta Mode"
    })
  },
  {
    name: "InputNumber: No Label Mode",
    descriptor: giveMe.anInputNumber({
      labelMode: null
    })
  },
  {
    name: "InputNumber: Float:On Mode",
    descriptor: giveMe.anInputNumber({
      label: "Float:On Mode"
    })
  },
  {
    name: "InputNumber: Float:In Mode",
    descriptor: giveMe.anInputNumber({
      labelVariant: "in",
      label: "Float:In Mode"
    })
  },
  {
    name: "InputNumber: Ifta Mode",
    descriptor: giveMe.anInputNumber({
      labelMode: "ifta",
      label: "Ifta Mode"
    })
  }
];
</script>
