import {
  getMachineName,
  getMachineID,
  getCurrentUser,
} from '../../helpers/Utils';
import React, { useEffect, useState } from 'react';
import { Row, label } from 'reactstrap';
import { Colxx } from 'components/common/CustomBootstrap';
import { Formik } from 'formik';
import { connect } from 'react-redux';
import WOW from 'wowjs';
import '../../assets/css/animate.css';
import {
  bindCanisterNoDropdown,
  bindMDFusionLabNoDropdown,
  addRefillTraking,
  bindProductDropdown,
  ResetStateRefillTrking,
  getUserByID,
} from 'redux/actions';
import '../../assets/css/style-dark.css';
import '../../assets/css/skeleton-wide.css';
import '../../assets/css/media.css';
import '../../assets/css/font-awesome.css';
import 'jquery-knob';
import Picker from 'react-scrollable-picker';
import mdText from '../../assets/img/MD_text.svg';
import mdFusionLogo from '../../assets/img/md_fusion_logo.svg';
import mdBottom from '../../assets/img/mdBottom.svg';
import triangle_qr from '../../assets/img/triangle_qr.png';
import triangle_print from '../../assets/img/triangle_print.png';
import triangle from '../../assets/img/triangle_rotate.png';
import '../../assets/js/pages/NewRefillTracking';
import { useHistory } from 'react-router-dom';
import PdfDocument from 'helpers/PdfDocument';

const NewRefillTracking = ({
  bindCanisterNoDropdownAction,
  bindMDFusionLabNoDropdownAction,
  bindProductDropdownAction,
  mdFusionLabNoDropdownList,
  canisterNoDropdownList,
  productDropdownList,
  AddRefillTrakingAction,
  isSucessfullyAdd,
  ResetStateRefillTrkingAction,
  refillPDFData,
  machineID,
}) => {
  const [desktopUserName, setDesktopUserName] = useState(0);
  const [selectedMachineID, setMachineID] = useState(0);
  const [currentUser, setCurrentUser] = useState(0);
  // const [userID, setUserID] = useState(0);
  const [stepNo, setStepNo] = useState(0);
  const [oldStepNo, setOldStepNo] = useState(0);
  const history = useHistory();
  const [isPdf, setIsPdf] = useState(false);
  const [valueForQRCode, setValueForQRCode] = useState(null);
  const [valueForPDFContent, SetValueForPDFContent] = useState(null);
  const [canisterNoItems, setcanisterNoItems] = useState([]);
  const [mdFusionLabNoItems, setMdFusionLabNoItems] = useState([]);
  const [productItems, setProductItems] = useState([]);
  const [reminderValueGroup, setReminderValueGroup] = React.useState({});
  const [canisterOptionGroups, setCanisterOption] = useState({});
  const [canisterValueGroup, setCanisterValueGroup] = React.useState({});
  const [mdFusionLabNoOptionGroups, setMdFusionLabNoOption] = useState({});
  const [mdFusionLabNoValueGroup, setMdFusionLabNoValueGroup] = React.useState(
    {}
  );
  const [productOptionGroups, setProductOption] = useState({});
  const [productValueGroup, setProductValueGroup] = React.useState({});
  const [lotNr, setLotNr] = useState('');
  const [lotNrError, SetLotNrError] = useState(false);
  const reminderData = [
    { title: '250', value: 250 },
    { title: '500', value: 500 },
    { title: '750', value: 750 },
    { title: '1,000', value: 1000 },
    { title: '1,500', value: 1500 },
    { title: '2,000 ', value: 2000 },
  ];
  const reminderOptionGroups = {
    reminder: reminderData.map((i) => ({ value: i.value, label: i.title })),
  };

  function checkVal(array, value) {
    array = array.map((i) => i.value);
    return array.some(function (entry) {
      if (Array.isArray(entry)) {
        return checkVal(entry, value);
      }
      return entry === value;
    });
  }

  useEffect(() => {
    if (isSucessfullyAdd != null && isSucessfullyAdd > 0) {
      setStepNo(100);
      ResetStateRefillTrkingAction();
      CanisterHandleChange('title', canisterOptionGroups.canisterNo[0]);
      reminderHandleChange('title', reminderOptionGroups.reminder[0]);
      setLotNr('');
      mdFusionLabHandleChange(
        'title',
        mdFusionLabNoOptionGroups.mdFusionLabNo[0]
      );
      SetLotNrError(false);
      // productHandleChange('title', productOptionGroups.product[0]);
      setValueForQRCode(isSucessfullyAdd);
      SetValueForPDFContent(refillPDFData);
      setIsPdf(true);
      setTimeout(() => {
        setStepNo(0);
      }, 4000);
    }
  }, [isSucessfullyAdd]);

  // useEffect(() => {
  //   bindMDFusionLabNoDropdownAction(currentUser.uid);
  // }, [bindMDFusionLabNoDropdownAction]);
  useEffect(() => {
    if (
      mdFusionLabNoDropdownList != null &&
      mdFusionLabNoDropdownList.length > 0
    ) {
      const mdFusionLabNoOption = mdFusionLabNoDropdownList.map((d) => ({
        value: d.value,
        label: d.label.trim(),
      }));
      setMdFusionLabNoItems(mdFusionLabNoOption);
      if (oldStepNo == 0) {
        setStepNo(1);
      }
    }
  }, [mdFusionLabNoDropdownList]);
  useEffect(() => {
    if (mdFusionLabNoItems != null && mdFusionLabNoItems.length > 0) {
      setMdFusionLabNoOption({
        mdFusionLabNo: mdFusionLabNoItems.map((i) => ({
          value: i.value,
          label: i.label.trim(),
        })),
      });
    }
    if (oldStepNo == 0) {
      setStepNo(1);
    }
  }, [mdFusionLabNoItems]);

  // useEffect(() => {
  //   bindCanisterNoDropdownAction(machineID);
  // }, [bindCanisterNoDropdownAction, machineID]);
  useEffect(() => {
    if (canisterNoDropdownList != null && canisterNoDropdownList.length > 0) {
      const canisterNoOption = canisterNoDropdownList.map((d) => ({
        value: d.id,
        label: d.canisterCode.trim(),
      }));
      setcanisterNoItems(canisterNoOption);
    } else {
      if (oldStepNo == 1) {
        setStepNo(2);
      }
    }
  }, [canisterNoDropdownList]);
  useEffect(() => {
    if (canisterNoItems != null && canisterNoItems.length > 0) {
      setCanisterOption({
        canisterNo: canisterNoItems.map((i) => ({
          value: i.value,
          label: i.label.trim(),
        })),
      });
      if (oldStepNo == 1) {
        setStepNo(2);
      }
    }
  }, [canisterNoItems]);

  // useEffect(() => {
  //   bindProductDropdownAction();
  // }, [bindProductDropdownAction]);
  useEffect(() => {
    if (productDropdownList != null && productDropdownList.length > 0) {
      const productOption = productDropdownList.map((d) => ({
        value: d.value,
        label: d.label.trim(),
      }));
      setProductItems(productOption);
      if (oldStepNo == 3) {
        setStepNo(4);
      }
    }
  }, [productDropdownList]);
  useEffect(() => {
    if (productItems != null && productItems.length > 0) {
      setProductOption({
        product: productItems.map((i) => ({
          value: i.value,
          label: i.label.trim(),
        })),
      });
      if (oldStepNo == 3) {
        setStepNo(4);
      }
    }
  }, [productItems]);

  useEffect(() => {
    setStepNo(stepNo);
  }, [stepNo]);

  useEffect(() => {
    let user = getCurrentUser();
    setCurrentUser(user);
  }, []);

  useEffect(() => {
    const desktopUserName = getMachineName();
    setDesktopUserName(desktopUserName);
  }, []);

  useEffect(() => {
    const machineID = getMachineID();
    setMachineID(machineID);
  }, []);

  const CanisterHandleChange = (name, value) => {
    if (value != null) {
      setCanisterValueGroup({
        ...canisterValueGroup,
        [name]: value,
      });
    }
  };
  const reminderHandleChange = (name, value) => {
    setReminderValueGroup({
      ...reminderValueGroup,
      [name]: value,
    });
  };
  const mdFusionLabHandleChange = (name, value) => {
    if (value != null) {
      setMdFusionLabNoValueGroup({
        ...mdFusionLabNoValueGroup,
        [name]: value,
      });
    }
  };
  const productHandleChange = (name, value) => {
    setProductValueGroup({
      ...productValueGroup,
      [name]: value,
    });
  };

  const handleChange = (event) => {
    setLotNr(event.target.value); // Update the state with the input value
  };

  const onSubmit = () => {
    if (stepNo === 6) {
      let user = getCurrentUser();
      if (lotNr) {
        const values = {
          id: 0,
          machineID: selectedMachineID,
          fusionLabNo: mdFusionLabNoValueGroup.mdFusionLabNo,
          canisterNO: canisterValueGroup.canisterNo,
          refillML: reminderValueGroup.reminder,
          lotNr: lotNr,
          // product: productValueGroup.product,
          isActive: true,
          userID: user.uid,
        };
        AddRefillTrakingAction(values);
      }
    }
  };
  const onBack = () => {
    new WOW.WOW({
      live: false,
    }).init();
    if (stepNo === 100) {
      setStepNo(6.5);
    } else if (stepNo === 6.5) {
      setStepNo(6);
    } else if (stepNo === 6) {
      setStepNo(5.5);
    } else if (stepNo === 5.5) {
      setStepNo(5);
    } else if (stepNo === 5) {
      setStepNo(4);
    } else if (stepNo === 4) {
      setLotNr('');
      setStepNo(3);
    } else if (stepNo === 3) {
      setStepNo(2);
    } else if (stepNo === 2) {
      setStepNo(1);
    } else if (stepNo === 1.1) {
      setStepNo(1);
    } else if (stepNo === 1) {
      setStepNo(0);
    } else if (stepNo === 0) {
      history.push('/app/dispense/dispenseManage');
    }
  };
  const onForward = () => {
    new WOW.WOW({
      live: false,
    }).init();
    if (stepNo === 0) {
      setOldStepNo(stepNo);
      setStepNo(-1);
      let user = getCurrentUser();
      const machineID = getMachineID();
      bindMDFusionLabNoDropdownAction(machineID);
    } else if (stepNo === 1) {
      setOldStepNo(stepNo);
      setStepNo(-1);
      const machineID = getMachineID();
      bindCanisterNoDropdownAction(machineID);
    } else if (stepNo === 2) {
      setStepNo(3);
    } else if (stepNo === 3) {
      console.log('lotNr', lotNr);
      setOldStepNo(stepNo);
      // setStepNo(-1);
      // bindProductDropdownAction();
      setStepNo(4);
    } else if (stepNo === 4) {
      console.log('lotNr', lotNr);
      if (!lotNr || lotNr == null) {
        SetLotNrError(true);
      } else if (lotNr && lotNr.toString().length != 4) {
        SetLotNrError(true);
      } else {
        setStepNo(5);
        SetLotNrError(false);
      }
    } else if (stepNo === 5) {
      setStepNo(5.5);
    } else if (stepNo === 5.5) {
      setStepNo(6);
    } else if (stepNo === 6) {
      setStepNo(6.5);
      onSubmit();
    }
  };

  useEffect(() => {
    const inputElement = document.getElementById('txtUserID');
    if (inputElement) {
      inputElement.focus();
    }
  }, []);

  return (
    <>
      {isPdf && valueForQRCode !== null ? (
        <PdfDocument
          valueForQRCode={valueForQRCode}
          pageName="RefillTracking"
          valueForPDFContent={valueForPDFContent}
        />
      ) : null}

      <Formik>
        {() => (
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
                      <label className="logo-username">
                        {
                          JSON.parse(
                            localStorage.getItem('Haute_current_machine')
                          ).title
                        }
                      </label>
                      {/* <UncontrolledDropdown className="dropdown-menu-right">
                      <DropdownToggle className="p-0" color="empty">
                        <span className="name mr-1 logo-username">{JSON.parse(localStorage.getItem('Haute_current_machine')).title}</span>
                      </DropdownToggle>
                      <DropdownMenu className="mt-3" right>
                        <DropdownItem onClick={() => handleLogout()}>
                          Sign out
                        </DropdownItem>
                      </DropdownMenu>
                    </UncontrolledDropdown> */}
                    </div>
                    <div className="upper-content">
                      <div className="container center">
                        <div id="intro-wrapper">
                          <div className="sixteen columns column">
                            <div id="intro-top">REFILL BAG TRACKING</div>
                          </div>

                          <div className="sixteen columns">
                            {(() => {
                              if (stepNo === -1) {
                                return (
                                  <div
                                    id="intro-subtitle"
                                    className="wow fadeInDown"
                                  >
                                    LOADING...
                                  </div>
                                );
                              }
                              if (stepNo === 0) {
                                return (
                                  <div
                                    id="intro-subtitle"
                                    className="wow fadeInDown"
                                  >
                                    ENTER MACHINE NO
                                  </div>
                                );
                              }
                              if (stepNo === 1 || stepNo === 1.1) {
                                return (
                                  <div
                                    id="intro-subtitle"
                                    className="wow fadeInDown"
                                  >
                                    MD FUSION LAB NO.
                                  </div>
                                );
                              }
                              if (stepNo === 2) {
                                return (
                                  <div
                                    id="intro-subtitle"
                                    className="wow fadeInDown"
                                  >
                                    CANISTER NO.
                                  </div>
                                );
                              }
                              if (stepNo === 3) {
                                return (
                                  <div
                                    id="intro-subtitle"
                                    className="wow fadeInDown"
                                  >
                                    REFILL BAG AMOUNT
                                  </div>
                                );
                              }
                              if (stepNo === 4) {
                                return (
                                  <div
                                    id="intro-subtitle"
                                    className="wow fadeInDown"
                                  >
                                    Introduce the lot number printed in the base
                                    of the bottle.
                                  </div>
                                );
                              }
                              if (stepNo === 5 || stepNo === 5.5) {
                                return (
                                  <div
                                    id="intro-subtitle"
                                    className="wow fadeInDown"
                                  >
                                    GENERATE QR CODE
                                  </div>
                                );
                              }
                              if (stepNo === 6 || stepNo === 6.5) {
                                return (
                                  <div
                                    id="intro-subtitle"
                                    className="wow fadeInDown"
                                  >
                                    &nbsp;
                                  </div>
                                );
                              }
                              return (
                                <div
                                  id="intro-subtitle"
                                  className="wow fadeInDown"
                                >
                                  &nbsp;
                                </div>
                              );
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
                                    <input
                                      id="txtUserID"
                                      name="UserID"
                                      className=""
                                      type="text"
                                      placeholder="******"
                                      value={desktopUserName}
                                    />
                                  );
                                }
                                if (stepNo === 1) {
                                  return (
                                    <>
                                      <div className=" wow fadeInUp">
                                        <Picker
                                          height={100}
                                          itemHeight={45}
                                          id="picMDFusionLabNo"
                                          optionGroups={
                                            mdFusionLabNoOptionGroups
                                          }
                                          valueGroups={mdFusionLabNoValueGroup}
                                          onChange={(name, value) => {
                                            if (
                                              checkVal(
                                                mdFusionLabNoOptionGroups.mdFusionLabNo,
                                                value
                                              )
                                            ) {
                                              mdFusionLabHandleChange(
                                                name,
                                                value
                                              );
                                            }
                                          }}
                                        />
                                      </div>
                                    </>
                                  );
                                }
                                if (stepNo === 1.1) {
                                  return (
                                    <div
                                      className="alert alert-process alert-dismissible show m-auto w-75"
                                      role="alert"
                                    >
                                      This MDFusionLab Machin has no canister
                                      added Please add a canister first.
                                      <button
                                        type="button"
                                        className="close"
                                        data-dismiss="alert"
                                        aria-label="Close"
                                      >
                                        <span aria-hidden="true">&times;</span>
                                      </button>
                                    </div>
                                  );
                                }
                                if (stepNo === -1) {
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
                                if (stepNo === 2) {
                                  return (
                                    <div className=" wow fadeInUp">
                                      <Picker
                                        height={100}
                                        itemHeight={45}
                                        id="picCanisterNo"
                                        optionGroups={canisterOptionGroups}
                                        valueGroups={canisterValueGroup}
                                        onChange={(name, value) => {
                                          if (
                                            checkVal(
                                              canisterOptionGroups.canisterNo,
                                              value
                                            )
                                          ) {
                                            CanisterHandleChange(name, value);
                                          }
                                        }}
                                      />
                                    </div>
                                  );
                                }
                                if (stepNo === 3) {
                                  return (
                                    <>
                                      <div className="wow fadeInUp">
                                        <Picker
                                          height={100}
                                          itemHeight={45}
                                          indicatorClassName="my-picker-indicator"
                                          id="picreminderoption"
                                          optionGroups={reminderOptionGroups}
                                          valueGroups={reminderValueGroup}
                                          onChange={(name, value) => {
                                            if (
                                              checkVal(
                                                reminderOptionGroups.reminder,
                                                value
                                              )
                                            ) {
                                              reminderHandleChange(name, value);
                                            }
                                          }}
                                        />
                                      </div>
                                      <span className="picker-item-subtitle">
                                        ML
                                      </span>
                                    </>
                                  );
                                }
                                if (stepNo === 4) {
                                  return (
                                    <>
                                      <div className=" wow fadeInUp">
                                        {/* <Picker
                                          itemHeight={45}
                                          height={100}
                                          id="picProduct"
                                          className="wow fadeInUp"
                                          optionGroups={productOptionGroups}
                                          valueGroups={productValueGroup}
                                          onChange={(name, value) => {
                                            if (
                                              checkVal(
                                                productOptionGroups.product,
                                                value
                                              )
                                            ) {
                                              productHandleChange(name, value);
                                            }
                                          }}
                                        /> */}
                                        <div className="alert-process show m-auto w-25 alert-box-shadow">
                                          <input
                                            autoFocus
                                            id="txtUserID"
                                            name="Lot nr."
                                            className="fa-2x"
                                            style={{
                                              border: '1px solid #008ecc',
                                              fontSize: '2em',
                                            }}
                                            type="text"
                                            value={lotNr}
                                            maxLength={4}
                                            onChange={(event) => {
                                              const input = event.target.value;
                                              const filteredInput =
                                                input.replace(/\D/g, ''); // Remove non-digit characters
                                              console.log(
                                                'filteredInput',
                                                filteredInput
                                              );
                                              setLotNr(filteredInput);
                                            }}
                                          />
                                        </div>
                                        {lotNrError ? (
                                          <div className="invalid-feedback d-block fa-2x mt-3">
                                            lot number is required and all
                                            digits should be at least 4
                                          </div>
                                        ) : null}
                                      </div>
                                    </>
                                  );
                                }
                                if (stepNo === 5) {
                                  return (
                                    <div className=" wow fadeInUp">
                                      <img
                                        alt="triangle - MD"
                                        src={triangle_qr}
                                      />
                                    </div>
                                  );
                                }
                                if (stepNo === 5.5) {
                                  return (
                                    <div className=" wow fadeInUp">
                                      {' '}
                                      <div
                                        className="alert alert-process alert-dismissible show m-auto w-50 alert-box-shadow"
                                        role="alert"
                                      >
                                        PLEASE VERIFY ALL DATA IS CORRECT. You
                                        will continue to genrate QR Code and
                                        data will be saved in the system for
                                        tracking purposes.
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
                                if (stepNo === 6) {
                                  return (
                                    <div className=" wow fadeInUp">
                                      {' '}
                                      <img
                                        alt="triangle - MD"
                                        src={triangle_print}
                                      />
                                    </div>
                                  );
                                }
                                if (stepNo === 6.5) {
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
                                if (stepNo === 100) {
                                  return (
                                    <div className=" wow fadeInUp">
                                      {' '}
                                      <div
                                        className="alert alert-process alert-dismissible show m-auto w-50 alert-box-shadow"
                                        role="alert"
                                      >
                                        Success
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
                            <button
                              onClick={onBack}
                              className="font-weight-bold nav-links navigation-button"
                            >
                              BACK
                            </button>
                            <div className="w-50 d-inline-block">&nbsp;</div>
                            {(() => {
                              if (
                                stepNo != 1.1 ||
                                stepNo != 6 ||
                                stepNo != 100
                              ) {
                                return (
                                  <button
                                    onClick={onForward}
                                    className="font-weight-bold nav-links navigation-button"
                                  >
                                    {(() => {
                                      if (stepNo === 6) {
                                        return <>SUBMIT</>;
                                      }
                                      return <>FORWARD</>;
                                    })()}
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
        )}
      </Formik>
    </>
  );
};

const mapStateToProps = ({ refill, sanitisation, user, canister }) => {
  const {
    isSucessfullyAdd,
    isAddError,
    loading,
    resultMessage,
    refillPDFData,
  } = refill;
  const { machineID, mdFusionLabNoDropdownList } = user;
  const { productDropdownList } = canister;
  const { canisterNoDropdownList } = sanitisation;
  return {
    isSucessfullyAdd,
    isAddError,
    loading,
    canisterNoDropdownList,
    mdFusionLabNoDropdownList,
    productDropdownList,
    machineID,
    resultMessage,
    refillPDFData,
  };
};

export default connect(mapStateToProps, {
  AddRefillTrakingAction: addRefillTraking,
  bindCanisterNoDropdownAction: bindCanisterNoDropdown,
  bindMDFusionLabNoDropdownAction: bindMDFusionLabNoDropdown,
  bindProductDropdownAction: bindProductDropdown,
  ResetStateRefillTrkingAction: ResetStateRefillTrking,
})(NewRefillTracking);
