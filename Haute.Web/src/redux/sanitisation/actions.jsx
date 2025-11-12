// eslint-disable-next-line import/no-cycle
import {
  SANITISATION_LIST_GET_LIST,
  SANITISATION_LIST_GET_LIST_SUCCESS,
  SANITISATION_LIST_GET_LIST_ERROR,
  BIND_CANISTER_NO_DROPDOWN,
  BIND_CANISTER_NO_DROPDOWN_SUCCESS,
  BIND_CANISTER_NO_DROPDOWN_ERROR,
  ADD_SANITISATION_TRACKING,
  ADD_SANITISATION_TRACKING_SUCCESS,
  SANITISATION_TRACKING_RESET_STATE,
  SANITISATION_GET_BY_ID,
  SANITISATION_GET_BY_ID_SUCCESS,
  SANITISATION_GET_BY_ID_ERROR,
  SANITISATION_DELETE_SUCCESS,
  SANITISATION_DELETE,
  DOWNLOAD_SANITISATION_EXCEL_FILE,
  DOWNLOAD_SANITISATION_EXCEL_FILE_SUCCESS,
  DOWNLOAD_SANITISATION_EXCEL_FILE_ERROR,
} from '../actions';

export const getSanitisationList = (item) => ({
  type: SANITISATION_LIST_GET_LIST,
  payload: { item },
});

export const getSanitisationListSuccess = (items) => ({
  type: SANITISATION_LIST_GET_LIST_SUCCESS,
  payload: items,
});

export const getSanitisationListError = (error) => ({
  type: SANITISATION_LIST_GET_LIST_ERROR,
  payload: error,
});

export const bindCanisterNoDropdown = (id) => ({
  type: BIND_CANISTER_NO_DROPDOWN,
  payload: id,
});
export const bindCanisterNoDropdownSuccess = (items) => ({
  type: BIND_CANISTER_NO_DROPDOWN_SUCCESS,
  payload: { items },
});
export const bindCanisterNoDropdownError = (error) => ({
  type: BIND_CANISTER_NO_DROPDOWN_ERROR,
  payload: error,
});

export const addSanitisationTraking = (item) => ({
  type: ADD_SANITISATION_TRACKING,
  payload: { item },
});

export const addSanitisationTrakingSuccess = (items) => ({
  type: ADD_SANITISATION_TRACKING_SUCCESS,
  payload: { items },
});

export const addSanitisationTrakingError = (error) => ({
  type: ADD_SANITISATION_TRACKING_SUCCESS,
  payload: { error },
});

export const ResetState = () => ({
  type: SANITISATION_TRACKING_RESET_STATE,
});

export const SanitisationTrakingByID = (id) => ({
  type: SANITISATION_GET_BY_ID,
  payload: { id },
});
export const SanitisationTrakingGetByIDSuccess = (items) => ({
  type: SANITISATION_GET_BY_ID_SUCCESS,
  payload: { items },
});
export const SanitisationTrakingGetByIDError = (error) => ({
  type: SANITISATION_GET_BY_ID_ERROR,
  payload: { error },
});

export const deleteSanitisationTraking = (id) => ({
  type: SANITISATION_DELETE,
  payload: { id },
});

export const deleteSanitisationTrakingSuccess = (id) => ({
  type: SANITISATION_DELETE_SUCCESS,
  payload: { id },
});

export const downloadSanitisationExcelFile = () => ({
  type: DOWNLOAD_SANITISATION_EXCEL_FILE,
});

export const downloadSanitisationExcelFileSuccess = (items) => ({
  type: DOWNLOAD_SANITISATION_EXCEL_FILE_SUCCESS,
  payload: { items },
});

export const downloadSanitisationExcelFileError = (error) => ({
  type: DOWNLOAD_SANITISATION_EXCEL_FILE_ERROR,
  error,
});
