import {
  FORMULA_LIST_GET_LIST,
  FORMULA_LIST_GET_LIST_SUCCESS,
  FORMULA_LIST_GET_LIST_ERROR,
  FORMULA_ADD_RESET_STATE,
  FORMULA_ADD,
  FORMULA_ADD_SUCCESS,
  FORMULA_ADD_ERROR,
  FORMULA_DELETE,
  FORMULA_DELETE_SUCCESS,
  FORMULA_GET_BY_ID,
  FORMULA_GET_BY_ID_SUCCESS,
  FORMULA_GET_BY_ID_ERROR,
  DOWNLOAD_FORMULA_EXCEL_FILE,
  DOWNLOAD_FORMULA_EXCEL_FILE_SUCCESS,
  DOWNLOAD_FORMULA_EXCEL_FILE_ERROR,
} from '../actions';

export const downloadFormulaExcelFile = () => ({
  type: DOWNLOAD_FORMULA_EXCEL_FILE,
});

export const downloadFormulaExcelFileSuccess = (items) => ({
  type: DOWNLOAD_FORMULA_EXCEL_FILE_SUCCESS,
  payload: { items },
});

export const downloadFormulaExcelFileError = (error) => ({
  type: DOWNLOAD_FORMULA_EXCEL_FILE_ERROR,
  error,
});

export const getFormulaByIDError = (error) => ({
  type: FORMULA_GET_BY_ID_ERROR,
  payload: { error },
});

export const getFormulaByIDSuccess = (items) => ({
  type: FORMULA_GET_BY_ID_SUCCESS,
  payload: { items },
});

export const getFormulaByID = (id) => ({
  type: FORMULA_GET_BY_ID,
  payload: { id },
});

export const deleteFormula = (id) => ({
  type: FORMULA_DELETE,
  payload: { id },
});

export const deleteFormulaSuccess = (id) => ({
  type: FORMULA_DELETE_SUCCESS,
  payload: { id },
});

export const addFormula = (item) => ({
  type: FORMULA_ADD,
  payload: { item },
});

export const addFormulaSuccess = (items) => ({
  type: FORMULA_ADD_SUCCESS,
  payload: { items },
});

export const addFormulaError = (error) => ({
  type: FORMULA_ADD_ERROR,
  payload: { error },
});

export const getFormulaList = (item) => ({
  type: FORMULA_LIST_GET_LIST,
  payload: { item },
});

export const getFormulaListSuccess = (items) => ({
  type: FORMULA_LIST_GET_LIST_SUCCESS,
  payload: items,
});

export const getFormulaListError = (error) => ({
  type: FORMULA_LIST_GET_LIST_ERROR,
  payload: error,
});

export const addFormulaResetState = () => ({
  type: FORMULA_ADD_RESET_STATE,
});
