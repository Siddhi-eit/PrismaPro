import React, { useState } from 'react';
import { Badge, Card, Tooltip } from 'reactstrap';
import classnames from 'classnames';
import { ContextMenuTrigger } from 'react-contextmenu';
import { Colxx } from 'components/common/CustomBootstrap';
// import moment from 'moment';

const FormulaThumbListView = ({ formula, isSelect, collect }) => {
  const [tooltipProductCode, setTooltipProductCode] = useState(false);
  const [tooltipDispenseAmount, setTooltipDispenseAmount] = useState(false);
  const [tooltipColorCodes, setTooltipColorCodes] = useState(false);
  const [tooltipAmounts, setTooltipAmounts] = useState(false);
  return (
    <Colxx xxs="12" key={formula.ID}>
      <ContextMenuTrigger id="menu_id" data={formula.ID} collect={collect}>
        <Card
          className={classnames('d-flex flex-row', {
            active: isSelect,
          })}
        >
          <div className="pl-2 d-flex flex-grow-1 min-width-zero">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center">
              <p
                className="w-10 list-item-heading mb-1 truncate"
                id="ProductCode"
              >
                {formula.ProductCode}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipProductCode}
                target="ProductCode"
                toggle={() => setTooltipProductCode(!tooltipProductCode)}
                className="w-20"
              >
                {' '}
                ProductCode{' '}
              </Tooltip>
              <p className="w-10 list-item-heading mb-1" id="DispenseAmount">
                {formula.DispenseAmount}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipDispenseAmount}
                target="DispenseAmount"
                toggle={() => setTooltipDispenseAmount(!tooltipDispenseAmount)}
              >
                {' '}
                DispenseAmount{' '}
              </Tooltip>

              <p className="w-20 list-item-heading mb-1" id="ColorCodes">
                {formula.ColorCodes}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipColorCodes}
                target="ColorCodes"
                toggle={() => setTooltipColorCodes(!tooltipColorCodes)}
              >
                {' '}
                ColorCodes{' '}
              </Tooltip>

              <p className="w-20 list-item-heading mb-1" id="Amounts">
                {formula.Amounts}
              </p>
              <Tooltip
                placement="left"
                isOpen={tooltipAmounts}
                target="Amounts"
                toggle={() => setTooltipAmounts(!tooltipAmounts)}
              >
                {' '}
                Amounts{' '}
              </Tooltip>
            </div>
          </div>
        </Card>
      </ContextMenuTrigger>
    </Colxx>
  );
};

export default React.memo(FormulaThumbListView);
