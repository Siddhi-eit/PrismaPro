import { canisterLookupRoot } from 'constants/defaultValues';
import React, { Suspense } from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';

const CanisterLookupList = React.lazy(() => import('./canisterLookupList'));

const canisterLookup = ({ match }) => (
  <Suspense fallback={<div className="loading" />}>
    <Switch>
      <Redirect
        exact
        from={`${match.url}/`}
        to={`${match.url}${canisterLookupRoot}/canisterLookupList`}
      />
      <Route
        path={`${match.url}/canisterLookupList`}
        render={(props) => <CanisterLookupList {...props} />}
      />
      <Redirect to="../../error" />
    </Switch>
  </Suspense>
);
export default canisterLookup;
