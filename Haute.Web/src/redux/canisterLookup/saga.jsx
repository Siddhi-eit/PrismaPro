import { all, takeEvery, put, call, fork } from 'redux-saga/effects';
import api from 'api-config';
import { getMachineID, getCurrentUser } from 'helpers/Utils';
import {
  CANISTER_LOOKUP_LIST_GET_LIST,
  CANISTER_LOOKUP_DELETE,
  CANISTER_LOOKUP_ADD,
  CANISTER_LOOKUP_GET_BY_ID,
  DOWNLOAD_CANISTER_LOOKUP_EXCEL_FILE,
} from '../actions';
import {
  getCanisterLookupListSuccess,
  getCanisterLookupListError,
  deleteCanisterLookupSuccess,
  addCanisterLookupSuccess,
  addCanisterLookupError,
  getCanisterLookupByIDSuccess,
  getCanisterLookupByIDError,
  downloadCanisterLookupExcelFileSuccess,
  downloadCanisterLookupExcelFileError,
} from './actions';

export const getCanisterLookupListRequest = async (payload) => {
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
        .post('CanisterLookup/GetAllCanisterLookup', formData)
        .then((response) => {
          if (response.data.ResultCode === 'SUCCESS') {
            const canisterLookItems = JSON.stringify(
              response.data.ResultObject
            );

            success(canisterLookItems);
          }
        })
        .catch((error) => error);
    }, 1000);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

function* getSurveyListItems({ payload }) {
  try {
    const response = yield call(getCanisterLookupListRequest, payload);
    console.log('response', response);
    yield put(getCanisterLookupListSuccess(response));
  } catch (error) {
    yield put(getCanisterLookupListError(error));
  }
}

export const deleteCanisterLookupRequest = async (payload) => {
  return new Promise((success) => {
    const formData = new FormData();
    formData.append('id', payload.id.toString());
    api
      .post('CanisterLookup/Delete', formData)
      .then((response) => {
        if (response.data.resultCode === 'SUCCESS') {
          success(response.data.resultObjectID);
        }
        if (response.data.resultCode === 'ERROR') {
          success(response.data.resultObject);
        }
      })
      .catch((error) => {
        return error;
      });
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

function* canisterLookupDeleteItems({ payload }) {
  try {
    const response = yield call(deleteCanisterLookupRequest, payload);
    yield put(deleteCanisterLookupSuccess(response));
  } catch (error) {
    yield put(getCanisterLookupListError(error));
  }
}

const addCanisterLookupRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    let user = getCurrentUser();
    formData.append('CreatedBy', user.uid);
    formData.append('ID', payload.canisterLookupID);
    formData.append('CanisterCode', payload.canisterCode);
    formData.append('CanisterSKU', payload.canisterSKU);
    formData.append('CanisterName', payload.canisterName);
    formData.append('IsActive', payload.isActive);
    api
      .post('CanisterLookup/Save', formData)
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

function* addCanisterLookup({ payload }) {
  const { item } = payload;
  try {
    const resonse = yield call(addCanisterLookupRequest, item);
    if (resonse.resultObjectID > 0 || resonse.resultObjectID < 0) {
      yield put(addCanisterLookupSuccess(resonse));
    }
  } catch (error) {
    yield put(addCanisterLookupError(error));
  }
}

const GetByIDCanisterLookupRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append('id', payload.id);
    api
      .post('CanisterLookup/GetByID', formData)
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

function* canisterLookupByID({ payload }) {
  try {
    const response = yield call(GetByIDCanisterLookupRequest, payload);
    yield put(getCanisterLookupByIDSuccess(response));
  } catch (error) {
    yield put(getCanisterLookupByIDError(error));
  }
}

export const DownloadCanisterLookupExcelFileRequest = async () => {
  try {
    const response = await api.post(
      'CanisterLookup/GetCanisterLookupExcelFile',
      null,
      {
        responseType: 'arraybuffer',
      }
    );
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

function* downloadCanisterLookupExcelFile() {
  try {
    const response = yield call(DownloadCanisterLookupExcelFileRequest);
    yield put(downloadCanisterLookupExcelFileSuccess(response));
  } catch (error) {
    yield put(downloadCanisterLookupExcelFileError(error));
  }
}

export function* watchGetList() {
  yield takeEvery(CANISTER_LOOKUP_LIST_GET_LIST, getSurveyListItems);
}

export function* watchAddCanisterLookup() {
  yield takeEvery(CANISTER_LOOKUP_ADD, addCanisterLookup);
}

export function* watchDeleteLookupItem() {
  yield takeEvery(CANISTER_LOOKUP_DELETE, canisterLookupDeleteItems);
}
export function* watchGetByIDCanisterLookup() {
  yield takeEvery(CANISTER_LOOKUP_GET_BY_ID, canisterLookupByID);
}

export function* watchDownloadCanisterLookupExcelFile() {
  yield takeEvery(
    DOWNLOAD_CANISTER_LOOKUP_EXCEL_FILE,
    downloadCanisterLookupExcelFile
  );
}

export default function* rootSaga() {
  yield all([
    fork(watchGetList),
    fork(watchAddCanisterLookup),
    fork(watchDeleteLookupItem),
    fork(watchGetByIDCanisterLookup),
    fork(watchDownloadCanisterLookupExcelFile),
  ]);
}
