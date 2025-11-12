import React, { useEffect, useState } from 'react';
import { Row, Alert, Button, FormGroup } from 'reactstrap';
import { Colxx } from 'components/common/CustomBootstrap';
import { Formik, Form, Field } from 'formik';
import { connect } from 'react-redux';
import WOW from 'wowjs';
import '../../assets/css/animate.css';
import { loginUser, loginUserError } from 'redux/actions';
import '../../assets/css/style-dark.css';
import '../../assets/css/skeleton-wide.css';
import '../../assets/css/media.css';
import '../../assets/css/font-awesome.css';
import 'jquery-knob';
import Picker from 'react-scrollable-picker';
import mdText from '../../assets/img/MD_text.svg';
import mdFusionLogo from '../../assets/img/md_fusion_logo.svg';
import mdBottom from '../../assets/img/mdBottom.svg';
import '../../assets/js/pages/NewRefillTracking';
import triangle from '../../assets/img/triangle_rotate.png';
import createNotification from 'helpers/alerts';

const LoginNew = ({ history, loading, loginUserAction, error, LoginError }) => {
  console.log(LoginError);
  const [visible, setVisible] = useState(false);
  const [stepNo, setStepNo] = useState(0);
  let [email, setEmail] = useState('');
  let [password, setPassword] = useState('');

  const validatePassword = (value) => {
    let error;
    if (!value) {
      error = 'Please enter your password';
      setPassword(value);
    } else if (value.length < 4) {
      error = 'Value must be longer than 3 characters';
      setPassword(value);
    } else if (value && value.length >= 4) {
      setPassword(value);
      return undefined;
    }
    return error;
  };

  const validateEmail = (value) => {
    let error;
    if (!value) {
      error = 'Please enter your email address';
      setEmail(value);
    } else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(value)) {
      error = 'Invalid email address';
      setEmail(value);
    } else if (
      value &&
      /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(value)
    ) {
      setEmail(value);
      return undefined;
    }
    return error;
  };

  useEffect(() => {
    if (error) {
      if (error.id === -1) {
        setStepNo(1.1);
      }
      setVisible(true);
    }
  }, [error]);

  useEffect(() => {
    if (LoginError) {
      setStepNo(0);
      createNotification('error', 'Error', 'INCORRECT USERNAME OR PASSWORD');
    }
  }, [LoginError]);

  const onUserLogin = (values, events) => {
    setStepNo(1.5);
    // if (!loading) {
    if (values.email !== '' && values.password !== '') {
      loginUserAction(values, history);
    }
    // }
  };
  const onBack = () => {
    new WOW.WOW({
      live: false,
    }).init();

    if (stepNo === 2) {
      setStepNo(1);
    } else if (stepNo === 1.5) {
      setStepNo(1);
    } else if (stepNo === 1.1) {
      setStepNo(1);
    } else if (stepNo === 1) {
      setStepNo(0);
    }
  };
  const onForward = () => {
    new WOW.WOW({
      live: false,
    }).init();
    if (stepNo === 0) {
      let val = validateEmail(email);
      if (val === undefined) {
        setStepNo(1);
      }
    } else if (stepNo === 1) {
      let val = validatePassword(email);
      if (val === undefined) {
        setStepNo(1.5);
      }
    } else if (stepNo === 1.5) {
      setStepNo(2);
    }
  };

  const initialValues = { email, password };
  return (
    <>
      <Formik
        name="frmlogin"
        initialValues={initialValues}
        onSubmit={onUserLogin}
      >
        {({ errors, touched }) => (
          <Form>
            <Row className="tracking">
              <Colxx xxs="12" className="position-inherit">
                <div id="preload">
                  <div id="preload-status" />
                </div>
                <div id="img-bg" className="fusion_bg_circle" />
                <div id="curtains" />
                <div className="preload-content" />
                <div className="md_text">
                  <img src={mdText} alt="mdText" />
                </div>
                <div className="upper-page current">
                  <div className="center-container-home">
                    <div className="center-block">
                      <div className="logo-wrapper">
                        <img alt="mdFusionLogo" src={mdFusionLogo} />
                      </div>
                      <div className="upper-content">
                        <div className="container center">
                          <div id="intro-wrapper">
                            <div className="sixteen columns column">
                              <div id="intro-top">LOGIN</div>
                            </div>

                            <div className="sixteen columns">
                              {(() => {
                                if (stepNo === 0) {
                                  return (
                                    <div
                                      id="intro-subtitle"
                                      className="wow fadeInDown"
                                    >
                                      ENTER USER EMAIL ID
                                    </div>
                                  );
                                }
                                if (
                                  stepNo === 1 ||
                                  stepNo === 1.1 ||
                                  stepNo === 1.5
                                ) {
                                  return (
                                    <div
                                      id="intro-subtitle"
                                      className="wow fadeInDown"
                                    >
                                      ENTER PASSWORD
                                    </div>
                                  );
                                }
                              })()}
                            </div>
                            <div className="sixteen columns column">
                              <div id="intro-image-top" />
                            </div>
                            <div className="sixteen columns">
                              <div
                                id="intro-title"
                                className="flex-column align-items-center justify-content-around"
                              >
                                {(() => {
                                  if (stepNo === 0) {
                                    return (
                                      <>
                                        <Field
                                          id="txtemail"
                                          name="email"
                                          className=""
                                          type="text"
                                          placeholder="Enter email address"
                                          validate={validateEmail}
                                        />
                                        {errors.email && touched.email && (
                                          <div className="invalid-feedback d-block">
                                            {errors.email}
                                          </div>
                                        )}
                                      </>
                                    );
                                  }
                                  if (stepNo === 1) {
                                    return (
                                      <>
                                        <Field
                                          id="txtPassword"
                                          name="password"
                                          className=""
                                          type="Password"
                                          placeholder="Enter password"
                                          validate={validatePassword}
                                        />
                                        {errors.password &&
                                          touched.password && (
                                            <div className="invalid-feedback d-block">
                                              {errors.password}
                                            </div>
                                          )}
                                      </>
                                    );
                                  }
                                  if (stepNo === 1.1) {
                                    return (
                                      <div className=" wow fadeInUp">
                                        {' '}
                                        <div
                                          className="alert alert-process alert-dismissible show m-auto w-50 alert-box-shadow"
                                          role="alert"
                                        >
                                          Invalid Username or Password..
                                          <button
                                            type="button"
                                            className="close"
                                            data-dismiss="alert"
                                            aria-label="Close"
                                          >
                                            <span aria-hidden="true">x</span>
                                          </button>
                                        </div>
                                      </div>
                                    );
                                  }
                                  if (stepNo === 1.5) {
                                    return (
                                      <div className=" wow fadeInUp">
                                        {' '}
                                        <img
                                          alt="triangle - MD"
                                          src={triangle}
                                          className="print"
                                        />
                                      </div>
                                    );
                                  }
                                  return <div className="">&nbsp;</div>;
                                })()}
                              </div>
                            </div>
                            <div className="sixteen columns">
                              <div id="intro-image-bottom" />
                            </div>
                          </div>
                        </div>
                      </div>
                      <div id="keyboard-wrapper">
                        <div className="container center">
                          <div id="keyboard-wrap">
                            <div className="w-100 position-absolute no-select">
                              {(() => {
                                if (stepNo != 0) {
                                  return (
                                    <button
                                      type="button"
                                      onClick={onBack}
                                      className="font-weight-bold nav-links navigation-button"
                                    >
                                      BACK
                                    </button>
                                  );
                                }
                              })()}
                              <div className="w-50 d-inline-block">&nbsp;</div>
                              {(() => {
                                if (stepNo === 1 || stepNo === 1.1) {
                                  return (
                                    <>
                                      {errors.email && touched.email && (
                                        <div className="invalid-feedback d-block">
                                          {errors.email}
                                        </div>
                                      )}
                                      <Button className="font-weight-bold nav-links navigation-button">
                                        LOGIN
                                      </Button>
                                    </>
                                  );
                                } else {
                                  return (
                                    <button
                                      onClick={onForward}
                                      className="font-weight-bold nav-links navigation-button"
                                    >
                                      FORWARD
                                    </button>
                                  );
                                }
                              })()}
                            </div>
                            <img alt="mdBottom" src={mdBottom} />
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <script
                  type="text/javascript"
                  src="assets/js/jquery-1.11.2.min.js"
                />
                <script
                  type="text/javascript"
                  src="assets/js/jquery.nicescroll.3.5.4.js"
                />
                <script type="text/javascript" src="assets/js/sky.js" />
              </Colxx>
            </Row>
          </Form>
        )}
      </Formik>
    </>
  );
};

const mapStateToProps = ({ authUser }) => {
  const { loading, error, LoginError } = authUser;
  return { loading, error, LoginError };
};

export default connect(mapStateToProps, {
  loginUserAction: loginUser,
})(LoginNew);
