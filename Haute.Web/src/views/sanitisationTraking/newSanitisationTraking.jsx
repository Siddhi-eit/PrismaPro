import {
  getMachineID,
  getCurrentUser,
  getMachineName,
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
  addSanitisationTraking,
  bindProductDropdown,
  ResetStateRefillTrking,
} from 'redux/actions';
import { useHistory } from 'react-router-dom';
import SanitisationPDFDocument from 'helpers/SanitisationPDFDocument';
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
import triangleQr from '../../assets/img/triangle_qr.png';
import trianglePrint from '../../assets/img/triangle_print.png';
import triangle from '../../assets/img/triangle_rotate.png';

const newSanitisationTraking = ({
  bindCanisterNoDropdownAction,
  bindMDFusionLabNoDropdownAction,
  bindProductDropdownAction,
  mdFusionLabNoDropdownList,
  canisterNoDropdownList,
  productDropdownList,
  addSanitisationTrakingAction,
  isSucessfullyAdd,
  ResetStateRefillTrkingAction,
  sanitisationPDFData,
}) => {
  const [desktopUserName, setDesktopUserName] = useState(0);
  const [currentUser, setCurrentUser] = useState(0);
  // const [userID, setUserID] = useState(0);
  const [selectedMachineID, setMachineID] = useState(0);
  const history = useHistory();
  const [stepNo, setStepNo] = useState(0);
  const [oldStepNo, setOldStepNo] = useState(0);
  const [valueForQRCode, setValueForQRCode] = useState(null);
  const [valueForPDFContent, SetValueForPDFContent] = useState(null);
  const [isPdf, setIsPdf] = useState(false);
  const [canisterNoItems, setcanisterNoItems] = useState([]);
  const [mdFusionLabNoItems, setMdFusionLabNoItems] = useState([]);
  const [productItems, setProductItems] = useState([]);
  const [reminderValueGroup, setReminderValueGroup] = React.useState({});
  const [stickerValueGroup, setStickerValueGroup] = React.useState({});
  const [canisterOptionGroups, setCanisterOption] = useState({});
  const [canisterValueGroup, setCanisterValueGroup] = React.useState({});
  const [mdFusionLabNoOptionGroups, setMdFusionLabNoOption] = useState({});
  const [stickerValue, setStickerValue] = useState(1);
  const [mdFusionLabNoValueGroup, setMdFusionLabNoValueGroup] = React.useState(
    {}
  );
  const [productOptionGroups, setProductOption] = useState({});
  const [productValueGroup, setProductValueGroup] = React.useState({});
  const reminderData = [
    { title: '06', value: 6 },
    { title: '08', value: 8 },
    { title: '10', value: 10 },
  ];
  const reminderOptionGroups = {
    reminder: reminderData.map((i) => ({ value: i.value, label: i.title })),
  };
  const stickerData = [
    { title: '01', value: 1 },
    { title: '02', value: 2 },
    { title: '03', value: 3 },
    { title: '04', value: 4 },
    { title: '05', value: 5 },
  ];
  const stickerOptionGroups = {
    sticker: stickerData.map((i) => ({ value: i.value, label: i.title })),
  };

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
  const stickerHandleChange = (name, value) => {
    setStickerValueGroup({
      ...stickerValueGroup,
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
    let user = getCurrentUser();
    setCurrentUser(user);
  }, []);

  useEffect(() => {
    if (isSucessfullyAdd != null && isSucessfullyAdd > 0) {
      ResetStateRefillTrkingAction();
      mdFusionLabHandleChange(
        'title',
        mdFusionLabNoOptionGroups.mdFusionLabNo[0]
      );
      CanisterHandleChange('title', canisterOptionGroups.canisterID[0]);
      reminderHandleChange('title', reminderOptionGroups.reminder[0]);
      // productHandleChange('title', productOptionGroups.product[0]);
      stickerHandleChange('title', stickerOptionGroups.sticker[0]);
      setIsPdf(true);
      setValueForQRCode(isSucessfullyAdd);
      SetValueForPDFContent(sanitisationPDFData);
      setStepNo(100);
      setTimeout(() => {
        setStepNo(0);
      }, 4000);
    }
  }, [isSucessfullyAdd]);

  useEffect(() => {
    const machineID = getMachineID();
    setMachineID(machineID);
  }, []);

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
      if (oldStepNo == 0) {
        setStepNo(1);
      }
    }
  }, [mdFusionLabNoItems]);

  useEffect(() => {
    if (canisterNoDropdownList != null && canisterNoDropdownList.length > 0) {
      const canisterNoOption = canisterNoDropdownList.map((d) => ({
        value: d.id,
        label: d.canisterCode.trim(),
      }));
      setcanisterNoItems(canisterNoOption);
      if (oldStepNo == 1) {
        setStepNo(2);
      }
    }
  }, [canisterNoDropdownList]);
  useEffect(() => {
    if (canisterNoItems != null && canisterNoItems.length > 0) {
      setCanisterOption({
        canisterID: canisterNoItems.map((i) => ({
          value: i.value,
          label: i.label.trim(),
        })),
      });
      if (oldStepNo == 1) {
        setStepNo(2);
      }
    }
  }, [canisterNoItems]);

  useEffect(() => {
    if (productDropdownList != null && productDropdownList.length > 0) {
      const productOption = productDropdownList.map((d) => ({
        value: d.value,
        label: d.label.trim(),
      }));
      setProductItems(productOption);
      if (oldStepNo == 3.5) {
        setStepNo(5);
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
      if (oldStepNo == 3.5) {
        setStepNo(5);
      }
    }
  }, [productItems]);

  useEffect(() => {
    setStepNo(stepNo);
  }, [stepNo]);

  useEffect(() => {
    const desktopUserName = getMachineName();
    setDesktopUserName(desktopUserName);
  }, []);

  const onSubmit = () => {
    if (stepNo === 7) {
      let user = getCurrentUser();
      const values = {
        id: 0,
        userID: user.uid,
        machineID: selectedMachineID,
        fusionLabNo: mdFusionLabNoValueGroup.mdFusionLabNo,
        canisterID: canisterValueGroup.canisterID,
        refillingPeriod: reminderValueGroup.reminder,
        stickerPosition: stickerValueGroup.sticker,
        product: productValueGroup.product,
        isActive: true,
      };
      addSanitisationTrakingAction(values);
    }
  };
  const onBack = () => {
    new WOW.WOW({
      live: false,
    }).init();
    if (stepNo === 8) {
      setStepNo(7);
    } else if (stepNo === 7) {
      setStepNo(6.5);
    } else if (stepNo === 6.5) {
      setStepNo(6);
    } else if (stepNo === 6) {
      setStepNo(5);
    } else if (stepNo === 5) {
      setStepNo(3.5);
      // } else if (stepNo === 4) {
      //   setStepNo(3.5);
    } else if (stepNo === 3.5) {
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
      bindMDFusionLabNoDropdownAction(user.uid);
    } else if (stepNo === 1) {
      setOldStepNo(stepNo);
      setStepNo(-1);
      const machineID = getMachineID();
      bindCanisterNoDropdownAction(machineID);
      // bindCanisterNoDropdownAction(mdFusionLabNoValueGroup.mdFusionLabNo);
    } else if (stepNo === 2) {
      setStepNo(3);
    } else if (stepNo === 3) {
      setStepNo(3.5);
      // } else if (stepNo === 3.5) {
      //   setOldStepNo(stepNo);
      //   setStepNo(-1);
      //   bindProductDropdownAction();
    } else if (stepNo === 3.5) {
      setStepNo(5);
    } else if (stepNo === 5) {
      setStepNo(6);
    } else if (stepNo === 6) {
      setStepNo(6.5);
    } else if (stepNo === 6.5) {
      setStepNo(7);
    } else if (stepNo === 7) {
      setStepNo(8);
      onSubmit();
    }
  };

  return (
    <>
      {isPdf && valueForQRCode !== null ? (
        <SanitisationPDFDocument
          valueForQRCode={valueForQRCode}
          pageName="SanitisationTracking"
          valueForPDFContent={valueForPDFContent}
          stickerValue={stickerValue}
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
                    </div>
                    <div className="upper-content">
                      <div className="container center">
                        <div id="intro-wrapper">
                          <div className="sixteen columns column">
                            <div id="intro-top">
                              CANISTER SANITATION TRACKING
                            </div>
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
                              if (stepNo === 3 || stepNo === 3.5) {
                                return (
                                  <div
                                    id="intro-subtitle"
                                    className="wow fadeInDown"
                                  >
                                    NEXT SANITIZING REMINDER
                                  </div>
                                );
                              }

                              if (stepNo === 4) {
                                return (
                                  <div
                                    id="intro-subtitle"
                                    classname="wow fadeindown"
                                  >
                                    product
                                  </div>
                                );
                              }

                              if (stepNo === 5) {
                                return (
                                  <div
                                    id="intro-subtitle"
                                    className="wow fadeInDown"
                                  >
                                    CHOOSE STICKER 1 TO 5
                                  </div>
                                );
                              }
                              if (stepNo === 6 || stepNo === 6.5) {
                                return (
                                  <div
                                    id="intro-subtitle"
                                    className="wow fadeInDown"
                                  >
                                    GENERATE QR CODE
                                  </div>
                                );
                              }
                              if (stepNo === 7) {
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
                              className="flex-column align-items-center justify-content-around wow fadeInUp"
                            >
                              {(() => {
                                if (stepNo === 0) {
                                  return (
                                    <input
                                      id="txtUserID"
                                      name="UserID"
                                      className=""
                                      type="text"
                                      // placeholder="******"
                                      value={desktopUserName}
                                    />
                                  );
                                }
                                if (stepNo === 1) {
                                  return (
                                    <>
                                      <Picker
                                        height={100}
                                        id="picMDFusionLabNo"
                                        name="MDFusionLabNo"
                                        optionGroups={mdFusionLabNoOptionGroups}
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
                                    <Picker
                                      height={100}
                                      id="picCanisterID"
                                      name="canisterID"
                                      optionGroups={canisterOptionGroups}
                                      valueGroups={canisterValueGroup}
                                      onChange={(name, value) => {
                                        if (
                                          checkVal(
                                            canisterOptionGroups.canisterID,
                                            value
                                          )
                                        ) {
                                          CanisterHandleChange(name, value);
                                        }
                                      }}
                                    />
                                  );
                                }
                                if (stepNo === 3) {
                                  return (
                                    <>
                                      <div className="wow fadeInUp">
                                        <Picker
                                          height={100}
                                          id="picReminderoption"
                                          name="Reminderoption"
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
                                        MTS
                                      </span>
                                    </>
                                  );
                                }
                                if (stepNo === 3.5) {
                                  return (
                                    <div
                                      className="alert alert-process alert-dismissible show m-auto w-75"
                                      role="alert"
                                    >
                                      This sets next sanitizing date reminder
                                      from current date.
                                      <button
                                        type="button"
                                        className="close"
                                        data-dismiss="alert"
                                        aria-label="Close"
                                      >
                                        <span aria-hidden="true"></span>
                                      </button>
                                    </div>
                                  );
                                }
                                if (stepNo === 4) {
                                  return (
                                    <>
                                      <Picker
                                        height={100}
                                        id="picProduct"
                                        name="Product"
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
                                      />
                                    </>
                                  );
                                }
                                if (stepNo === 5) {
                                  return (
                                    <>
                                      <Picker
                                        height={100}
                                        id="picSticker"
                                        name="Sticker"
                                        className="wow fadeInUp"
                                        optionGroups={stickerOptionGroups}
                                        valueGroups={stickerValueGroup}
                                        onChange={(name, value) => {
                                          if (
                                            checkVal(
                                              stickerOptionGroups.sticker,
                                              value
                                            )
                                          ) {
                                            stickerHandleChange(name, value);
                                            setStickerValue(value);
                                          }
                                        }}
                                      />
                                    </>
                                  );
                                }
                                if (stepNo === 6) {
                                  return (
                                    <div className=" wow fadeInUp">
                                      <img
                                        alt="triangle - MD"
                                        src={triangleQr}
                                      />
                                    </div>
                                  );
                                }
                                if (stepNo === 6.5) {
                                  return (
                                    <div
                                      className="alert alert-process alert-dismissible show m-auto w-75"
                                      role="alert"
                                    >
                                      PLEASE VERIFY ALL DATA IS CORRECT. You
                                      will continue to genrate QR Code and data
                                      will be saved in the system for tracking
                                      purposes.
                                      <button
                                        type="button"
                                        className="close"
                                        data-dismiss="alert"
                                        aria-label="Close"
                                      >
                                        <span aria-hidden="true"></span>
                                      </button>
                                    </div>
                                  );
                                }
                                if (stepNo === 7) {
                                  return (
                                    <div className=" wow fadeInUp">
                                      {' '}
                                      <img
                                        alt="triangle - MD"
                                        src={trianglePrint}
                                      />
                                    </div>
                                  );
                                }
                                if (stepNo === 8) {
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
                                    <div
                                      className="alert alert-process alert-dismissible show m-auto w-75"
                                      role="alert"
                                    >
                                      Success
                                      <button
                                        type="button"
                                        className="close"
                                        data-dismiss="alert"
                                        aria-label="Close"
                                      >
                                        {/* <span aria-hidden="true">&times;</span> */}
                                        <span aria-hidden="true">x</span>
                                      </button>
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
                              if (stepNo != -1 || stepNo != 100) {
                                return (
                                  <button
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
                              if (
                                stepNo != 1.1 ||
                                stepNo != -1 ||
                                stepNo != 8 ||
                                stepNo != 100
                              ) {
                                return (
                                  <button
                                    onClick={onForward}
                                    className="font-weight-bold nav-links navigation-button"
                                  >
                                    {(() => {
                                      if (stepNo === 7) {
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
  const { isAddError, loading, resultMessage, refillPDFData } = refill;
  const { mdFusionLabNoDropdownList, machineID } = user;
  const { productDropdownList } = canister;
  const { canisterNoDropdownList, isSucessfullyAdd, sanitisationPDFData } =
    sanitisation;
  return {
    isSucessfullyAdd,
    isAddError,
    loading,
    canisterNoDropdownList,
    mdFusionLabNoDropdownList,
    productDropdownList,
    resultMessage,
    refillPDFData,
    sanitisationPDFData,
    machineID,
  };
};

export default connect(mapStateToProps, {
  addSanitisationTrakingAction: addSanitisationTraking,
  bindCanisterNoDropdownAction: bindCanisterNoDropdown,
  bindMDFusionLabNoDropdownAction: bindMDFusionLabNoDropdown,
  bindProductDropdownAction: bindProductDropdown,
  ResetStateRefillTrkingAction: ResetStateRefillTrking,
})(newSanitisationTraking);
