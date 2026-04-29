<template>
  <UiSpec title="Inputs - Query Bound - Optional">
    <Message severity="info">
      <span class="text-xl">
        ⬆️  Check if values sync with query string above ⬆️
      </span>
    </Message>
    <div
      class="border-4 border-gray-500 rounded p-4 space-x-4"
      data-testid="component"
    >
      <Inputs
        :inputs
        @ready="onReady"
        @changed="onChanged"
      />
    </div>
    <Message severity="info">
      <span class="text-xl">
        ⬇️  Check if ready is true when all required inputs are set ⬇️
      </span>
    </Message>
    <div class="border-4 border-gray-500 rounded p-4">
      <span class="text-gray-500">ready=</span>
      <span data-testid="ready">{{ ready }}</span>
    </div>
    <Message severity="info">
      <span class="text-xl">
        ⬇️  Check if unique key changes when any parameter changes ⬇️
      </span>
    </Message>
    <div class="border-4 border-gray-500 rounded p-4">
      <span class="text-gray-500">unique-key=</span>
      <span data-testid="unique-key">{{ uniqueKey }}</span>
    </div>
    <Message severity="info">
      <span class="text-xl">
        ⬇️  Click and check if resets all inputs except required with default ⬆️
      </span>
    </Message>
    <div class="border-4 border-gray-500 rounded p-4 flex gap-4">
      <Button
        as="router-link"
        label="RESET"
        to="/specs/inputs--query-bound--optional"
        data-testid="reset"
      />
      <Bake
        name="reactor"
        :descriptor="reactor"
        class="border border-green-500 rounded p-2"
      />
    </div>
  </UiSpec>
</template>
<script setup>
import { ref } from "vue";
import { Button, Message } from "primevue";
import giveMe from "@utils/giveMe";

const ready = ref();
const uniqueKey = ref();

const inputs = [
  giveMe.anInput({
    name: "optional",
    queryBound: true,
    component: giveMe.anExpectedInput({
      testId: "optional",
      action: giveMe.aPublishAction({ pageContextKey: "optional" })
    })
  }),
  giveMe.anInput({
    name: "optional-two",
    queryBound: true,
    component: giveMe.anInputNumber()
  }),
  giveMe.anInput({
    name: "optional-three",
    queryBound: true,
    component: giveMe.aSelect()
  }),
  giveMe.anInput({
    name: "optional-four",
    queryBound: true,
    component: giveMe.aSelectButton()
  })
];

const reactor = giveMe.anExpected({
  testId: "reactor",
  value: "Reacting...",
  reactions: {
    show: giveMe.aTrigger({
      when: "optional",
      constraint: giveMe.aConstraint({ is: "react" })
    })
  }
});

function onReady(value) {
  ready.value = value;
}

function onChanged(event) {
  uniqueKey.value = event.uniqueKey;
}
</script>
