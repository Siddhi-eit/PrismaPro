import { getMachineID } from 'helpers/Utils';
import {
  SANITISATION_LIST_GET_LIST,
  SANITISATION_LIST_GET_LIST_SUCCESS,
  SANITISATION_LIST_GET_LIST_ERROR,
  BIND_CANISTER_NO_DROPDOWN,
  BIND_CANISTER_NO_DROPDOWN_SUCCESS,
  ADD_SANITISATION_TRACKING,
  ADD_SANITISATION_TRACKING_SUCCESS,
  ADD_SANITISATION_TRACKING_ERROR,
  SANITISATION_TRACKING_RESET_STATE,
  SANITISATION_GET_BY_ID,
  SANITISATION_GET_BY_ID_SUCCESS,
  SANITISATION_DELETE,
  SANITISATION_DELETE_SUCCESS,
  DOWNLOAD_SANITISATION_EXCEL_FILE,
  DOWNLOAD_SANITISATION_EXCEL_FILE_SUCCESS,
  DOWNLOAD_SANITISATION_EXCEL_FILE_ERROR,
} from '../actions';

const INIT_STATE = {
  allSanitisationItems: null,
  error: '',
  loading: false,
  selectedItems: [],
  isSucessfullyDelete: 0,
  isAddError: '',
  isSucessfullyAdd: null,
  userDetail: null,
  machineDropdownList: [],
  machineID: getMachineID(),
  resultMessage: null,
  userTypeDropdownList: [],
  canisterNoDropdownList: [],
  sanitisationDetail: null,
  sanitisationPDFData: null,
};

export default (state = INIT_STATE, action) => {
  switch (action.type) {
    case SANITISATION_LIST_GET_LIST:
      return { ...state, loading: true, isSucessfullyDelete: 0 };
    case SANITISATION_LIST_GET_LIST_SUCCESS:
      return {
        ...state,
        loading: false,
        allSanitisationItems: action.payload,
      };
    case SANITISATION_LIST_GET_LIST_ERROR:
      return {
        ...state,
        loading: false,
        error: action.payload,
      };
    case BIND_CANISTER_NO_DROPDOWN:
      return {
        ...state,
      };
    case BIND_CANISTER_NO_DROPDOWN_SUCCESS:
      return {
        ...state,
        canisterNoDropdownList: action.payload.items,
      };
    case ADD_SANITISATION_TRACKING:
      return {
        ...state,
        loading: false,
        isSucessfullyAdd: null,
        sanitisationPDFData: null,
      };
    case ADD_SANITISATION_TRACKING_SUCCESS:
      return {
        ...state,
        isSucessfullyAdd: action.payload.items.resultObjectID,
        resultMessage: action.payload.items.resultMessage,
        loading: false,
        sanitisationPDFData: action.payload.items.resultObject,
      };
    case ADD_SANITISATION_TRACKING_ERROR:
      return {
        ...state,
        isAddError: action.payload.items,
        resultMessage: action.payload.items.resultMessage,
        loading: false,
      };
    case SANITISATION_TRACKING_RESET_STATE:
      return {
        ...state,
        isSucessfullyAdd: null,
        loading: false,
        resultMessage: null,
        isSucessfullyDelete: 0,
        sanitisationDetail: null,
        sanitisationPDFData: null,
      };
    case SANITISATION_GET_BY_ID:
      return {
        ...state,
        loading: true,
      };
    case SANITISATION_GET_BY_ID_SUCCESS:
      return {
        ...state,
        loading: false,
        sanitisationDetail: action.payload.items,
      };
    case SANITISATION_DELETE:
      return {
        loading: false,
        isSucessfullyDelete: 0,
      };
    case SANITISATION_DELETE_SUCCESS:
      return {
        loading: false,
        isSucessfullyDelete: parseInt(action.payload.id, 10),
      };
    case DOWNLOAD_SANITISATION_EXCEL_FILE:
      return {
        ...state,
        loading: true,
      };
    case DOWNLOAD_SANITISATION_EXCEL_FILE_SUCCESS:
      return {
        ...state,
        loading: false,
        sanitisationExcelData: action.payload.items,
      };
    case DOWNLOAD_SANITISATION_EXCEL_FILE_ERROR:
      return {
        ...state,
        loading: false,
      };
    default:
      return { ...state };
  }
};
