// import React, { Suspense } from 'react';
// import { Route, withRouter, Switch, Redirect } from 'react-router-dom';
// import { connect } from 'react-redux';

// import AppLayout from '../../../layout/AppLayout';
// // import { ProtectedRoute, UserRole } from 'helpers/authHelper';

// const AccountList = React.lazy(() =>
//   import(/* webpackChunkName: "viwes-home" */ './list')
// );

// const App = ({ match }) => {
//   return (
//     <AppLayout>
//       <div className="dashboard-wrapper">
//         <Suspense fallback={<div className="loading" />}>
//           <Switch>
//             <Redirect exact from={`${match.url}/`} to={`${match.url}/list`} />
//             {/* <ProtectedRoute
//                     path={`${match.url}/list`}
//                     component={AccountList}
//                     roles={[UserRole.Admin]}
//             /> */}
//             <Route
//               path={`${match.url}/list`}
//               render={(props) => <AccountList {...props} />}
//             />
//             <Redirect to="/error" />
//           </Switch>
//         </Suspense>
//       </div>
//     </AppLayout>
//   );
// };

// const mapStateToProps = ({ menu }) => {
//   const { containerClassnames } = menu;
//   return { containerClassnames };
// };

// export default withRouter(connect(mapStateToProps, {})(App));

import React, { Suspense } from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';

const UserManagement = React.lazy(() =>
  import(/* webpackChunkName: "viwes-home" */ './user')
);
const UserList = React.lazy(() =>
  import(/* webpackChunkName: "viwes-home" */ './list')
);

const Gogo = ({ match }) => (
  <Suspense fallback={<div className="loading" />}>
    <Switch>
      <Redirect exact from={`${match.url}/`} to={`${match.url}/start`} />
      <Route
        path={`${match.url}/user`}
        render={(props) => <UserManagement {...props} />}
      />
      <Route
        path={`${match.url}/list`}
        render={(props) => <UserList {...props} />}
      />
      <Redirect to="../../error" />
    </Switch>
  </Suspense>
);
export default Gogo;
