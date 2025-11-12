import React, { Suspense } from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';

const DispenseManage = React.lazy(() => import('./dispenseManage'));
const DispenseHistory = React.lazy(() => import('./dispenseHistory'));
const Dispense = ({ match }) => (
  <Suspense fallback={<div className="loading" />}>
    <Switch>
      <Redirect exact from={`${match.url}/`} to={`${match.url}/start`} />
      <Route
        path={`${match.url}/dispenseManage`}
        render={(props) => <DispenseManage {...props} />}
      />
      <Route
        path={`${match.url}/dispenseHistory`}
        render={(props) => <DispenseHistory {...props} />}
      />
      <Redirect to="../../error" />
    </Switch>
  </Suspense>
);
export default Dispense;
