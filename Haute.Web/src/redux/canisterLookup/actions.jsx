import {
  CANISTER_LOOKUP_LIST_GET_LIST,
  CANISTER_LOOKUP_LIST_GET_LIST_SUCCESS,
  CANISTER_LOOKUP_DELETE_SUCCESS,
  CANISTER_LOOKUP_DELETE,
  CANISTER_LOOKUP_LIST_GET_LIST_ERROR,
  CANISTER_LOOKUP_ADD,
  CANISTER_LOOKUP_ADD_SUCCESS,
  CANISTER_LOOKUP_ADD_ERROR,
  CANISTER_LOOKUP_ADD_RESET_STATE,
  CANISTER_LOOKUP_GET_BY_ID,
  CANISTER_LOOKUP_GET_BY_ID_SUCCESS,
  CANISTER_LOOKUP_GET_BY_ID_ERROR,
  DOWNLOAD_CANISTER_LOOKUP_EXCEL_FILE,
  DOWNLOAD_CANISTER_LOOKUP_EXCEL_FILE_SUCCESS,
  DOWNLOAD_CANISTER_LOOKUP_EXCEL_FILE_ERROR,
} from '../actions';

export const getCanisterLookupList = (item) => ({
  type: CANISTER_LOOKUP_LIST_GET_LIST,
  payload: { item },
});

export const getCanisterLookupListSuccess = (items) => ({
  type: CANISTER_LOOKUP_LIST_GET_LIST_SUCCESS,
  payload: items,
});

export const getCanisterLookupListError = (error) => ({
  type: CANISTER_LOOKUP_LIST_GET_LIST_ERROR,
  payload: error,
});

export const addCanisterLookupResetState = () => ({
  type: CANISTER_LOOKUP_ADD_RESET_STATE,
});

export const deleteCanisterLookup = (id) => ({
  type: CANISTER_LOOKUP_DELETE,
  payload: { id },
});
export const deleteCanisterLookupSuccess = (id) => ({
  type: CANISTER_LOOKUP_DELETE_SUCCESS,
  payload: { id },
});

export const addCanisterLookup = (item) => ({
  type: CANISTER_LOOKUP_ADD,
  payload: { item },
});

export const addCanisterLookupSuccess = (items) => ({
  type: CANISTER_LOOKUP_ADD_SUCCESS,
  payload: { items },
});

export const addCanisterLookupError = (error) => ({
  type: CANISTER_LOOKUP_ADD_ERROR,
  payload: { error },
});

export const getCanisterLookupByID = (id) => ({
  type: CANISTER_LOOKUP_GET_BY_ID,
  payload: { id },
});

export const getCanisterLookupByIDSuccess = (items) => ({
  type: CANISTER_LOOKUP_GET_BY_ID_SUCCESS,
  payload: { items },
});

export const getCanisterLookupByIDError = (error) => ({
  type: CANISTER_LOOKUP_GET_BY_ID_ERROR,
  payload: { error },
});

export const downloadCanisterLookupExcelFile = () => ({
  type: DOWNLOAD_CANISTER_LOOKUP_EXCEL_FILE,
});

export const downloadCanisterLookupExcelFileSuccess = (items) => ({
  type: DOWNLOAD_CANISTER_LOOKUP_EXCEL_FILE_SUCCESS,
  payload: { items },
});

export const downloadCanisterLookupExcelFileError = (error) => ({
  type: DOWNLOAD_CANISTER_LOOKUP_EXCEL_FILE_ERROR,
  error,
});
