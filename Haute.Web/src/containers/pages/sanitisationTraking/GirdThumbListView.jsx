import React, { useState } from 'react';
import { Badge, Card, Tooltip } from 'reactstrap';
// import { NavLink } from 'react-router-dom';
import classnames from 'classnames';
import { ContextMenuTrigger } from 'react-contextmenu';
import { Colxx } from 'components/common/CustomBootstrap';
import moment from 'moment';

// onCheckItem
const GirdThumbListView = ({ sanitisation, isSelect, collect }) => {
  const [tooltipFusionLabNo, setTooltipFusionLabNo] = useState(false);
  const [tooltipCanisterCode, setTooltipCanisterCode] = useState(false);
  const [tooltipDateSanitised, setTooltipDateSanitised] = useState(false);
  // const [tooltipProductID, setTooltipProductID] = useState(false);
  // const [tooltipSetReminder, setTooltipSetReminder] = useState(false);
  // const [tooltipIsSanitized, setTooltipIsSanitized] = useState(false);
  const [tooltipCreatedDate, setTooltipCreatedDate] = useState(false);
  const [tooltipIsActive, setTooltipIsActive] = useState(false);
  const [tooltipIsSanitized, setTooltipIsSanitized] = useState(false);
  return (
    <Colxx xxs="12" key={sanitisation.id}>
      <ContextMenuTrigger id="menu_id" data={sanitisation.id} collect={collect}>
        <Card
          className={classnames('d-flex flex-row', {
            active: isSelect,
          })}
        >
          <div className="pl-2 d-flex flex-grow-1 ">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center">
              <p
                className="list-item-heading mb-1 truncate w-10"
                id="FusionLabNo"
              >
                {sanitisation.fusionLabNo}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipFusionLabNo}
                target="FusionLabNo"
                toggle={() => setTooltipFusionLabNo(!tooltipFusionLabNo)}
              >
                {' '}
                FusionLab No
              </Tooltip>
              <p
                className="list-item-heading mb-1 w-10 w-sm-100"
                id="CanisterCode"
              >
                {sanitisation.canisterCode}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipCanisterCode}
                target="CanisterCode"
                toggle={() => setTooltipCanisterCode(!tooltipCanisterCode)}
              >
                {' '}
                Canister No
              </Tooltip>

              <p
                className="list-item-heading mb-1 w-10 w-sm-100 text-center"
                id="DateSanitised"
              >
                {sanitisation.dateSanitised
                  ? moment(sanitisation.dateSanitised).format('YYYY/MM/DD')
                  : '-'}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipDateSanitised}
                target="DateSanitised"
                toggle={() => setTooltipDateSanitised(!tooltipDateSanitised)}
              >
                {' '}
                Sanitised Date
              </Tooltip>

              <p
                className="list-item-heading mb-1 w-10 w-sm-100 text-center"
                id="CreatedDate"
              >
                {moment(sanitisation.createdDate).format('YYYY/MM/DD')}
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
              <div>
                <p
                  className="w-15 list-item-heading mb-1 w-10 w-sm-100"
                  id="IsSanitized"
                >
                  {sanitisation.isSanitized === true ? (
                    <Badge color="success"> Yes </Badge>
                  ) : (
                    <Badge color="danger"> No </Badge>
                  )}
                </p>
                <Tooltip
                  placement="left"
                  isOpen={tooltipIsSanitized}
                  target="IsSanitized"
                  toggle={() => setTooltipIsSanitized(!tooltipIsSanitized)}
                >
                  {' '}
                  Sanitized?{' '}
                </Tooltip>
              </div>
              <p
                className="w-15 list-item-heading mb-1 w-10 w-sm-100 text-center"
                id="IsActive"
              >
                {sanitisation.isActive === true ? (
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
