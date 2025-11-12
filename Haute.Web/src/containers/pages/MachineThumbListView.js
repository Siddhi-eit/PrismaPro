import React, { useState } from 'react';
import { Badge, Card, Tooltip } from 'reactstrap';
import classnames from 'classnames';
import { ContextMenuTrigger } from 'react-contextmenu';
import { Colxx } from 'components/common/CustomBootstrap';
import moment from 'moment';

const MachineThumbListView = ({ machine, isSelect, collect }) => {
  const [tooltipMachineRegNo, setTooltipMachineRegNo] = useState(false);
  const [tooltipShopName, setTooltipShopName] = useState(false);
  const [tooltipShopAddress, setTooltipShopAddress] = useState(false);
  const [tooltipCity, setTooltipCity] = useState(false);
  const [tooltipState, setTooltipState] = useState(false);
  const [tooltipMacAddress, setTooltipMacAddress] = useState(false);
  const [tooltipCreatedDate, setTooltipCreatedDate] = useState(false);
  const [tooltipIsActive, setTooltipIsActive] = useState(false);

  return (
    <Colxx xxs="12" key={machine.ID}>
      <ContextMenuTrigger id="menu_id" data={machine.ID} collect={collect}>
        <Card
          className={classnames('d-flex flex-row', {
            active: isSelect,
          })}
        >
          <div className="pl-2 d-flex flex-grow-1 min-width-zero">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center">
              <p
                className="w-15 list-item-heading mb-1 truncate"
                id="MachineRegNo"
              >
                {machine.MachineRegNo}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipMachineRegNo}
                target="MachineRegNo"
                toggle={() => setTooltipMachineRegNo(!tooltipMachineRegNo)}
                className="w-20"
              >
                {' '}
                MachineRegNo{' '}
              </Tooltip>

              <p className="w-15 list-item-heading mb-1" id="ShopName">
                {machine.ShopName}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipShopName}
                target="ShopName"
                toggle={() => setTooltipShopName(!tooltipShopName)}
              >
                {' '}
                ShopName{' '}
              </Tooltip>

              <p className="w-15 list-item-heading mb-1" id="ShopAddress">
                {machine.ShopAddress}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipShopAddress}
                target="ShopAddress"
                toggle={() => setTooltipShopAddress(!tooltipShopAddress)}
              >
                {' '}
                ShopAddress{' '}
              </Tooltip>

              <p className="w-15 list-item-heading mb-1" id="City">
                {machine.City}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipCity}
                target="City"
                toggle={() => setTooltipCity(!tooltipCity)}
              >
                {' '}
                City{' '}
              </Tooltip>

              <p className="w-15 list-item-heading mb-1" id="State">
                {machine.State}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipState}
                target="State"
                toggle={() => setTooltipState(!tooltipState)}
              >
                {' '}
                State{' '}
              </Tooltip>

              <p className="w-15 list-item-heading mb-1" id="MacAddress">
                {machine.MacAddress}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipMacAddress}
                target="MacAddress"
                toggle={() => setTooltipMacAddress(!tooltipMacAddress)}
              >
                {' '}
                MacAddress{' '}
              </Tooltip>

              <p className="w-15 list-item-heading mb-1" id="CreatedDate">
                {moment(machine.CreatedDate).format('YYYY/MM/DD')}
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

              <p className="w-15 list-item-heading mb-1" id="IsActive">
                {machine.IsActive === 1 ? (
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

export default React.memo(MachineThumbListView);
