export default defineNuxtRouteMiddleware((to, _) => {
  if (!!to.params?.lineItemId === false) return navigateTo("/");
});
