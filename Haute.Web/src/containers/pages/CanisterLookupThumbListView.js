import React, { useState } from 'react';
import { Badge, Card, Tooltip } from 'reactstrap';
// import { NavLink } from 'react-router-dom';
import classnames from 'classnames';
import { ContextMenuTrigger } from 'react-contextmenu';
import { Colxx } from 'components/common/CustomBootstrap';
import moment from 'moment';

// onCheckItem
const CanisterLookupThumbListView = ({ product, isSelect, collect }) => {
  const [tooltipOpen, setTooltipOpen] = useState(false);
  const [tooltipColor, setTooltipColor] = useState(false);
  const [tooltipCurrentAmount, setTooltipCurrentAmount] = useState(false);
  const [tooltipCreatedDate, setTooltipCreatedDate] = useState(false);
  const [tooltipName, setTooltipName] = useState(false);
  const [tooltipIsActive, setTooltipIsActive] = useState(false);

  return (
    <Colxx xxs="12" key={product.ID}>
      <ContextMenuTrigger id="menu_id" data={product.ID} collect={collect}>
        <Card
          className={classnames('d-flex flex-row', {
            active: isSelect,
          })}
        >
          <div className="pl-2 d-flex flex-grow-1 min-width-zero">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center">
              <p
                className="w-10 list-item-heading mb-1 truncate"
                id="CanisterCode"
              >
                {product.CanisterCode}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipOpen}
                target="CanisterCode"
                toggle={() => setTooltipOpen(!tooltipOpen)}
                className="w-20"
              >
                {' '}
                Code{' '}
              </Tooltip>

              <p className="w-15 list-item-heading mb-1" id="SKU">
                {product.SKU}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipColor}
                target="SKU"
                toggle={() => setTooltipColor(!tooltipColor)}
              >
                {' '}
                SKU{' '}
              </Tooltip>

              <p className="w-20 list-item-heading mb-1" id="Name">
                {product.Name}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipName}
                target="Name"
                toggle={() => setTooltipName(!tooltipName)}
              >
                {' '}
                Name{' '}
              </Tooltip>

              <p
                className="w-15 list-item-heading mb-1 text-center"
                id="CreatedDate"
              >
                {moment(product.CreatedDate).format('YYYY/MM/DD')}
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
              <p className="w-10 list-item-heading mb-1" id="IsActive">
                {product.IsActive === 1 ? (
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

export default React.memo(CanisterLookupThumbListView);
