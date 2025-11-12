import { getCurrentUser } from 'helpers/Utils';
import { isAuthGuardActive, currentUser } from 'constants/defaultValues';
import {
  LOGIN_USER,
  LOGIN_USER_SUCCESS,
  LOGIN_USER_ERROR,
  LOGOUT_USER,
} from '../actions';

const INIT_STATE = {
  currentUser: getCurrentUser(),
  forgotUserMail: '',
  newPassword: '',
  resetPasswordCode: '',
  loading: false,
  error: '',
  LoginError: '',
};

export default (state = INIT_STATE, action) => {
  switch (action.type) {
    case LOGIN_USER:
      return { ...state, loading: true, error: '' };
    case LOGIN_USER_SUCCESS:
      return {
        ...state,
        loading: false,
        currentUser: action.payload,
        error: '',
      };
    case LOGIN_USER_ERROR:
      return {
        ...state,
        loading: false,
        LoginError: action.payload.message,
        error: action.payload.message,
      };
    case LOGOUT_USER:
      return { ...state, currentUser: null, error: '' };
    default:
      return { ...state };
  }
};
