import { getMachineID } from 'helpers/Utils';
import {
  REFILL_LIST_GET_LIST,
  REFILL_LIST_GET_LIST_SUCCESS,
  REFILL_LIST_GET_LIST_ERROR,
  BIND_CAN_SIZE_DROPDOWN,
  BIND_CAN_SIZE_DROPDOWN_SUCCESS,
  ADD_REFILL_TRACKING,
  ADD_REFILL_TRACKING_SUCCESS,
  ADD_REFILL_TRACKING_ERROR,
  REFILL_TRACKING_RESET_STATE,
  REFILL_TRACKING_GET_BY_ID,
  REFILL_TRACKING_GET_BY_ID_SUCCESS,
  REFILL_TRACKING_DELETE,
  REFILL_TRACKING_DELETE_SUCCESS,
  DOWNLOAD_REFILL_EXCEL_FILE,
  DOWNLOAD_REFILL_EXCEL_FILE_SUCCESS,
  DOWNLOAD_REFILL_EXCEL_FILE_ERROR,
  DOWNLOAD_REFILL_SCANNER,
  DOWNLOAD_REFILL_SCANNER_SUCCESS,
  DOWNLOAD_REFILL_SCANNER_ERROR,
  CLEAR_REFILE_SCANNER,
} from '../actions';

const INIT_STATE = {
  allRefillItems: null,
  error: '',
  loading: false,
  isSucessfullyDelete: 0,
  isAddError: '',
  isSucessfullyAdd: null,
  machineID: getMachineID(),
  resultMessage: null,
  userTypeDropdownList: [],
  canSizeDropdownList: [],
  refillDetail: null,
  refillPDFData: null,
  refillScannerData: null,
};

export default (state = INIT_STATE, action) => {
  switch (action.type) {
    case REFILL_LIST_GET_LIST:
      return {
        ...state,
        loading: true,
        isSucessfullyDelete: 0,
        refillScannerData: null,
      };

    case REFILL_LIST_GET_LIST_SUCCESS:
      return {
        ...state,
        loading: false,
        allRefillItems: action.payload,
        refillScannerData: null,
      };
    case REFILL_LIST_GET_LIST_ERROR:
      return {
        ...state,
        loading: false,
        error: action.payload,
      };
    case BIND_CAN_SIZE_DROPDOWN:
      return {
        ...state,
        refillScannerData: null,
      };
    case BIND_CAN_SIZE_DROPDOWN_SUCCESS:
      return {
        ...state,
        canSizeDropdownList: action.payload.items,
        refillScannerData: null,
      };

    case ADD_REFILL_TRACKING:
      return {
        ...state,
        loading: false,
        isSucessfullyAdd: null,
        refillPDFData: null,
        refillScannerData: null,
      };
    case ADD_REFILL_TRACKING_SUCCESS:
      return {
        ...state,
        isSucessfullyAdd: action.payload.items.resultObjectID,
        resultMessage: action.payload.items.resultMessage,
        loading: false,
        refillPDFData: action.payload.items.resultObject,
        refillScannerData: null,
      };
    case ADD_REFILL_TRACKING_ERROR:
      return {
        ...state,
        isAddError: action.payload.items,
        resultMessage: action.payload.items.resultMessage,
        loading: false,
        refillScannerData: null,
      };
    case REFILL_TRACKING_RESET_STATE:
      return {
        ...state,
        isSucessfullyAdd: null,
        loading: false,
        resultMessage: null,
        isSucessfullyDelete: 0,
        refillDetail: null,
        refillPDFData: null,
        refillScannerData: null,
      };
    case REFILL_TRACKING_GET_BY_ID:
      return {
        ...state,
        loading: true,
        refillScannerData: null,
      };

    case REFILL_TRACKING_GET_BY_ID_SUCCESS:
      return {
        ...state,
        loading: false,
        refillDetail: action.payload.items,
        refillScannerData: null,
      };
    case REFILL_TRACKING_DELETE:
      return {
        loading: false,
        isSucessfullyDelete: 0,
      };
    case REFILL_TRACKING_DELETE_SUCCESS:
      return {
        loading: false,
        isSucessfullyDelete: parseInt(action.payload.id, 10),
      };
    case DOWNLOAD_REFILL_EXCEL_FILE:
      return {
        ...state,
        loading: true,
        refillScannerData: null,
      };
    case DOWNLOAD_REFILL_EXCEL_FILE_SUCCESS:
      return {
        ...state,
        loading: false,
        refillExcelData: action.payload.items,
      };
    case DOWNLOAD_REFILL_EXCEL_FILE_ERROR:
      return {
        ...state,
        loading: false,
      };
    case DOWNLOAD_REFILL_SCANNER:
      return {
        ...state,
        loading: true,
      };
    case DOWNLOAD_REFILL_SCANNER_SUCCESS:
      return {
        ...state,
        loading: false,
        refillScannerData: action.payload.items,
      };
    case DOWNLOAD_REFILL_SCANNER_ERROR:
      return {
        ...state,
        loading: false,
      };
    case CLEAR_REFILE_SCANNER:
      return {
        ...state,
        refillScannerData: null,
      };
    default:
      return { ...state };
  }
};
