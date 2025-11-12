// eslint-disable-next-line import/no-cycle
import {
  REFILL_LIST_GET_LIST,
  REFILL_LIST_GET_LIST_SUCCESS,
  REFILL_LIST_GET_LIST_ERROR,
  ADD_REFILL_TRACKING,
  BIND_CAN_SIZE_DROPDOWN,
  BIND_CAN_SIZE_DROPDOWN_SUCCESS,
  ADD_REFILL_TRACKING_SUCCESS,
  ADD_REFILL_TRACKING_ERROR,
  REFILL_TRACKING_RESET_STATE,
  REFILL_TRACKING_GET_BY_ID,
  REFILL_TRACKING_GET_BY_ID_SUCCESS,
  REFILL_TRACKING_GET_BY_ID_ERROR,
  REFILL_TRACKING_DELETE,
  REFILL_TRACKING_DELETE_SUCCESS,
  DOWNLOAD_REFILL_EXCEL_FILE,
  DOWNLOAD_REFILL_EXCEL_FILE_SUCCESS,
  DOWNLOAD_REFILL_EXCEL_FILE_ERROR,
  DOWNLOAD_REFILL_SCANNER,
  DOWNLOAD_REFILL_SCANNER_SUCCESS,
  DOWNLOAD_REFILL_SCANNER_ERROR,
  CLEAR_REFILE_SCANNER,
} from '../actions';

export const getRefillList = (item) => ({
  type: REFILL_LIST_GET_LIST,
  payload: { item },
});

export const getRefillListSuccess = (items) => ({
  type: REFILL_LIST_GET_LIST_SUCCESS,
  payload: items,
});

export const getRefillListError = (error) => ({
  type: REFILL_LIST_GET_LIST_ERROR,
  payload: { error },
});

export const addRefillTraking = (item) => ({
  type: ADD_REFILL_TRACKING,
  payload: { item },
});
export const addRefillTrakingSuccess = (items) => ({
  type: ADD_REFILL_TRACKING_SUCCESS,
  payload: { items },
});

export const addRefillTrakingError = (error) => ({
  type: ADD_REFILL_TRACKING_ERROR,
  payload: { error },
});

export const bindCanSizeDropdown = () => ({
  type: BIND_CAN_SIZE_DROPDOWN,
});

export const bindCanSizeDropdownSuccess = (items) => ({
  type: BIND_CAN_SIZE_DROPDOWN_SUCCESS,
  payload: { items },
});
export const ResetStateRefillTrking = () => ({
  type: REFILL_TRACKING_RESET_STATE,
});
export const RefillTrkingGetByID = (id) => ({
  type: REFILL_TRACKING_GET_BY_ID,
  payload: { id },
});
export const RefillTrkingGetByIDSuccess = (items) => ({
  type: REFILL_TRACKING_GET_BY_ID_SUCCESS,
  payload: { items },
});
export const RefillTrkingGetByIDError = (error) => ({
  type: REFILL_TRACKING_GET_BY_ID_ERROR,
  payload: { error },
});

export const deleteRefillTracking = (id) => ({
  type: REFILL_TRACKING_DELETE,
  payload: { id },
});

export const deleteRefillTrackingSuccess = (id) => ({
  type: REFILL_TRACKING_DELETE_SUCCESS,
  payload: { id },
});

export const downloadRefillExcelFile = () => ({
  type: DOWNLOAD_REFILL_EXCEL_FILE,
});

export const downloadRefillExcelFileSuccess = (items) => ({
  type: DOWNLOAD_REFILL_EXCEL_FILE_SUCCESS,
  payload: { items },
});

export const downloadRefillExcelFileError = (error) => ({
  type: DOWNLOAD_REFILL_EXCEL_FILE_ERROR,
  error,
});

export const downloadRefillScanner = (items) => ({
  type: DOWNLOAD_REFILL_SCANNER,
  payload: { items },
});

export const downloadRefillScannerSuccess = (items) => ({
  type: DOWNLOAD_REFILL_SCANNER_SUCCESS,
  payload: { items },
});

export const downloadRefillScannerError = (error) => ({
  type: DOWNLOAD_REFILL_SCANNER_ERROR,
  error,
});

export const clearRefillScanner = () => ({
  type: CLEAR_REFILE_SCANNER,
});
