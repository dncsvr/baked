<template>
  <UiSpec
    title="Number"
    :variants="variants"
  />
</template>
<script setup>
import giveMe from "~/utils/giveMe";

const variants = [
  {
    name: "Base",
    descriptor: giveMe.aNumber({ data: 1_499 })
  },
  {
    name: "Decimal Digits",
    descriptor: giveMe.aNumber({ data: 999.99 })
  },
  {
    name: "Fractionless Trailing Zeros",
    descriptor: giveMe.aNumber({ data: 200 })
  },
  {
    name: "Millions",
    descriptor: giveMe.aNumber({ data: 1_499_000 })
  },
  {
    name: "Billions",
    descriptor: giveMe.aNumber({ data: 1_499_000_000 })
  }
];
</script>
