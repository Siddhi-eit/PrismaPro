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
  FORMULA_GET_BY_ID_SUCCESS,
  FORMULA_GET_BY_ID,
  FORMULA_GET_BY_ID_ERROR,
  DOWNLOAD_FORMULA_EXCEL_FILE,
  DOWNLOAD_FORMULA_EXCEL_FILE_ERROR,
  DOWNLOAD_FORMULA_EXCEL_FILE_SUCCESS,
} from '../actions';

const INIT_STATE = {
  allFormulaItems: null,
  isSucessfullyAdd: null,
  error: '',
  loading: false,
  canisterLookupData: null,
  resultMessage: null,
  isSucessfullyDelete: 0,
  formulaDetail: null,
};

export default (state = INIT_STATE, action) => {
  switch (action.type) {
    case DOWNLOAD_FORMULA_EXCEL_FILE:
      return {
        ...state,
        loading: true,
      };
    case DOWNLOAD_FORMULA_EXCEL_FILE_SUCCESS:
      return {
        ...state,
        loading: false,
        FormulaExcelData: action.payload.items,
      };
    case DOWNLOAD_FORMULA_EXCEL_FILE_ERROR:
      return {
        ...state,
        loading: false,
      };
    case FORMULA_GET_BY_ID:
      return {
        ...state,
        loading: true,
      };
    case FORMULA_GET_BY_ID_SUCCESS:
      return {
        ...state,
        loading: false,
        formulaDetail: action.payload.items,
      };
    case FORMULA_GET_BY_ID_ERROR:
      return {
        ...state,
        loading: false,
        formulaDetail: action.payload.items,
        formulaDetail: null,
      };
    case FORMULA_LIST_GET_LIST:
      return {
        ...state,
        loading: true,
        isSuccessfullyDelete: 0,
        formulaDetail: null,
      };

    case FORMULA_LIST_GET_LIST_SUCCESS:
      return {
        ...state,
        loading: false,
        allFormulaItems: action.payload,
      };
    case FORMULA_LIST_GET_LIST_ERROR:
      return {
        ...state,
        loading: false,
        error: action.payload,
      };
    case FORMULA_ADD:
      return {
        ...state,
        loading: false,
        resultMessage: null,
        lookupData: null,
        isSucessfullyAdd: null,
      };
    case FORMULA_ADD_SUCCESS:
      return {
        ...state,
        isSucessfullyAdd: action.payload.items.resultObjectID,
        loading: false,
        lookupData: null,
        resultMessage: action.payload.items.resultMessage,
      };
    case FORMULA_ADD_ERROR:
      return {
        ...state,
        isAddError: action.payload.items,
        loading: false,
        lookupData: null,
        resultMessage: action.payload.items.resultMessage,
      };
    case FORMULA_ADD_RESET_STATE:
      return {
        ...state,
        isSucessfullyAdd: null,
        loading: false,
        formulaDetail: null,
        isSucessfullyDelete: 0,
        resultMessage: null,
        lookupData: null,
      };
    case FORMULA_DELETE:
      return {
        loading: false,
        isSucessfullyDelete: 0,
      };
    case FORMULA_DELETE_SUCCESS:
      return {
        loading: false,
        isSucessfullyDelete: parseInt(action.payload.id, 10),
      };
    default:
      return { ...state };
  }
};
