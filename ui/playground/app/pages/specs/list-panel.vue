<template>
  <UiSpec
    title="List Panel"
    :variants
    no-loading-variant
  />
</template>
<script setup>
import giveMe from "@utils/giveMe";

const variants = [
  {
    name: "Inputs",
    descriptor: giveMe.aDataPanel({
      inputs: [
        giveMe.anInput({
          name: "take",
          queryBound: true,
          defaultValue:"10",
          required: true,
          component: giveMe.aSelect({
            data: ["10", "20"],
          })
        }),
        giveMe.anInput({
          name: "skip",
          queryBound: true,
          required: false,
          defaultValue:"0",
          component: giveMe.aSelect({
            data: ["0", "10", "20"],
          })
        })
      ],
      content: giveMe.anExpected({
        testId: "content",
        data: giveMe.theParentContext()
      })
    })
  }
];
</script>
