import { all, takeEvery, put, call, fork } from 'redux-saga/effects';
import api from 'api-config';
import {
  USER_LIST_GET_LIST,
  USER_ADD,
  USER_GET_BY_ID,
  BIND_MACHINE_DROPDOWN,
  SET_DESKTOP_USERID,
  setMachineIDSucess,
  BIND_USER_TYPE_DROPDOWN,
  BIND_MD_FUSION_LAB_NO_DROPDOWN,
  bindUsertypeDropdownSuccess,
  bindMDFusionLabNoDropdownSuccess,
  bindMDFusionLabNoDropdownError,
  USER_DELETE,
  deleteUserSuccess,
  DOWNLOAD_USER_EXCEL_FILE,
  DownloadUserExcelFileSuccess,
  DownloadUserExcelFileError,
} from '../actions';
import {
  getUserListSuccess,
  getUserListError,
  addUserSuccess,
  addUserError,
  getUserByIDSuccess,
  getUserByIDError,
  bindMachineDropdownSuccess,
} from './actions';

export const getUserListRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    setTimeout(() => {
      const formData = new FormData();
      formData.append('start', payload.item.currentPage);
      formData.append('length', payload.item.pageSize);
      formData.append('draw', '0');
      formData.append('search', payload.item.search);
      formData.append('order', payload.item.orderBy);
      formData.append('orderDir', payload.item.orderDirection);
      api
        .post('User/GetUserGridResult', formData)
        .then((response) => {
          if (response.data.resultCode === 'SUCCESS') {
            const userItems = JSON.stringify(response.data.resultObject);
            success(userItems);
          }
        })
        .catch((error) => error);
    }, 1000);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

export const deleteUserRequest = async (payload) => {
  return new Promise((success) => {
    const formData = new FormData();
    formData.append('id', payload.id.toString());
    api
      .post('User/Delete', formData)
      .then((response) => {
        if (response.data.resultCode === 'SUCCESS') {
          success(response.data.resultObjectID);
        }
      })
      .catch((error) => error);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

const addUserRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append(
      'UserID',
      JSON.parse(localStorage.getItem('Haute_current_machine')).uid
    );
    formData.append('ID', payload.ID);
    formData.append('Password', payload.password);
    formData.append('Email', payload.email);
    formData.append('FirstName', payload.firstName);
    formData.append('LastName', payload.lastName);
    formData.append('Phone', payload.phone);
    formData.append('UserName', payload.userName);
    formData.append('RoleId', payload.userType);
    formData.append('MDFusionLabNo', payload.mdFusionLabNo);
    formData.append('consultantID', payload.consultantID);
    formData.append('country', payload.country);
    formData.append('shop', payload.shop);
    formData.append('bachLotNo', payload.bachLotNo);
    formData.append('IsActive', payload.isActive);
    api
      .post('User/UpsertUser', formData)
      .then((response) => {
        if (
          response.data.resultCode === 'SUCCESS' &&
          (response.data.resultObjectID > 0 ||
            response.data.resultObjectID === -1 ||
            response.data.resultObjectID === -2 ||
            response.data.resultObjectID === -3)
        ) {
          success(response.data);
        } else {
          // put(addUserError(response))
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

const GetByIDUserRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append('id', payload.id);
    api
      .post('User/GetUserById', formData)
      .then((response) => {
        if (
          response.data.resultCode === 'SUCCESS' &&
          response.data.resultObjectID > 0
        ) {
          success(response.data.resultObject);
        } else {
          // put(addUserError(response))
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

const BindMachineDropdownRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append('id', payload.id);
    api
      .post('Machine/BindDropdownRolewise', formData)
      .then((response) => {
        if (response.data.resultCode === 'SUCCESS') {
          success(response.data.resultObject);
        } else {
          // put(addUserError(response))
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

const BindUsertypeDropdownRequest = async () => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    api
      .post('User/BindDropdownUsertype')
      .then((response) => {
        if (response.data.resultCode === 'SUCCESS') {
          success(response.data.resultObject);
        } else {
          // put(addUserError(response))
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

const BindMDFusionLabNoDropdownRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append('id', payload.id);
    api
      .post('User/BindDropdownMDFusionLab', formData)
      .then((response) => {
        if (response.data.resultCode === 'SUCCESS') {
          success(response.data.resultObject);
        } else {
          // put(addUserError(response))
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

const setMachineIDRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    localStorage.setItem('Haute_desktop_machine_ID', payload.id);
    localStorage.setItem('Haute_desktop_machine_Name', payload.label);
    success(localStorage.getItem('Haute_desktop_machine_ID'));
  })
    .then((response) => response)
    .catch((error) => error);
};

export const DownloadUserExcelFileRequest = async () => {
  try {
    const response = await api.post('User/GetUserExcelFile', null, {
      responseType: 'arraybuffer',
    });
    const blob = new Blob([response.data], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    });

    const blobUrl = URL.createObjectURL(blob);
    const downloadLink = document.createElement('a');
    downloadLink.href = blobUrl;
    downloadLink.download = 'output.xlsx';
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);

    URL.revokeObjectURL(blobUrl);

    return { success: true, response: response.data };
  } catch (error) {
    console.error('Error downloading Excel file:', error);
    return { success: false, error };
  }
};

function* getSurveyListItems({ payload }) {
  try {
    const response = yield call(getUserListRequest, payload);
    yield put(getUserListSuccess(response));
  } catch (error) {
    yield put(getUserListError(error));
  }
}
function* userByID({ payload }) {
  try {
    const response = yield call(GetByIDUserRequest, payload);
    yield put(getUserByIDSuccess(response));
  } catch (error) {
    yield put(getUserByIDError(error));
  }
}
function* userDeleteItems({ payload }) {
  try {
    const response = yield call(deleteUserRequest, payload);
    yield put(deleteUserSuccess(response));
  } catch (error) {
    yield put(getUserListError(error));
  }
}
function* addUser({ payload }) {
  const { item } = payload;
  try {
    const response = yield call(addUserRequest, item);
    if (
      response.resultObjectID > 0 ||
      response.resultObjectID === -1 ||
      response.resultObjectID === -2 ||
      response.resultObjectID === -3
    ) {
      yield put(addUserSuccess(response));
    } else {
      yield put(addUserError(response));
    }
  } catch (error) {
    yield put(addUserError(error));
  }
}
function* bindMachineDropdown({ payload }) {
  try {
    const response = yield call(BindMachineDropdownRequest, payload);
    yield put(bindMachineDropdownSuccess(response));
  } catch (error) {
    yield put(getMachineByIDError(error));
  }
}
function* setMachineID({ payload }) {
  try {
    const response = yield call(setMachineIDRequest, payload);
    yield put(setMachineIDSucess(response));
  } catch (error) {
    yield put(getUserByIDError(error));
  }
}
function* bindUsertypeDropdown({ payload }) {
  try {
    const response = yield call(BindUsertypeDropdownRequest, payload);
    yield put(bindUsertypeDropdownSuccess(response));
  } catch (error) {
    yield put(getUserByIDError(error));
  }
}
function* bindMDFusionLabNoDropdown({ payload }) {
  try {
    const response = yield call(BindMDFusionLabNoDropdownRequest, payload);
    yield put(bindMDFusionLabNoDropdownSuccess(response));
  } catch (error) {
    yield put(bindMDFusionLabNoDropdownError(error));
  }
}
function* DownloadUserExcelFile() {
  try {
    const response = yield call(DownloadUserExcelFileRequest);
    yield put(DownloadUserExcelFileSuccess(response));
  } catch (error) {
    yield put(DownloadUserExcelFileError(error));
  }
}

export function* wathcAddUser() {
  yield takeEvery(USER_ADD, addUser);
}
export function* watchGetList() {
  yield takeEvery(USER_LIST_GET_LIST, getSurveyListItems);
}
export function* wathcDeleteItem() {
  yield takeEvery(USER_DELETE, userDeleteItems);
}
export function* wathcGetByIDUser() {
  yield takeEvery(USER_GET_BY_ID, userByID);
}
export function* wathcBindMachineDropdown() {
  yield takeEvery(BIND_MACHINE_DROPDOWN, bindMachineDropdown);
}
export function* wathcSetMachineID() {
  yield takeEvery(SET_DESKTOP_USERID, setMachineID);
}
export function* wathcBindUsertypeDropdown() {
  yield takeEvery(BIND_USER_TYPE_DROPDOWN, bindUsertypeDropdown);
}
export function* wathcBindMdFusionLabNoDropdown() {
  yield takeEvery(BIND_MD_FUSION_LAB_NO_DROPDOWN, bindMDFusionLabNoDropdown);
}
export function* watchDownloadUserExcelFile() {
  yield takeEvery(DOWNLOAD_USER_EXCEL_FILE, DownloadUserExcelFile);
}

export default function* rootSaga() {
  yield all([
    fork(watchGetList),
    fork(wathcAddUser),
    fork(wathcDeleteItem),
    fork(wathcGetByIDUser),
    fork(wathcBindMachineDropdown),
    fork(wathcSetMachineID),
    fork(wathcBindUsertypeDropdown),
    fork(wathcBindMdFusionLabNoDropdown),
    fork(watchDownloadUserExcelFile),
  ]);
}
