import { all, takeEvery, put, call, fork } from 'redux-saga/effects';
import api from 'api-config';
import { getMachineID, getCurrentUser } from 'helpers/Utils';
import {
  CANISTER_LIST_GET_LIST,
  CANISTER_ADD,
  CANISTER_GET_BY_ID,
  CANISTER_LOOKUP_GET_BY_CANISTER_CODE,
  bindDispenseUnitDropdownSuccess,
  bindProductDropdownSuccess,
  bindProductDropdownError,
  BIND_DISPENSE_UNIT_DROPDOWN,
  BIND_PRODUCT_DROPDOWN,
  CANISTER_DELETE,
  deleteCanisterSuccess,
  DOWNLOAD_CANISTER_EXCEL_FILE,
  downloadCanisterExcelFileSuccess,
  downloadCanisterExcelFileError,
} from '../actions';
import {
  getCanisterLookupByCanisterCodeSuccess,
  getCanisterLookupByCanisterCodeError,
  getCanisterListSuccess,
  getCanisterListError,
  addCanisterSuccess,
  addCanisterError,
  getByIDSuccess,
  getByIDError,
} from './actions';

export const getCanisterListRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    setTimeout(() => {
      const formData = new FormData();
      let user = getCurrentUser();
      formData.append('start', payload.item.currentPage);
      formData.append('length', payload.item.pageSize);
      formData.append('draw', '0');
      formData.append('search', payload.item.search);
      formData.append('order', payload.item.orderBy);
      formData.append('orderDir', payload.item.orderDirection);
      formData.append('userID', user.uid);
      formData.append('machineID', payload.item.machineID);
      api
        .post('Canisters/GetAllCanisters', formData)
        .then((response) => {
          if (response.data.ResultCode === 'SUCCESS') {
            const canisterItems = JSON.stringify(response.data.ResultObject);

            success(canisterItems);
          }
        })
        .catch((error) => error);
    }, 1000);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

export const deleteCanisterRequest = async (payload) => {
  return new Promise((success) => {
    const formData = new FormData();
    formData.append('id', payload.id.toString());
    api
      .post('Canisters/Delete', formData)
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

const addCanisterRequest = async (payload) => {
  const machineID = getMachineID();
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    let user = getCurrentUser();
    formData.append('CreatedBy', user.uid);
    formData.append('UserID', user.uid);
    formData.append('MachineID', machineID);
    formData.append('ID', payload.canisterID);
    formData.append('CanisterLookupId', Number(payload.canisterLookupId));
    formData.append('MaximumAmount', payload.maximumAmount);
    formData.append('MinimumAmount', payload.minimumAmount);
    formData.append('CurrentAmount', payload.currentAmount);
    formData.append('WarningAmount', payload.warningAmount);
    formData.append('UnitID', payload.canisterSelect);
    formData.append('IsActive', payload.isActive);
    api
      .post('Canisters/Save', formData)
      .then((response) => {
        if (
          response.data.resultCode === 'SUCCESS' &&
          (response.data.resultObjectID > 0 || response.data.resultObjectID < 0)
        ) {
          success(response.data);
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

const GetByIDCanisterRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append('id', payload.id);
    api
      .post('Canisters/GetByID', formData)
      .then((response) => {
        if (
          response.data.resultCode === 'SUCCESS' &&
          response.data.resultObjectID > 0
        ) {
          success(response.data.resultObject);
        } else {
          // put(addCanisterError(response))
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

const GetCanisterLookupByCode = async () => {
  return await new Promise((success) => {
    api
      .post('Canisters/GetCanisterLookup')
      .then((response) => {
        if (response.data.resultCode === 'SUCCESS') {
          success(response.data.resultObject);
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

const BindDispenseUnitDropdownRequest = async () => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    api
      .post('Canisters/BindDispenseUnitDropdown')
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

const BindProductDropdownRequest = async () => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    api
      .post('Canisters/BindProductDropdown')
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

export const DownloadCanisterExcelFileRequest = async () => {
  try {
    const response = await api.post('Canisters/GetCanisterExcelFile', null, {
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
    const response = yield call(getCanisterListRequest, payload);
    yield put(getCanisterListSuccess(response));
  } catch (error) {
    yield put(getCanisterListError(error));
  }
}

function* canisterByID({ payload }) {
  try {
    const response = yield call(GetByIDCanisterRequest, payload);
    yield put(getByIDSuccess(response));
  } catch (error) {
    yield put(getByIDError(error));
  }
}

function* getCanisterLookupByCanisterCode() {
  try {
    const response = yield call(GetCanisterLookupByCode);
    yield put(getCanisterLookupByCanisterCodeSuccess(response));
  } catch (error) {
    yield put(getCanisterLookupByCanisterCodeError(error));
  }
}

function* bindDispenseUnitDropdown({ payload }) {
  try {
    const response = yield call(BindDispenseUnitDropdownRequest, payload);
    yield put(bindDispenseUnitDropdownSuccess(response));
  } catch (error) {
    yield put(getByIDError(error));
  }
}

function* bindProductDropdown({ payload }) {
  try {
    const response = yield call(BindProductDropdownRequest, payload);
    yield put(bindProductDropdownSuccess(response));
  } catch (error) {
    yield put(bindProductDropdownError(error));
  }
}

function* canisterDeleteItems({ payload }) {
  try {
    const response = yield call(deleteCanisterRequest, payload);
    yield put(deleteCanisterSuccess(response));
  } catch (error) {
    yield put(getCanisterListError(error));
  }
}
function* addCanister({ payload }) {
  const { item } = payload;
  try {
    const resonse = yield call(addCanisterRequest, item);
    if (resonse.resultObjectID > 0 || resonse.resultObjectID < 0) {
      yield put(addCanisterSuccess(resonse));
    }
    // else {
    //   yield put(addCanisterError(resonse));
    // }
  } catch (error) {
    yield put(addCanisterError(error));
  }
}

function* downloadCanisterExcelFile() {
  try {
    const response = yield call(DownloadCanisterExcelFileRequest);
    yield put(downloadCanisterExcelFileSuccess(response));
  } catch (error) {
    yield put(downloadCanisterExcelFileError(error));
  }
}

export function* wathcAddCanister() {
  yield takeEvery(CANISTER_ADD, addCanister);
}

export function* watchGetList() {
  yield takeEvery(CANISTER_LIST_GET_LIST, getSurveyListItems);
}
export function* wathcDeleteItem() {
  yield takeEvery(CANISTER_DELETE, canisterDeleteItems);
}
export function* wathcGetByIDCanister() {
  yield takeEvery(CANISTER_GET_BY_ID, canisterByID);
}
export function* watchbindDispenseUnitDropdown() {
  yield takeEvery(BIND_DISPENSE_UNIT_DROPDOWN, bindDispenseUnitDropdown);
}
export function* watchbindProductDropdown() {
  yield takeEvery(BIND_PRODUCT_DROPDOWN, bindProductDropdown);
}
export function* watchDownloadCanisterExcelFile() {
  yield takeEvery(DOWNLOAD_CANISTER_EXCEL_FILE, downloadCanisterExcelFile);
}
export function* watchGetCanisterLookupByCanisterCode() {
  yield takeEvery(
    CANISTER_LOOKUP_GET_BY_CANISTER_CODE,
    getCanisterLookupByCanisterCode
  );
}

export default function* rootSaga() {
  yield all([
    fork(watchGetList),
    fork(wathcAddCanister),
    fork(wathcDeleteItem),
    fork(wathcGetByIDCanister),
    fork(watchbindDispenseUnitDropdown),
    fork(watchbindProductDropdown),
    fork(watchDownloadCanisterExcelFile),
    fork(watchGetCanisterLookupByCanisterCode),
  ]);
}
