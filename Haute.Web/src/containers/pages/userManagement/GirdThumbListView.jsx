import React, { useState } from 'react';
import { Badge, Card, Tooltip } from 'reactstrap';
// import { NavLink } from 'react-router-dom';
import classnames from 'classnames';
import { ContextMenuTrigger } from 'react-contextmenu';
import { Colxx } from 'components/common/CustomBootstrap';
import moment from 'moment';

// onCheckItem
const GirdThumbListView = ({ user, isSelect, collect }) => {
  const [tooltipOpen, setTooltipOpen] = useState(false);
  const [tooltipLastName, setTooltipLastName] = useState(false);
  const [tooltipUserName, setTooltipUserName] = useState(false);
  const [tooltipEmail, setTooltipEmail] = useState(false);
  const [tooltipPhone, setTooltipPhone] = useState(false);
  const [tooltipRoleName, setTooltipRoleName] = useState(false);
  const [tooltipCreatedDate, setTooltipCreatedDate] = useState(false);
  const [tooltipIsActive, setTooltipIsActive] = useState(false);
  return (
    <Colxx xxs="12" key={user.id} className=" ">
      <ContextMenuTrigger id="menu_id" data={user.id} collect={collect}>
        <Card
          className={classnames('d-flex ', {
            active: isSelect,
          })}
        >
          <div className="pl-2 ">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center align-center">
              <p
                className="w-10 list-item-heading mb-1 truncate"
                id="FirstName"
              >
                {user.firstName}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipOpen}
                target="FirstName"
                toggle={() => setTooltipOpen(!tooltipOpen)}
              >
                {' '}
                First Name
              </Tooltip>
              {/* </NavLink> */}
              <p className="w-10 list-item-heading mb-1 truncate" id="LastName">
                {user.lastName}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipLastName}
                target="LastName"
                toggle={() => setTooltipLastName(!tooltipLastName)}
              >
                {' '}
                Last Name
              </Tooltip>

              <p className="w-10 list-item-heading mb-1 truncate" id="UserName">
                {user.userName}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipUserName}
                target="UserName"
                toggle={() => setTooltipUserName(!tooltipUserName)}
              >
                {' '}
                User Name
              </Tooltip>

              <p className="w-20 list-item-heading mb-1 truncate" id="Email">
                {user.email}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipEmail}
                target="Email"
                toggle={() => setTooltipEmail(!tooltipEmail)}
              >
                Email Id
              </Tooltip>

              <p className="w-10 list-item-heading mb-1 truncate" id="Phone">
                {user.phone}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipPhone}
                target="Phone"
                toggle={() => setTooltipPhone(!tooltipPhone)}
              >
                Phone No
              </Tooltip>

              <p className="w-10 list-item-heading mb-1 truncate" id="RoleName">
                {user.roleName}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipRoleName}
                target="RoleName"
                toggle={() => setTooltipRoleName(!tooltipRoleName)}
              >
                User Type
              </Tooltip>

              <p
                className="w-10 list-item-heading mb-1 truncate"
                id="CreatedDate"
              >
                {moment(user.createdDate).format('YYYY/MM/DD')}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipCreatedDate}
                target="CreatedDate"
                toggle={() => setTooltipCreatedDate(!tooltipCreatedDate)}
              >
                {' '}
                Created Date
              </Tooltip>

              <p className="w-10 list-item-heading mb-1 truncate" id="IsActive">
                {user.isActive === true ? (
                  <Badge color="success"> Yes </Badge>
                ) : (
                  <Badge color="danger"> No </Badge>
                )}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipIsActive}
                target="IsActive"
                toggle={() => setTooltipIsActive(!tooltipIsActive)}
              >
                {' '}
                Active?{' '}
              </Tooltip>
            </div>
          </div>
        </Card>
      </ContextMenuTrigger>
    </Colxx>
  );
};

export default React.memo(GirdThumbListView);
