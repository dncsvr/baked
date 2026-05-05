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
      labelMode: null,
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
    name: "InputText: Float:On Mode",
    descriptor: giveMe.anInputText()
  },
  {
    name: "InputText: Float:In Mode",
    descriptor: giveMe.anInputText({
      labelVariant: "in"
    })
  },
  {
    name: "InputText: Ifta Mode",
    descriptor: giveMe.anInputText({
      labelMode: "ifta"
    })
  },
  {
    name: "InputNumber: Float:On Mode",
    descriptor: giveMe.anInputNumber()
  },
  {
    name: "InputNumber: Float:In Mode",
    descriptor: giveMe.anInputNumber({
      labelVariant: "in"
    })
  },
  {
    name: "InputNumber: Ifta Mode",
    descriptor: giveMe.anInputNumber({
      labelMode: "ifta"
    })
  },
];
</script>
