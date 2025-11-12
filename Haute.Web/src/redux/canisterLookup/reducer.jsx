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

const INIT_STATE = {
  allCanisterLookupItems: null,
  CanisterLookupData: null,
  error: '',
  isAddError: null,
  loading: false,
  isSuccessfullyAdd: null,
  isSuccessfullyDelete: 0,
  resultMessage: null,
};

export default (state = INIT_STATE, action) => {
  switch (action.type) {
    case CANISTER_LOOKUP_LIST_GET_LIST:
      return { ...state, loading: true };

    case CANISTER_LOOKUP_LIST_GET_LIST_SUCCESS:
      return {
        ...state,
        loading: false,
        CanisterLookupData: null,
        isSuccessfullyDelete: 0,
        allCanisterLookupItems: action.payload,
      };
    case CANISTER_LOOKUP_DELETE:
      return {
        loading: false,
        isSuccessfullyDelete: 0,
      };
    case CANISTER_LOOKUP_DELETE_SUCCESS:
      return {
        loading: false,
        isSuccessfullyDelete: parseInt(action.payload.id, 10),
      };
    case CANISTER_LOOKUP_ADD_RESET_STATE:
      return {
        ...state,
        allCanisterLookupItems: null,
        CanisterLookupData: null,
        error: '',
        loading: false,
        isSuccessfullyAdd: null,
        CanisterLookupExcelData: null,
        isSuccessfullyDelete: 0,
        resultMessage: null,
        isAddError: null,
      };
    case CANISTER_LOOKUP_ADD:
      return {
        ...state,
        loading: true,
        isSuccessfullyAdd: null,
        resultMessage: null,
      };
    case CANISTER_LOOKUP_ADD_SUCCESS:
      return {
        ...state,
        isSuccessfullyAdd: action.payload.items.resultObjectID,
        resultMessage: action.payload.items.resultMessage,
        loading: false,
      };

    case CANISTER_LOOKUP_ADD_ERROR:
      return {
        ...state,
        isAddError: action.payload.items,
        loading: false,
        lookupData: null,
        resultMessage: action.payload.items.resultMessage,
      };
    case CANISTER_LOOKUP_GET_BY_ID:
      return {
        ...state,
        loading: true,
      };

    case CANISTER_LOOKUP_GET_BY_ID_SUCCESS:
      return {
        ...state,
        loading: false,
        CanisterLookupData: action.payload.items,
      };

    case DOWNLOAD_CANISTER_LOOKUP_EXCEL_FILE:
      return {
        ...state,
        loading: true,
      };
    case DOWNLOAD_CANISTER_LOOKUP_EXCEL_FILE_SUCCESS:
      return {
        ...state,
        loading: false,
        CanisterLookupExcelData: action.payload.items,
      };
    case DOWNLOAD_CANISTER_LOOKUP_EXCEL_FILE_ERROR:
      return {
        ...state,
        loading: false,
      };

    default:
      return { ...state };
  }
};
