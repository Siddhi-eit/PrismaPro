import { canisterRoot } from 'constants/defaultValues';
import React, { Suspense } from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';

const CanisterList = React.lazy(() => import('./canisterList'));

const Canister = ({ match }) => (
  <Suspense fallback={<div className="loading" />}>
    <Switch>
      <Redirect
        exact
        from={`${match.url}/`}
        to={`${match.url}${canisterRoot}/canisterList`}
      />
      <Route
        path={`${match.url}/canisterList`}
        render={(props) => <CanisterList {...props} />}
      />
      <Redirect to="../../error" />
    </Switch>
  </Suspense>
);
export default Canister;