<template>
  <UiSpec
    title="Data Container"
    :variants
    no-loading-variant
  />
</template>
<script setup>
import giveMe from "@utils/giveMe";

const variants = [
  {
    name: "Inputs",
    descriptor: giveMe.aDataContainer({
      inputs: [
        giveMe.anInput({
          name: "take",
          required: true,
          defaultValue: 10,
          queryBound: true,
          numeric: true,
          component: giveMe.aSelect({
            label: "Take",
            localizeLabel: false,
            stateful: true,
            noFloatLabel: true,
            data: [10, 20, 50, 100],
            action: giveMe.aPublishAction({
              pageContextKey: "take",
              data: giveMe.aContextData({ key: "model" }) }
            )
          })
        }),
        giveMe.anInput({
          name: "skip",
          required: true,
          default_: { type: "Inline", value: 0 },
          queryBound: true,
          numeric: true,
          component: {
            type: "ServerPaginator",
            schema: {},
            data: {
              type: "Composite",
              parts: [
                giveMe.anInlineData({ "length": 10 } ),
                giveMe.aContextData({ key: "page", prop: "take", targetProp: "take" })
              ]
            },
            reactions: {
              reload: giveMe.aTrigger({ when: "take" })
            }
          }
        }),
        giveMe.anInput({
          name: "required",
          required: true,
          queryBound: true,
          component:  giveMe.anExpectedInput({ testId: "required" })
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
