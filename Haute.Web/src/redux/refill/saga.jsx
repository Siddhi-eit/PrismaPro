import { all, takeEvery, put, call, fork } from 'redux-saga/effects';
import api from 'api-config';
import { getMachineID } from 'helpers/Utils';
// import moment from 'moment';

import {
  addRefillTrakingError,
  addRefillTrakingSuccess,
  ADD_REFILL_TRACKING,
  bindCanSizeDropdownSuccess,
  BIND_CAN_SIZE_DROPDOWN,
  deleteRefillTrackingSuccess,
  RefillTrkingGetByIDError,
  RefillTrkingGetByIDSuccess,
  REFILL_LIST_GET_LIST,
  REFILL_TRACKING_DELETE,
  REFILL_TRACKING_GET_BY_ID,
  DOWNLOAD_REFILL_EXCEL_FILE,
  downloadRefillExcelFileSuccess,
  downloadRefillExcelFileError,
  DOWNLOAD_REFILL_SCANNER,
  downloadRefillScannerSuccess,
  downloadRefillScannerError,
} from '../actions';
import { getRefillListSuccess, getRefillListError } from './actions';

export const getRefillListRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    setTimeout(() => {
      let machineID = getMachineID();
      const formData = new FormData();
      formData.append('start', payload.item.currentPage);
      formData.append('length', payload.item.pageSize);
      formData.append('draw', '0');
      formData.append('search', payload.item.search);
      formData.append('order', payload.item.orderBy);
      formData.append('orderDir', payload.item.orderDirection);
      formData.append('MachineID', machineID);
      api
        .post('RefillTracking/GetAll', formData)
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

const BindCanSizeDropdownRequest = async () => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    api
      .post('RefillTracking/GetCanSize')
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

const addRefillTrakinRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append(
      'CreatedBy',
      JSON.parse(localStorage.getItem('Haute_current_machine')).uid
    );
    formData.append('ID', payload.id);
    formData.append('FusionLabNo', payload.fusionLabNo);
    formData.append('CanisterID', payload.canisterNO);
    formData.append('refillML', payload.refillML);
    formData.append('LotNr', payload.lotNr);
    formData.append('UnitID', '2');
    // formData.append('Quantity', payload.refillCanSize);
    formData.append('IsActive', payload.isActive);
    formData.append('MachineID', payload.machineID);
    formData.append('UserID', payload.userID);
    api
      .post('RefillTracking/Save', formData)
      .then((response) => {
        success(JSON.stringify(response.data));
      })
      .catch((error) => error);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

const GetByIDRefillRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append('id', payload.id);
    api
      .post('RefillTracking/GetByID', formData)
      .then((response) => {
        if (
          response.data.resultCode === 'SUCCESS' &&
          response.data.resultObjectID > 0
        ) {
          success(response.data.resultObject);
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

const GetRefillScannerDetailByID = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append('id', payload);
    api
      .post('RefillTracking/GetScannerDetailByID', formData)
      .then((response) => {
        if (
          response.data.resultCode === 'SUCCESS' &&
          response.data.resultObjectID > 0
        ) {
          success(response.data.resultObject);
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

export const deleteRefillTrakingRequest = async (payload) => {
  return new Promise((success) => {
    const formData = new FormData();
    formData.append(
      'userId',
      JSON.parse(localStorage.getItem('Haute_current_machine')).uid
    );
    formData.append('id', payload.id.toString());
    api
      .post('RefillTracking/Delete', formData)
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

export const DownloadRefillExcelFileRequest = async () => {
  try {
    const response = await api.post('RefillTracking/GetRefillExcelFile', null, {
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
    const response = yield call(getRefillListRequest, payload);
    yield put(getRefillListSuccess(response));
  } catch (error) {
    yield put(getRefillListError(error));
  }
}

function* bindcanSizeDropdown({ payload }) {
  try {
    const response = yield call(BindCanSizeDropdownRequest, payload);
    yield put(bindCanSizeDropdownSuccess(response));
  } catch (error) {
    yield put(getRefillListError(error));
  }
}

function* addRefillTraking({ payload }) {
  const { item } = payload;
  try {
    const response = yield call(addRefillTrakinRequest, item);
    yield put(addRefillTrakingSuccess(response));
  } catch (error) {
    yield put(addRefillTrakingError(error));
  }
}

function* refillGetByID({ payload }) {
  try {
    const response = yield call(GetByIDRefillRequest, payload);
    yield put(RefillTrkingGetByIDSuccess(response));
  } catch (error) {
    yield put(RefillTrkingGetByIDError(error));
  }
}

function* refillDeleteItems({ payload }) {
  try {
    const response = yield call(deleteRefillTrakingRequest, payload);
    yield put(deleteRefillTrackingSuccess(response));
  } catch (error) {
    yield put(RefillTrkingGetByIDError(error));
  }
}

function* downloadRefillExcelFile() {
  try {
    const response = yield call(DownloadRefillExcelFileRequest);
    yield put(downloadRefillExcelFileSuccess(response));
  } catch (error) {
    yield put(downloadRefillExcelFileError(error));
  }
}

function* downloadRefillScanner({ payload }) {
  const { items } = payload;
  try {
    const response = yield call(GetRefillScannerDetailByID, items);
    yield put(downloadRefillScannerSuccess(response));
  } catch (error) {
    yield put(downloadRefillScannerError(error));
  }
}

export function* watchGetList() {
  yield takeEvery(REFILL_LIST_GET_LIST, getSurveyListItems);
}
export function* watchCanSizeList() {
  yield takeEvery(BIND_CAN_SIZE_DROPDOWN, bindcanSizeDropdown);
}
export function* wathcAddRefillTraking() {
  yield takeEvery(ADD_REFILL_TRACKING, addRefillTraking);
}
export function* wathcGetByIDRefill() {
  yield takeEvery(REFILL_TRACKING_GET_BY_ID, refillGetByID);
}

export function* wathcDeleteItem() {
  yield takeEvery(REFILL_TRACKING_DELETE, refillDeleteItems);
}

export function* watchDownloadRefillExcelFile() {
  yield takeEvery(DOWNLOAD_REFILL_EXCEL_FILE, downloadRefillExcelFile);
}

export function* watchDownloadRefillScanner() {
  yield takeEvery(DOWNLOAD_REFILL_SCANNER, downloadRefillScanner);
}

export default function* rootSaga() {
  yield all([
    fork(watchGetList),
    fork(watchCanSizeList),
    fork(wathcAddRefillTraking),
    fork(wathcGetByIDRefill),
    fork(wathcDeleteItem),
    fork(watchDownloadRefillExcelFile),
    fork(watchDownloadRefillScanner),
  ]);
}
