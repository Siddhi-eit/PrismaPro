import {
  CANISTER_LIST_GET_LIST,
  CANISTER_LIST_GET_LIST_SUCCESS,
  CANISTER_LIST_GET_LIST_ERROR,
  CANISTER_DELETE_SUCCESS,
  CANISTER_ADD,
  CANISTER_ADD_SUCCESS,
  CANISTER_ADD_ERROR,
  CANISTER_ADD_RESET_STATE,
  CANISTER_GET_BY_ID,
  CANISTER_GET_BY_ID_SUCCESS,
  BIND_DISPENSE_UNIT_DROPDOWN,
  BIND_DISPENSE_UNIT_DROPDOWN_SUCCESS,
  CANISTER_DELETE,
  BIND_PRODUCT_DROPDOWN,
  BIND_PRODUCT_DROPDOWN_SUCCESS,
  BIND_PRODUCT_DROPDOWN_ERROR,
  DOWNLOAD_CANISTER_EXCEL_FILE,
  DOWNLOAD_CANISTER_EXCEL_FILE_SUCCESS,
  DOWNLOAD_CANISTER_EXCEL_FILE_ERROR,
  CANISTER_LOOKUP_GET_BY_CANISTER_CODE,
  CANISTER_LOOKUP_GET_BY_CANISTER_CODE_SUCCESS,
  CANISTER_LOOKUP_GET_BY_CANISTER_CODE_ERROR,
} from '../actions';

const INIT_STATE = {
  allCanisterItems: null,
  error: '',
  loading: false,
  selectedItems: [],
  isSucessfullyDelete: 0,
  isAddError: '',
  isSucessfullyAdd: null,
  canisterDetail: null,
  dispenseUnitDropdownList: [],
  resultMessage: null,
  lookupData: null,
};

export default (state = INIT_STATE, action) => {
  switch (action.type) {
    case CANISTER_LIST_GET_LIST:
      return { ...state, loading: true, isSucessfullyDelete: 0 };

    case CANISTER_LIST_GET_LIST_SUCCESS:
      return {
        ...state,
        loading: false,
        allCanisterItems: action.payload,
        lookupData: null,
      };
    case CANISTER_LIST_GET_LIST_ERROR:
      return {
        ...state,
        loading: false,
        error: action.payload,
      };
    case CANISTER_DELETE:
      return {
        loading: false,
        isSucessfullyDelete: 0,
      };
    case CANISTER_DELETE_SUCCESS:
      return {
        loading: false,
        isSucessfullyDelete: parseInt(action.payload.id, 10),
      };
    case CANISTER_ADD:
      return {
        ...state,
        loading: false,
        resultMessage: null,
        lookupData: null,
        isSucessfullyAdd: null,
      };
    case CANISTER_ADD_SUCCESS:
      return {
        ...state,
        isSucessfullyAdd: action.payload.items.resultObjectID,
        loading: false,
        lookupData: null,
        resultMessage: action.payload.items.resultMessage,
      };
    case CANISTER_ADD_ERROR:
      return {
        ...state,
        isAddError: action.payload.items,
        loading: false,
        lookupData: null,
        resultMessage: action.payload.items.resultMessage,
      };
    case CANISTER_GET_BY_ID:
      return {
        ...state,
        loading: true,
      };
    case CANISTER_GET_BY_ID_SUCCESS:
      return {
        ...state,
        loading: false,
        lookupData: null,
        canisterDetail: action.payload.items,
      };
    case CANISTER_ADD_RESET_STATE:
      return {
        ...state,
        isSucessfullyAdd: null,
        loading: false,
        canisterDetail: null,
        isSucessfullyDelete: 0,
        resultMessage: null,
        lookupData: null,
      };
    case BIND_DISPENSE_UNIT_DROPDOWN:
      return {
        ...state,
      };
    case BIND_DISPENSE_UNIT_DROPDOWN_SUCCESS:
      return {
        ...state,
        dispenseUnitDropdownList: action.payload.items,
      };
    case BIND_PRODUCT_DROPDOWN:
      return {
        ...state,
      };
    case BIND_PRODUCT_DROPDOWN_SUCCESS:
      return {
        ...state,
        productDropdownList: action.payload.items,
      };
    case BIND_PRODUCT_DROPDOWN_ERROR:
      return {
        ...state,
        productDropdownList: action.payload.items,
      };
    case DOWNLOAD_CANISTER_EXCEL_FILE:
      return {
        ...state,
        loading: true,
      };
    case DOWNLOAD_CANISTER_EXCEL_FILE_SUCCESS:
      return {
        ...state,
        loading: false,
        CanisterExcelData: action.payload.items,
      };
    case DOWNLOAD_CANISTER_EXCEL_FILE_ERROR:
      return {
        ...state,
        loading: false,
      };
    case CANISTER_LOOKUP_GET_BY_CANISTER_CODE:
      return {
        ...state,
        loading: true,
      };

    case CANISTER_LOOKUP_GET_BY_CANISTER_CODE_SUCCESS:
      return {
        ...state,
        loading: false,
        lookupData: action.payload.items,
      };
    case CANISTER_LOOKUP_GET_BY_CANISTER_CODE_ERROR:
      return {
        ...state,
        loading: false,
      };

    default:
      return { ...state };
  }
};
