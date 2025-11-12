// eslint-disable-next-line import/no-cycle
import {
  CANISTER_LIST_GET_LIST,
  CANISTER_LIST_GET_LIST_SUCCESS,
  CANISTER_LIST_GET_LIST_ERROR,
  CANISTER_DELETE_SUCCESS,
  CANISTER_ADD_ERROR,
  CANISTER_ADD,
  CANISTER_ADD_SUCCESS,
  CANISTER_ADD_RESET_STATE,
  CANISTER_GET_BY_ID,
  CANISTER_GET_BY_ID_SUCCESS,
  BIND_DISPENSE_UNIT_DROPDOWN,
  BIND_DISPENSE_UNIT_DROPDOWN_SUCCESS,
  BIND_PRODUCT_DROPDOWN,
  BIND_PRODUCT_DROPDOWN_SUCCESS,
  BIND_PRODUCT_DROPDOWN_ERROR,
  CANISTER_DELETE,
  DOWNLOAD_CANISTER_EXCEL_FILE,
  DOWNLOAD_CANISTER_EXCEL_FILE_SUCCESS,
  DOWNLOAD_CANISTER_EXCEL_FILE_ERROR,
  CANISTER_LOOKUP_GET_BY_CANISTER_CODE,
  CANISTER_LOOKUP_GET_BY_CANISTER_CODE_SUCCESS,
  CANISTER_LOOKUP_GET_BY_CANISTER_CODE_ERROR,
} from '../actions';

export const getCanisterList = (item) => ({
  type: CANISTER_LIST_GET_LIST,
  payload: { item },
});

export const getCanisterListSuccess = (items) => ({
  type: CANISTER_LIST_GET_LIST_SUCCESS,
  payload: items,
});

export const getCanisterListError = (error) => ({
  type: CANISTER_LIST_GET_LIST_ERROR,
  payload: error,
});

export const deleteCanister = (id) => ({
  type: CANISTER_DELETE,
  payload: { id },
});
export const deleteCanisterSuccess = (id) => ({
  type: CANISTER_DELETE_SUCCESS,
  payload: { id },
});

export const addCanister = (item) => ({
  type: CANISTER_ADD,
  payload: { item },
});

export const addCanisterSuccess = (items) => ({
  type: CANISTER_ADD_SUCCESS,
  payload: { items },
});

export const addCanisterError = (error) => ({
  type: CANISTER_ADD_ERROR,
  payload: { error },
});
export const addCanisterResetState = () => ({
  type: CANISTER_ADD_RESET_STATE,
});

export const getByID = (id) => ({
  type: CANISTER_GET_BY_ID,
  payload: { id },
});
export const getByIDSuccess = (items) => ({
  type: CANISTER_GET_BY_ID_SUCCESS,
  payload: { items },
});
export const getByIDError = (error) => ({
  type: CANISTER_GET_BY_ID_SUCCESS,
  payload: { error },
});

export const bindDispenseUnitDropdown = () => ({
  type: BIND_DISPENSE_UNIT_DROPDOWN,
});

export const bindDispenseUnitDropdownSuccess = (items) => ({
  type: BIND_DISPENSE_UNIT_DROPDOWN_SUCCESS,
  payload: { items },
});

export const bindProductDropdown = () => ({
  type: BIND_PRODUCT_DROPDOWN,
});

export const bindProductDropdownSuccess = (items) => ({
  type: BIND_PRODUCT_DROPDOWN_SUCCESS,
  payload: { items },
});

export const bindProductDropdownError = (items) => ({
  type: BIND_PRODUCT_DROPDOWN_ERROR,
  payload: { items },
});

export const downloadCanisterExcelFile = () => ({
  type: DOWNLOAD_CANISTER_EXCEL_FILE,
});

export const downloadCanisterExcelFileSuccess = (items) => ({
  type: DOWNLOAD_CANISTER_EXCEL_FILE_SUCCESS,
  payload: { items },
});

export const downloadCanisterExcelFileError = (error) => ({
  type: DOWNLOAD_CANISTER_EXCEL_FILE_ERROR,
  error,
});

export const getCanisterLookupByCanisterCode = () => ({
  type: CANISTER_LOOKUP_GET_BY_CANISTER_CODE,
});

export const getCanisterLookupByCanisterCodeSuccess = (items) => ({
  type: CANISTER_LOOKUP_GET_BY_CANISTER_CODE_SUCCESS,
  payload: { items },
});

export const getCanisterLookupByCanisterCodeError = (items) => ({
  type: CANISTER_LOOKUP_GET_BY_CANISTER_CODE_ERROR,
  payload: { items },
});
