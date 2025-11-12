import { all, takeEvery, put, call, fork } from 'redux-saga/effects';
import api from 'api-config';
import { getCurrentUser, getMachineID } from 'helpers/Utils';

import {
  bindProductDropdownsSuccess,
  DISPENSE_PRODUCT_DROPDOWN,
  DISPENSE_PRODUCT_GRID,
  bindProductGridSuccess,
  DISPENSE_NOW,
  dispenseNowSuccess,
  DISPENSEHISTORY_GET_LIST,
  getDispenseHistoryListSuccess,
  getDispenseHistoryListError,
  DISPENSE_SUCCESS,
  SuccessDispenseSuccess,
  GetDispanseDataSuccess,
  GET_DISPANSE_DATA,
  CHECK_CANISTER_DATA,
  CheckCanisterDataSuccess,
  DOWNLOAD_DISPENSE_EXCEL_FILE,
  DownloadDispenseExcelFileSuccess,
  DownloadDispenseExcelFileError,
} from '../actions';

export const bindDropdownsRequest = async () => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    setTimeout(() => {
      api
        .post('Dispense/GetAllDispenseData')
        .then((response) => {
          if (response.data.resultCode === 'SUCCESS') {
            const dropdownDataList = JSON.stringify(response.data.resultObject);
            success(dropdownDataList);
          }
        })
        .catch((error) => error);
    }, 1000);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

export const bindProductGridRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    setTimeout(() => {
      const formData = [
        payload.item.ProductCode,
        payload.item.Collection,
        payload.item.ProductName,
      ];
      api
        .post('Dispense/GetDispenseDataByFilter', JSON.stringify(formData))
        .then((response) => {
          if (response.data.resultCode === 'SUCCESS') {
            const productGridData = JSON.stringify(
              response.data.resultObject.productFormulaList
            );
            success(productGridData);
          }
        })
        .catch((error) => error);
    }, 1000);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

export const dispenseSuccessRequest = async (payload) => {
  const user = getCurrentUser();
  const machineID = getMachineID();
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    setTimeout(() => {
      const formData = new FormData();
      formData.append(
        'CreatedBy',
        JSON.parse(localStorage.getItem('Haute_current_machine')).uid
      );
      formData.append('ProductCode', payload.items.ProductCode);
      formData.append('Collection', payload.items.Collection);
      formData.append('ProductName', payload.items.ProductName);
      formData.append('ComponentNames', payload.items.ColorCode);
      formData.append('ComponentAmounts', payload.items.Amount);
      formData.append('AmountToDispense', payload.items.CanSize);
      formData.append('AmountToDispenseUnitID', payload.items.DispenseUnit);
      formData.append('UserID', user.uid);
      formData.append('MachineID', machineID);
      formData.append('DispanseQuantity', 1);
      formData.append('IsDispense', 'True');
      api
        .post('Dispense/DispenseSuccess', formData)
        .then((response) => {
          success(JSON.stringify(response.data));
        })
        .catch((error) => error);
    }, 1000);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

export const GetDispanseDataRequest = async (payload) => {
  // return await new Promise((success) => {
  //   const formData = [payload.tailoringCode, payload.userID];
  //   api
  //     .post('Dispense/GetDispanseData', JSON.stringify(formData))
  //     .then((response) => {
  //       success(JSON.stringify(response.data));
  //     })
  //     .catch((error) => error);
  // })
  //   .then((response) => JSON.parse(response))
  //   .catch((error) => {
  //     error;
  //   });
};

export const dispenseNowRequest = async (payload) => {
  const machineID = getMachineID();
  // eslint-disable-next-line no-return-await
  const CurrentUser = getCurrentUser();
  return await new Promise((success) => {
    setTimeout(() => {
      const formData = new FormData();
      formData.append(
        'CreatedBy',
        JSON.parse(localStorage.getItem('Haute_current_machine')).uid
      );
      formData.append('ProductCode', payload.items.ProductCode);
      formData.append('Collection', payload.items.Collection);
      formData.append('ProductName', payload.items.ProductName);
      formData.append('ComponentNames', payload.items.ColorCode);
      formData.append('ComponentAmounts', payload.items.Amount);
      formData.append('AmountToDispense', payload.items.TotalDispenseAmount);
      formData.append('AmountToDispenseUnitID', payload.items.DispenseUnit);
      formData.append('MachineID', machineID);
      formData.append('DispenseNumber', payload.items.DispenseNumber);
      formData.append('FormulaDispenseAmount', payload.items.DispenseAmount);
      formData.append('CurrentUserID', CurrentUser.uid);

      api
        .post('Dispense/DispenseNow', formData)
        .then((response) => {
          success(JSON.stringify(response.data));
        })
        .catch((error) => error);
    }, 1000);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

export const getDispenseHistoryListRequest = async (payload) => {
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
      formData.append('machineID', payload.item.machineID);
      api
        .post('Dispense/GetAllDispense', formData)
        .then((response) => {
          if (response.data.resultCode === 'SUCCESS') {
            const DispenseItems = JSON.stringify(response.data.resultObject);
            success(DispenseItems);
          }
        })
        .catch((error) => error);
    }, 1000);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

export const CheckIsCanisterExistsRequest = async (payload) => {
  const userID = getMachineID();
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    setTimeout(() => {
      const formData = new FormData();
      formData.append(
        'CreatedBy',
        JSON.parse(localStorage.getItem('Haute_current_machine')).uid
      );
      formData.append('UserID', payload.userID);
      formData.append('ComponentNames', payload.ComponentNames);
      api
        .post('Dispense/CheckIsCanisterExists', formData)
        .then((response) => {
          success(JSON.stringify(response.data.resultObject));
        })
        .catch((error) => error);
    }, 1000);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

export const DownloadDispenseExcelFileRequest = async (payload) => {
  try {
    const formData = new FormData();
    formData.append('machineID', payload.items);
    const response = await api.post('Dispense/GetDispenseExcelFile', formData, {
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

function* getDispanseListItems({ payload }) {
  try {
    const response = yield call(getDispenseHistoryListRequest, payload);
    yield put(getDispenseHistoryListSuccess(response));
  } catch (error) {
    yield put(getDispenseHistoryListError(error));
  }
}

export function* watchGetList() {
  yield takeEvery(DISPENSEHISTORY_GET_LIST, getDispanseListItems);
}

function* bindDropdowns({ payload }) {
  const response = yield call(bindDropdownsRequest, payload);
  yield put(bindProductDropdownsSuccess(response));
}

function* bindProductGrid({ payload }) {
  const response = yield call(bindProductGridRequest, payload);
  yield put(bindProductGridSuccess(response));
}

function* dispenseNow({ payload }) {
  try {
    const response = yield call(dispenseNowRequest, payload);
    yield put(dispenseNowSuccess(response));
  } catch (error) {
    yield put(dispenseNowSuccess(error));
  }
}
function* dispenseSuccess({ payload }) {
  try {
    const response = yield call(dispenseSuccessRequest, payload);
    yield put(SuccessDispenseSuccess(response));
  } catch (error) {
    yield put(SuccessDispenseSuccess(error));
  }
}

function* GetDispanseData({ payload }) {
  try {
    const response = yield call(GetDispanseDataRequest, payload);
    yield put(GetDispanseDataSuccess(response));
  } catch (error) {
    // yield put(GetDispanseData(error));
  }
}

function* CheckCanisterData({ payload }) {
  try {
    const response = yield call(CheckIsCanisterExistsRequest, payload);
    yield put(CheckCanisterDataSuccess(response));
  } catch (error) {
    // yield put(GetDispanseData(error));
  }
}

function* DownloadDispenseExcelFile({ payload }) {
  try {
    const response = yield call(DownloadDispenseExcelFileRequest, payload);
    yield put(DownloadDispenseExcelFileSuccess(response));
  } catch (error) {
    yield put(DownloadDispenseExcelFileError(error));
  }
}

export function* watchBindDropdowns() {
  yield takeEvery(DISPENSE_PRODUCT_DROPDOWN, bindDropdowns);
}

export function* watchProductGrid() {
  yield takeEvery(DISPENSE_PRODUCT_GRID, bindProductGrid);
}

export function* watchDispenseNow() {
  yield takeEvery(DISPENSE_NOW, dispenseNow);
}
export function* watchDispenseSuccess() {
  yield takeEvery(DISPENSE_SUCCESS, dispenseSuccess);
}

export function* watchGetDispanseData() {
  yield takeEvery(GET_DISPANSE_DATA, GetDispanseData);
}

export function* watchCheckCanisterData() {
  yield takeEvery(CHECK_CANISTER_DATA, CheckCanisterData);
}

export function* watchDownloadDispenseExcelFile() {
  yield takeEvery(DOWNLOAD_DISPENSE_EXCEL_FILE, DownloadDispenseExcelFile);
}

export default function* rootSaga() {
  yield all([
    fork(watchBindDropdowns),
    fork(watchProductGrid),
    fork(watchDispenseNow),
    fork(watchGetList),
    fork(watchDispenseSuccess),
    fork(watchGetDispanseData),
    fork(watchCheckCanisterData),
    fork(watchDownloadDispenseExcelFile),
  ]);
}
