import { getMachineID } from 'helpers/Utils';
import {
  USER_LIST_GET_LIST,
  USER_LIST_GET_LIST_SUCCESS,
  USER_LIST_GET_LIST_ERROR,
  USER_DELETE_SUCCESS,
  USER_ADD,
  USER_ADD_SUCCESS,
  USER_ADD_ERROR,
  USER_ADD_RESET_STATE,
  USER_GET_BY_ID,
  USER_GET_BY_ID_SUCCESS,
  BIND_MACHINE_DROPDOWN,
  BIND_MACHINE_DROPDOWN_SUCCESS,
  SET_DESKTOP_USERID,
  SET_MACHINE_ID_SUCCESS,
  BIND_USER_TYPE_DROPDOWN,
  BIND_USER_TYPE_DROPDOWN_SUCCESS,
  BIND_MD_FUSION_LAB_NO_DROPDOWN,
  BIND_MD_FUSION_LAB_NO_DROPDOWN_SUCCESS,
  BIND_MD_FUSION_LAB_NO_DROPDOWN_ERROR,
  USER_DELETE,
  DOWNLOAD_USER_EXCEL_FILE,
  DOWNLOAD_USER_EXCEL_FILE_SUCCESS,
  DOWNLOAD_USER_EXCEL_FILE_ERROR,
} from '../actions';

const INIT_STATE = {
  allUserItems: null,
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
};

export default (state = INIT_STATE, action) => {
  switch (action.type) {
    case USER_LIST_GET_LIST:
      return { ...state, loading: true, isSucessfullyDelete: 0 };

    case USER_LIST_GET_LIST_SUCCESS:
      return {
        ...state,
        loading: false,
        allUserItems: action.payload,
      };
    case USER_LIST_GET_LIST_ERROR:
      return {
        ...state,
        loading: false,
        error: action.payload,
      };
    case USER_DELETE:
      return {
        loading: false,
        isSucessfullyDelete: 0,
      };
    case USER_DELETE_SUCCESS:
      return {
        loading: true,
        isSucessfullyDelete: parseInt(action.payload.id, 10),
        machineID: getMachineID(),
      };
    case USER_ADD:
      return {
        ...state,
        loading: false,
        isSucessfullyAdd: null,
      };
    case USER_ADD_SUCCESS:
      return {
        ...state,
        isSucessfullyAdd: action.payload.items.resultObjectID,
        resultMessage: action.payload.items.resultMessage,
        loading: false,
      };
    case USER_ADD_ERROR:
      return {
        ...state,
        isAddError: action.payload.items,
        resultMessage: action.payload.items.resultMessage,
        loading: false,
      };
    case USER_GET_BY_ID:
      return {
        ...state,
        loading: true,
      };
    case USER_GET_BY_ID_SUCCESS:
      return {
        ...state,
        loading: false,
        userDetail: action.payload.items,
      };
    case USER_ADD_RESET_STATE:
      return {
        ...state,
        isSucessfullyAdd: null,
        loading: false,
        userDetail: null,
        resultMessage: null,
        isSucessfullyDelete: 0,
      };
    case BIND_MACHINE_DROPDOWN:
      return {
        ...state,
      };
    case BIND_MACHINE_DROPDOWN_SUCCESS:
      return {
        ...state,
        machineDropdownList: action.payload.items,
      };
    case BIND_USER_TYPE_DROPDOWN:
      return {
        ...state,
      };
    case BIND_USER_TYPE_DROPDOWN_SUCCESS:
      return {
        ...state,
        userTypeDropdownList: action.payload.items,
      };
    case BIND_MD_FUSION_LAB_NO_DROPDOWN:
      return {
        ...state,
      };
    case BIND_MD_FUSION_LAB_NO_DROPDOWN_SUCCESS:
      return {
        ...state,
        mdFusionLabNoDropdownList: action.payload.items,
      };
    case BIND_MD_FUSION_LAB_NO_DROPDOWN_ERROR:
      return {
        ...state,
        mdFusionLabNoDropdownList: action.payload.items,
      };
    case SET_DESKTOP_USERID:
      return {
        ...state,
      };
    case SET_MACHINE_ID_SUCCESS:
      return {
        ...state,
        machineID: action.payload.items,
      };
    case DOWNLOAD_USER_EXCEL_FILE:
      return {
        ...state,
        loading: true,
      };
    case DOWNLOAD_USER_EXCEL_FILE_SUCCESS:
      return {
        ...state,
        loading: false,
        UserExcelData: action.payload.items,
      };
    case DOWNLOAD_USER_EXCEL_FILE_ERROR:
      return {
        ...state,
        loading: false,
      };
    default:
      return { ...state };
  }
};
