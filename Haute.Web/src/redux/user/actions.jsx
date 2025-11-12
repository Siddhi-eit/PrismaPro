// eslint-disable-next-line import/no-cycle
import {
  USER_LIST_GET_LIST,
  USER_LIST_GET_LIST_SUCCESS,
  USER_LIST_GET_LIST_ERROR,
  USER_DELETE_SUCCESS,
  USER_ADD_ERROR,
  USER_ADD,
  USER_ADD_SUCCESS,
  USER_ADD_RESET_STATE,
  USER_GET_BY_ID,
  USER_GET_BY_ID_SUCCESS,
  BIND_MACHINE_DROPDOWN,
  BIND_MACHINE_DROPDOWN_SUCCESS,
  SET_DESKTOP_USERID,
  SET_MACHINE_ID_SUCCESS,
  BIND_USER_TYPE_DROPDOWN,
  BIND_USER_TYPE_DROPDOWN_SUCCESS,
  USER_DELETE,
  BIND_MD_FUSION_LAB_NO_DROPDOWN,
  BIND_MD_FUSION_LAB_NO_DROPDOWN_SUCCESS,
  BIND_MD_FUSION_LAB_NO_DROPDOWN_ERROR,
  DOWNLOAD_USER_EXCEL_FILE,
  DOWNLOAD_USER_EXCEL_FILE_SUCCESS,
  DOWNLOAD_USER_EXCEL_FILE_ERROR,
} from '../actions';

export const getUserList = (item) => ({
  type: USER_LIST_GET_LIST,
  payload: { item },
});

export const getUserListSuccess = (items) => ({
  type: USER_LIST_GET_LIST_SUCCESS,
  payload: items,
});

export const getUserListError = (error) => ({
  type: USER_LIST_GET_LIST_ERROR,
  payload: error,
});

export const deleteUser = (id) => ({
  type: USER_DELETE,
  payload: { id },
});

export const deleteUserSuccess = (id) => ({
  type: USER_DELETE_SUCCESS,
  payload: { id },
});

export const addUser = (item) => ({
  type: USER_ADD,
  payload: { item },
});

export const addUserSuccess = (items) => ({
  type: USER_ADD_SUCCESS,
  payload: { items },
});

export const addUserError = (error) => ({
  type: USER_ADD_ERROR,
  payload: { error },
});

export const addUserResetState = () => ({
  type: USER_ADD_RESET_STATE,
});

export const getUserByID = (id) => ({
  type: USER_GET_BY_ID,
  payload: { id },
});

export const getUserByIDSuccess = (items) => ({
  type: USER_GET_BY_ID_SUCCESS,
  payload: { items },
});

export const getUserByIDError = (error) => ({
  type: USER_GET_BY_ID_SUCCESS,
  payload: { error },
});

export const bindMachineDropdown = (id) => ({
  type: BIND_MACHINE_DROPDOWN,
  payload: { id },
});

export const bindMachineDropdownSuccess = (items) => ({
  type: BIND_MACHINE_DROPDOWN_SUCCESS,
  payload: { items },
});

export const setDesktopUsersID = (id, label) => ({
  type: SET_DESKTOP_USERID,
  payload: { id, label },
});

export const setMachineIDSucess = (items) => ({
  type: SET_MACHINE_ID_SUCCESS,
  payload: { items },
});

export const bindUsertypeDropdown = () => ({
  type: BIND_USER_TYPE_DROPDOWN,
});

export const bindUsertypeDropdownSuccess = (items) => ({
  type: BIND_USER_TYPE_DROPDOWN_SUCCESS,
  payload: { items },
});

export const bindMDFusionLabNoDropdown = (id) => ({
  type: BIND_MD_FUSION_LAB_NO_DROPDOWN,
  payload: { id },
});

export const bindMDFusionLabNoDropdownSuccess = (items) => ({
  type: BIND_MD_FUSION_LAB_NO_DROPDOWN_SUCCESS,
  payload: { items },
});

export const bindMDFusionLabNoDropdownError = (error) => ({
  type: BIND_MD_FUSION_LAB_NO_DROPDOWN_ERROR,
  payload: error,
});

export const DownloadUserExcelFile = () => ({
  type: DOWNLOAD_USER_EXCEL_FILE,
});

export const DownloadUserExcelFileSuccess = (items) => ({
  type: DOWNLOAD_USER_EXCEL_FILE_SUCCESS,
  payload: { items },
});

export const DownloadUserExcelFileError = (error) => ({
  type: DOWNLOAD_USER_EXCEL_FILE_ERROR,
  error,
});
