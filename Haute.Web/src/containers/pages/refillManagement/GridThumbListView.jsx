import React, { useState } from 'react';
import { Badge, Card, Tooltip } from 'reactstrap';
import classnames from 'classnames';
import { ContextMenuTrigger } from 'react-contextmenu';
import { Colxx } from 'components/common/CustomBootstrap';
import moment from 'moment';

// onCheckItem
const GirdThumbListView = ({ refill, isSelect, collect, onDownloadClick }) => {
  const [tooltipCanisterCode, setTooltipCanisterCode] = useState(false);
  const [tooltipFusionLabNo, setTooltipFusionLabNo] = useState(false);
  const [tooltipDateFilled, setTooltipDateFilled] = useState(false);
  const [tooltipLotNo, setTooltipLotNo] = useState(false);
  const [tooltipProductName, setTooltipProductName] = useState(false);
  const [tooltipQuantity, setTooltipQuantity] = useState(false);
  const [tooltipCreatedDate, setTooltipCreatedDate] = useState(false);
  const [tooltipIsActive, setTooltipIsActive] = useState(false);
  const [tooltipIsRefilled, setTooltipIsRefilled] = useState(false);
  return (
    <Colxx xxs="12" key={refill.id}>
      <ContextMenuTrigger id="menu_id" data={refill.id} collect={collect}>
        <Card
          className={classnames('', {
            active: isSelect,
          })}
        >
          <div className="pl-2 d-flex flex-grow-1 min-width-zero">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center ">
              <p
                className="w-10 list-item-heading mb-1 text-center"
                id="CanisterCode"
              >
                {refill.canisterCode}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipCanisterCode}
                target="CanisterCode"
                toggle={() => setTooltipCanisterCode(!tooltipCanisterCode)}
              >
                Canister Code
              </Tooltip>
              <p
                className="w-10 list-item-heading mb-1 text-center"
                id="FusionLabNo"
              >
                {refill.fusionLabNo}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipFusionLabNo}
                target="FusionLabNo"
                toggle={() => setTooltipFusionLabNo(!tooltipFusionLabNo)}
              >
                Fusion Lab No
              </Tooltip>
              <p
                className="w-10 list-item-heading mb-1 text-center"
                id="DateFilled"
              >
                {refill.dateFilled
                  ? moment(refill.dateFilled).format('YYYY/MM/DD')
                  : '-'}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipDateFilled}
                target="DateFilled"
                toggle={() => setTooltipDateFilled(!tooltipDateFilled)}
              >
                Date Filled
              </Tooltip>
              <p
                className="w-10 list-item-heading mb-1 text-center"
                id="LOT_NO"
              >
                {refill.lotNr ? refill.lotNr : '-'}
              </p>

              <Tooltip
                placement="left"
                isOpen={tooltipLotNo}
                target="LOT_NO"
                toggle={() => setTooltipLotNo(!tooltipLotNo)}
              >
                LOT No
              </Tooltip>
              <p
                className="w-20 list-item-heading mb-1 text-center"
                id="Product"
              >
                {refill.name ? refill.name : '-'}
              </p>

              <Tooltip
                placement="left"
                isOpen={tooltipProductName}
                target="Product"
                toggle={() => setTooltipProductName(!tooltipProductName)}
              >
                Name
              </Tooltip>
              <p
                className="w-10 list-item-heading mb-1 text-center"
                id="Quantity"
              >
                {parseFloat(refill.quantity).toFixed(2)}
              </p>

              <Tooltip
                placement="left"
                isOpen={tooltipQuantity}
                target="Quantity"
                toggle={() => setTooltipQuantity(!tooltipQuantity)}
              >
                Quantity
              </Tooltip>

              <p
                className="w-10 list-item-heading mb-1 text-center"
                id="CreatedDate"
              >
                {moment(refill.createdDate).format('YYYY/MM/DD')}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipCreatedDate}
                target="CreatedDate"
                toggle={() => setTooltipCreatedDate(!tooltipCreatedDate)}
              >
                Created Date
              </Tooltip>
              {/* <div> */}
              <p
                className="w-10 list-item-heading mb-1 text-center"
                id="IsRefilled"
              >
                {refill.isRefilled === true ? (
                  <Badge color="success"> Yes </Badge>
                ) : (
                  <Badge color="danger"> No </Badge>
                )}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipIsRefilled}
                target="IsRefilled"
                toggle={() => setTooltipIsRefilled(!tooltipIsRefilled)}
              >
                {' '}
                Refilled?{' '}
              </Tooltip>
              {/* </div> */}
              <p
                className="w-10 list-item-heading mb-1 text-center"
                id="IsActive"
              >
                {refill.isActive === true ? (
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
              <p
                className="w-10 list-item-heading mb-1 text-center"
                onClick={() => onDownloadClick(refill.id)}
              >
                <i className="iconsminds-download d-block" />
              </p>
            </div>
          </div>
        </Card>
      </ContextMenuTrigger>
    </Colxx>
  );
};

export default React.memo(GirdThumbListView);
