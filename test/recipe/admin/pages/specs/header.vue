<template>
  <UiSpec
    title="Header"
    :variants="variants"
  />
</template>
<script setup>
import giveMe from "~/utils/giveMe";

const variants = [
  {
    name: "Multi Level",
    descriptor: giveMe.aHeader({
      sitemapItems: [
        giveMe.aHeaderItem({ route: "/" }),
        giveMe.aHeaderItem({
          route: "/root",
          icon: "pi pi-heart",
          title: "Spec: Root Page"
        }),
        giveMe.aHeaderItem({
          route: "/root/mid",
          icon: "pi pi-wave-pulse",
          title: "Spec: Mid Page",
          parentRoute: "/root"
        }),
        giveMe.aHeaderItem({
          route: "/root/mid/leaf",
          icon: "pi pi-sun",
          title: "Spec: Leaf Page",
          parentRoute: "/root/mid"
        })
      ],
      data: { path: "/root/mid/leaf" }
    })
  },
  {
    name: "Wildcard",
    descriptor: giveMe.aHeader({
      sitemapItems: [
        giveMe.aHeaderItem({ route: "/" }),
        giveMe.aHeaderItem({ route: "/entity/{id}", title: "Spec: Entity" })
      ],
      data: { path: "/entity/adf-123" }
    })
  },
  {
    name: "Hidden at Home",
    descriptor: giveMe.aHeader({
      sitemapItems: [ giveMe.aHeaderItem({ route: "/" }) ],
      data: { path: "/" }
    })
  },
  {
    name: "Hidden at Unknown",
    descriptor: giveMe.aHeader({
      sitemapItems: [
        giveMe.aHeaderItem({ route: "/" }),
        giveMe.aHeaderItem({
          route: "/known",
          icon: "pi pi-heart",
          title: "Known Page"
        })
      ],
      data: { path: "/unknown" }
    })
  },
  {
    name: "No Title",
    descriptor: giveMe.aHeader({
      sitemapItems: [
        giveMe.aHeaderItem({ route: "/" }),
        giveMe.aHeaderItem({
          route: "/no-title",
          icon: "pi pi-wave-pulse",
          title: null
        })
      ],
      data: { path: "/no-title" }
    })
  },
  {
    name: "No Icon",
    descriptor: giveMe.aHeader({
      sitemapItems: [
        giveMe.aHeaderItem({ route: "/" }),
        giveMe.aHeaderItem({
          route: "/no-icon",
          icon: null,
          title: "Spec: No Icon"
        })
      ],
      data: { path: "/no-icon" }
    })
  }
];
</script>
