// eslint-disable-next-line import/no-cycle
import {
  DISPENSE_PRODUCT_DROPDOWN,
  DISPENSE_PRODUCT_DROPDOWN_SUCESS,
  DISPENSE_PRODUCT_GRID,
  DISPENSE_PRODUCT_GRID_SUCCESS,
  DISPENSE_NOW,
  DISPENSE_NOW_SUCCESS,
  DISPENSE_RESET_STATE,
  DISPENSEHISTORY_GET_LIST,
  DISPENSEHISTORY_GET_LIST_SUCCESS,
  DISPENSEHISTORY_GET_LIST_ERROR,
  SIGNALR_DISPENSE_SUCCESS,
  DISPENSE_SUCCESS_SUCCESS,
  DISPENSE_SUCCESS,
  SIGNALR_DISPENSE_ERROR,
  DISPENSE_LOADING_LOADER,
  GET_DISPANSE_DATA,
  CHECK_CANISTER_DATA,
  CHECK_CANISTER_DATA_SUCCESS,
  DOWNLOAD_DISPENSE_EXCEL_FILE,
  DOWNLOAD_DISPENSE_EXCEL_FILE_SUCCESS,
  DOWNLOAD_DISPENSE_EXCEL_FILE_ERROR,
} from '../actions';

export const bindProductDropdowns = () => ({
  type: DISPENSE_PRODUCT_DROPDOWN,
});

export const bindProductDropdownsSuccess = (items) => ({
  type: DISPENSE_PRODUCT_DROPDOWN_SUCESS,
  payload: { items },
});

export const bindProductGrid = (item) => ({
  type: DISPENSE_PRODUCT_GRID,
  payload: { item },
});

export const bindProductGridSuccess = (items) => ({
  type: DISPENSE_PRODUCT_GRID_SUCCESS,
  payload: { items },
});

export const dispenseNow = (items) => ({
  type: DISPENSE_NOW,
  payload: { items },
});

export const dispenseNowSuccess = (items) => ({
  type: DISPENSE_NOW_SUCCESS,
  payload: { items },
});

export const DisepnseResetState = () => ({
  type: DISPENSE_RESET_STATE,
});
export const getDispenseHistoryList = (item) => ({
  type: DISPENSEHISTORY_GET_LIST,
  payload: { item },
});

export const getDispenseHistoryListSuccess = (items) => ({
  type: DISPENSEHISTORY_GET_LIST_SUCCESS,
  payload: items,
});

export const getDispenseHistoryListError = (error) => ({
  type: DISPENSEHISTORY_GET_LIST_ERROR,
  payload: error,
});

export const signalRDispenseSuccess = (userID) => ({
  type: SIGNALR_DISPENSE_SUCCESS,
  payload: { userID },
});
export const signalRDispenseError = (userID) => ({
  type: SIGNALR_DISPENSE_ERROR,
  payload: userID,
});

export const dispenseSuccess = (items, IsDispense) => ({
  type: DISPENSE_SUCCESS,
  payload: { items, IsDispense },
});

export const SuccessDispenseSuccess = (items) => ({
  type: DISPENSE_SUCCESS_SUCCESS,
  payload: { items },
});
export const DispenseLoadingLoader = () => ({
  type: DISPENSE_LOADING_LOADER,
});

export const GetDispanseData = (tailoringCode, userID) => ({
  type: GET_DISPANSE_DATA,
  payload: { tailoringCode, userID },
});

export const GetDispanseDataSuccess = (items) => ({
  type: GET_DISPANSE_DATA_SUCCESS,
  payload: { items },
});

export const CheckCanisterData = (ComponentNames, userID) => ({
  type: CHECK_CANISTER_DATA,
  payload: { ComponentNames, userID },
});

export const CheckCanisterDataSuccess = (items) => ({
  type: CHECK_CANISTER_DATA_SUCCESS,
  payload: { items },
});

export const DownloadDispenseExcelFile = (items) => ({
  type: DOWNLOAD_DISPENSE_EXCEL_FILE,
  payload: { items },
});

export const DownloadDispenseExcelFileSuccess = (items) => ({
  type: DOWNLOAD_DISPENSE_EXCEL_FILE_SUCCESS,
  payload: { items },
});

export const DownloadDispenseExcelFileError = (error) => ({
  type: DOWNLOAD_DISPENSE_EXCEL_FILE_ERROR,
  error,
});
