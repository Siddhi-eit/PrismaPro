/* eslint-disable jsx-a11y/click-events-have-key-events */
/* eslint-disable jsx-a11y/no-static-element-interactions */
/* eslint-disable no-use-before-define */
import React, { useState, useEffect } from 'react';
import { injectIntl } from 'react-intl';
import IntlMessages from 'helpers/IntlMessages';
import {
  FormGroup,
  Label,
  UncontrolledDropdown,
  DropdownItem,
  DropdownToggle,
  DropdownMenu,
} from 'reactstrap';
import { NavLink } from 'react-router-dom';
import { connect } from 'react-redux';

import {
  setContainerClassnames,
  clickOnMobileMenu,
  logoutUser,
  changeLocale,
  bindMachineDropdown,
  setDesktopUsersID,
} from 'redux/actions';

import {
  searchPath,
  // isDarkSwitchActive,
  adminRoot,
} from 'constants/defaultValues';

import { MobileMenuIcon, MenuIcon } from 'components/svg';
import Select from 'react-select';
import {
  getCurrentUser,
  getMachineID,
  getMachineName,
  setMachineID,
} from 'helpers/Utils';
// import TopnavDarkSwitch from './Topnav.DarkSwitch';

const TopNav = ({
  history,
  containerClassnames,
  menuClickCount,
  selectedMenuHasSubItems,
  setContainerClassnamesAction,
  clickOnMobileMenuAction,
  logoutUserAction,
  bindMachineDropdownAction,
  machineDropdownList,
  setDesktopMachineIDAction,
}) => {
  const [isInFullScreen, setIsInFullScreen] = useState(false);
  const [searchKeyword, setSearchKeyword] = useState('');
  // const [_machineID, _setMachineID] = useState(0);
  const [selectedMachine, setSelectedMachine] = useState(null);

  const search = () => {
    history.push(`${searchPath}?key=${searchKeyword}`);
    setSearchKeyword('');
  };

  const isInFullScreenFn = () => {
    return (
      (document.fullscreenElement && document.fullscreenElement !== null) ||
      (document.webkitFullscreenElement &&
        document.webkitFullscreenElement !== null) ||
      (document.mozFullScreenElement &&
        document.mozFullScreenElement !== null) ||
      (document.msFullscreenElement && document.msFullscreenElement !== null)
    );
  };

  const handleDocumentClickSearch = (e) => {
    let isSearchClick = false;
    if (
      e.target &&
      e.target.classList &&
      (e.target.classList.contains('navbar') ||
        e.target.classList.contains('simple-icon-magnifier'))
    ) {
      isSearchClick = true;
      if (e.target.classList.contains('simple-icon-magnifier')) {
        search();
      }
    } else if (
      e.target.parentElement &&
      e.target.parentElement.classList &&
      e.target.parentElement.classList.contains('search')
    ) {
      isSearchClick = true;
    }

    if (!isSearchClick) {
      const input = document.querySelector('.mobile-view');
      if (input && input.classList) input.classList.remove('mobile-view');
      removeEventsSearch();
      setSearchKeyword('');
    }
  };

  const removeEventsSearch = () => {
    document.removeEventListener('click', handleDocumentClickSearch, true);
  };

  const toggleFullScreen = () => {
    const isFS = isInFullScreenFn();

    const docElm = document.documentElement;
    if (!isFS) {
      if (docElm.requestFullscreen) {
        docElm.requestFullscreen();
      } else if (docElm.mozRequestFullScreen) {
        docElm.mozRequestFullScreen();
      } else if (docElm.webkitRequestFullScreen) {
        docElm.webkitRequestFullScreen();
      } else if (docElm.msRequestFullscreen) {
        docElm.msRequestFullscreen();
      }
    } else if (document.exitFullscreen) {
      document.exitFullscreen();
    } else if (document.webkitExitFullscreen) {
      document.webkitExitFullscreen();
    } else if (document.mozCancelFullScreen) {
      document.mozCancelFullScreen();
    } else if (document.msExitFullscreen) {
      document.msExitFullscreen();
    }
    setIsInFullScreen(!isFS);
  };

  const handleLogout = () => {
    logoutUserAction(history);
  };

  const menuButtonClick = (e, _clickCount, _conClassnames) => {
    e.preventDefault();

    setTimeout(() => {
      const event = document.createEvent('HTMLEvents');
      event.initEvent('resize', false, false);
      window.dispatchEvent(event);
    }, 350);
    setContainerClassnamesAction(
      _clickCount + 1,
      _conClassnames,
      selectedMenuHasSubItems
    );
  };

  const mobileMenuButtonClick = (e, _containerClassnames) => {
    e.preventDefault();
    clickOnMobileMenuAction(_containerClassnames);
  };

  useEffect(() => {
    let user = getCurrentUser();
    bindMachineDropdownAction(user.uid);
  }, []);

  const [machinedropdownItem, setMachinedropdownItem] = useState([]);
  const [defaulValuedropdown, setDefaulValuedropdown] = useState();

  useEffect(() => {
    if (machineDropdownList != null && machineDropdownList.length > 0) {
      const options = machineDropdownList.map((d) => ({
        value: d.id,
        label: `${!d.machineRegNo ? '' : d.machineRegNo} ${
          !d.shopName ? '' : d.shopName
        }`,
      }));
      let machineID = getMachineID();
      let machineName = getMachineName();
      setMachinedropdownItem(options);
      if (machineID != 0 && machineID != null && machineID != undefined) {
        setMachineID(machineID, machineName);
      } else {
        setMachineID(options[0].value, options[0].label);
        setSelectedMachine(options[0].value, options[0].label);
        setDesktopMachineIDAction(options[0].value, options[0].label);
      }
    }
  }, [machineDropdownList]);
  const handleChange = (e) => {
    setDefaulValuedropdown(e);
    setDesktopMachineIDAction(e.value, e.label);
    setSelectedMachine(e.value, e.label);
    setMachineID(e.value, e.label);
  };

  useEffect(() => {
    const path = window.location.pathname.split('/');
    const pathLastValue = path[path.length - 1];
    if (pathLastValue === 'machine') {
      document.getElementById('machineDropdown').style.display = 'none';
    } else {
      document.getElementById('machineDropdown').style.display = 'block';
    }

    let machineID = getMachineID();
    let machineName = getMachineName();
    if (machineID != 0 && machineID != null && machineID != undefined) {
      setMachineID(machineID, machineName);
    }
  }, []);

  return (
    <nav className="navbar fixed-top">
      <div className="d-flex align-items-center navbar-left">
        <NavLink
          to="#"
          location={{}}
          className="menu-button d-none d-md-block"
          onClick={(e) =>
            menuButtonClick(e, menuClickCount, containerClassnames)
          }
        >
          <MenuIcon />
        </NavLink>
        <NavLink
          to="#"
          location={{}}
          className="menu-button-mobile d-xs-block d-sm-block d-md-none"
          onClick={(e) => mobileMenuButtonClick(e, containerClassnames)}
        >
          <MobileMenuIcon />
        </NavLink>
        <div className="d-inline-block w-50 pt-3">
          <FormGroup
            className="form-group has-float-label"
            id="machineDropdown"
          >
            <Label>
              <IntlMessages id="forms.state" />
            </Label>
            <Select
              id="machines"
              options={machinedropdownItem}
              onChange={handleChange}
              classNamePrefix="react-select"
              defaultValue={machinedropdownItem[0]}
              value={[{ value: getMachineID(), label: getMachineName() }]}
            />
            {/* <FormikReactSelect
              name="state"
              id="state"
              // value={values.state}
              // options={options}
              // onChange={setFieldValue}
              // onBlur={setFieldTouched}
            /> */}
            {/* {errors.state && touched.state ? (
              <div className="invalid-feedback d-block">{errors.state}</div>
            ) : null} */}
          </FormGroup>
        </div>
      </div>
      <NavLink className="navbar-logo" to={adminRoot}>
        <span className="logo d-none d-xs-block " />
        <span className="logo-mobile d-block d-xs-none" />
      </NavLink>

      <div className="navbar-right">
        {/* {isDarkSwitchActive && <TopnavDarkSwitch />} */}
        <div className="header-icons d-inline-block align-middle">
          <button
            className="header-icon btn btn-empty d-none d-sm-inline-block"
            type="button"
            id="fullScreenButton"
            onClick={toggleFullScreen}
          >
            {isInFullScreen ? (
              <i className="simple-icon-size-actual d-block" />
            ) : (
              <i className="simple-icon-size-fullscreen d-block" />
            )}
          </button>
        </div>
        <div className="machine d-inline-block">
          <UncontrolledDropdown className="dropdown-menu-right">
            <DropdownToggle className="p-0" color="empty">
              <span className="name mr-1">
                {
                  JSON.parse(localStorage.getItem('Haute_current_machine'))
                    .title
                }
              </span>
              <span>
                <img
                  alt="Profile"
                  src="/assets/img/profiles/Unknown_person.jpg"
                />
              </span>
            </DropdownToggle>
            <DropdownMenu className="mt-3" right>
              {/* <DropdownItem>Account</DropdownItem>
              <DropdownItem divider /> */}
              <DropdownItem onClick={() => handleLogout()}>
                Sign out
              </DropdownItem>
            </DropdownMenu>
          </UncontrolledDropdown>
        </div>
      </div>
    </nav>
  );
};

const mapStateToProps = ({ menu, settings, user }) => {
  const { containerClassnames, menuClickCount, selectedMenuHasSubItems } = menu;
  const { locale } = settings;
  const { machineDropdownList } = user;
  return {
    containerClassnames,
    menuClickCount,
    selectedMenuHasSubItems,
    locale,
    machineDropdownList,
  };
};

export default injectIntl(
  connect(mapStateToProps, {
    setContainerClassnamesAction: setContainerClassnames,
    clickOnMobileMenuAction: clickOnMobileMenu,
    logoutUserAction: logoutUser,
    bindMachineDropdownAction: bindMachineDropdown,
    changeLocaleAction: changeLocale,
    setDesktopMachineIDAction: setDesktopUsersID,
  })(TopNav)
);
