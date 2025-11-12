import React, { Suspense } from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';

const NewRefillTracking = React.lazy(() => import('./newRefillTracking'));

const RefillTracking = ({ match }) => (
  <Suspense fallback={<div className="loading" />}>
    <Switch>
      <Redirect exact from={`${match.url}/`} to={`${match.url}/start`} />
      <Route
        path={`${match.url}/new`}
        render={(props) => <NewRefillTracking {...props} />}
      />
      <Redirect to="../../error" />
    </Switch>
  </Suspense>
);

export default RefillTracking;