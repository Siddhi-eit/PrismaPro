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

const INIT_STATE = {
  allMachineItems: null,
  error: '',
  loading: false,
  selectedItems: [],
  isSucessfullyDelete: 0,
  isAddError: '',
  isSucessfullyAdd: null,
  machineDetail: null,
  resultMessage: null,
};

export default (state = INIT_STATE, action) => {
  switch (action.type) {
    case MACHINE_LIST_GET_LIST:
      return { ...state, loading: true, isSucessfullyDelete: 0 };

    case MACHINE_LIST_GET_LIST_SUCCESS:
      return {
        ...state,
        loading: false,
        allMachineItems: action.payload,
      };
    case MACHINE_LIST_GET_LIST_ERROR:
      return {
        ...state,
        loading: false,
        error: action.payload,
      };
    case MACHINE_DELETE:
      return {
        loading: false,
        isSucessfullyDelete: 0,
      };
    case MACHINE_DELETE_SUCCESS:
      return {
        loading: false,
        isSucessfullyDelete: parseInt(action.payload.id, 10),
      };
    case MACHINE_DELETE_ERROR:
      return {
        loading: false,
        isSucessfullyDelete: parseInt(action.payload.id, 10),
      };
    case MACHINE_ADD:
      return {
        ...state,
        loading: false,
        resultMessage: null,
        isSucessfullyAdd: null,
      };
    case MACHINE_ADD_SUCCESS:
      return {
        ...state,
        isSucessfullyAdd: action.payload.items.resultObjectID,
        loading: false,
        resultMessage: action.payload.items.resultMessage,
      };
    case MACHINE_ADD_ERROR:
      return {
        ...state,
        isAddError: action.payload.items,
        loading: false,
        resultMessage: action.payload.items.resultMessage,
      };
    case MACHINE_GET_BY_ID:
      return {
        ...state,
        loading: true,
      };
    case MACHINE_GET_BY_ID_SUCCESS:
      return {
        ...state,
        loading: false,
        machineDetail: action.payload.items,
      };
    case MACHINE_GET_BY_ID_ERROR:
      return {
        ...state,
        loading: false,
        machineDetail: action.payload.items,
      };
    case MACHINE_ADD_RESET_STATE:
      return {
        ...state,
        isSucessfullyAdd: null,
        loading: false,
        machineDetail: null,
        isSucessfullyDelete: 0,
        resultMessage: null,
      };
    case DOWNLOAD_MACHINE_EXCEL_FILE:
      return {
        ...state,
        loading: true,
      };
    case DOWNLOAD_MACHINE_EXCEL_FILE_SUCCESS:
      return {
        ...state,
        loading: false,
        MachineExcelData: action.payload.items,
      };
    case DOWNLOAD_MACHINE_EXCEL_FILE_ERROR:
      return {
        ...state,
        loading: false,
      };
    default:
      return { ...state };
  }
};
