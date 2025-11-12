import { formulaRoot } from 'constants/defaultValues';
import React, { Suspense } from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';

const FormulaList = React.lazy(() => import('./formulaList'));
console.log('FormulaList', FormulaList);

const Formula = ({ match }) => (
  <Suspense fallback={<div className="loading" />}>
    <Switch>
      <Redirect
        exact
        from={`${match.url}/`}
        to={`${match.url}${formulaRoot}/formulaList`}
      />
      <Route
        path={`${match.url}/formulaList`}
        render={(props) => <FormulaList {...props} />}
      />
      <Redirect to="../../error" />
    </Switch>
  </Suspense>
);
export default Formula;
