export default defineNuxtRouteMiddleware((to, _) => {
  if (to.name !== "create-project" && !!to.query["lineItemId"] === false) {
    return navigateTo("/create-project/");
  }
});
