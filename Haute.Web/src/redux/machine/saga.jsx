import { all, takeEvery, put, call, fork } from 'redux-saga/effects';
import api from 'api-config';
import { getCurrentUser } from 'helpers/Utils';
import {
  MACHINE_LIST_GET_LIST,
  MACHINE_GET_BY_ID,
  MACHINE_ADD,
  MACHINE_DELETE,
  deleteMachineSuccess,
  downloadMachineExcelFileSuccess,
  downloadMachineExcelFileError,
  dOWNLOAD_MACHINE_EXCEL_FILE,
  DOWNLOAD_MACHINE_EXCEL_FILE,
} from '../actions';
import {
  getMachineListSuccess,
  getMachineListError,
  addMachineSuccess,
  addMachineError,
  getMachineByIDSuccess,
  getMachineByIDError,
} from './actions';

export const getMachineListRequest = async (payload) => {
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
      api
        .post('Machine/GetAllMachine', formData)
        .then((response) => {
          if (response.data.ResultCode === 'SUCCESS') {
            const machineItems = JSON.stringify(response.data.ResultObject);
            success(machineItems);
          }
        })
        .catch((error) => error);
    }, 1000);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

export const deleteMachineRequest = async (payload) => {
  return new Promise((success) => {
    const formData = new FormData();
    formData.append('id', payload.id.toString());
    api
      .post('Machine/Delete', formData)
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

const addMachineRequest = async (payload) => {
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append('ID', payload.machineID);
    formData.append('MachineRegNo', payload.machineRegNo);
    formData.append('ShopName', payload.shopName);
    formData.append('ShopAddress', payload.shopAddress);
    formData.append('City', payload.city);
    formData.append('State', payload.state);
    formData.append('IsActive', payload.isActive);
    formData.append('MacAddress', payload.macAddress.toString());
    api
      .post('Machine/Save', formData)
      .then((response) => {
        if (
          response.data.resultCode === 'SUCCESS' &&
          (response.data.resultObjectID > 0 || response.data.resultObjectID < 0)
        ) {
          success(response.data);
        } else {
          put(addMachineError(response.data));
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

const GetByIDMachineRequest = async (payload) => {
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append('ID', payload.id);
    api
      .post('Machine/GetByID', formData)
      .then((response) => {
        if (
          response.data.resultCode === 'SUCCESS' &&
          response.data.resultObjectID > 0
        ) {
          success(response.data.resultObject);
        } else {
          put(addMachineError(response));
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

export const DownloadMachineExcelFileRequest = async () => {
  try {
    const response = await api.post('Machine/GetMachineExcelFile', null, {
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
    const response = yield call(getMachineListRequest, payload);
    yield put(getMachineListSuccess(response));
  } catch (error) {
    yield put(getMachineListError(error));
  }
}

function* machineByID({ payload }) {
  try {
    const response = yield call(GetByIDMachineRequest, payload);
    yield put(getMachineByIDSuccess(response));
  } catch (error) {
    yield put(getMachineByIDError(error));
  }
}

function* addMachine({ payload }) {
  const { item } = payload;
  try {
    const response = yield call(addMachineRequest, item);
    if (response.resultObjectID > 0 || response.resultObjectID < 0) {
      yield put(addMachineSuccess(response));
    } else {
      yield put(addMachineError(response));
    }
  } catch (error) {
    yield put(addMachineError(error));
  }
}

function* machineDeleteItems({ payload }) {
  try {
    const response = yield call(deleteMachineRequest, payload);
    yield put(deleteMachineSuccess(response));
  } catch (error) {
    yield put(deleteMachineError(error));
  }
}

function* downloadMachineExcelFile() {
  try {
    const response = yield call(DownloadMachineExcelFileRequest);
    yield put(downloadMachineExcelFileSuccess(response));
  } catch (error) {
    yield put(downloadMachineExcelFileError(error));
  }
}

export function* watchGetList() {
  yield takeEvery(MACHINE_LIST_GET_LIST, getSurveyListItems);
}

export function* watchGetByIDMachine() {
  yield takeEvery(MACHINE_GET_BY_ID, machineByID);
}

export function* watchAddMachine() {
  yield takeEvery(MACHINE_ADD, addMachine);
}

export function* watchDeleteItem() {
  yield takeEvery(MACHINE_DELETE, machineDeleteItems);
}

export function* watchDownloadMachineExcelFile() {
  yield takeEvery(DOWNLOAD_MACHINE_EXCEL_FILE, downloadMachineExcelFile);
}

export default function* rootSaga() {
  yield all([
    fork(watchGetList),
    fork(watchGetByIDMachine),
    fork(watchAddMachine),
    fork(watchDeleteItem),
    fork(watchDownloadMachineExcelFile),
  ]);
}
