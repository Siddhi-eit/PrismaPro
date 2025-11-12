import { all } from 'redux-saga/effects';
import authSagas from './auth/saga';
import todoSagas from './todo/saga';
import chatSagas from './chat/saga';
import surveyListSagas from './surveyList/saga';
import surveyDetailSagas from './surveyDetail/saga';
import userSagas from './user/saga';
import canisterListSagas from './canister/saga';
import SanitisationListSagas from './sanitisation/saga';
import dispenseSagas from './dispense/saga';
import refillSagas from './refill/saga';
import machineSagas from './machine/saga';
import canisterLookupSagas from './canisterLookup/saga';
import FormulaSagas from './formula/saga';

export default function* rootSaga() {
  yield all([
    authSagas(),
    canisterListSagas(),
    userSagas(),
    todoSagas(),
    chatSagas(),
    surveyListSagas(),
    surveyDetailSagas(),
    refillSagas(),
    SanitisationListSagas(),
    dispenseSagas(),
    machineSagas(),
    canisterLookupSagas(),
    FormulaSagas(),
  ]);
}
