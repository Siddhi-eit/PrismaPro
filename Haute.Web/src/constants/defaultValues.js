export const UserRole = {
  Admin: 1,
  Web: 3,
};

/*
Menu Types:
"menu-default", "menu-sub-hidden", "menu-hidden"
*/

export const APIRooT = 'http://api.hautecustombeauty.com/api';
export const connectionHubURL =
  'http://api.hautecustombeauty.com/connectionhub';
// export const APIRooT = 'https://localhost:7216/api';
// export const connectionHubURL = 'https://localhost:7216/connectionhub';
export const defaultMenuType = 'menu-default';
export const subHiddenBreakpoint = 1440;
export const menuHiddenBreakpoint = 768;
export const defaultLocale = 'en';
export const localeOptions = [
  { id: 'en', name: 'English - LTR', direction: 'ltr' },
  { id: 'es', name: 'Español', direction: 'ltr' },
  { id: 'enrtl', name: 'English - RTL', direction: 'rtl' },
];
export const firebaseConfig = {
  apiKey: 'AIzaSyBBksq-Asxq2M4Ot-75X19IyrEYJqNBPcg',
  authDomain: 'gogo-react-login.firebaseapp.com',
  databaseURL: 'https://gogo-react-login.firebaseio.com',
  projectId: 'gogo-react-login',
  storageBucket: 'gogo-react-login.appspot.com',
  messagingSenderId: '216495999563',
};
export const adminRoot = '/app';
export const userRoot = '/user';
export const machineRoot = '/machine';
export const formulaRoot = '/formula';
export const canisterRoot = '/canister';
export const dispenseRoot = '/dispense';
export const canisterLookupRoot = '/canisterLookup';
export const refillTrackingRoot = '/refillTracking';
export const sanitisationTraking = '/sanitisationTraking';
export const buyUrl = 'https://1.envato.market/k4z0';
export const searchPath = `${adminRoot}/#`;
export const servicePath = 'https://api.coloredstrategies.com';
export const bgPDF = '/assets/img/profiles/hcbmdfl_logo.png';

// export const currentUser = {
//   id: 1,
//   title: 'Sarah Kortney',
//   img: '/assets/img/profiles/l-1.jpg',
//   date: 'Last seen today 15:24',
//   role: UserRole.Admin,
// };

export const themeColorStorageKey = '__theme_selected_color';
export const isMultiColorActive = false;
export const defaultColor = 'light.blueolympic';
export const isDarkSwitchActive = true;
export const defaultDirection = 'ltr';
export const themeRadiusStorageKey = '__theme_radius';
export const isAuthGuardActive = true;
export const colors = [
  'bluenavy',
  'blueyale',
  'blueolympic',
  'greenmoss',
  'greenlime',
  'purplemonster',
  'orangecarrot',
  'redruby',
  'yellowgranola',
  'greysteel',
];
