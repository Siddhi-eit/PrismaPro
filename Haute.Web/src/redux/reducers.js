import { combineReducers } from 'redux';
import settings from './settings/reducer';
import menu from './menu/reducer';
import authUser from './auth/reducer';
import todoApp from './todo/reducer';
import chatApp from './chat/reducer';
import surveyListApp from './surveyList/reducer';
import surveyDetailApp from './surveyDetail/reducer';
import user from './user/reducer';
import canister from './canister/reducer';
import canisterLookup from './canisterLookup/reducer';
import sanitisation from './sanitisation/reducers';
import dispense from './dispense/reducer';
import refill from './refill/reducer';
import machine from './machine/reducer';
import formula from './formula/reducer';

const reducers = combineReducers({
  menu,
  settings,
  authUser,
  canister,
  canisterLookup,
  user,
  refill,
  todoApp,
  chatApp,
  surveyListApp,
  surveyDetailApp,
  sanitisation,
  dispense,
  machine,
  formula,
});

export default reducers;
