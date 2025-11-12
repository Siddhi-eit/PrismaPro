import React, { useState } from 'react';
import { Badge, Card, Tooltip } from 'reactstrap';
// import { NavLink } from 'react-router-dom';
import classnames from 'classnames';
import { ContextMenuTrigger } from 'react-contextmenu';
import { Colxx } from 'components/common/CustomBootstrap';
import moment from 'moment';
// import Dispense from 'views/app/dispense';

// onCheckItem
const GirdThumbListView = ({ dispensehislist, isSelect, collect }) => {
  const [tooltipAmountToDispense, setTooltipAmountToDispense] = useState(false);
  const [tooltipComponentNames, setTooltipComponentNames] = useState(false);
  const [tooltipComponentAmounts, setTooltipComponentAmounts] = useState(false);
  const [tooltipDispensationsNumber, setTooltipDispensationsNumber] =
    useState(false);
  const [tooltipCreatedDate, setTooltipCreatedDate] = useState(false);
  const [tooltipProductCode, setTooltipProductCode] = useState(false);
  const [tooltipLotNr, setTooltipLotNr] = useState(false);
  const [tooltipIsDispense, setTooltipIsDispense] = useState(false);
  return (
    <Colxx xxs="12" key={dispensehislist.id}>
      <ContextMenuTrigger
        id="menu_id"
        data={dispensehislist.id}
        collect={collect}
      >
        <Card
          className={classnames('d-flex flex-row', {
            active: isSelect,
          })}
        >
          <div className="pl-2 d-flex flex-grow-1 ">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center">
              <p
                className="list-item-heading mb-1 truncate w-10"
                id="ProductCode"
              >
                {dispensehislist.productCode}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipProductCode}
                target="ProductCode"
                toggle={() => setTooltipProductCode(!tooltipProductCode)}
              >
                {' '}
                Tailoring Code
              </Tooltip>

              {/* <p
                className="list-item-heading mb-1 truncate w-15"
                id="ProductName"
              >
                {dispensehislist.productName}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipProductName}
                target="ProductName"
                toggle={() => setTooltipProductName(!tooltipProductName)}
              >
                {' '}
                Product Name
              </Tooltip> */}

              <p
                className="list-item-heading mb-1 truncate w-20"
                id="dispensationsNumber"
              >
                {dispensehislist.amountToDispense.replace(
                  /(\d)([a-zA-Z]+)/,
                  '$1 $2'
                )}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipDispensationsNumber}
                target="dispensationsNumber"
                toggle={() =>
                  setTooltipDispensationsNumber(!tooltipDispensationsNumber)
                }
              >
                {' '}
                Amount To Dispense Per Bottle
              </Tooltip>
              <p
                className="list-item-heading mb-1 truncate w-15"
                id="AmountToDispense"
              >
                {dispensehislist.dispensationsNumber}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipAmountToDispense}
                target="AmountToDispense"
                toggle={() =>
                  setTooltipAmountToDispense(!tooltipAmountToDispense)
                }
              >
                {' '}
                Total Number of Bottles
              </Tooltip>
              <p className="list-item-heading mb-1 w-20" id="ComponentNames">
                {dispensehislist.componentNames}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipComponentNames}
                target="ComponentNames"
                toggle={() => setTooltipComponentNames(!tooltipComponentNames)}
              >
                {' '}
                Component Name
              </Tooltip>

              <p className="list-item-heading mb-1 w-25" id="ComponentAmounts">
                {dispensehislist.componentAmounts}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipComponentAmounts}
                target="ComponentAmounts"
                toggle={() =>
                  setTooltipComponentAmounts(!tooltipComponentAmounts)
                }
              >
                {' '}
                Component Amount
              </Tooltip>

              <p className="list-item-heading mb-1 w-10" id="CreatedDate">
                {moment(dispensehislist.createdDate).format('YYYY/MM/DD')}
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
              <p className="list-item-heading mb-1 w-10" id="bachLotNo">
                {dispensehislist.bachLotNo}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipLotNr}
                target="bachLotNo"
                toggle={() => setTooltipLotNr(!tooltipLotNr)}
              >
                {' '}
                bachLotNo
              </Tooltip>
              <p className="list-item-heading mb-1 w-10" id="IsDispense">
                {dispensehislist.isDispense === true ? (
                  <Badge color="success"> Yes </Badge>
                ) : (
                  <Badge color="danger"> No </Badge>
                )}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipIsDispense}
                target="IsDispense"
                toggle={() => setTooltipIsDispense(!tooltipIsDispense)}
              >
                {' '}
                Dispense?
              </Tooltip>
            </div>
          </div>
        </Card>
      </ContextMenuTrigger>
    </Colxx>
  );
};

export default React.memo(GirdThumbListView);
