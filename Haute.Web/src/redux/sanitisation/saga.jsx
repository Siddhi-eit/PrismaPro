import { all, takeEvery, put, call, fork } from 'redux-saga/effects';
import api from 'api-config';
import { getMachineID } from 'helpers/Utils';
import {
  addSanitisationTrakingError,
  addSanitisationTrakingSuccess,
  ADD_SANITISATION_TRACKING,
  bindCanisterNoDropdownError,
  bindCanisterNoDropdownSuccess,
  BIND_CANISTER_NO_DROPDOWN,
  deleteSanitisationTrakingSuccess,
  SanitisationTrakingGetByIDSuccess,
  SANITISATION_DELETE,
  SANITISATION_GET_BY_ID,
  SANITISATION_LIST_GET_LIST,
  DOWNLOAD_SANITISATION_EXCEL_FILE,
  downloadSanitisationExcelFileSuccess,
  downloadSanitisationExcelFileError,
} from '../actions';
import {
  getSanitisationListSuccess,
  getSanitisationListError,
  SanitisationTrakingGetByIDError,
} from './actions';

export const getSanitisationList = async (payload) => {
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
        .post('SanitisingTraking/GetAll', formData)
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

const BindCanisterNoDropdownRequest = async (id) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append('machineID', id);
    api
      .post('Canisters/getByUserID', formData)
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

const addSanitisationTrakinRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append(
      'CreatedBy',
      JSON.parse(localStorage.getItem('Haute_current_machine')).uid
    );
    formData.append('ID', payload.id);
    formData.append('FusionLabNo', payload.fusionLabNo);
    formData.append('CanisterID', payload.canisterID);
    formData.append('RefillingPeriod', payload.refillingPeriod);
    // formData.append('ProductID', payload.product);
    formData.append('IsActive', payload.isActive);
    formData.append('UserID', payload.userID);
    formData.append('MachineID', payload.machineID);
    api
      .post('SanitisingTraking/UpsertSanitisingTraking', formData)
      .then((response) => {
        success(JSON.stringify(response.data));
      })
      .catch((error) => error);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};
const GetByIDSanisationRequest = async (payload) => {
  // eslint-disable-next-line no-return-await
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append('id', payload.id);
    api
      .post('SanitisingTraking/GetByID', formData)
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
export const deleteSanitisingTrakingRequest = async (payload) => {
  return new Promise((success) => {
    const formData = new FormData();
    formData.append(
      'userId',
      JSON.parse(localStorage.getItem('Haute_current_machine')).uid
    );
    formData.append('id', payload.id.toString());
    api
      .post('SanitisingTraking/Delete', formData)
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

export const DownloadSanitisationExcelFileRequest = async () => {
  try {
    const response = await api.post(
      'SanitisingTraking/GetSanitisingExcelFile',
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

function* addSanitisationTraking({ payload }) {
  const { item } = payload;
  try {
    const response = yield call(addSanitisationTrakinRequest, item);
    // if (response.resultObjectID > 0 || response.resultObjectID === -1 || response.resultObjectID === -2) {
    yield put(addSanitisationTrakingSuccess(response));
    // } else {
    //   yield put(addSanitisationTrakingError(response));
    // }
  } catch (error) {
    yield put(addSanitisationTrakingError(error));
  }
}

function* getSanitisationListItems({ payload }) {
  try {
    const response = yield call(getSanitisationList, payload);
    yield put(getSanitisationListSuccess(response));
  } catch (error) {
    yield put(getSanitisationListError(error));
  }
}
function* bindCaniterNoDropdown({ payload }) {
  try {
    const response = yield call(BindCanisterNoDropdownRequest, payload);
    yield put(bindCanisterNoDropdownSuccess(response));
  } catch (error) {
    yield put(bindCanisterNoDropdownError(error));
  }
}
function* sanitisationByID({ payload }) {
  try {
    const response = yield call(GetByIDSanisationRequest, payload);
    yield put(SanitisationTrakingGetByIDSuccess(response));
  } catch (error) {
    yield put(SanitisationTrakingGetByIDError(error));
  }
}
function* sanitisationDeleteItems({ payload }) {
  try {
    const response = yield call(deleteSanitisingTrakingRequest, payload);
    yield put(deleteSanitisationTrakingSuccess(response));
  } catch (error) {
    yield put(SanitisationTrakingGetByIDError(error));
  }
}

function* downloadSanitisationExcelFile() {
  try {
    const response = yield call(DownloadSanitisationExcelFileRequest);
    yield put(downloadSanitisationExcelFileSuccess(response));
  } catch (error) {
    yield put(downloadSanitisationExcelFileError(error));
  }
}

export function* watchGetList() {
  yield takeEvery(SANITISATION_LIST_GET_LIST, getSanitisationListItems);
}
export function* wathcBindCanisterNoDropdown() {
  yield takeEvery(BIND_CANISTER_NO_DROPDOWN, bindCaniterNoDropdown);
}
export function* wathcAddSanitisationTraking() {
  yield takeEvery(ADD_SANITISATION_TRACKING, addSanitisationTraking);
}
export function* wathcGetByIDSanitisation() {
  yield takeEvery(SANITISATION_GET_BY_ID, sanitisationByID);
}

export function* wathcDeleteItem() {
  yield takeEvery(SANITISATION_DELETE, sanitisationDeleteItems);
}

export function* watchDownloadSanitisationExcelFile() {
  yield takeEvery(
    DOWNLOAD_SANITISATION_EXCEL_FILE,
    downloadSanitisationExcelFile
  );
}

export default function* rootSaga() {
  yield all([
    fork(watchGetList),
    fork(wathcBindCanisterNoDropdown),
    fork(wathcAddSanitisationTraking),
    fork(wathcGetByIDSanitisation),
    fork(wathcDeleteItem),
    fork(watchDownloadSanitisationExcelFile),
  ]);
}
