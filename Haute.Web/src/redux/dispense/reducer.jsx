import {
  DISPENSE_PRODUCT_DROPDOWN,
  DISPENSE_PRODUCT_DROPDOWN_SUCESS,
  DISPENSE_PRODUCT_GRID,
  DISPENSE_PRODUCT_GRID_SUCCESS,
  DISPENSE_NOW,
  DISPENSE_NOW_SUCCESS,
  DISPENSE_RESET_STATE,
  DISPENSEHISTORY_GET_LIST,
  DISPENSEHISTORY_GET_LIST_SUCCESS,
  DISPENSEHISTORY_GET_LIST_ERROR,
  SIGNALR_DISPENSE_SUCCESS,
  DISPENSE_SUCCESS,
  GET_DISPANSE_DATA,
  DISPENSE_SUCCESS_SUCCESS,
  SIGNALR_DISPENSE_ERROR,
  DISPENSE_LOADING_LOADER,
  GET_DISPANSE_DATA_SUCCESS,
  CHECK_CANISTER_DATA,
  CHECK_CANISTER_DATA_SUCCESS,
  DOWNLOAD_DISPENSE_EXCEL_FILE,
  DOWNLOAD_DISPENSE_EXCEL_FILE_SUCCESS,
  DOWNLOAD_DISPENSE_EXCEL_FILE_ERROR,
} from '../actions';

const INIT_STATE = {
  productDropdowns: [],
  productGrid: [],
  allDispenseItems: null,
  dispenseNowMsg: '',
  isSucessfullyAdd: null,
  loading: false,
  isLoadingProductGrid: false,
  resultMessage: null,
  isSignalRSuccess: false,
  isSignalRError: false,
  dispenseSuccessIsSucessfullyAdd: null,
  dispenseSuccessResultMessage: null,
};

export default (state = INIT_STATE, action) => {
  switch (action.type) {
    case DISPENSE_PRODUCT_DROPDOWN:
      return {
        ...state,
      };
    case DISPENSE_PRODUCT_DROPDOWN_SUCESS:
      return {
        ...state,
        productDropdowns: action.payload.items,
      };
    case DISPENSE_PRODUCT_GRID:
      return {
        ...state,
        isLoadingProductGrid: true,
      };
    case DISPENSE_PRODUCT_GRID_SUCCESS:
      return {
        ...state,
        productGrid: action.payload.items,
        isLoadingProductGrid: false,
      };
    case DISPENSE_NOW:
      return {
        ...state,
        loading: true,
        isLoadingProductGrid: false,
        isSucessfullyAdd: null,
      };
    case DISPENSE_NOW_SUCCESS:
      return {
        ...state,
        loading: true,
        isSucessfullyAdd: action.payload.items.resultObjectID,
        resultMessage: action.payload.items.resultMessage,
      };
    case DISPENSE_RESET_STATE:
      return {
        ...state,
        productGrid: [],
        isLoadingProductGrid: false,
        isSucessfullyAdd: null,
        isSignalRSuccess: false,
        isSignalRError: false,
        dispenseSuccessResultMessage: null,
        dispenseSuccessIsSucessfullyAdd: null,
        loading: false,
      };
    case DISPENSEHISTORY_GET_LIST:
      return { ...state, loading: true };

    case DISPENSEHISTORY_GET_LIST_SUCCESS:
      return {
        ...state,
        loading: false,
        allDispenseItems: action.payload,
        isSucessfullyAdd: null,
        productGrid: [],
      };
    case DISPENSEHISTORY_GET_LIST_ERROR:
      return {
        ...state,
        loading: false,
        error: action.payload,
      };
    case SIGNALR_DISPENSE_SUCCESS:
      return {
        ...state,
        isSignalRSuccess: true,
        dispense_success_data: action.payload,
        isSucessfullyAdd: null,
      };
    case SIGNALR_DISPENSE_ERROR:
      return {
        ...state,
        isSignalRError: true,
        isSucessfullyAdd: null,
      };
    case DISPENSE_SUCCESS:
      return {
        ...state,
        loading: true,
        isLoadingProductGrid: false,
        dispenseSuccessIsSucessfullyAdd: null,
      };
    case DISPENSE_SUCCESS_SUCCESS:
      return {
        ...state,
        loading: false,
        isSucessfullyAdd: null,
        dispenseSuccessIsSucessfullyAdd: action.payload.items.resultObjectID,
        dispenseSuccessResultMessage: action.payload.items.resultMessage,
      };
    case GET_DISPANSE_DATA:
      return {
        ...state,
        loading: true,
        isLoadingProductGrid: false,
        GetDispanseDataSuccess: null,
      };
    case GET_DISPANSE_DATA_SUCCESS:
      return {
        ...state,
        loading: true,
        isLoadingProductGrid: false,
        GetDispanseDataSuccess: action.payload.items.resultObjectID,
      };
    case CHECK_CANISTER_DATA:
      return {
        ...state,
        loading: true,
        isLoadingProductGrid: false,
        CheckCanisterDataSuccess: null,
      };
    case CHECK_CANISTER_DATA_SUCCESS:
      return {
        ...state,
        loading: false,
        isLoadingProductGrid: false,
        CheckCanisterDataSuccess: action.payload.items,
      };
    case DISPENSE_LOADING_LOADER:
      return {
        ...state,
        loading: false,
      };
    case DOWNLOAD_DISPENSE_EXCEL_FILE:
      return {
        ...state,
        loading: true,
      };
    case DOWNLOAD_DISPENSE_EXCEL_FILE_SUCCESS:
      return {
        ...state,
        loading: false,
        DispenseExcelData: action.payload.items,
      };
    case DOWNLOAD_DISPENSE_EXCEL_FILE_ERROR:
      return {
        ...state,
        loading: false,
      };
    default:
      return { ...state };
  }
};
