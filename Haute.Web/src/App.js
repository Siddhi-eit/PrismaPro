import React, { Suspense } from 'react';
import { connect } from 'react-redux';
import {
  BrowserRouter as Router,
  Route,
  Switch,
  Redirect,
} from 'react-router-dom';
import { IntlProvider } from 'react-intl';
import './helpers/Firebase';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { signalRDispenseError, signalRDispenseSuccess } from 'redux/actions';
import AppLocale from './lang';
import ColorSwitcher from './components/common/ColorSwitcher';
import { NotificationContainer } from './components/common/react-notifications';
import {
  isMultiColorActive,
  adminRoot,
  UserRole,
} from './constants/defaultValues';
import { getDirection } from './helpers/Utils';
import { ProtectedRoute } from './helpers/authHelper';
import { CONNECTION_Hub_URL } from 'api-config';

const ViewApp = React.lazy(() =>
  import(/* webpackChunkName: "views-app" */ './views/app')
);
const ViewUser = React.lazy(() =>
  import(/* webpackChunkName: "views-user" */ './views/user')
);
const RefillTracking = React.lazy(() =>
  import(/* webpackChunkName: "views-user" */ './views/refillTracking')
);
const SanitisationTraking = React.lazy(() =>
  import(/* webpackChunkName: "views-user" */ './views/sanitisationTraking')
);
const ViewError = React.lazy(() =>
  import(/* webpackChunkName: "views-error" */ './views/error')
);
const ViewUnauthorized = React.lazy(() =>
  import(/* webpackChunkName: "views-error" */ './views/unauthorized')
);

const ConnectSignalR = async (
  signalRDispenseSuccessAction,
  signalRDispenseErrorAction
) => {
  const connection = new HubConnectionBuilder()
    .configureLogging(LogLevel.Trace)
    .withUrl(CONNECTION_Hub_URL)
    .build();

  connection.on('DispenseSuccess', (dispenseCompletedParm) => {
    signalRDispenseSuccessAction(dispenseCompletedParm);
  });

  connection.on('DispenseError', (userId) => {
    signalRDispenseErrorAction(userId);
  });

  connection.onclose(async () => {
    await ConnectSignalR(
      signalRDispenseSuccessAction,
      signalRDispenseErrorAction
    );
  });

  try {
    await connection.start();
    const currentUser = JSON.parse(
      localStorage.getItem('Haute_current_machine')
    );
    if (currentUser) {
      await connection.invoke('SetUserID', currentUser.uid.toString());
    }
  } catch (error) {
    // Handle connection error
  }
};

class App extends React.Component {
  constructor(props) {
    ConnectSignalR(
      props.signalRDispenseSuccessAction,
      props.signalRDispenseErrorAction
    );
    super(props);
    const direction = getDirection();
    if (direction.isRtl) {
      document.body.classList.add('rtl');
      document.body.classList.remove('ltr');
    } else {
      document.body.classList.add('ltr');
      document.body.classList.remove('rtl');
    }
  }

  render() {
    const { locale } = this.props;
    const currentAppLocale = AppLocale[locale];

    return (
      <div className="h-100">
        <IntlProvider
          locale={currentAppLocale.locale}
          messages={currentAppLocale.messages}
        >
          <>
            <NotificationContainer />
            {isMultiColorActive && <ColorSwitcher />}
            <Suspense fallback={<div className="loading" />}>
              <Router>
                <Switch>
                  <ProtectedRoute
                    path={adminRoot}
                    component={ViewApp}
                    roles={[UserRole.Admin, UserRole.Web]}
                  />
                  <Route
                    path="/user"
                    render={(props) => <ViewUser {...props} />}
                  />
                  <ProtectedRoute
                    path="/refillTracking"
                    roles={[UserRole.Admin, UserRole.Web]}
                    component={RefillTracking}
                  />
                  <ProtectedRoute
                    path="/sanitisationTraking"
                    roles={[UserRole.Admin, UserRole.Web]}
                    component={SanitisationTraking}
                  />
                  <Route
                    path="/error"
                    exact
                    render={(props) => <ViewError {...props} />}
                  />
                  <Route
                    path="/unauthorized"
                    exact
                    render={(props) => <ViewUnauthorized {...props} />}
                  />
                  <Redirect exact from="/" to={adminRoot} />
                  <Redirect to="/error" />
                </Switch>
              </Router>
            </Suspense>
          </>
        </IntlProvider>
      </div>
    );
  }
}

const mapStateToProps = ({ authUser, settings }) => ({
  currentUser: authUser.currentUser,
  locale: settings.locale,
});

const mapDispatchToProps = {
  signalRDispenseSuccessAction: signalRDispenseSuccess,
  signalRDispenseErrorAction: signalRDispenseError,
};

export default connect(mapStateToProps, mapDispatchToProps)(App);
