export default defineNuxtRouteMiddleware((_) => {
  return abortNavigation();
});
