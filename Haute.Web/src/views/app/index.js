import { getCurrentUser } from 'helpers/Utils';
import React, { Suspense } from 'react';
import { Route, withRouter, Switch, Redirect } from 'react-router-dom';
import { connect } from 'react-redux';
import AppLayout from 'layout/AppLayout';
import { dispenseRoot, UserRole, userRoot } from 'constants/defaultValues';
import { ProtectedRoute } from '../../helpers/authHelper';

const User = React.lazy(() =>
  import(/* webpackChunkName: "viwes-home" */ './user')
);
const Canister = React.lazy(() =>
  import(/* webpackChunkName: "viwes-blank-page" */ './canister')
);

const CanisterLookup = React.lazy(() =>
  import(/*webpackChunkName: "viwes-blank-page" */ './canisterLookup')
);

const Machine = React.lazy(() =>
  import(/* webpackChunkName: "viwes-blank-page" */ './machine')
);
const Formula = React.lazy(() =>
  import(/* webpackChunkName: "viwes-blank-page" */ './formula')
);
const Dispense = React.lazy(() =>
  import(/* webpackChunkName: "viwes-blank-page" */ './dispense')
);
const RefillTracking = React.lazy(() =>
  import(/* webpackChunkName: "viwes-blank-page" */ './refillTracking')
);
const SanitisationTraking = React.lazy(() =>
  import(/* webpackChunkName: "viwes-blank-page" */ './sanitisationTraking')
);
const BlankPage = React.lazy(() =>
  import(/* webpackChunkName: "viwes-blank-page" */ './blank-page')
);

const user = getCurrentUser();

const App = ({ match }) => {
  return (
    <AppLayout>
      <div className="dashboard-wrapper">
        <Suspense fallback={<div className="loading" />}>
          <Switch>
            <Redirect
              exact
              from={`${match.url}/`}
              to={
                user.role === UserRole.Admin
                  ? `${match.url}${userRoot}/user`
                  : `${match.url}${dispenseRoot}/dispenseManage`
              }
            />
            <ProtectedRoute
              path={`${match.url}/user`}
              component={User}
              roles={[UserRole.Admin]}
            />
            <Route
              path={`${match.url}/canister`}
              render={(props) => <Canister {...props} />}
            />
            <Route
              path={`${match.url}/canisterLookup`}
              render={(props) => <CanisterLookup {...props} />}
            />
            <Route
              path={`${match.url}/machine`}
              render={(props) => <Machine {...props} />}
            />
            <Route
              path={`${match.url}/formula`}
              render={(props) => <Formula {...props} />}
            />
            <Route
              path={`${match.url}/dispense`}
              render={(props) => <Dispense {...props} />}
            />
            <ProtectedRoute
              path={`${match.url}/refillTracking`}
              component={RefillTracking}
              roles={[UserRole.Admin]}
            />
            <ProtectedRoute
              path={`${match.url}/sanitisationTraking`}
              component={SanitisationTraking}
              roles={[UserRole.Admin]}
            />
            <Route
              path={`${match.url}/blank-page`}
              render={(props) => <BlankPage {...props} />}
            />
            <Redirect to="/error" />
          </Switch>
        </Suspense>
      </div>
    </AppLayout>
  );
};

const mapStateToProps = ({ menu }) => {
  const { containerClassnames } = menu;
  return { containerClassnames };
};

export default withRouter(connect(mapStateToProps, {})(App));
