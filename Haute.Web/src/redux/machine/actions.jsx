// eslint-disable-next-line import/no-cycle
import {
  MACHINE_LIST_GET_LIST,
  MACHINE_LIST_GET_LIST_SUCCESS,
  MACHINE_LIST_GET_LIST_ERROR,
  MACHINE_ADD,
  MACHINE_ADD_SUCCESS,
  MACHINE_ADD_ERROR,
  MACHINE_ADD_RESET_STATE,
  MACHINE_GET_BY_ID,
  MACHINE_GET_BY_ID_SUCCESS,
  MACHINE_GET_BY_ID_ERROR,
  MACHINE_DELETE,
  MACHINE_DELETE_SUCCESS,
  MACHINE_DELETE_ERROR,
  DOWNLOAD_MACHINE_EXCEL_FILE,
  DOWNLOAD_MACHINE_EXCEL_FILE_SUCCESS,
  DOWNLOAD_MACHINE_EXCEL_FILE_ERROR,
} from '../actions';

export const getMachineList = (item) => ({
  type: MACHINE_LIST_GET_LIST,
  payload: { item },
});

export const getMachineListSuccess = (items) => ({
  type: MACHINE_LIST_GET_LIST_SUCCESS,
  payload: items,
});

export const getMachineListError = (error) => ({
  type: MACHINE_LIST_GET_LIST_ERROR,
  payload: error,
});

export const deleteMachine = (id) => ({
  type: MACHINE_DELETE,
  payload: { id },
});

export const deleteMachineSuccess = (id) => ({
  type: MACHINE_DELETE_SUCCESS,
  payload: { id },
});

export const deleteMachineError = (id) => ({
  type: MACHINE_DELETE_ERROR,
  payload: { id },
});

export const addMachine = (item) => ({
  type: MACHINE_ADD,
  payload: { item },
});

export const addMachineSuccess = (items) => ({
  type: MACHINE_ADD_SUCCESS,
  payload: { items },
});

export const addMachineError = (error) => ({
  type: MACHINE_ADD_ERROR,
  payload: { error },
});

export const addMachineResetState = () => ({
  type: MACHINE_ADD_RESET_STATE,
});

export const getMachineByID = (id) => ({
  type: MACHINE_GET_BY_ID,
  payload: { id },
});

export const getMachineByIDSuccess = (items) => ({
  type: MACHINE_GET_BY_ID_SUCCESS,
  payload: { items },
});

export const getMachineByIDError = (error) => ({
  type: MACHINE_GET_BY_ID_ERROR,
  payload: { error },
});

export const downloadMachineExcelFile = () => ({
  type: DOWNLOAD_MACHINE_EXCEL_FILE,
});

export const downloadMachineExcelFileSuccess = (items) => ({
  type: DOWNLOAD_MACHINE_EXCEL_FILE_SUCCESS,
  payload: { items },
});

export const downloadMachineExcelFileError = (error) => ({
  type: DOWNLOAD_MACHINE_EXCEL_FILE_ERROR,
  error,
});
