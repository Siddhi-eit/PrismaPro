import { machineRoot } from 'constants/defaultValues';
import React, { Suspense } from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';

const MachineList = React.lazy(() => import('./machineList'));

const Machine = ({ match }) => (
  <Suspense fallback={<div className="loading" />}>
    <Switch>
      <Redirect
        exact
        from={`${match.url}/`}
        to={`${match.url}${machineRoot}/machineList`}
      />
      <Route
        path={`${match.url}/machineList`}
        render={(props) => <MachineList {...props} />}
      />
      <Redirect to="../../error" />
    </Switch>
  </Suspense>
);
export default Machine;
