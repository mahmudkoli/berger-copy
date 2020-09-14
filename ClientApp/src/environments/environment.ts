// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
    production: false,
    clientId: 'cab5412f-46b7-40f1-a096-93b91de6cb39',
    authority: 'https://login.microsoftonline.com/04900ed8-19fc-43dc-95ee-1f02b224607f/',
    redirectUri: document.getElementsByTagName('base')[0].href,
    postLogoutRedirectUri: document.getElementsByTagName('base')[0].href,
    exposed_api: 'api://cab5412f-46b7-40f1-a096-93b91de6cb39/scope'
};
// export const environment = {
//     production: false,
//     clientId: '836be710-ad3e-4c56-a12c-77ad83d913bd',
//     authority: 'https://login.microsoftonline.com/4930e3b1-a0c5-46f8-84fe-b3b03553363e/',
//     redirectUri: 'http://localhost:40875/',
//     postLogoutRedirectUri: 'http://localhost:40875/',
//     exposed_api: 'api://836be710-ad3e-4c56-a12c-77ad83d913bd/scopes'
// };

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
