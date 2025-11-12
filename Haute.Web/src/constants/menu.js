import {
  adminRoot,
  userRoot,
  UserRole,
  formulaRoot,
  canisterRoot,
  dispenseRoot,
  machineRoot,
  refillTrackingRoot,
  sanitisationTraking,
  canisterLookupRoot,
} from './defaultValues';

const data = [
  // {
  //   id: 'Home',
  //   icon: 'iconsminds-shop-4',
  //   label: 'menu.home',
  //   to: `${adminRoot}/home`,
  // },
  {
    id: 'User',
    icon: 'iconsminds-user',
    label: 'menu.user',
    to: `${adminRoot}${userRoot}/user`,
    roles: [UserRole.Admin],
  },
  {
    id: 'Machine',
    icon: 'iconsminds-safe-box',
    label: 'menu.machine',
    to: `${adminRoot}${machineRoot}/machineList`,
    roles: [UserRole.Admin],
  },
  {
    id: 'Formula',
    icon: 'iconsminds-arrow-inside-gap',
    label: 'menu.formula',
    to: `${adminRoot}${formulaRoot}/formulaList`,
    roles: [UserRole.Admin],
  },
  {
    id: 'CanisterLookup',
    icon: 'simple-icon-list',
    label: 'menu.canisterLookup',
    to: `${adminRoot}${canisterLookupRoot}/canisterLookupList`,
    roles: [UserRole.Admin],
  },
  {
    id: 'Canister',
    icon: 'iconsminds-bucket',
    label: 'menu.canister',
    to: `${adminRoot}${canisterRoot}/canisterList`,
  },
  {
    id: 'Dispense',
    icon: 'iconsminds-can',
    label: 'menu.dispense',
    to: `${adminRoot}${dispenseRoot}/dispenseManage`,
  },
  {
    id: 'RefillTracking',
    icon: 'iconsminds-receipt-4',
    label: 'menu.refillTrackingManage',
    to: `${adminRoot}${refillTrackingRoot}/refillManage`,
    roles: [UserRole.Admin],
  },
  {
    id: 'refillTrackingManage',
    icon: 'iconsminds-arrow-inside-gap',
    label: 'menu.refillTracking',
    to: `${refillTrackingRoot}/new`,
  },
  {
    id: 'SanitisingTraking',
    icon: 'iconsminds-receipt-4',
    label: 'menu.sanitisationTrakingManage',
    to: `${adminRoot}${sanitisationTraking}/sanitisationManage`,
    roles: [UserRole.Admin],
  },
  {
    id: 'SanitisingTrakingManage',
    icon: 'simple-icon-social-dropbox',
    label: 'menu.sanitisationTraking',
    to: `${sanitisationTraking}/new`,
  },

  // {
  //   id: 'gogo',
  //   icon: 'iconsminds-air-balloon-1',
  //   label: 'menu.gogo',
  //   to: `${adminRoot}/gogo`,
  //   subs: [
  //     {
  //       icon: 'simple-icon-paper-plane',
  //       label: 'menu.start',
  //       to: `${adminRoot}/gogo/start`,
  //     },
  //   ],
  // },
  // {
  //   id: 'secondmenu',
  //   icon: 'iconsminds-three-arrow-fork',
  //   label: 'menu.second-menu',
  //   to: `${adminRoot}/second-menu`,
  //   // roles: [UserRole.Admin, UserRole.Editor],
  //   subs: [
  //     {
  //       icon: 'simple-icon-paper-plane',
  //       label: 'menu.second',
  //       to: `${adminRoot}/second-menu/second`,
  //     },
  //   ],
  // },
  // {
  //   id: 'blankpage',
  //   icon: 'iconsminds-bucket',
  //   label: 'menu.blank-page',
  //   to: `${adminRoot}/blank-page`,
  // },
  // {
  //   id: 'docs',
  //   icon: 'iconsminds-library',
  //   label: 'menu.docs',
  //   to: 'https://gogo-react-docs.coloredstrategies.com/',
  //   newWindow: true,
  // },
];

export default data;
