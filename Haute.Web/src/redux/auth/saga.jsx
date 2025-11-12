import { all, call, fork, put, takeEvery } from 'redux-saga/effects';
import { auth } from 'helpers/Firebase';
import { adminRoot, UserRole, dispenseRoot } from 'constants/defaultValues';
import { setCurrentUser } from 'helpers/Utils';
import api from 'api-config';
// import createNotification from 'helpers/alerts';

import { LOGIN_USER, LOGOUT_USER } from '../actions';

import { loginUserSuccess, loginUserError } from './actions';
import { error } from 'jquery';

export function* watchLoginUser() {
  // eslint-disable-next-line no-use-before-define
  yield takeEvery(LOGIN_USER, loginWithEmailPassword);
}

const loginWithEmailPasswordRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success, error) => {
    const formData = new FormData();
    const { email, password } = payload.user;
    formData.append('Email', email);
    formData.append('Password', password);
    api
      .post('account/SignInWithEmailAndPassword', formData)
      .then((response) => {
        const loginUser = response.data;
        if (loginUser.roleID === 1) {
          const loginUser = response.data;
          success(loginUser);
        } else {
          // createNotification(
          //   'error',
          //   'Error',
          //   'INCORRECT USERNAME OR PASSWORD'
          // );
          error('INCORRECT USERNAME OR PASSWORD');
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

export const loginWithEmailPassword1 = async (payload) => {
  const formData = new FormData();
  const { email, password } = payload.payload.user;
  formData.append('Email', email);
  formData.append('Password', password);
  await api
    .post('account/SignInWithEmailAndPassword/', formData)
    .then((response) => {
      const loginUser = response.data;
      let RoleName;
      if (loginUser.roleID === 1) {
        RoleName = UserRole.Admin;
        if (loginUser.roleID === 3) {
          RoleName = UserRole.Web;
        }
        if (loginUser.id > 0) {
          const item = {
            uid: loginUser.id,
            title: loginUser.userName,
            img: loginUser.profileImage,
            role: RoleName,
          };
          setCurrentUser(item);
          put(loginUserSuccess(item));
          const { history } = payload.payload;
          history.push(adminRoot);
        } else {
          put(loginUserError(loginUser.message));
        }
      } else {
        // createNotification('error', 'Error', 'INCORRECT USERNAME OR PASSWORD');
        put(loginUserError('INCORRECT USERNAME OR PASSWORD'));
      }
    });
};
function* loginWithEmailPassword({ payload }) {
  try {
    const loginUser = yield call(loginWithEmailPasswordRequest, payload);
    let RoleName;
    let root;
    if (loginUser.roleID === 1) {
      RoleName = UserRole.Admin;
      root = adminRoot;
      if (loginUser.roleID === 3) {
        RoleName = UserRole.Web;
        root = adminRoot + dispenseRoot + '/dispenseManage';
      }
      if (loginUser.id > 0) {
        const item = {
          uid: loginUser.id,
          title: loginUser.userName,
          img: loginUser.profileImage,
          role: RoleName,
        };
        setCurrentUser(item);
        yield put(loginUserSuccess(item));
        const { history } = payload;
        history.push(root);
      }
    } else {
      // createNotification('error', 'Error', 'INCORRECT USERNAME OR PASSWORD');
      yield put(loginUserError('INCORRECT USERNAME OR PASSWORD'));
    }
    if (loginUser.id === -1) {
      yield put(loginUserError(loginUser));
    }
    // else {
    //   yield put(addCanisterError(resonse));
    // }
  } catch (error) {
    yield put(loginUserError(error));
  }
}

export function* watchLogoutUser() {
  // eslint-disable-next-line no-use-before-define
  yield takeEvery(LOGOUT_USER, logout);
}

const logoutAsync = async (history) => {
  await auth
    .signOut()
    .then((user) => user)
    .catch((error) => error);
  history.push(adminRoot);
};

function* logout({ payload }) {
  const { history } = payload;
  setCurrentUser();
  yield call(logoutAsync, history);
}

export default function* rootSaga() {
  yield all([fork(watchLoginUser), fork(watchLogoutUser)]);
}
