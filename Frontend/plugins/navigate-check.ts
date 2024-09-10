export default defineNuxtPlugin(() => {
  const router = useRouter();

  addRouteMiddleware(
    "check-id-create-project",
    async (to, from) => {
      const lineItemsStore = useLineItemsStore();
      const integratedProductsStore = useIntegratedProductsStore();

      let lineItem = null;

      if (to.query.lineItemId) {
        lineItem = await lineItemsStore.fetchLineItemById({
          id: String(to.query.lineItemId),
        });
        if (!!lineItem === false) router.push({ path: "/create-project/" });
      }
      if (
        from.name === "create-project" &&
        to.name === "create-project-choose-template"
      ) {
        integratedProductsStore.resetTemplates();
      }
    },
    { global: true },
  );

  addRouteMiddleware(
    "check-id-params",
    async (to, _) => {
      const lineItemsStore = useLineItemsStore();
      let lineItem = null;
      if (to.params?.lineItemId) {
        lineItem = await lineItemsStore.fetchLineItemById({
          id: to.params.lineItemId.toString(),
        });
        if (!!lineItem === false) router.push({ path: "/" });
      }
    },
    { global: true },
  );

  addRouteMiddleware(
    "template-options-clear",
    async (to, from) => {
      const integratedProductsStore = useIntegratedProductsStore();
      const chooseTemplatePathName = "create-project-choose-template";
      if (
        to.name === chooseTemplatePathName ||
        from.name === chooseTemplatePathName
      ) {
        await integratedProductsStore.clearOptionList();
      }
    },
    { global: true },
  );
});
