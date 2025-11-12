import { all, takeEvery, put, call, fork } from 'redux-saga/effects';
import api from 'api-config';
import { getCurrentUser } from 'helpers/Utils';
import {
  FORMULA_LIST_GET_LIST,
  FORMULA_GET_BY_ID,
  FORMULA_ADD,
  FORMULA_DELETE,
  deleteFormulaSuccess,
  downloadFormulaExcelFileSuccess,
  downloadFormulaExcelFileError,
  DOWNLOAD_FORMULA_EXCEL_FILE,
} from '../actions';
import {
  getFormulaListSuccess,
  getFormulaListError,
  addFormulaSuccess,
  addFormulaError,
  getFormulaByIDSuccess,
  getFormulaByIDError,
} from './actions';

export const getFormulaListRequest = async (payload) => {
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
        .post('Formula/GetAllFormula', formData)
        .then((response) => {
          if (response.data.ResultCode === 'SUCCESS') {
            const formulaItems = JSON.stringify(response.data.ResultObject);
            success(formulaItems);
          }
        })
        .catch((error) => error);
    }, 1000);
  })
    .then((response) => JSON.parse(response))
    .catch((error) => error);
};

export const deleteFormulaRequest = async (payload) => {
  return new Promise((success) => {
    const formData = new FormData();
    formData.append('id', payload.id.toString());
    api
      .post('Formula/Delete', formData)
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

const addFormulaRequest = async (payload) => {
  return await new Promise((success) => {
    let user = getCurrentUser();
    const formData = {
      formulaID: payload.formulaID,
      ProductCode: payload.productCode,
      Amount: payload.dispenseAmount,
      UnitID: 2,
      UserID: user.uid,
      CreatedBy: user.uid.toString(),
      ModifiedBy: user.uid.toString(),
      colorDetail: payload.canisterData.map((detail) => ({
        colorCode: detail.colorCode?.toString()?.trim(),
        dispenseAmount: detail.amount?.toString().trim(),
      })),
    };

    api
      .post('Formula/SaveFormulaProfile', formData)
      .then((response) => {
        if (
          response.data.resultCode === 'SUCCESS' &&
          (response.data.resultObjectID > 0 || response.data.resultObjectID < 0)
        ) {
          success(response.data);
        } else {
          put(addFormulaError(response.data));
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

const GetByIDFormulaRequest = async (payload) => {
  return await new Promise((success) => {
    const formData = new FormData();
    formData.append('ID', payload.id);
    api
      .post('Formula/GetByID', formData)
      .then((response) => {
        if (
          response.data.resultCode === 'SUCCESS' &&
          response.data.resultObjectID > 0
        ) {
          success(response.data.resultObject);
        } else {
          put(addFormulaError(response));
        }
      })
      .catch((error) => error);
  })
    .then((response) => response)
    .catch((error) => error);
};

export const DownloadFormulaExcelFileRequest = async () => {
  try {
    const response = await api.post('Formula/GetFormulaExcelFile', null, {
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

function* getFormulaListItems({ payload }) {
  try {
    const response = yield call(getFormulaListRequest, payload);
    yield put(getFormulaListSuccess(response));
  } catch (error) {
    yield put(getFormulaListError(error));
  }
}

function* getFormulaByID({ payload }) {
  try {
    const response = yield call(GetByIDFormulaRequest, payload);
    yield put(getFormulaByIDSuccess(response));
  } catch (error) {
    yield put(getFormulaByIDError(error));
  }
}

function* addFormula({ payload }) {
  const { item } = payload;
  try {
    const response = yield call(addFormulaRequest, item);

    if (response.resultObjectID > 0 || response.resultObjectID < 0) {
      yield put(addFormulaSuccess(response));
    } else {
      yield put(addFormulaError(response));
    }
  } catch (error) {
    yield put(addFormulaError(error));
  }
}

function* formulaDeleteItems({ payload }) {
  try {
    const response = yield call(deleteFormulaRequest, payload);
    yield put(deleteFormulaSuccess(response));
  } catch (error) {
    yield put(deleteFormulaError(error));
  }
}

function* downloadFormulaExcelFile() {
  try {
    const response = yield call(DownloadFormulaExcelFileRequest);
    yield put(downloadFormulaExcelFileSuccess(response));
  } catch (error) {
    yield put(downloadFormulaExcelFileError(error));
  }
}

export function* watchGetList() {
  yield takeEvery(FORMULA_LIST_GET_LIST, getFormulaListItems);
}

export function* watchGetByIDFormula() {
  yield takeEvery(FORMULA_GET_BY_ID, getFormulaByID);
}

export function* watchAddFormula() {
  yield takeEvery(FORMULA_ADD, addFormula);
}

export function* watchDeleteItem() {
  yield takeEvery(FORMULA_DELETE, formulaDeleteItems);
}

export function* watchDownloadFormulaExcelFile() {
  yield takeEvery(DOWNLOAD_FORMULA_EXCEL_FILE, downloadFormulaExcelFile);
}

export default function* rootSaga() {
  yield all([
    fork(watchGetList),
    fork(watchGetByIDFormula),
    fork(watchAddFormula),
    fork(watchDeleteItem),
    fork(watchDownloadFormulaExcelFile),
  ]);
}
